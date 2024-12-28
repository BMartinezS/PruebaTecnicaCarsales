using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PruebaTecnicaCarsales.Core.Interfaces;
using PruebaTecnicaCarsales.Core.Models;
using System.Net.Http.Json;

namespace PruebaTecnicaCarsales.Infrastructure.Services
{
    /// <summary>
    /// Servicio para interactuar con la API de Rick and Morty.
    /// Proporciona métodos para obtener información sobre episodios y personajes.
    /// </summary>
    public class RickAndMortyService : IRickAndMortyService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RickAndMortyService> _logger;
        private readonly string _baseUrl;

        /// <summary>
        /// Inicializa una nueva instancia del servicio RickAndMorty.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para realizar las peticiones a la API.</param>
        /// <param name="configuration">Configuración de la aplicación.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        /// <exception cref="ArgumentNullException">Se lanza cuando httpClient, configuration o la URL base son nulos.</exception>
        public RickAndMortyService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<RickAndMortyService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _baseUrl = configuration["RickAndMortyApi:BaseUrl"]
                ?? throw new ArgumentNullException("RickAndMortyApi:BaseUrl no está configurado");
        }

        /// <summary>
        /// Obtiene una lista paginada de episodios.
        /// </summary>
        /// <param name="page">Número de página a consultar.</param>
        /// <returns>Respuesta que contiene la lista de episodios y información de paginación.</returns>
        /// <exception cref="HttpRequestException">Se lanza cuando hay un error en la petición HTTP.</exception>
        /// <exception cref="ArgumentException">Se lanza cuando el número de página es menor a 1.</exception>
        public async Task<EpisodeResponse> GetEpisodesAsync(int page)
        {
            if (page < 1)
                throw new ArgumentException("El número de página debe ser mayor a 0", nameof(page));

            try
            {
                _logger.LogInformation("Obteniendo episodios de la página {Page}", page);
                var response = await _httpClient.GetAsync($"{_baseUrl}/episode?page={page}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<EpisodeResponse>();
                return result ?? throw new InvalidOperationException("No se pudo deserializar la respuesta");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener los episodios de la página {Page}", page);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un episodio específico por su ID.
        /// </summary>
        /// <param name="id">ID del episodio a consultar.</param>
        /// <returns>Información detallada del episodio.</returns>
        /// <exception cref="HttpRequestException">Se lanza cuando hay un error en la petición HTTP.</exception>
        /// <exception cref="ArgumentException">Se lanza cuando el ID es menor a 1.</exception>
        public async Task<Episode> GetEpisodeByIdAsync(int id)
        {
            if (id < 1)
                throw new ArgumentException("El ID del episodio debe ser mayor a 0", nameof(id));

            try
            {
                _logger.LogInformation("Obteniendo episodio con ID {Id}", id);
                var response = await _httpClient.GetAsync($"{_baseUrl}/episode/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Episode>();
                return result ?? throw new InvalidOperationException("No se pudo deserializar la respuesta");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener el episodio con ID {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Obtiene un personaje específico por su ID.
        /// </summary>
        /// <param name="id">ID del personaje a consultar.</param>
        /// <returns>Información detallada del personaje.</returns>
        /// <exception cref="HttpRequestException">Se lanza cuando hay un error en la petición HTTP.</exception>
        /// <exception cref="ArgumentException">Se lanza cuando el ID es menor a 1.</exception>
        public async Task<Character> GetCharacterByIdAsync(int id)
        {
            if (id < 1)
                throw new ArgumentException("El ID del personaje debe ser mayor a 0", nameof(id));

            try
            {
                _logger.LogInformation("Obteniendo personaje con ID {Id}", id);
                var response = await _httpClient.GetAsync($"{_baseUrl}/character/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<Character>();
                return result ?? throw new InvalidOperationException("No se pudo deserializar la respuesta");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener el personaje con ID {Id}", id);
                throw;
            }
        }

        /// <summary>
        /// Obtiene múltiples personajes por sus IDs de forma paralela.
        /// </summary>
        /// <param name="ids">Lista de IDs de los personajes a consultar.</param>
        /// <returns>Lista de personajes encontrados.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando la lista de IDs está vacía o contiene IDs inválidos.</exception>
        /// <exception cref="AggregateException">Se lanza cuando hay errores en múltiples peticiones.</exception>
        public async Task<List<Character>> GetCharactersByIdsAsync(List<int> ids)
        {
            if (ids == null || !ids.Any())
                throw new ArgumentException("La lista de IDs no puede estar vacía", nameof(ids));

            if (ids.Any(id => id < 1))
                throw new ArgumentException("Todos los IDs deben ser mayores a 0", nameof(ids));

            try
            {
                _logger.LogInformation("Obteniendo {Count} personajes", ids.Count);

                // Crear las tareas para todas las peticiones en paralelo
                var tasks = ids.Select(id => GetCharacterByIdAsync(id));

                // Ejecutar todas las peticiones en paralelo
                var characters = await Task.WhenAll(tasks);
                return characters.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener múltiples personajes");
                throw;
            }
        }
    }
}