using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaCarsales.Core.Interfaces;
using PruebaTecnicaCarsales.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PruebaTecnicaCarsales.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los episodios de Rick and Morty.
    /// Proporciona endpoints para listar episodios y obtener episodios específicos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EpisodesController : ControllerBase
    {
        private readonly IRickAndMortyService _rickAndMortyService;
        private readonly ILogger<EpisodesController> _logger;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de episodios.
        /// </summary>
        /// <param name="rickAndMortyService">Servicio para interactuar con la API de Rick and Morty.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        /// <exception cref="ArgumentNullException">Se lanza cuando alguna dependencia es nula.</exception>
        public EpisodesController(
            IRickAndMortyService rickAndMortyService,
            ILogger<EpisodesController> logger)
        {
            _rickAndMortyService = rickAndMortyService ?? throw new ArgumentNullException(nameof(rickAndMortyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene una lista paginada de episodios.
        /// </summary>
        /// <param name="page">Número de página a consultar (por defecto: 1).</param>
        /// <returns>Lista paginada de episodios con información de navegación.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// GET /api/episodes?page=1
        /// </remarks>
        /// <response code="200">Retorna la lista de episodios solicitada.</response>
        /// <response code="400">Si el número de página no es válido.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpGet]
        [ProducesResponseType(typeof(EpisodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EpisodeResponse>> GetEpisodes(
            [FromQuery]
            [Range(1, int.MaxValue, ErrorMessage = "El número de página debe ser mayor a 0")]
            int page = 1)
        {
            _logger.LogInformation("Obteniendo episodios de la página {Page}", page);

            try
            {
                var response = await _rickAndMortyService.GetEpisodesAsync(page);

                if (response == null)
                {
                    _logger.LogWarning("No se encontraron episodios en la página {Page}", page);
                    return NotFound($"No se encontraron episodios en la página {page}");
                }

                if (response.Results == null || !response.Results.Any())
                {
                    _logger.LogInformation("La página {Page} no contiene episodios", page);
                }

                return Ok(response);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning(ex, "Página {Page} no encontrada", page);
                return NotFound($"La página {page} no existe");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener los episodios de la página {Page}", page);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al procesar la solicitud", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no esperado al obtener los episodios de la página {Page}", page);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene un episodio específico por su ID.
        /// </summary>
        /// <param name="id">ID del episodio a buscar.</param>
        /// <returns>Información detallada del episodio solicitado.</returns>
        /// <response code="200">Retorna el episodio solicitado.</response>
        /// <response code="400">Si el ID no es válido.</response>
        /// <response code="404">Si el episodio no fue encontrado.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Episode), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Episode>> GetEpisode(
            [Range(1, int.MaxValue, ErrorMessage = "El ID debe ser mayor a 0")]
            int id)
        {
            _logger.LogInformation("Obteniendo episodio con ID {Id}", id);

            try
            {
                var episode = await _rickAndMortyService.GetEpisodeByIdAsync(id);

                if (episode == null)
                {
                    _logger.LogWarning("Episodio con ID {Id} no encontrado", id);
                    return NotFound($"No se encontró el episodio con ID {id}");
                }

                return Ok(episode);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning(ex, "Episodio con ID {Id} no encontrado", id);
                return NotFound($"No se encontró el episodio con ID {id}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener el episodio con ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al procesar la solicitud", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no esperado al obtener el episodio con ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}