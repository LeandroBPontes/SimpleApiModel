using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class User{
        [key]
        public int Id {get; set;}

        [required(ErrorMessage="Este campo é obragatório")]
        [MaxLenght(20, ErrorMessage="Este campo deve conter entre 3 e 20 caracteres")]
        [MinLenght(3, ErrorMessage="Este campo deve conter entre 3 e 20 caracteres")]
        
        public string Username {get; set;}

        [required(ErrorMessage="Este campo é obragatório")]
        [MaxLenght(20, ErrorMessage="Este campo deve conter entre 3 e 20 caracteres")]
        [MinLenght(3, ErrorMessage="Este campo deve conter entre 3 e 20 caracteres")]

        public string Password {get; set;}

        //gerente-funcionario-adm
        public string Role {get; set;}

    }
}