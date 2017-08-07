using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projeto.Entidades;

namespace Projeto.WEB.Areas.AreaRestrita.Models.CEP
{
    public class ViewModelCalcularLimites
    {
        public int idLote { get; set; }
        public string Lote { get; set; }
        public DateTime DataCriacao { get; set; }
        public int IdOperadorCriacao { get; set; }
        public DateTime DataAnalise { get; set; }
        public int IdOperadorAnalise { get; set; }
        public int TotaLentes { get; set; }
        public int QtdNaoConforme { get; set; }

        
    }
}