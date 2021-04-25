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
        }

        private string buildConnectionString()
        {
            // need to update this for everyone's personal machines
            //                  ---------------------------------------------------------------------
            //                                       |                                              |
            //                                       v                                              v
            return "Host = localhost; Username = postgres; Database = Milestone2db; password= z";
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

    }
}
