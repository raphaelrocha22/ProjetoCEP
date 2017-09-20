using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projeto.WEB.Areas.AreaRestrita.Models.CEP
{
    public class ConsultarLimitesViewModel
    {
        public int IdLimites { get; set; }
        public DateTime DataCalculo { get; set; }
        public double LSC { get; set; }
        public double LC { get; set; }
        public double LIC { get; set; }
        public string Status { get; set; }
        public string Modelo { get; set; }
        public string Carta { get; set; }
    }
}