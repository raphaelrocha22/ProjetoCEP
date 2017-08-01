using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Util
{
    public class Criptografia
    {
        public static string EncriptarSenha(string senha)
        {
            var md5 = new MD5CryptoServiceProvider();

            byte[] senhaBinario = Encoding.UTF8.GetBytes(senha);
            byte[] hash = md5.ComputeHash(senhaBinario);

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
