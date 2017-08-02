using Projeto.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projeto.DAL.Conexoes;
using System.Data.SqlClient;
using Projeto.Util;

namespace Projeto.DAL.Persistencias
{
    public class UsuarioDAL : Conexao
    {
        public void Inserir(Usuario u)
        {
            AbrirConexao();

            string query = "insert into Usuario (Nome,Login, Senha, DataCadastro, Status) " +
                "values (@Nome, @Login, @Senha,@DataCadastro, @Status)";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Nome", u.Nome);
            cmd.Parameters.AddWithValue("@Login", u.Login);
            cmd.Parameters.AddWithValue("@Senha", Criptografia.EncriptarSenha(u.Senha));
            cmd.Parameters.AddWithValue("@DataCadastro", u.DataCadastro);
            cmd.Parameters.AddWithValue("@Status", u.Status);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }

        public void AtualizarUsuario(Usuario u)
        {
            AbrirConexao();

            string query = "update Usuario set (Nome = @Nome, Senha = @Senha) where IdUsuario = @IdUsuario";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Nome", u.Nome);
            cmd.Parameters.AddWithValue("@Senha", Criptografia.EncriptarSenha(u.Senha));
            cmd.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }

        public void AtualizarStatus(Usuario u)
        {
            AbrirConexao();

            string query = "update Usuario set (Status = @Status) where IdUsuario = @IdUsuario";

            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Status", u.Status);
            cmd.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
            cmd.ExecuteNonQuery();

            FecharConexao();
        }

        public Usuario ObterPorLoginSenha(string login, string senha)
        {
            AbrirConexao();

            string query = "select * from Usuario where Login = @Login and Senha = @Senha and Status = 1";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Login", login);
            cmd.Parameters.AddWithValue("@Senha", Criptografia.EncriptarSenha(senha));
            dr = cmd.ExecuteReader();

            Usuario u = null;

            if (dr.Read())
            {
                u = new Usuario();
                u.Nome = (string)dr["Nome"];
                u.Login = (string)dr["Login"];
                u.DataCadastro = (DateTime)dr["DataCadastro"];
            }

            FecharConexao();
            return u;
        }
    }
}
