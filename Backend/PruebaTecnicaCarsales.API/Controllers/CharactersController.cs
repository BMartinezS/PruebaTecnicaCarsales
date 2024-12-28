using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaCarsales.Core.Interfaces;
using PruebaTecnicaCarsales.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PruebaTecnicaCarsales.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los personajes de Rick and Morty.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CharactersController : ControllerBase
    {
        private readonly IRickAndMortyService _rickAndMortyService;
        private readonly ILogger<CharactersController> _logger;

        /// <summary>
        /// Inicializa una nueva instancia del controlador de personajes.
        /// </summary>
        /// <param name="rickAndMortyService">Servicio para interactuar con la API de Rick and Morty.</param>
        /// <param name="logger">Logger para registrar eventos y errores.</param>
        /// <exception cref="ArgumentNullException">Se lanza cuando alguna dependencia es nula.</exception>
        public CharactersController(
            IRickAndMortyService rickAndMortyService,
            ILogger<CharactersController> logger)
        {
            _rickAndMortyService = rickAndMortyService ?? throw new ArgumentNullException(nameof(rickAndMortyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene un personaje por su ID.
        /// </summary>
        /// <param name="id">ID del personaje a buscar.</param>
        /// <returns>Información del personaje solicitado.</returns>
        /// <response code="200">Retorna el personaje solicitado.</response>
        /// <response code="400">Si el ID no es válido.</response>
        /// <response code="404">Si el personaje no fue encontrado.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Character), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Character>> GetCharacter([Range(1, int.MaxValue, ErrorMessage = "El ID debe ser mayor a 0")] int id)
        {
            _logger.LogInformation("Obteniendo personaje con ID {Id}", id);

            try
            {
                var character = await _rickAndMortyService.GetCharacterByIdAsync(id);

                if (character == null)
                {
                    _logger.LogWarning("Personaje con ID {Id} no encontrado", id);
                    return NotFound($"No se encontró el personaje con ID {id}");
                }

                return Ok(character);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning(ex, "Personaje con ID {Id} no encontrado", id);
                return NotFound($"No se encontró el personaje con ID {id}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener el personaje con ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno al procesar la solicitud", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no esperado al obtener el personaje con ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Obtiene múltiples personajes por sus IDs.
        /// </summary>
        /// <param name="ids">Lista de IDs separados por comas (ejemplo: "1,2,3").</param>
        /// <returns>Lista de personajes solicitados.</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        /// GET /api/characters/batch?ids=1,2,3
        /// </remarks>
        /// <response code="200">Retorna la lista de personajes solicitados.</response>
        /// <response code="400">Si el formato de los IDs no es válido.</response>
        /// <response code="500">Si ocurre un error interno del servidor.</response>
        [HttpGet("batch")]
        [ProducesResponseType(typeof(List<Character>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Character>>> GetCharacters(
            [Required(ErrorMessage = "La lista de IDs es requerida")]
            [RegularExpression(@"^\d+(,\d+)*$", ErrorMessage = "Formato inválido. Use números separados por comas")]
            [FromQuery] string ids)
        {
            _logger.LogInformation("Obteniendo personajes con IDs: {Ids}", ids);

            try
            {
                var idList = ids.Split(',')
                    .Select(id => int.Parse(id.Trim()))
                    .ToList();

                if (idList.Any(id => id < 1))
                {
                    return BadRequest("Todos los IDs deben ser mayores a 0");
                }

                if (idList.Count > 20) // Límite arbitrario para evitar sobrecarga
                {
                    return BadRequest("No se pueden solicitar más de 20 personajes a la vez");
                }

                var characters = await _rickAndMortyService.GetCharactersByIdsAsync(idList);
                return Ok(characters);
            }
            catch (FormatException ex)
            {
                _logger.LogWarning(ex, "Formato inválido en la lista de IDs: {Ids}", ids);
                return BadRequest(new { message = "Formato de ID inválido", details = "Los IDs deben ser números enteros separados por comas" });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error al obtener los personajes con IDs: {Ids}", ids);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error al procesar la solicitud", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no esperado al obtener los personajes con IDs: {Ids}", ids);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Error interno del servidor" });
            }
        }
    }
}