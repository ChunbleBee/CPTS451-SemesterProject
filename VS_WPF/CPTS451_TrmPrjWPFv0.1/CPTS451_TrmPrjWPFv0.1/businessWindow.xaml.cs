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
using System.Windows.Shapes;
using Npgsql;

namespace CPTS451_TrmPrjWPFv0._1
{
    /// <summary>
    /// Interaction logic for businessWindow.xaml
    /// </summary>
    public partial class businessWindow : Window
    {
        private string businessid = "";

        public businessWindow(string bid)
        {
            InitializeComponent();
            this.businessid = String.Copy(bid);
            loadBusinessDetails();
            loadBusinessNumbers();
            addColumns2Grid();
            this.businessNameTextBox.IsReadOnly = true;
            this.stateNameTextBox.IsReadOnly = true;
            this.cityNameTextBox.IsReadOnly = true;
        }

        private string buildConnectionString()
        {
            // need to update this for everyone's personal machines
            //                  ---------------------------------------------------------------------
            //                                       |                                              |
            //                                       v                                              v
            return "Host = localhost; Username = postgres; Database = milestone1db; password= z";
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
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read(); // businessid's are unique
                        myf(reader);
                    }
                    catch(NpgsqlException er)
                    {
                        System.Windows.MessageBox.Show("SQL Error - " + er.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void setBusinessDetails(NpgsqlDataReader R)
        {
            businessNameTextBox.Text = R.GetString(0);
            stateNameTextBox.Text = R.GetString(1);
            cityNameTextBox.Text = R.GetString(2);
            // does not need businessid.
        }

        void setNumInState(NpgsqlDataReader R)
        {
            statebusinesscount.Content = R.GetInt16(0).ToString();
        }

        void setNumInCity(NpgsqlDataReader R)
        {
            citybusinesscount.Content = R.GetInt16(0).ToString();
        }

        private void loadBusinessNumbers()
        {
            string sqlstr1 = "SELECT count(*) from business WHERE state = (SELECT state FROM business WHERE businessid = '" + this.businessid + "');";
            executeQuery(sqlstr1, setNumInState);
            string sqlstr2 = "SELECT count(*) from business WHERE city = (SELECT city FROM business WHERE businessid = '" + this.businessid+"');";
            executeQuery(sqlstr2, setNumInCity);
        }

        private void loadBusinessDetails()
        {
            string sqlstr = "SELECT businessname, state, city FROM business WHERE businessid = '" + this.businessid + "';";
            executeQuery(sqlstr, setBusinessDetails);
        }

        private void addColumns2Grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("userid");
            col1.Header = "User ID";
            col1.Width = 60;
            tipsDataGrid.Columns.Add(col1);
            //businessGridDataGrid.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("usertips");
            col2.Header = "User Tips";
            col2.Width = 260;
            tipsDataGrid.Columns.Add(col2);
        }

        private void addGridRow(NpgsqlDataReader R)
        {
            // should we make a partial class "UserTips" which just holds a userID and a tip from the user?
            //tipsDataGrid.Items.Add();

        }

        private void loadUsers()
        {
            //string sqlstr = "SELECT businessname, state, city FROM business WHERE businessid = '" + this.businessid + "';";
            //executeQuery(sqlstr, setBusinessDetails);
        }

        private void loadTips()
        {
            //string sqlstr = "SELECT businessname, state, city FROM business WHERE businessid = '" + this.businessid + "';";
            //executeQuery(sqlstr, setBusinessDetails);
        }



        private void AddTipButton_Click(object sender, RoutedEventArgs e)
        {
            // here we need to open up a new AddTipWindow and populate it with the appropriate information.
            AddTipWindow addTipWindow = new AddTipWindow(this.businessNameTextBox.Text.ToString(), this.businessid); // all we care about passing is the businessID of the currently selected business
            addTipWindow.Show(); // show the new window
        }
    }
}
