using Microsoft.AspNetCore.Components;
using SportsLeague.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SportsLeague.WebAssembly.Services
{
    public class SportsLeagueApiService : ISportsLeagueApiService
    {
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private string _baseUrl;
        private string _fallbackUrl;

        public SportsLeagueApiService(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;

            // Detección automática del protocolo del navegador para evitar errores de contenido mixto (Mixed Content)
            if (_navigationManager.BaseUri.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                _baseUrl = "https://localhost:7169/api/";
                _fallbackUrl = "http://localhost:5070/api/";
            }
            else
            {
                _baseUrl = "http://localhost:5070/api/";
                _fallbackUrl = "https://localhost:7169/api/";
            }
        }

        public string GetApiBaseUrl() => _baseUrl;

        public async Task<bool> CheckApiHealthAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}teams");
                if (response.IsSuccessStatusCode) return true;
            }
            catch
            {
                // Fallback
            }

            try
            {
                var responseFallback = await _httpClient.GetAsync($"{_fallbackUrl}teams");
                if (responseFallback.IsSuccessStatusCode)
                {
                    SwitchBaseUrl();
                    return true;
                }
            }
            catch
            {
                // Ambos fallaron
            }

            return false;
        }

        private void SwitchBaseUrl()
        {
            var temp = _baseUrl;
            _baseUrl = _fallbackUrl;
            _fallbackUrl = temp;
        }

        #region Métodos de Equipos

        public async Task<IEnumerable<TeamDto>> GetTeamsAsync(string? cityFilter = null)
        {
            var query = string.IsNullOrWhiteSpace(cityFilter) 
                ? "teams" 
                : $"teams?city={Uri.EscapeDataString(cityFilter)}";

            try
            {
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<TeamDto>>($"{_baseUrl}{query}");
                return result ?? new List<TeamDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Intento principal con {_baseUrl} falló: {ex.Message}. Probando fallback...");
                try
                {
                    var fallbackResult = await _httpClient.GetFromJsonAsync<IEnumerable<TeamDto>>($"{_fallbackUrl}{query}");
                    if (fallbackResult != null)
                    {
                        SwitchBaseUrl();
                        return fallbackResult;
                    }
                }
                catch (Exception fallbackEx)
                {
                    Console.WriteLine($"Fallback falló: {fallbackEx.Message}");
                }
                return new List<TeamDto>();
            }
        }

        public async Task<TeamDto?> GetTeamByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<TeamDto>($"{_baseUrl}teams/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al consultar equipo {id}: {ex.Message}");
                try
                {
                    var fallback = await _httpClient.GetFromJsonAsync<TeamDto>($"{_fallbackUrl}teams/{id}");
                    if (fallback != null)
                    {
                        SwitchBaseUrl();
                        return fallback;
                    }
                }
                catch { }
                return null;
            }
        }

        public async Task<TeamDto?> CreateTeamAsync(TeamDto teamDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}teams", teamDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TeamDto>();
                }

                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"El servidor respondió ({response.StatusCode}): {errorText}");
            }
            catch (Exception ex) when (ex.Message.Contains("Failed to fetch") || ex is HttpRequestException)
            {
                Console.WriteLine($"Error en {_baseUrl}. Intentando fallback en {_fallbackUrl}...");
                try
                {
                    var fallbackResponse = await _httpClient.PostAsJsonAsync($"{_fallbackUrl}teams", teamDto);
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        SwitchBaseUrl();
                        return await fallbackResponse.Content.ReadFromJsonAsync<TeamDto>();
                    }

                    var fallbackError = await fallbackResponse.Content.ReadAsStringAsync();
                    throw new Exception($"El servidor respondió ({fallbackResponse.StatusCode}): {fallbackError}");
                }
                catch (Exception finalEx)
                {
                    Console.WriteLine($"Error en fallback: {finalEx.Message}");
                    throw new Exception($"No se pudo conectar con el Backend de la API. Verifique que SportsLeagueApi esté en ejecución en https://localhost:7169 o http://localhost:5070. Detalle: {finalEx.Message}");
                }
            }
        }

        public async Task<bool> UpdateTeamAsync(int id, TeamDto teamDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}teams/{id}", teamDto);
                if (response.IsSuccessStatusCode) return true;

                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar ({response.StatusCode}): {errorText}");
            }
            catch (Exception ex) when (ex.Message.Contains("Failed to fetch") || ex is HttpRequestException)
            {
                try
                {
                    var fallbackResponse = await _httpClient.PutAsJsonAsync($"{_fallbackUrl}teams/{id}", teamDto);
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        SwitchBaseUrl();
                        return true;
                    }
                    var errorText = await fallbackResponse.Content.ReadAsStringAsync();
                    throw new Exception($"Error al actualizar ({fallbackResponse.StatusCode}): {errorText}");
                }
                catch (Exception finalEx)
                {
                    throw new Exception($"Error de conexión con el Backend: {finalEx.Message}");
                }
            }
        }

        public async Task<bool> DeleteTeamAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}teams/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) when (ex.Message.Contains("Failed to fetch") || ex is HttpRequestException)
            {
                try
                {
                    var fallbackResponse = await _httpClient.DeleteAsync($"{_fallbackUrl}teams/{id}");
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        SwitchBaseUrl();
                        return true;
                    }
                }
                catch { }
                return false;
            }
        }

        #endregion

        #region Métodos de Jugadores

        public async Task<IEnumerable<PlayerDto>> GetPlayersAsync(int? teamIdFilter = null)
        {
            var query = (teamIdFilter.HasValue && teamIdFilter.Value > 0)
                ? $"players?teamId={teamIdFilter.Value}"
                : "players";

            try
            {
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<PlayerDto>>($"{_baseUrl}{query}");
                return result ?? new List<PlayerDto>();
            }
            catch
            {
                try
                {
                    var fallbackResult = await _httpClient.GetFromJsonAsync<IEnumerable<PlayerDto>>($"{_fallbackUrl}{query}");
                    if (fallbackResult != null)
                    {
                        SwitchBaseUrl();
                        return fallbackResult;
                    }
                }
                catch { }
                return new List<PlayerDto>();
            }
        }

        public async Task<PlayerDto?> GetPlayerByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<PlayerDto>($"{_baseUrl}players/{id}");
            }
            catch
            {
                try
                {
                    var fallback = await _httpClient.GetFromJsonAsync<PlayerDto>($"{_fallbackUrl}players/{id}");
                    if (fallback != null)
                    {
                        SwitchBaseUrl();
                        return fallback;
                    }
                }
                catch { }
                return null;
            }
        }

        public async Task<PlayerDto?> CreatePlayerAsync(PlayerDto playerDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}players", playerDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PlayerDto>();
                }

                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"El servidor respondió ({response.StatusCode}): {errorText}");
            }
            catch (Exception ex) when (ex.Message.Contains("Failed to fetch") || ex is HttpRequestException)
            {
                try
                {
                    var fallbackResponse = await _httpClient.PostAsJsonAsync($"{_fallbackUrl}players", playerDto);
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        SwitchBaseUrl();
                        return await fallbackResponse.Content.ReadFromJsonAsync<PlayerDto>();
                    }

                    var fallbackError = await fallbackResponse.Content.ReadAsStringAsync();
                    throw new Exception($"El servidor respondió ({fallbackResponse.StatusCode}): {fallbackError}");
                }
                catch (Exception finalEx)
                {
                    throw new Exception($"No se pudo conectar con el Backend de la API: {finalEx.Message}");
                }
            }
        }

        public async Task<bool> UpdatePlayerAsync(int id, PlayerDto playerDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}players/{id}", playerDto);
                if (response.IsSuccessStatusCode) return true;

                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar ({response.StatusCode}): {errorText}");
            }
            catch (Exception ex) when (ex.Message.Contains("Failed to fetch") || ex is HttpRequestException)
            {
                try
                {
                    var fallbackResponse = await _httpClient.PutAsJsonAsync($"{_fallbackUrl}players/{id}", playerDto);
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        SwitchBaseUrl();
                        return true;
                    }
                    var errorText = await fallbackResponse.Content.ReadAsStringAsync();
                    throw new Exception($"Error al actualizar ({fallbackResponse.StatusCode}): {errorText}");
                }
                catch (Exception finalEx)
                {
                    throw new Exception($"Error de conexión con el Backend: {finalEx.Message}");
                }
            }
        }

        public async Task<bool> DeletePlayerAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}players/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) when (ex.Message.Contains("Failed to fetch") || ex is HttpRequestException)
            {
                try
                {
                    var fallbackResponse = await _httpClient.DeleteAsync($"{_fallbackUrl}players/{id}");
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        SwitchBaseUrl();
                        return true;
                    }
                }
                catch { }
                return false;
            }
        }

        #endregion

        #region Métodos de Partidos (Matches / Calendario)

        public async Task<IEnumerable<MatchDto>> GetMatchesAsync(string? statusFilter = null, int? teamIdFilter = null)
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(statusFilter) && !statusFilter.Equals("Todos", StringComparison.OrdinalIgnoreCase))
            {
                queryParams.Add($"status={Uri.EscapeDataString(statusFilter.Trim())}");
            }
            if (teamIdFilter.HasValue && teamIdFilter.Value > 0)
            {
                queryParams.Add($"teamId={teamIdFilter.Value}");
            }

            var query = queryParams.Count > 0 ? $"matches?{string.Join("&", queryParams)}" : "matches";

            try
            {
                var result = await _httpClient.GetFromJsonAsync<IEnumerable<MatchDto>>($"{_baseUrl}{query}");
                return result ?? new List<MatchDto>();
            }
            catch
            {
                try
                {
                    var fallbackResult = await _httpClient.GetFromJsonAsync<IEnumerable<MatchDto>>($"{_fallbackUrl}{query}");
                    if (fallbackResult != null)
                    {
                        SwitchBaseUrl();
                        return fallbackResult;
                    }
                }
                catch { }
                return new List<MatchDto>();
            }
        }

        public async Task<MatchDto?> GetMatchByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<MatchDto>($"{_baseUrl}matches/{id}");
            }
            catch
            {
                try
                {
                    var fallback = await _httpClient.GetFromJsonAsync<MatchDto>($"{_fallbackUrl}matches/{id}");
                    if (fallback != null)
                    {
                        SwitchBaseUrl();
                        return fallback;
                    }
                }
                catch { }
                return null;
            }
        }

        public async Task<MatchDto?> CreateMatchAsync(MatchDto matchDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}matches", matchDto);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<MatchDto>();
                }

                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"El servidor respondió ({response.StatusCode}): {errorText}");
            }
            catch (Exception ex) when (ex.Message.Contains("Failed to fetch") || ex is HttpRequestException)
            {
                try
                {
                    var fallbackResponse = await _httpClient.PostAsJsonAsync($"{_fallbackUrl}matches", matchDto);
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        SwitchBaseUrl();
                        return await fallbackResponse.Content.ReadFromJsonAsync<MatchDto>();
                    }

                    var fallbackError = await fallbackResponse.Content.ReadAsStringAsync();
                    throw new Exception($"El servidor respondió ({fallbackResponse.StatusCode}): {fallbackError}");
                }
                catch (Exception finalEx)
                {
                    throw new Exception($"No se pudo conectar con el Backend de la API: {finalEx.Message}");
                }
            }
        }

        public async Task<bool> UpdateMatchAsync(int id, MatchDto matchDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}matches/{id}", matchDto);
                if (response.IsSuccessStatusCode) return true;

                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al actualizar ({response.StatusCode}): {errorText}");
            }
            catch (Exception ex) when (ex.Message.Contains("Failed to fetch") || ex is HttpRequestException)
            {
                try
                {
                    var fallbackResponse = await _httpClient.PutAsJsonAsync($"{_fallbackUrl}matches/{id}", matchDto);
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        SwitchBaseUrl();
                        return true;
                    }
                    var errorText = await fallbackResponse.Content.ReadAsStringAsync();
                    throw new Exception($"Error al actualizar ({fallbackResponse.StatusCode}): {errorText}");
                }
                catch (Exception finalEx)
                {
                    throw new Exception($"Error de conexión con el Backend: {finalEx.Message}");
                }
            }
        }

        public async Task<bool> DeleteMatchAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_baseUrl}matches/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) when (ex.Message.Contains("Failed to fetch") || ex is HttpRequestException)
            {
                try
                {
                    var fallbackResponse = await _httpClient.DeleteAsync($"{_fallbackUrl}matches/{id}");
                    if (fallbackResponse.IsSuccessStatusCode)
                    {
                        SwitchBaseUrl();
                        return true;
                    }
                }
                catch { }
                return false;
            }
        }

        #endregion
    }
}