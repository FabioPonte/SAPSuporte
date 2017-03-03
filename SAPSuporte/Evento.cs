using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace SAPSuporte
{
    public class Evento
    {
        DispatcherTimer time = new DispatcherTimer();
        public void Iniciar()
        {
            Config.Obterlocal();
            if (Config.local != "")
            {
                time.Interval = new TimeSpan(0, 0, 1);
                time.Tick += new EventHandler(time_Tick);
                time.Start();
            }
        }

        void time_Tick(object sender, EventArgs e)
        {
            verificar();
        }
        int error = 0;
        private void verificar()
        {
            try
            {
                
                DirectoryInfo diretorio = new DirectoryInfo(Config.local);
                FileInfo[] Arquivos = diretorio.GetFiles("*.consulta");
                foreach (FileInfo fileinfo in Arquivos)
                {
                    info(abrir(fileinfo.FullName), fileinfo.FullName,fileinfo.Name);
                    try {
                        File.Delete(fileinfo.FullName);
                    }
                    catch { }
                }
            }
            catch (Exception exd)
            {
                error++;
                if (error > 10)
                {
                    MessageBox.Show(exd + "");
                    Environment.Exit(1);
                }
            }
        }

        private DataSet abrir(string arquivo)
        {
           
            try
            {
                DataSet tabelas = new DataSet();
                tabelas.ReadXml(arquivo);
                return tabelas;

            }
            catch { return null; }
        }


       
        public delegate void sql(DataSet tabelas,string arquivo,string arquivonome);
        public event sql info;

        
    }
}
