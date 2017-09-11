using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projeto.Entidades;
using System.Web.Mvc;
using Projeto.DAL.Persistencias;
using System.ComponentModel.DataAnnotations;
using Projeto.WEB.Validators;

namespace Projeto.WEB.Areas.AreaRestrita.Models.CEP
{
    public class CalcularLimitesViewModel
    {
        public int idLote { get; set; }

        [Required(ErrorMessage = "Informe o Lote")]
        [Display(Name = "Lote")]
        public int Lote { get; set; }

        [DateValidator(ErrorMessage = "A data de Análise não pode ser maior que a data atual")]
        [Required(ErrorMessage = "Informe a data da análise")]
        [Display(Name = "Data Análise")]
        public DateTime DataAnalise { get; set; }

        [Required(ErrorMessage = "Informe o Operador da Análise")]
        [Display(Name = "Operador Análise")]
        public int IdOperadorAnalise { get; set; }

        public string OperadorAnaliseNome { get; set; }

        [Required(ErrorMessage = "Informe o Total de Lentes do Lote")]
        [Display(Name = "Total Lentes")]
        public int TotaLentes { get; set; }

        [Required(ErrorMessage = "Informe a qtd de defeitos")]
        [Display(Name = "Qtd Não conforme")]
        public int QtdNaoConforme { get; set; }

        public double Percentual { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        public string Status { get; set; }
        public double LSC { get; set; }
        public double LC { get; set; }
        public double LIC { get; set; }

        public List<SelectListItem> ListagemOperador
        {
            get
            {
                var lista = new List<SelectListItem>();

                var d = new OperadorDAL();
                foreach (var a in d.ListaOperador())
                {
                    var item = new SelectListItem();
                    item.Value = a.IdOperador.ToString();
                    item.Text = a.Nome.ToString();

                    lista.Add(item);
                }
                return lista;
            }
        }

        public string DataAnaliseGrafico { get; set; }
        public DateTime DataCalculo { get; set; }
    }
}