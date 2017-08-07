using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projeto.Util
{
    public class Logger
    {
        public static void LogErro(string path, Exception e)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var sw = new StreamWriter(path + "Log.txt", true);

            try
            {
                sw.WriteLine("\n");
                sw.WriteLine($"Data: { DateTime.Now}");
                sw.WriteLine($"ErroCode: {e.HResult }");
                sw.WriteLine($"HelpLink: {e.HelpLink}");
                sw.WriteLine($"Source: {e.Source}");
                sw.WriteLine($"Message: {e.Message}");
                sw.WriteLine($"StackTrace: { e.StackTrace}");
                sw.WriteLine("------------------\n\n");
            }
            catch(Exception ex)
            {
                sw.WriteLine($"------...Houve um erro na gravação do log...------");
                sw.WriteLine(ex.Message);
            }

            sw.Close();
        }
    }
}
