using Projeto.DAL.Conexoes;
using Projeto.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.DAL.Persistencias
{
    public class CepDAL:Conexao
    {
        public void CadastrarAmostras(Lote l)
        {
            AbrirConexao();

            string query = "Insert into Amostras (Lote,DataCriacao,IdOperadorCriacao,DataAnalise,IdOperadorAnalise,TotalLentes,QtdNaoConforme,Percentual) " +
                "values (@Lote,@DataCriacao,@IdOperadorCriacao,@DataAnalise,@IdOperadorAnalise,@TotalLentes,@QtdNaoConforme,@Percentual)";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Lote", l.NumeroLote);
            cmd.Parameters.AddWithValue("@DataCriacao", l.DataCriacao);
            cmd.Parameters.AddWithValue("@IdOperadorCriacao", l.IdOperadorCriacao);
            cmd.Parameters.AddWithValue("@DataAnalise", l.DataAnalise);
            cmd.Parameters.AddWithValue("@IdOperadorAnalise", l.IdOperadorAnalise);
            cmd.Parameters.AddWithValue("@TotalLentes", l.TotalLentes);
            cmd.Parameters.AddWithValue("@QtdNaoConforme", l.QtdNaoConforme);
            cmd.Parameters.AddWithValue("@Percentual", l.Percentual);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }

        public List<Lote> ObterAmostras()
        {
            AbrirConexao();

            string query = "select * from Amostras";

            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();

            var lista = new List<Lote>();

            while (dr.Read())
            {
                var l = new Lote();
                l.IdLote = (int)dr["IdLote"];
                l.NumeroLote = (int)dr["Lote"];
                l.DataCriacao = (DateTime)dr["DataCriacao"];
                l.IdOperadorCriacao = (int)dr["IdOperadorCriacao"];
                l.DataAnalise = (DateTime)dr["DataAnalise"];
                l.IdOperadorAnalise = (int)dr["IdOperadorAnalise"];
                l.TotalLentes = (int)dr["TotalLentes"];
                l.QtdNaoConforme = (int)dr["QtdNaoConforme"];
                l.Percentual = (decimal)dr["Percentual"];

                lista.Add(l);
            }
            FecharConexao();
            return lista;
        }
    }
}
