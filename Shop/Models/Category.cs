using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Category{
        [key]
        public int Id {get; set;}

        [required(ErrorMessage="Este campo é obragatório")]
        [MaxLenght(60, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        [MinLenght(3, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        public string Title {get; set;}
    }
}