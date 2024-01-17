using adaptatechwebapibackend.DTOs;
using adaptatechwebapibackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace adaptatechwebapibackend.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    public class TemasForoController : ControllerBase
        {

        /**
         * 
         * Clase controladora sobre el modelo TEMA_FORO.
         * 
         * Siempre que se refiera en este documento a TEMA_FORO, 
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

        public TemasForoController(AdaptatechContext context)
            {
            _context = context;
            }

        /**
         * 
         * Método GetTemasForo()
         * 
         * Devuelve todos los temas del foro en la base de datos.
         * 
         * */

        [HttpGet]
        public async Task<List<TemasForo>> GetTemasForo()
            {

            var lista = await _context.TemasForos.ToListAsync();

            return lista;

            }

        /**
         * 
         * Método GetTemasForo()
         * 
         * Devuelve todos los temas del foro en la base de datos con sus correspondientes mensajes.
         * 
         * */

        [HttpGet]
        public async Task<List<TemasForo>> GetTemasForoInclude()
            {

            var lista = await _context.TemasForos.Include(x => x.MensajeForos).ToListAsync();

            return lista;

            }

        /**
         * 
         * Método GetTemasForoSincrono()
         * 
         * Devuelve todos los perfiles de usuario de forma síncrona.
         * 
         * */

        [HttpGet("sincrono")]
        public List<TemasForo> GetTemasForoSincrono()
            {
            // Las operaciones contra una base de datos DEBEN SER SIEMPRE ASÍNCRONAS. Para liberar los hilos de ejecución en cada petición, eso no debe hacerse nunca
            var lista = _context.TemasForos.ToList();
            return lista;
            }

        /**
        * 
        * Método GetTemasForoOrdenadosAsc()
        * 
        * Devuelve todos los temas del foro ordenados ascendentemente por fecha de creación.
        * 
        * */

        [HttpGet("ordenadosfechaascendente")]
        public async Task<List<TemasForo>> GetTemasForoOrdenadosAsc()
            {
            var listaOrdenadaAscendente = await _context.TemasForos.OrderBy(x => x.FechaCreacion).ToListAsync();
            return listaOrdenadaAscendente;
            }

        /**
         * 
         * Método GetTemasForoOrdenadosDesc()
         * 
         * Devuelve todos los temas del foro ordenados descendentemente por fecha de creación.
         * 
         * */

        [HttpGet("ordenadosfechadescendente")]
        public async Task<List<TemasForo>> GetTemasForoOrdenadosDesc()
            {
            var listaOrdenadaDescendente = await _context.TemasForos.OrderByDescending(x => x.FechaCreacion).ToListAsync();
            return listaOrdenadaDescendente;
            }

        /**
         * 
         * Método GetTemaForoPorId([FromRoute] int id)
         * 
         * Devuelve un tema del foro correspondiente a una id de TEMAS_FORO.
         * 
         * */

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PerfilUsuario>> GetTemaForoPorId([FromRoute] int id)
            {
            var tema = await _context.TemasForos.FindAsync(id);

            if (tema == null)
                {
                return NotFound("El tema con " + id + " no existe.");
                }
            return Ok(tema);
            }


        /**
         * 
         * Método GetTemasForoMensajesSelect()
         * 
         * Devuelve todos los temas del foro con sus correspondientes mensajes.
         * 
         * Queda añadir el MensajesController
         * 
         * 
         * */


        [HttpGet("temasmensajes/{id:int}")]
        public async Task<ActionResult<List<DTOTemasMensajes>>> GetTemasForoMensajesSelect()
            {

            var temas = await (from x in _context.TemasForos.Include(y => y.MensajeForos)
                               select new DTOTemasMensajes
                                   {
                                   IdTemaDTO = x.IdTema,
                                   TituloDTO = x.Titulo!,
                                   MensajesDTO = x.MensajeForos.Select(y => new DTOMensajesItem
                                       {

                                       }).ToList(),

                                   }).ToListAsync();

            if (temas.Count() == 0)
                {
                return NotFound("No hay datos de temas de foros");
                }

            return Ok(temas);
            }





        /**
         * 
         * Método PostPerfilUsuario([FromRoute] int id, DTOPerfilUsuarioPost perfil)
         * 
         * Añade un tema a la tabla TEMA_FORO.
         *
         * 
         * */

        [HttpPost]
        public async Task<ActionResult> PostTemaForo([FromBody] DTOTemasForoPost tema)
            {
            var newTema = new TemasForo()
                {

                Titulo = tema.TituloDTO,
                IdTemaUsuario = tema.IdTemaUsuarioDTO,
                FechaCreacion = tema.FechaCreacionDTO,


                };
            await _context.AddAsync(newTema);
            await _context.SaveChangesAsync();

            return Created("PerfilUsuario", new { tema = newTema });
            }

        /**
         * 
         * Método PutTemaForo([FromRoute] int id, DTOPerfilUsuarioPut perfil)
         * 
         * Actualiza la información o módifica la información de un registro de PERFIL_USUARIO en la base de datos.
         * 
         * */

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutPerfilUsuario([FromRoute] int id, DTOTemasForoPut tema)
            {
            if (id != tema.IdTemaDTO)
                {
                return BadRequest("Los ids proporcionados son diferentes");
                }
            var perfilUpdate = await _context.TemasForos.AsTracking().FirstOrDefaultAsync(x => x.IdTema == id);
            if (perfilUpdate == null)
                {
                return NotFound();
                }



            _context.Update(perfilUpdate);

            await _context.SaveChangesAsync();
            return NoContent();
            }

        /**
         * 
         * Método DeleteTemaForoId(int id)
         * 
         * Elimina un perfil de usuario correspondiente a una id de PERFIL_USUARIO.
         * 
         * */

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTemaForoId([FromRoute] int id)
            {

            var tema = await _context.TemasForos.FindAsync(id);
            if (tema is null)
                {
                return NotFound($"El tema del foro con id: ${id} no existe.");
                }

            _context.Remove(tema);
            await _context.SaveChangesAsync();
            return Ok();

            }

        /**
         * 
         * Método DeleteTemaForoSqlId(int id)
         * 
         * Elimina un perfil de usuario correspondiente a una id de PERFIL_USUARIO por SQL.
         * 
         * */

        [HttpDelete("deleteSQL/{id:int}")]
        public async Task<ActionResult<TemasForo>> DeleteTemaForoSqlId([FromRoute] int id)
            {

            var tema = await _context.TemasForos
                        .FromSqlInterpolated($"SELECT * FROM TemasForo WHERE IdTema = {id}")
                        .FirstOrDefaultAsync();
            if (tema is null) return NotFound("No existe ese tema.");


            var tieneMensajes = await _context.MensajeForos
                        .FromSqlInterpolated($"SELECT * FROM MensajeForo WHERE IdTema = {id}")
                        .AnyAsync();
            if (tieneMensajes) return BadRequest("Hay mensajes en ese tema del foro.");


            await _context.Database.ExecuteSqlInterpolatedAsync($@"DELETE FROM TemasForo WHERE IdTema = {id}");
            return Ok();
            }





        /**
         * 
         * Método GetTemasMensajesSelect([FromRoute] int id)
         * 
         * Devuelve un tema del foro con sus mensajes seleccionando información.
         * 
         * EN DESARROLLO
         * 
         * 
         * */

        //Método en desarrollo

        //[HttpGet("temasymensajes/{id:int}")]
        //public async Task<ActionResult<TemasForo>> GetTemasMensajesSelect(int id)
        //    {

        //    var casa = await (from x in _context.TemasForos
        //                      select new DTOCasasMonstruo
        //                          {
        //                          IdCasa = x.IdCasa,
        //                          NombreCasaMonstruo = x.NombreCasa,
        //                          ListaMonstruos = x.Monstruos.Select(y => new DTOMonstruoItem
        //                              {
        //                              IdMonstruoItem = y.IdMonstruo,
        //                              NombreMonstruo = y.NombreMonstruo,
        //                              Comportamiento = y.ComportamientoMonstruo,
        //                              }).ToList(),
        //                          }).FirstOrDefaultAsync(x => x.IdCasa == id);

        //    if (casa == null)
        //        {
        //        return NotFound("No hay casa de mosntruos con esa ID.");
        //        }
        //    return Ok(casa);
        //    }





        }
    }
