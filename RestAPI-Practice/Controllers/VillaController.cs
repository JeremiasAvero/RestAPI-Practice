using Microsoft.AspNetCore.Http;
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

        [HttpGet] // Obtener todas las villas 
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas() 
        {
            //ActionResult es la Implementación de la Interfaz IAction Result
            // con la cual podemos usar cualquier tipo de retorno que queramos
            // en este caso una lista IEnumerable de tipo VillaDto

            //retornamos los datos que tenemos almacenados en VillaStore 
            //especificamente en el metodo villaList, También un Ok que es de tipo 200
            return Ok(VillaStore.villaList);
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
           
            if(id == 0) return BadRequest();
            // caso que el id sea 0 retornamos 400 Bad Request

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            
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
            if(VillaStore.villaList.FirstOrDefault(v=> v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
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

            villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDto);

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

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if(villa == null)
            {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);

            return NoContent();
        }
    }
}
