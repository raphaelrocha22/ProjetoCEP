using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projeto.WEB.Areas.AreaRestrita.Models.CEP
{
    public class CadastrarLimitesViewModel
    {
        public DateTime DataCalculo { get; set; }
        public double LSC { get; set; }
        public double LC { get; set; }
        public double LIC { get; set; }
    }
}