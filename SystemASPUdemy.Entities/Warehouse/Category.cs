using System.ComponentModel.DataAnnotations;

namespace SystemASPUdemy.Entities.Warehouse
{
    public class Category
    {
        public int id { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 3,ErrorMessage = "El campo {0} no debe contener menos de {2} ni mas de {1} caracteres")]
        public string Name { get; set; }
        [StringLength(256)]
        public string Description { get; set; }
        public bool Status { get; set; }

    }
}
