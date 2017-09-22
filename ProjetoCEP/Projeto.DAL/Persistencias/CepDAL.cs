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

            string query = "Insert into Amostras (Lote,DataAnalise,IdOperadorAnalise,TotalLentes,QtdNaoConforme,Percentual,Observacao) " +
                "values (@Lote,@DataAnalise,@IdOperadorAnalise,@TotalLentes,@QtdNaoConforme,@Percentual,@Observacao)";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Lote", l.NumeroLote);
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

            string query = "select am.*,an.Nome as 'Operador Analise' from Amostras am " +
                            "inner join Operador an on am.IdOperadorAnalise = an.IdOperador";

            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();

            var lista = new List<Lote>();

            while (dr.Read())
            {
                var l = new Lote();
                l.OperadorAnalise = new Operador();

                l.IdLote = (int)dr["IdLote"];
                l.NumeroLote = (int)dr["Lote"];
                l.DataAnalise = (DateTime)dr["DataAnalise"];
                l.OperadorAnalise.IdOperador = (int)dr["IdOperadorAnalise"];
                l.OperadorAnalise.Nome = (string)dr["Operador Analise"];
                l.TotalLentes = (int)dr["TotalLentes"];
                l.QtdNaoConforme = (int)dr["QtdNaoConforme"];
                l.Percentual = (double)dr["Percentual"];
                l.Observacao = (string)dr["Observacao"].ToString();
                                
                lista.Add(l);
            }
            FecharConexao();
            return lista;
        }

        public Lote ObterPorId(int id)
        {
            AbrirConexao();

            string query = "select am.*,an.Nome as 'Operador Analise' from Amostras am " +
                            "inner join Operador an on am.IdOperadorAnalise = an.IdOperador " +
                            "where am.IdLote = @id";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            dr = cmd.ExecuteReader();

            Lote l = null;

            if (dr.Read())
            {
                l = new Lote();
                l.OperadorAnalise = new Operador();

                l.IdLote = (int)dr["IdLote"];
                l.NumeroLote = (int)dr["Lote"];
                l.DataAnalise = (DateTime)dr["DataAnalise"];
                l.OperadorAnalise.IdOperador = (int)dr["IdOperadorAnalise"];
                l.OperadorAnalise.Nome = (string)dr["Operador Analise"];
                l.TotalLentes = (int)dr["TotalLentes"];
                l.QtdNaoConforme = (int)dr["QtdNaoConforme"];
                l.Percentual = (double)dr["Percentual"];
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

        public void Excluir(List<Lote> lista)
        {
            foreach (var item in lista)
            {
                Excluir(item.IdLote);
            }
        }

        public LimitesControle CalcularLimites(List<Lote> lotes)
        {
            var limites = new LimitesControle();
            limites.TipoCarta = new TipoCarta();
            
            double Di = 0;
            int m = lotes.Count, n = 0;
            
            foreach (var item in lotes)
            {
                Di += item.QtdNaoConforme;
                n = item.TotalLentes;
            }
            double p = Di/(m*n);

            limites.LSC = Math.Round(p + 3 * (Math.Sqrt(((p * (1 - p)) / n))),3);
            limites.LIC = Math.Round(p - 3 * (Math.Sqrt(((p * (1 - p)) / n))),3);
            limites.LC = Math.Round(p,3);
            limites.DataCalculo = DateTime.Now;
            limites.TipoCarta.Modelo = "Atributo";
            limites.TipoCarta.Carta = "Gráfico de Controle para a Fração Não Conforme";
            
            return limites;
        }

        public void CadastrarLimites(LimitesControle limites)
        {
            AbrirConexao();

            cmd = new SqlCommand("update LimitesControle set Status = 0 where Status = 1",con);
            cmd.ExecuteNonQuery();


            string query = "insert into LimitesControle (DataCalculo, LSC, LC, LIC, Status, IdTipoCarta) " +
                           "values ( @DataCalculo, @LSC, @LC, @LIC, @Status, @IdTipoCarta)";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@DataCalculo", limites.DataCalculo);
            cmd.Parameters.AddWithValue("@LSC", limites.LSC);
            cmd.Parameters.AddWithValue("@LC", limites.LC);
            cmd.Parameters.AddWithValue("@LIC", limites.LIC);
            cmd.Parameters.AddWithValue("@Status", limites.Status);
            cmd.Parameters.AddWithValue("@IdTipoCarta", limites.TipoCarta.IdTipoCarta);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }

        public LimitesControle ObterLimiteAtivo()
        {
            AbrirConexao();

            string query = "select l.*, c.Modelo, c.Carta from LimitesControle l " +
                "inner join TipoCarta c on l.IdTipoCarta = c.IdTipoCarta where Status = 1";
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();

            LimitesControle limite = null;

            if (dr.Read())
            {
                limite = new LimitesControle();
                limite.TipoCarta = new TipoCarta();

                limite.IdLimites = (int)dr["IdLimites"];
                limite.DataCalculo = (DateTime)dr["DataCalculo"];
                limite.LSC = Convert.ToDouble(dr["LSC"]);
                limite.LC = Convert.ToDouble(dr["LC"]);
                limite.LIC =Convert.ToDouble(dr["LIC"]);
                limite.Status = (bool)dr["Status"];
                limite.TipoCarta.IdTipoCarta = (int)dr["IdTipoCarta"];
                limite.TipoCarta.Modelo = (string)dr["Modelo"];
                limite.TipoCarta.Carta = (string)dr["Carta"];
            }

            FecharConexao();
            return limite;
        }

        public List<LimitesControle> ObterLimites()
        {
            AbrirConexao();

            string query = "select l.*, c.Modelo, c.Carta from LimitesControle l " +
                "inner join TipoCarta c on l.IdTipoCarta = c.IdTipoCarta";
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();

            var lista = new List<LimitesControle>();

            while (dr.Read())
            {
                var l = new LimitesControle();
                l.TipoCarta = new TipoCarta();

                l.IdLimites = (int)dr["IdLimites"];
                l.DataCalculo = (DateTime)dr["DataCalculo"];
                l.LSC = Convert.ToDouble(dr["LSC"]);
                l.LC = Convert.ToDouble(dr["LC"]);
                l.LIC = Convert.ToDouble(dr["LIC"]);
                l.Status = (bool)dr["Status"];
                l.TipoCarta.Modelo = (string)dr["Modelo"];
                l.TipoCarta.Carta = (string)dr["Carta"];

                lista.Add(l);
            }
            
            FecharConexao();
            return lista;
        }
        
        public void TransferirAmostrasParaLote(List<Lote> lista, LimitesControle limite)
        {
            AbrirConexao();

            foreach (var item in lista)
            {
                string query = "insert into Lote (Lote, DataAnalise, TotalLentes, QtdNaoConforme, Percentual, " +
                    "Resultado, TipoLote, IdLimites, IdOperadorAnalise, Observacao) " +
                    "values (@Lote, @DataAnalise, @TotalLentes, @QtdNaoConforme, @Percentual, " +
                    "@Resultado, @TipoLote, @IdLimites, @IdOperadorAnalise, @Observacao)";

                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Lote", item.NumeroLote);
                cmd.Parameters.AddWithValue("@DataAnalise", item.DataAnalise);
                cmd.Parameters.AddWithValue("@TotalLentes", item.TotalLentes);
                cmd.Parameters.AddWithValue("@QtdNaoConforme", item.QtdNaoConforme);
                cmd.Parameters.AddWithValue("@Percentual", item.Percentual);
                cmd.Parameters.AddWithValue("@TipoLote", "Amostra");
                cmd.Parameters.AddWithValue("@IdLimites", limite.IdLimites);
                cmd.Parameters.AddWithValue("@IdOperadorAnalise", item.OperadorAnalise.IdOperador);
                cmd.Parameters.AddWithValue("@Observacao", item.Observacao);

                if (item.Percentual <= limite.LSC && item.Percentual >= limite.LIC)
                {
                    cmd.Parameters.AddWithValue("@Resultado", "Aprovado");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Resultado", "Reprovado");
                }

                cmd.ExecuteNonQuery();
            }

            FecharConexao();
        }

        public void Ativarlimite(int id)
        {
            AbrirConexao();

            string query = "update LimitesControle set Status = 0";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();

            query = "update LimitesControle set Status = 1 where IdLimites = @id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }

        public void CadastrarLote(Lote l)
        {
            AbrirConexao();

            string query = "insert into Lote (Lote, DataAnalise, TotalLentes, QtdNaoConforme, Percentual, Resultado, TipoLote, IdLimites, IdOperadorAnalise, Observacao) " +
                "values(@Lote, @DataAnalise, @TotalLentes, @QtdNaoConforme, @Percentual, @Resultado, @TipoLote, @IdLimites, @IdOperadorAnalise, @Observacao)";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Lote", l.NumeroLote);
            cmd.Parameters.AddWithValue("@DataAnalise", l.DataAnalise);
            cmd.Parameters.AddWithValue("@TotalLentes", l.TotalLentes);
            cmd.Parameters.AddWithValue("@QtdNaoConforme", l.QtdNaoConforme);
            cmd.Parameters.AddWithValue("@Percentual", l.Percentual);
            cmd.Parameters.AddWithValue("@Resultado", l.Status);
            cmd.Parameters.AddWithValue("@TipoLote", l.TipoLote);
            cmd.Parameters.AddWithValue("@IdLimites", l.Limites.IdLimites);
            cmd.Parameters.AddWithValue("@IdOperadorAnalise", l.OperadorAnalise.IdOperador);
            cmd.Parameters.AddWithValue("@Observacao", l.Observacao);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }

        public List<Lote> ObterLotesProducao(DateTime dataInicio, DateTime dataFim)
        {
            AbrirConexao();

            string query = "select lote.*,an.Nome as 'OperadorAnalise', l.LSC, l.LC, l.LIC from Lote lote " +
                "inner join Operador an on an.IdOperador = lote.IdOperadorAnalise " +
                "inner join LimitesControle l on l.IdLimites = lote.IdLimites " +
                "where lote.TipoLote = 'Producao' and lote.DataAnalise >= @DataInicio and lote.DataAnalise <= @DataFim";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@DataInicio", dataInicio);
            cmd.Parameters.AddWithValue("@DataFim", dataFim);
            dr = cmd.ExecuteReader();

            var lista = new List<Lote>();

            while (dr.Read())
            {
                var l = new Lote();
                l.OperadorAnalise = new Operador();
                l.Limites = new LimitesControle();

                l.IdLote = (int)dr["IdLote"];
                l.NumeroLote = (int)dr["Lote"];
                l.DataAnalise = (DateTime)dr["DataAnalise"];
                l.OperadorAnalise.Nome = (string)dr["OperadorAnalise"];
                l.TotalLentes = (int)dr["TotalLentes"];
                l.QtdNaoConforme = (int)dr["QtdNaoConforme"];
                l.Percentual = (double)dr["Percentual"];
                l.Status = (string)dr["Resultado"];
                l.Limites.LSC = Convert.ToDouble(dr["LSC"]);
                l.Limites.LC = Convert.ToDouble(dr["LC"]);
                l.Limites.LIC = Convert.ToDouble(dr["LIC"]);

                lista.Add(l);
            }
            FecharConexao();
            return lista;
        }
    }
}
