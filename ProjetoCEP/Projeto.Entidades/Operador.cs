using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Entidades
{
    public class Operador
    {
        public int IdOperador { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Status { get; set; }

        public List<Lote> Lotes { get; set; }
    }
}
