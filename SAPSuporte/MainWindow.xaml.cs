using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAPSuporte
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Evento evento = new Evento();
        Boolean security = false;

        public MainWindow()
        {
            InitializeComponent();
            evento.info += ObterSQL;
            evento.Iniciar();
        }

        private void ObterSQL(DataSet tabelas, string arquivo,string arquivonome)
        {
            string osql = tabelas.Tables[0].Rows[0][0] + "";
            listBox.Items.Add("Nova consulta solicitada: " + DateTime.Now);
            executar(osql, arquivo, arquivonome);
        }

        
        private Boolean seguranca(string sql)
        {
            if (sql.IndexOf("insert") != -1)
            {
                return false;
            }

            if (sql.IndexOf("delete") != -1)
            {
                return false;
            }

            if (sql.IndexOf("update") != -1)
            {
                return false;
            }

            //if (sql.IndexOf("-") != -1)
            //{
            //    return false;
            //}

            if (sql.IndexOf("/") != -1)
            {
                return false;
            }

            if (sql.IndexOf("\\") != -1)
            {
                return false;
            }

            return true;
        }

        public void executar(string sql,string arquivo, string arquivonome)
        {
            if (seguranca(sql))
            {
                security = true;
                espere("Iniciando a consulta,espere!",arquivo);
                DataTable retorno = iniciar(sql,arquivo);
                if (retorno != null)
                {
                    retorna(retorno, arquivo, arquivonome);

                }
            }
            else
            {
                erro("Sistema de segurança sql encontrou um nome não autorizado!",arquivo);
            }
        }

        private void retorna(DataTable retorno, string arquivo,string arquivonome)
        {

            try {
                DataTable info = new DataTable();
                info.Columns.Add("arq");
                info.Rows.Add(arquivo);

                DataSet ret = new DataSet();
                ret.Tables.Add(retorno);
                ret.Tables.Add(info);

                Config.Obterlocal();

                ret.WriteXml(Config.local + "" + arquivonome + ".retorno");
            }
            catch(Exception ex) { erro("Erro ao criar arquivo xml." + Environment.NewLine + "" + ex + "", arquivo); }
        }

        private DataTable iniciar(string sql,string arquivo)
        {
            if (security)
            {
                try {
                    //string connectionString = "Data Source=192.168.5.10,1433;Network Library=DBMSSOCN;Initial Catalog = SBO_ATANOR; User ID = sa; Password = atanor@001; ";
                    string connectionString = @"Data Source=ALBR01SQP\ALBA_PROD;Network Library=DBMSSOCN;Initial Catalog = SBO_ATANOR; User ID = sga; Password = atanor@001; ";
                    SqlConnection sqlConn = new SqlConnection(connectionString);
                    sqlConn.Open();
                    DataTable retorno = new DataTable();
                    SqlDataAdapter dan = new SqlDataAdapter(sql, sqlConn);
                    dan.Fill(retorno);
                    dan.Dispose();
                    sqlConn.Close();
                    return retorno;
                }
                catch(Exception ex) { erro("Erro de SQL."+Environment.NewLine+""+ex+"", arquivo); return null; }
            }
            else
            {
                return null;
            }
        }

        private void erro(string msg,string arquivo)
        {
            StreamWriter escritor = new StreamWriter(arquivo + ".erro");
            escritor.Write(msg);
            escritor.Close();
        }
        private void espere(string msg, string arquivo)
        {
            StreamWriter escritor = new StreamWriter(arquivo + ".esp");
            escritor.Write(msg);
            escritor.Close();
        }

        private void fim(string msg,string arquivo)
        {
            StreamWriter escritor = new StreamWriter(arquivo + ".fim");
            escritor.Write(msg);
            escritor.Close();
        }
    }
}
