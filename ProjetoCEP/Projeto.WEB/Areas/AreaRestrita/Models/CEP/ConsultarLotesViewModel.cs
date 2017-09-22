using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projeto.WEB.Areas.AreaRestrita.Models.CEP
{
    public class ConsultarLotesViewModel
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        public int IdLote { get; set; }
        public int Lote { get; set; }
        public string DataAnalise { get; set; }
        public string OperadorAnaliseNome { get; set; }
        public int TotalLentes { get; set; }
        public int QtdNaoConforme { get; set; }
        public double Percentual { get; set; }
        public string Status { get; set; }
        public double LSC { get; set; }
        public double LC { get; set; }
        public double LIC { get; set; }
        public string Observacao { get; set; }
    }
}