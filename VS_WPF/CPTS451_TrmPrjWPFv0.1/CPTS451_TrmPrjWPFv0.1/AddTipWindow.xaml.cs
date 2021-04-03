using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
    /// Interaction logic for AddTipWindow.xaml
    /// </summary>
    public partial class AddTipWindow : Window
    {

        private string businessName = "";
        private string businessID = "";
        private string userID;
        private string tip2Add = "";
        public AddTipWindow(string bName, string bid)
        {
            InitializeComponent();
            this.businessName = String.Copy(bName); // business name to show user what business they are adding a tip for.
            this.businessID = String.Copy(bid); // businessID used for selecting the appropriate business to insert the tip for.
            this.businessNameTextBox.Text = this.businessName;
            this.businessNameTextBox.IsReadOnly = true;
        }

        private string buildConnectionString()
        {
            // need to update this for everyone's personal machines
            //                  ---------------------------------------------------------------------
            //                                       |                                              |
            //                                       v                                              v
            return "Host = localhost; Username = postgres; Database = milestone1db; password= ';'";
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

                    //cmd.CommandText = "SELECT name, state, city FROM business WHERE state = '" + stateListComboBox.SelectedItem.ToString() + "' AND city = '" + cityListComboBox.SelectedItem.ToString() + "ORDER BY city;";
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

        private bool checkTip()
        {
            //bool checkTip = false;
            //while (checkTip == false)
            //{
                // whatever is written in the textbox will be pulled
                // if the textbox is NOT empty AND the string in the textbox is NOT  the placeholder text...
                if (this.tipTextBox.Text != "" && this.tipTextBox.Text != "Please type your tip here.")
                {
                    // pull the string from the textbox.
                    tip2Add = this.tipTextBox.Text;
                    //checkTip = true;
                }
                else
                {
                    MessageBox.Show("Please insert a tip for other users.");
                    return false;
                }
            //}

            return true;
        }

        private void saveTip()
        {
            string sqlstr = "INSERT INTO tips VALUES('" + this.businessID + "', '" +this.userID + "', '" + tipTextBox.Text + "', '" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            this.executeQuery(sqlstr, addTip);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // check tip is not placeholder text or empty string.
            bool check = checkTip();

            if (check == false) // check the user typed in a tip
                return;
            if (tip2Add == "") // check tip is not empty.
            {
                MessageBox.Show("Looks like you forgot to type a tip!");
                return;
            }
            // here we should insert the tip that was entered into the textbox into the database
            // as a tip for the selected business.
            saveTip();

            // close window
            this.Close();
        }
        private void addTip(NpgsqlDataReader R)
        {
        //    tipsDataGrid.Items.Add(R.GetString(0));
        }
    }
}
