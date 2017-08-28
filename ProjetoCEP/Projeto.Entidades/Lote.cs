using Projeto.Entidades.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Entidades
{
    public class Lote
    {
        public int IdLote { get; set; }
        public int NumeroLote { get; set; }
        public DateTime DataAnalise { get; set; }
        public Operador OperadorAnalise { get; set; }
        public int TotalLentes { get; set; }
        public int QtdNaoConforme { get; set; }
        public decimal Percentual { get; set; }
        public Status Status { get; set; }
        public LimitesControle Limites { get; set; }
        public TipoLote TipoLote { get; set; }
        public string Observacao { get; set; }
    }
}

