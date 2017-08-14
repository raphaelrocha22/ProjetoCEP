using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Projeto.Entidades;
using System.Web.Mvc;
using Projeto.DAL.Persistencias;
using System.ComponentModel.DataAnnotations;

namespace Projeto.WEB.Areas.AreaRestrita.Models.CEP
{
    public class CalcularLimitesViewModel
    {
        public int idLote { get; set; }

        [Required(ErrorMessage = "Informe o Lote")]
        [Display(Name = "Lote")]
        public int Lote { get; set; }

        [Required(ErrorMessage = "Informe a Data de criação")]
        [Display(Name = "Data Criacao")]
        public DateTime DataCriacao { get; set; }

        [Required(ErrorMessage = "Informe o Operador de Criação")]
        [Display(Name = "Operador Criacao")]
        public int IdOperadorCriacao { get; set; }

        [Required(ErrorMessage = "Informe a data da análise")]
        [Display(Name = "Data Análise")]
        public DateTime DataAnalise { get; set; }

        [Required(ErrorMessage = "Informe o Operador da Análise")]
        [Display(Name = "Operador Análise")]
        public int IdOperadorAnalise { get; set; }

        [Required(ErrorMessage = "Informe o Total de Lentes do Lote")]
        [Display(Name = "Total Lentes")]
        public decimal TotaLentes { get; set; }

        [Required(ErrorMessage = "Informe a qtd de defeitos")]
        [Display(Name = "Qtd Não conforme")]
        public decimal QtdNaoConforme { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }


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

        public string OperadorCriacaoNome { get; set; }
        public string OperadorAnaliseNome { get; set; }
        public decimal Percentual { get; set; }
    }
}