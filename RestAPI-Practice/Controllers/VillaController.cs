using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestAPI_Practice.Data;
using RestAPI_Practice.Models;
using RestAPI_Practice.Models.Dto;

namespace RestAPI_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        //Los endpoints no son mas que metodos de esta clase villacontroller

        // Cada endpoint debe tener un verbo http (GET, POST, etc)
        //IEnumerable porque nos va a retornar una lista de tipo de nuestro model villa
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {

            _logger = logger;
            _db = db;

        }

        [HttpGet] // Obtener todas las villas 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas() 
        {
            //ActionResult es la Implementación de la Interfaz IAction Result
            // con la cual podemos usar cualquier tipo de retorno que queramos
            // en este caso una lista IEnumerable de tipo VillaDto

            //retornamos los datos que tenemos almacenados en VillaStore 
            //especificamente en el metodo villaList, También un Ok que es de tipo 200


            //este logger se logea en la consola 
            _logger.LogInformation("Obtener las villas");
            return Ok(_db.Villas.ToList());
        }

        //no puedo tener dos endpoints de tipo http get sin diferenciarlos
        // le agregamos el id entre comillas dobles al httpget
        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id) 
        {
            //aquí ActionResult me devuelve un VillaDto

            if (id == 0)
            {
                _logger.LogError("Error al traer villa con id" + id);
                return BadRequest();
            
            }

            // caso que el id sea 0 retornamos 400 Bad Request

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null) return NotFound();
            // caso que no encuentre una villa retornamos 404 Not Found


            return Ok(villa);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villaDto)
        {
            //validacions con data annotations del estado del modelo
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //validacion personalizada
            if(_db.Villas.FirstOrDefault(v=> v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExistente", "Ya existe una Villa con ese nombre");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            { 
                return BadRequest(villaDto);
            }
           
            if(villaDto.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError); 
            }

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad,
            };

            _db.Villas.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new {id = villaDto.Id}, villaDto);
   
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //en este caso en vez de ActionResult Utilizamos IActionResult porque no necesitamos el modelo
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if(villa == null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateVilla(int id , [FromBody] VillaDto villaDto) 
        {
            if(villaDto== null || id != villaDto.Id)
            {
                return BadRequest();    
            }
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            Villa modelo = new()
            {
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad,
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();

            return NoContent(); 
        }

        [HttpPatch("{id:int}")]
        public IActionResult UpdateVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            //Aqui necesitamos JSONpatch si queremos reemplazar una sola propiedad
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            VillaDto villaDto = new()
            {
                Id = id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad,

            };

            if (villa == null) return BadRequest();
            

            //Validamos modelstate
            patchDto.ApplyTo(villaDto, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            //Ahore este es el modelo que pasamos a la base de datos
            Villa modelo = new()
            {
                Id = id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad,
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
