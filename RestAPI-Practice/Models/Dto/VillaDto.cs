using System.ComponentModel.DataAnnotations;

namespace RestAPI_Practice.Models.Dto
{
    public class VillaDto
    {
        //la clase que usamos sin mostrar la fecha de creación
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Nombre { get; set; }
    }
}

