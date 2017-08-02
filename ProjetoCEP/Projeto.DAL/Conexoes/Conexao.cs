using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.DAL.Conexoes
{
    public class Conexao
    {
        protected SqlConnection con;
        protected SqlCommand cmd;
        protected SqlDataReader dr;

        protected void AbrirConexao()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["BDcep"].ConnectionString);
            con.Open();
        }

        protected void FecharConexao()
        {
            con.Close();
        }
    }
}
