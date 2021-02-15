using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPTS451_TrmPrjWPFv0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public partial class Business
        {
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set;  }
        }

        public MainWindow()
        {
            InitializeComponent();
            addState();
        }

        private void addState()
        {
            stateListComboBox.Items.Add("WA");
            stateListComboBox.Items.Add("CA");
            stateListComboBox.Items.Add("ID");
            stateListComboBox.Items.Add("NV");
        }

        private void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "BusinessName";
            col1.Width = 255;
            businessGridDataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("state");
            col2.Header = "State";
            col2.Width = 60;
            businessGridDataGrid.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 150;
            businessGridDataGrid.Columns.Add(col3);

            businessGridDataGrid.Items.Add(new Business() { name = "business-1", state = "WA", city = "Pullman" });
            businessGridDataGrid.Items.Add(new Business() { name = "business-2", state = "CA", city = "Pasadena" });
            businessGridDataGrid.Items.Add(new Business() { name = "business-3", state = "NV", city = "Las Vegas" });

        }
    }
}
