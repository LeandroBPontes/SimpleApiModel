using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Product{
        [key]
        public int Id {get; set;}

        [required(ErrorMessage="Este campo é obragatório")]
        [MaxLenght(60, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        [MinLenght(3, ErrorMessage="Este campo deve conter entre 3 e 60 caracteres")]
        public string Title {get; set;}

        [MaxLenght(1024, ErrorMessage="Este campo deve conter no máximo 1024 caracteres")]
        public string Description {get; set;}

        [required(ErrorMessage="Este campo é obragatório")]
        [range(1,int.MaxValue, ErrorMessage="O preço deve ser maior que zero")]
        public decimal Price {get; set;}

        [required(ErrorMessage="Este campo é obragatório")]
        [range(1,int.MaxValue, ErrorMessage="Categoria inválida")]
        public int CategoryId {get; set;}
        public Category Category {get; set;}
    }
}