using adaptatechwebapibackend.DTOs;
using adaptatechwebapibackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilUsuarioController : ControllerBase
        {

        /**
         * 
         * Clase controladora sobre el modelo PERFIL_USUARIO.
         * 
         * Siempre que se refiera en este documento a PERFIL_USUARIO, 
         * me estaré refiriendo a la tabla en la DATABASE.
         * 
         * */


        //Variable objeto _context
        //Instanciamos el contexto de Entity Framework.

        private readonly AdaptatechContext _context;


        /**
         * 
         * Constructor con un parámetro de inyección de servicio SCOPED
         * 
         * */

        public PerfilUsuarioController(AdaptatechContext context)
            {
            _context = context;
            }

        /**
         * 
         * Método GetPerfilesUsuarios()
         * 
         * Devuelve todos los perfiles de usuario en la base de datos.
         * 
         * */

        [HttpGet]
        public async Task<List<PerfilUsuario>> GetPerfilesUsuarios()
            {

            var lista = await _context.PerfilUsuarios.ToListAsync();

            return lista;

            }

        /**
         * 
         * Método GetPerfilesUsuariosSincrono()
         * 
         * Devuelve todos los perfiles de usuario de forma síncrona.
         * 
         * */

        [HttpGet("sincrono")]
        public List<PerfilUsuario> GetPerfilesUsuariosSincrono()
            {
            // Las operaciones contra una base de datos DEBEN SER SIEMPRE ASÍNCRONAS. Para liberar los hilos de ejecución en cada petición, eso no debe hacerse nunca
            var lista = _context.PerfilUsuarios.ToList();
            return lista;
            }

        /**
         * 
         * Método GetPerfilesUsuariosOrdenadosAsc()
         * 
         * Devuelve todos los perfiles de usuario ordenados ascendentemente por fecha de nacimiento.
         * 
         * */

        [HttpGet("ordenadosfechaascendente")]
        public async Task<List<PerfilUsuario>> GetPerfilesUsuariosOrdenadosAsc()
            {
            var listaOrdenadaAscendente = await _context.PerfilUsuarios.OrderBy(x => x.FechaNacimiento).ToListAsync();
            return listaOrdenadaAscendente;
            }

        /**
         * 
         * Método GetPerfilesUsuariosOrdenadosDesc()
         * 
         * Devuelve todos los perfiles de usuario ordenados descendentemente por fecha de nacimiento.
         * 
         * */

        [HttpGet("ordenadosfechadescendente")]
        public async Task<List<PerfilUsuario>> GetPerfilesUsuariosOrdenadosDesc()
            {
            var listaOrdenadaDescendente = await _context.PerfilUsuarios.OrderByDescending(x => x.FechaNacimiento).ToListAsync();
            return listaOrdenadaDescendente;
            }

        /**
         * 
         * Método GetPerfilUsuarioPorId([FromRoute] int id)
         * 
         * Devuelve un perfil de usuario correspondiente a una id de PERFIL_USUARIO.
         * 
         * */

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PerfilUsuario>> GetPerfilUsuarioPorId([FromRoute] int id)
            {
            var perfil = await _context.PerfilUsuarios.FindAsync(id);

            if (perfil == null)
                {
                return NotFound("El perfil con " + id + " no existe.");
                }
            return Ok(perfil);
            }

        /**
         * 
         * Método GetPerfilUsuarioPorNombre([FromRoute] string nombre)
         * 
         * Devuelve un perfil de usuario correspondiente a un nombre de PERFIL_USUARIO.
         * 
         * */

        [HttpGet("pornombre/{nombre}")]
        public async Task<ActionResult<PerfilUsuario>> GetPerfilUsuarioPorNombre([FromRoute] string nombre)
            {
            var perfil = await _context.PerfilUsuarios.Where(x => x.Nombre.ToLower().Equals(nombre.ToLower())).ToListAsync();

            if (perfil == null)
                {
                return NotFound("El perfil con nombre: " + nombre + " no existe.");
                }
            return Ok(perfil);
            }

        /**
         * 
         * Método GetPerfilUsuarioPorApellidos([FromRoute] string nombre)
         * 
         * Devuelve un perfil de usuario correspondiente a un nombre de PERFIL_USUARIO.
         * 
         * */

        [HttpGet("porapellidos/{apellidos}")]
        public async Task<ActionResult<PerfilUsuario>> GetPerfilUsuarioPorApellidos([FromRoute] string apellidos)
            {
            var perfil = await _context.PerfilUsuarios.Where(x => x.Apellidos.ToLower().Equals(apellidos.ToLower())).ToListAsync();

            if (perfil == null)
                {
                return NotFound("El perfil con apellidos: " + apellidos + " no existe.");
                }
            return Ok(perfil);
            }

        /**
         * 
         * Método PostPerfilUsuario([FromRoute] int id, DTOPerfilUsuarioPost perfil)
         * 
         * Añade un PERFIL_USUARIO.
         *
         * 
         * */

        [HttpPost]
        public async Task<ActionResult> PostPerfilUsuario([FromBody] DTOPerfilUsuarioPost perfil)
            {
            var newPerfil = new PerfilUsuario()
                {

                Nombre = perfil.NombreDTO,
                Apellidos = perfil.ApellidosDTO,
                Telefono = perfil.TelefonoDTO,
                FechaNacimiento = perfil.FechaNacimientoDTO,
                Avatar = perfil.AvatarDTO,
                Alias = perfil.AliasDTO,

                };
            await _context.AddAsync(newPerfil);
            await _context.SaveChangesAsync();

            return Created("PerfilUsuario", new { perfil = newPerfil });
            }

        /**
         * 
         * Método PutPerfilUsuario([FromRoute] int id, DTOPerfilUsuarioPut perfil)
         * 
         * Actualiza la información o módifica la información de un registro de PERFIL_USUARIO en la base de datos.
         * 
         * */

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutPerfilUsuario([FromRoute] int id, DTOPerfilUsuarioPut perfil)
            {
            if (id != perfil.IdPerfilDTO)
                {
                return BadRequest("Los ids proporcionados son diferentes");
                }
            var perfilUpdate = await _context.PerfilUsuarios.AsTracking().FirstOrDefaultAsync(x => x.IdPerfil == id);
            if (perfilUpdate == null)
                {
                return NotFound();
                }
            perfilUpdate.Nombre = perfil.NombreDTO;
            perfilUpdate.Apellidos = perfil.ApellidosDTO;
            perfilUpdate.FechaNacimiento = perfil.FechaNacimientoDTO;
            perfilUpdate.Avatar = perfil.AvatarDTO;
            perfilUpdate.Alias = perfil.AliasDTO;


            _context.Update(perfilUpdate);

            await _context.SaveChangesAsync();
            return NoContent();
            }


        /**
         * 
         * Método DeletePerfilUsuarioId(int id)
         * 
         * Elimina un perfil de usuario correspondiente a una id de PERFIL_USUARIO.
         * 
         * */

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeletePerfilUsuarioId(int id)
            {

            var perfil = await _context.PerfilUsuarios.FindAsync(id);
            if (perfil is null)
                {
                return NotFound("El perfil de usuario no existe.");
                }

            _context.Remove(perfil);
            await _context.SaveChangesAsync();
            return Ok();

            }


        }
    }

