using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CentaurosBackAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using CentaurosData.DTOS;
using CentaurosData.Response;
using Microsoft.AspNetCore.Authorization;

namespace CentaurosBackAPI.Controllers
{
    [EnableCors("ReglasCors")] //Habilita CORS establecidos en program

    [Route("api/[controller]")]
    [Authorize] //Restrige el acceso a los endpoints de este controlador,espera token JWT
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly CentaurosContext _context;

        public EstudianteController(CentaurosContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Listar()
        {
            Respuesta<List<Estudiante>> respuesta = new Respuesta<List<Estudiante>>();

            try
            {
                var lst = await _context.Estudiantes.OrderByDescending(e => e.Nombres).ToListAsync();

                respuesta.Exito = 1;
                respuesta.Mensaje = "Lista de estudiantes recuperada exitosamente";
                respuesta.Data = lst;

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = $"Error al recuperar la lista de estudiantes: {ex.Message}";
                return StatusCode(500, respuesta);
            }
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Buscar(string Id)
        {
            Respuesta<List<Estudiante>> respuesta = new Respuesta<List<Estudiante>>(); // Cambia el tipo de respuesta

            try
            {
                var lst = await _context.Estudiantes.Where(e => e.Cedula == Id).ToListAsync(); 
                if (lst == null || lst.Count == 0)
                {
                    respuesta.Mensaje = "No se encontró un estudiante con ese número de cédula.";
                    return NotFound(respuesta);
                }

                respuesta.Exito = 1;
                respuesta.Mensaje = "Estudiante recuperado exitosamente"; 
                respuesta.Data = lst;

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = $"Error al buscar el estudiante: {ex.Message}";
                return StatusCode(500, respuesta);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Insertar(EstudianteDTO model)
        {
            Respuesta<object> respuesta = new Respuesta<object>();

            try
            {
                // Valida los datos de entrada
                if (!ModelState.IsValid)
                {
                    respuesta.Mensaje = "Datos de estudiante no válidos";
                    return BadRequest(respuesta);
                }

                Estudiante estudiante = new Estudiante();
                estudiante.Cedula = model.Cedula;
                estudiante.Nombres = model.Nombres;
                estudiante.Apellidos = model.Apellidos;
                estudiante.Correo = model.Correo;

                await _context.Estudiantes.AddAsync(estudiante);
                await _context.SaveChangesAsync();

                respuesta.Exito = 1;
                respuesta.Mensaje = "Estudiante agregado exitosamente";

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = $"Error al agregar el estudiante: {ex.Message}";
                return StatusCode(500, respuesta);
            }
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Editar(EstudianteDTO model)
        {
            Respuesta<object> respuesta = new Respuesta<object>();

            try
            {
                // Valida que el estudiante existe en la base de datos
                Estudiante estudiante = await _context.Estudiantes.FindAsync(model.Cedula);
                if (estudiante == null)
                {
                    respuesta.Mensaje = "Estudiante no encontrado";
                    return NotFound(respuesta);
                }

                // Actualiza los datos del estudiante
                estudiante.Nombres = model.Nombres;
                estudiante.Apellidos = model.Apellidos;
                estudiante.Correo = model.Correo;

                // Marca el estudiante como modificado en el contexto
                _context.Entry(estudiante).State = EntityState.Modified;

                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();

                respuesta.Exito = 1;
                respuesta.Mensaje = "Estudiante actualizado exitosamente";
                return Ok(respuesta);
            }
            catch (DbUpdateConcurrencyException)
            {
                // Maneja conflictos de concurrencia
                respuesta.Mensaje = "Error al actualizar el estudiante debido a un conflicto de concurrencia";
                return Conflict(respuesta);
            }
            catch (Exception ex)
            {
                // Maneja otras excepciones
                respuesta.Mensaje = $"Error al actualizar el estudiante: {ex.Message}";
                return StatusCode(500, respuesta);
            }
        }

        [HttpDelete("{Cedula}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Borrar(string Cedula)
        {
            Respuesta<object> respuesta = new Respuesta<object>();

            try
            {
                // Valida si el estudiante existe en la base de datos
                Estudiante estudiante = await _context.Estudiantes.FindAsync(Cedula);
                if (estudiante == null)
                {
                    respuesta.Mensaje = "Estudiante no encontrado";
                    return NotFound(respuesta);
                }

                // Borra el estudiante de la base de datos
                _context.Remove(estudiante);
                await _context.SaveChangesAsync();

                respuesta.Exito = 1;
                respuesta.Mensaje = "Estudiante borrado exitosamente";
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = $"Error al borrar el estudiante: {ex.Message}";
                return StatusCode(500, respuesta);
            }
        }



    }
}
