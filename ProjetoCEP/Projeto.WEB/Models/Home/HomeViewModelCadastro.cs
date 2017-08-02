using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Projeto.WEB.Models.Home
{
    public class HomeViewModelCadastro
    {
        [RegularExpression("^[A-Za-zÀ-Üà-ü\\s]{4,50}$",ErrorMessage = "Nome inválido")]
        [Required(ErrorMessage = "Por favor, informe o nome do usuário.")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [RegularExpression("^[a-z0-9]{4,50}$", ErrorMessage = "Login inválido")]
        [Required(ErrorMessage = "Por favor, informe o Login do usuário.")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [RegularExpression("^[a-z0-9@&]{4,50}$", ErrorMessage = "Senha inválida")]
        [Required(ErrorMessage = "Por favor, informe a Senha do usuário.")]
        [Display(Name = "Senha")]
        public string Senha { get; set; }

        [Compare("Senha",ErrorMessage = "Senhas não conferem")]
        [Required(ErrorMessage = "Por favor, confirme sua senha")]
        [Display(Name = "Confirmar senha")]
        public string SenhaConfirm { get; set; }
    }
}