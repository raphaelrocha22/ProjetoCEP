using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Projeto.WEB.Areas.AreaRestrita.Models.Operador
{
    public class OperadorViewModel
    {
        public int IdOperador { get; set; }

        [RegularExpression("^[a-zA-Zà-üÀ-Ü\\s]{4,20}$",ErrorMessage = "Nome inválido, informe apenas letras de 4 a 20 caracteres")]
        [Required(ErrorMessage = "Por favor, informe o nome do operador")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Status { get; set; }
    }
}