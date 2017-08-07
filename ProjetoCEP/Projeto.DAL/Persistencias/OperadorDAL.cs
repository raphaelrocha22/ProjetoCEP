using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projeto.Entidades;
using Projeto.DAL.Conexoes;
using System.Data.SqlClient;

namespace Projeto.DAL.Persistencias
{
    public class OperadorDAL:Conexao
    {
        public void Cadastrar(Operador o)
        {
            AbrirConexao();

            string quey = "Insert into Operador (Nome,DataCadastro, Status) " +
                "values (@Nome, @DataCadastro, @Status)";

            cmd = new SqlCommand(quey, con);
            cmd.Parameters.AddWithValue("@Nome", o.Nome);
            cmd.Parameters.AddWithValue("@DataCadastro", o.DataCadastro);
            cmd.Parameters.AddWithValue("@Status", o.Status);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }

        public List<Operador> ListaOperador()
        {
            AbrirConexao();

            string query = "Select * from Operador where Status = 1";
            cmd = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();

            var lista = new List<Operador>();

            while (dr.Read())
            {
                var o = new Operador();

                o = new Operador();
                o.IdOperador = (int)dr["IdOperador"];
                o.Nome = (string)dr["Nome"];
                o.DataCadastro = (DateTime)dr["DataCadastro"];

                lista.Add(o);
            }

            FecharConexao();
            return lista;
        }

        public bool ObterOperador(string Nome)
        {
            AbrirConexao();

            string query = "select count(*) from Operador where Nome = @Nome";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Nome", Nome);
            int qtd = Convert.ToInt32(cmd.ExecuteScalar());

            FecharConexao();
            return qtd > 0;
            
        }

        public Operador ObterPorId(int id)
        {
            AbrirConexao();

            string query = "select * from Operador where IdOperador = @IdOperador";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@IdOperador", id);
            dr = cmd.ExecuteReader();

            Operador o = null;

            if (dr.Read())
            {
                o = new Operador();
                o.IdOperador = (int)dr["IdOperador"];
                o.Nome = (string)dr["Nome"];
                o.DataCadastro = (DateTime)dr["DataCadastro"];
            }
            FecharConexao();
            return o;
        }

        public void Excluir(int id)
        {
            AbrirConexao();

            string query = "update Operador set Status = 0 where IdOperador = @Id";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }
    }
}
