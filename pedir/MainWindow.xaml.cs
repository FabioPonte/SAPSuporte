using System;
using System.Collections.Generic;
using System.Data;
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

namespace pedir
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            pedir();
        }

        private void pedir()
        {
            DataSet ret = new DataSet();
            DataTable a1 = new DataTable();
            a1.Columns.Add("osql");
            a1.Rows.Add("Select cardname,docentry from oinv where docentry <100");

            DataTable a2 = new DataTable();
            a2.Columns.Add("asql");
            ret.Tables.Add(a1);
            ret.Tables.Add(a2);


            ret.WriteXml("C:\\notas\\ianez.consulta");

        }
    }
}
