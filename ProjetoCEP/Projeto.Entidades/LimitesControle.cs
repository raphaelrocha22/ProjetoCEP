using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Entidades
{
    public class LimitesControle
    {
        public int IdLimites { get; set; }
        public double LSC { get; set; }
        public double LC { get; set; }
        public double LIC { get; set; }
        public DateTime DataCalculo { get; set; }
        public bool Status { get; set; }
        public TipoCarta TipoCarta { get; set; }

        public List<Lote> Lotes { get; set; }
    }
}
