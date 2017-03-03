using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace SAPSuporte
{
    public class Config
    {
        static public string local = "";
  

        static public void Obterlocal()
        {
            try
            {
                string l = Environment.CurrentDirectory + "\\";
                if (File.Exists(l + "config.ianez"))
                {
                    StreamReader leitor = new StreamReader(l + "config.ianez");
                    local = leitor.ReadLine();
                    leitor.Close();
                }
            }
            catch (Exception exd)
            {
                
            }

        }
    }
}
