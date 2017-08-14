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

            string query = "Insert into Amostras (Lote,DataCriacao,IdOperadorCriacao,DataAnalise,IdOperadorAnalise,TotalLentes,QtdNaoConforme,Percentual,Observacao) " +
                "values (@Lote,@DataCriacao,@IdOperadorCriacao,@DataAnalise,@IdOperadorAnalise,@TotalLentes,@QtdNaoConforme,@Percentual,@Observacao)";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Lote", l.NumeroLote);
            cmd.Parameters.AddWithValue("@DataCriacao", l.DataCriacao);
            cmd.Parameters.AddWithValue("@IdOperadorCriacao", l.OperadorCriacao.IdOperador);
            cmd.Parameters.AddWithValue("@DataAnalise", l.DataAnalise);
            cmd.Parameters.AddWithValue("@IdOperadorAnalise", l.OperadorAnalise.IdOperador);
            cmd.Parameters.AddWithValue("@TotalLentes", l.TotalLentes);
            cmd.Parameters.AddWithValue("@QtdNaoConforme", l.QtdNaoConforme);
            cmd.Parameters.AddWithValue("@Percentual", l.Percentual);
            cmd.Parameters.AddWithValue("@Observacao", l.Observacao);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }

        public List<Lote> ObterAmostras()
        {
            AbrirConexao();

            string query = "select am.*,cr.Nome as 'Operador Criacao',an.Nome as 'Operador Analise' from Amostras am " +
                            "inner join Operador cr on am.IdOperadorCriacao = cr.IdOperador " +
                            "inner join Operador an on am.IdOperadorAnalise = an.IdOperador";

            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();

            var lista = new List<Lote>();

            while (dr.Read())
            {
                var l = new Lote();
                l.OperadorCriacao = new Operador();
                l.OperadorAnalise = new Operador();

                l.IdLote = (int)dr["IdLote"];
                l.NumeroLote = (int)dr["Lote"];
                l.DataCriacao = (DateTime)dr["DataCriacao"];
                l.OperadorCriacao.IdOperador = (int)dr["IdOperadorCriacao"];
                l.OperadorCriacao.Nome = (string)dr["Operador Criacao"];
                l.DataAnalise = (DateTime)dr["DataAnalise"];
                l.OperadorAnalise.IdOperador = (int)dr["IdOperadorAnalise"];
                l.OperadorAnalise.Nome = (string)dr["Operador Analise"];
                l.TotalLentes = (int)dr["TotalLentes"];
                l.QtdNaoConforme = (int)dr["QtdNaoConforme"];
                l.Percentual = (decimal)dr["Percentual"];
                
                lista.Add(l);
            }
            FecharConexao();
            return lista;
        }

        public Lote ObterPorId(int id)
        {
            AbrirConexao();

            string query = "select am.*,cr.Nome as 'Operador Criacao',an.Nome as 'Operador Analise' from Amostras am " +
                            "inner join Operador cr on am.IdOperadorCriacao = cr.IdOperador " +
                            "inner join Operador an on am.IdOperadorAnalise = an.IdOperador " +
                            "where am.IdLote = @id";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            dr = cmd.ExecuteReader();

            Lote l = null;

            if (dr.Read())
            {
                l = new Lote();
                l.OperadorCriacao = new Operador();
                l.OperadorAnalise = new Operador();

                l.IdLote = (int)dr["IdLote"];
                l.NumeroLote = (int)dr["Lote"];
                l.DataCriacao = (DateTime)dr["DataCriacao"];
                l.OperadorCriacao.IdOperador = (int)dr["IdOperadorCriacao"];
                l.OperadorCriacao.Nome = (string)dr["Operador Criacao"];
                l.DataAnalise = (DateTime)dr["DataAnalise"];
                l.OperadorAnalise.IdOperador = (int)dr["IdOperadorAnalise"];
                l.OperadorAnalise.Nome = (string)dr["Operador Analise"];
                l.TotalLentes = (int)dr["TotalLentes"];
                l.QtdNaoConforme = (int)dr["QtdNaoConforme"];
                l.Percentual = (decimal)dr["Percentual"];
                l.Observacao = dr["Observacao"].ToString();
            }
            
            FecharConexao();
            return l;
        }

        public void Excluir(int id)
        {
            AbrirConexao();

            string query = "delete from Amostras where IdLote = @id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteReader();

            FecharConexao();
        }
    }
}
