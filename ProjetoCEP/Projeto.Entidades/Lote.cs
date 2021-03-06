﻿using System;
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
        public double Percentual { get; set; }
        public string Status { get; set; }
        public LimitesControle Limites { get; set; }
        public string TipoLote { get; set; }
        public string Observacao { get; set; }
    }
}

