using System.ComponentModel.DataAnnotations;

namespace NetCoreApiBase.Domain.DTO
{
    public class CategoryDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Deve conter no minimo 3 caracteres")]
        public string Title { get; set; }
    }
}
