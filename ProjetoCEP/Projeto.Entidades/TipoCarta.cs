using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Entidades
{
    public class TipoCarta
    {
        public int IdTipoCarta { get; set; }
        public string Modelo { get; set; }
        public string Carta { get; set; }

        public List<LimitesControle> LimitesControle { get; set; }
    }
}
