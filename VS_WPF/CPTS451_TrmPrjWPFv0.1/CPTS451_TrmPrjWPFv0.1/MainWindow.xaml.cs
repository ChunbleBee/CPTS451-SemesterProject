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
using Npgsql;

namespace CPTS451_TrmPrjWPFv0._1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public partial class Business
        {
            public string businessid { get; set; } // for querying businesses in a state/city
            public string businessname { get; set; }
            public string state { get; set; }
            public string city { get; set;  }
        }

        public MainWindow()
        {
            InitializeComponent();
            addState();
            addColumns2Grid(); // THIS IS WHERE WE CALL ADDCOLUMNS2GRID!
        }

        // not a great way of building a connection. unsafe to show user businessname and password.
        private string buildConnectionString()
        {
            // need to update this for everyone's personal machines
            //                  ---------------------------------------------------------------------
            //                                       |                                              |
            //                                       v                                              v
            return "Host = localhost; Username = postgres; Database = milestone1db; password= [ENTER YOUR PASSWORD HERE]";
        }

        private void addState()
        {
            //stateListComboBox.Items.Add("WA");
            //stateListComboBox.Items.Add("CA");
            //stateListComboBox.Items.Add("ID");
            //stateListComboBox.Items.Add("NV");
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            stateListComboBox.Items.Add(reader.GetString(0));
                        }
                    }
                    catch (NpgsqlException er)
                    {
                        // Sakire gives us two options on handling catched errors. I prefer option 2.
                        // Option 1.
                        //Console.WriteLine(er.Message.ToString());

                        // Option 2.
                        System.Windows.MessageBox.Show("SQL Error - " + er.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("businessname");
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

            // new column of width 0 and no header to hide the info from users.
            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("businessid");
            col4.Header = "";
            col4.Width = 0;
            businessGridDataGrid.Columns.Add(col4);

            //businessGridDataGrid.Items.Add(new Business() { businessname = R.GetString(0), state = R.GetString(1), city = R.GetString(2) });
            /*businessGridDataGrid.Items.Add(new Business() { businessname = "business-1", state = "WA", city = "Pullman" });
            businessGridDataGrid.Items.Add(new Business() { businessname = "business-2", state = "CA", city = "Pasadena" });
            businessGridDataGrid.Items.Add(new Business() { businessname = "business-3", state = "NV", city = "Las Vegas" });
*/
        }

        private void executeQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;

                    //cmd.CommandText = "SELECT businessname, state, city FROM business WHERE state = '" + stateListComboBox.SelectedItem.ToString() + "' AND city = '" + cityListComboBox.SelectedItem.ToString() + "ORDER BY city;";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            myf(reader);
                        }
                    }
                    catch (NpgsqlException er)
                    {
                        // Sakire gives us two options on handling catched errors. I prefer option 2.
                        // Option 1.
                        //Console.WriteLine(er.Message.ToString());

                        // Option 2.
                        System.Windows.MessageBox.Show("SQL Error - " + er.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void addCity(NpgsqlDataReader R)
        {
            cityListComboBox.Items.Add(R.GetString(0));
        }

        private void StateListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cityListComboBox.Items.Clear();
            if (cityListComboBox.SelectedIndex == -1)
            {
               string sqlstr = "SELECT distinct city FROM business WHERE state = '" + stateListComboBox.SelectedItem.ToString() + "' ORDER BY city";
                        //cmd.CommandText = "SELECT businessname, state, city FROM business WHERE state = '" + stateListComboBox.SelectedItem.ToString() + "' AND city = '" + cityListComboBox.SelectedItem.ToString() + "ORDER BY city;";
               executeQuery(sqlstr, addCity);
            }

        }

        private void addGridRow(NpgsqlDataReader R)
        {
            businessGridDataGrid.Items.Add(new Business() { businessname = R.GetString(0), state = R.GetString(1), city = R.GetString(2), businessid = R.GetString(3) });

        }

        private void CityListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessGridDataGrid.Items.Clear();
            if (stateListComboBox.SelectedIndex > -1)
            {
                string sqlstr = "SELECT businessname, state, city, businessid FROM business WHERE state = '" + stateListComboBox.SelectedItem.ToString() + "' AND city = '" + cityListComboBox.SelectedItem.ToString() + "' ORDER BY businessname;";
                executeQuery(sqlstr, addGridRow);
                //executeQuery(sqlstr, addColumns2Grid); // revert changes.

            }
        }

        // used to open the new businessWindow from MainWindow
        private void BusinessGridDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (businessGridDataGrid.SelectedIndex > -1)
            {
                // grab the businessid string.
                Business B = businessGridDataGrid.Items[businessGridDataGrid.SelectedIndex] as Business;
                if((B.businessid != null) && (B.businessid.ToString().CompareTo("") != 0))
                {
                    // create new instance of business window.
                    businessWindow businessWindow = new businessWindow(B.businessid.ToString());
                    businessWindow.Show(); // show the new window
                }
            }
        }

        //private void CityListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    businessGridDataGrid.Items.Clear();
        //    if (stateListComboBox.SelectedIndex > -1)
        //    {

        //        using (var connection = new NpgsqlConnection(buildConnectionString()))
        //        {
        //            connection.Open();
        //            using (var cmd = new NpgsqlCommand())
        //            {
        //                cmd.Connection = connection;
        //                cmd.CommandText = "SELECT businessname, state, city FROM business WHERE state = '" + stateListComboBox.SelectedItem.ToString() + "' AND city = '" + cityListComboBox.SelectedItem.ToString() + "' ORDER BY businessname;";

        //                try
        //                {
        //                    var reader = cmd.ExecuteReader();
        //                    while (reader.Read())
        //                    {
        //                        //stateListComboBox.Items.Add(reader.GetString(0));
        //                        businessGridDataGrid.Items.Add(new Business() { businessname = reader.GetString(0), state = reader.GetString(1), city = reader.GetString(2)});
        //                    }
        //                }
        //                catch (NpgsqlException er)
        //                {
        //                    // Sakire gives us two options on handling catched errors. I prefer option 2.
        //                    // Option 1.
        //                    //Console.WriteLine(er.Message.ToString());

        //                    // Option 2.
        //                    System.Windows.MessageBox.Show("SQL Error - " + er.Message.ToString());
        //                }
        //                finally
        //                {
        //                    connection.Close();
        //                }
        //            }
        //        }
        //    }
        //}

        //private void StateListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    cityListComboBox.Items.Clear();
        //    if (cityListComboBox.SelectedIndex > -1)
        //    {

        //        using (var connection = new NpgsqlConnection(buildConnectionString()))
        //        {
        //            connection.Open();
        //            using (var cmd = new NpgsqlCommand())
        //            {
        //                cmd.Connection = connection;
        //                cmd.CommandText = "SELECT distinct city FROM business WHERE state = '" + stateListComboBox.SelectedItem.ToString() + "' ORDER BY city";

        //                //cmd.CommandText = "SELECT businessname, state, city FROM business WHERE state = '" + stateListComboBox.SelectedItem.ToString() + "' AND city = '" + cityListComboBox.SelectedItem.ToString() + "ORDER BY city;";
        //                try
        //                {
        //                    var reader = cmd.ExecuteReader();
        //                    while (reader.Read())
        //                    {
        //                        cityListComboBox.Items.Add(reader.GetString(0));
        //                    }
        //                }
        //                catch (NpgsqlException er)
        //                {
        //                    // Sakire gives us two options on handling catched errors. I prefer option 2.
        //                    // Option 1.
        //                    //Console.WriteLine(er.Message.ToString());

        //                    // Option 2.
        //                    System.Windows.MessageBox.Show("SQL Error - " + er.Message.ToString());
        //                }
        //                finally
        //                {
        //                    connection.Close();
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
