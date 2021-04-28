using Npgsql;
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

namespace CPTS451_TrmPrjWPFv0._1
{

    /// <summary>
    /// Interaction logic for TipsWindow.xaml
    /// </summary>
    public partial class TipsWindow : Window
    {
        private User acct { get; set; }
        private Business busi { get; set; }
        private Tip tip { get; set; }

        public TipsWindow(string uid, string bid)
        {
            InitializeComponent();
            SetBusinessTipsColumns();
            //this.AddTipBusinessNameTextBox.Text = bname;
            //this.AddTipBusinessNameTextBox.IsReadOnly = true;

            ExecuteQuery(
                @"SELECT Businesses.BusinessID, Businesses.BusinessName
                FROM Businesses
                WHERE Businesses.BusinessID='" + bid + @"';", AddBusiness);

            ExecuteQuery(
                @"SELECT Users.UserID
                FROM Users
                WHERE Users.UserID='" + uid + @"';", AddUserAccount);

            this.AllTipsGrid.Items.Clear();
            this.FriendsTipsGrid.Items.Clear();

            ExecuteQuery(
                @"SELECT Users.UserID, Users.UserName, Tips.Date, Tips.Likes, Tips.Text, Tips.BusinessID
                        FROM Tips, Users
                        WHERE Users.UserID=Tips.UserID
                        AND BusinessID='" + this.busi.BusinessID + @"'
                        ORDER BY Date DESC;",
                AddTipsToAllGrid);

            ExecuteQuery(
                @"SELECT Users.UserID, Users.UserName, Tips.Date, Tips.Likes, Tips.Text, Tips.BusinessID
                        FROM Tips, Users, Friends
                        WHERE Tips.BusinessID='" + this.busi.BusinessID + @"'
                        AND Friends.User01='" + this.acct.ID + @"'
                        AND Friends.User02=Users.UserID
                        AND Users.UserID=Tips.UserID
                        ORDER BY Date DESC;",
                AddTipsToFriendsGrid);

            this.AddTipBusinessNameTextBox.Text = this.busi.BusinessName.ToString(); // display the appropriate name for the business.
            this.AddTipBusinessNameTextBox.IsReadOnly = true; // make the textbox not edittable

            this.FriendsTipsGrid.IsReadOnly = true;
            this.AllTipsGrid.IsReadOnly = true;

        }

        // not a great way of building a connection. unsafe to show user businessname and password.
        private string GetConnectionString()
        {
            // need to update this for everyone's personal machines
            //                  ---------------------------------------------------------------------
            //                                       v                                              v
            return "Host = localhost; Username = postgres; Database = milestone3; password= 'SegaSaturn'";
        }

        private void ExecuteQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;

                    try
                    {
                        var reader = cmd.ExecuteReader();

                        if (myf != null)
                        {
                            while (reader.Read())
                            {
                                /*AsyncCall newcall = new AsyncCall(myf);
                                newcall.BeginInvoke(reader, null, null);*/
                                myf(reader);
                            }
                        }
                    }
                    catch (NpgsqlException er)
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

        private void ExecuteNonQuery(string sqlstr)
        {
            using (var connection = new NpgsqlConnection(GetConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sqlstr;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (NpgsqlException er)
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

        private void SetBusinessTipsColumns()
        {
            DataGridTextColumn tdate = new DataGridTextColumn();
            DataGridTextColumn tname = new DataGridTextColumn();
            DataGridTextColumn tlikes = new DataGridTextColumn();
            DataGridTextColumn ttext = new DataGridTextColumn();

            DataGridTextColumn fdate = new DataGridTextColumn();
            DataGridTextColumn fname = new DataGridTextColumn();
            DataGridTextColumn flikes = new DataGridTextColumn();
            DataGridTextColumn ftext = new DataGridTextColumn();

            tname.Binding = new Binding("UserName");
            tlikes.Binding = new Binding("Likes");
            ttext.Binding = new Binding("Text");
            tdate.Binding = new Binding("CreationDate");

            fname.Binding = new Binding("UserName");
            flikes.Binding = new Binding("Likes");
            ftext.Binding = new Binding("Text");
            fdate.Binding = new Binding("CreationDate");

            tname.Header = "User Name";
            ttext.Header = "Tip Text";
            tlikes.Header = "Likes";
            tdate.Header = "Date Of Tip";

            fname.Header = "User Name";
            ftext.Header = "Tip Text";
            flikes.Header = "Likes";
            fdate.Header = "Date Of Tip";

            this.FriendsTipsGrid.Columns.Add(fname);
            this.FriendsTipsGrid.Columns.Add(fdate);
            this.FriendsTipsGrid.Columns.Add(flikes);
            this.FriendsTipsGrid.Columns.Add(ftext);

            this.AllTipsGrid.Columns.Add(tname);
            this.AllTipsGrid.Columns.Add(tdate);
            this.AllTipsGrid.Columns.Add(tlikes);
            this.AllTipsGrid.Columns.Add(ttext);
        }

        private void AddUserAccount(NpgsqlDataReader reader)
        {
            //SELECT UserID, UserName, TotalLikes, AvgStarRating, CreationDate
            this.acct = new User()
            {
                ID = reader.GetString(0),
                //Name = reader.GetString(1),
                //Likes = reader.GetInt32(2),
                //Stars = reader.GetFloat(3),
                //CreationDate = reader.GetDateTime(4)
            };
        }

        private void AddTip(NpgsqlDataReader reader)
        {
            //int tLikes = 0;
            this.tip = new Tip()
            {
                BusinessID = reader.GetString(0),
                UserID = reader.GetString(1),
                Likes = Int32.Parse(reader.GetString(2)),
                Text = reader.GetString(3)
            };
        }

        private void AddBusiness(NpgsqlDataReader reader)
        {
            this.busi = new Business()
            {
                BusinessID = reader.GetString(0),
                BusinessName = reader.GetString(1)

            };
        }

        private void AddTipsToAllGrid(NpgsqlDataReader reader)
        {
            //Users.UserID, Users.UserName, Date, Likes, Text
            this.AllTipsGrid.Items.Add(new TipHelper()
            {
                UserID = reader.GetString(0),
                UserName = reader.GetString(1),
                CreationDate = reader.GetDateTime(2),
                Likes = reader.GetInt32(3),
                Text = reader.GetString(4),
                BusinessID = reader.GetString(5)
            });
        }

        private void AddTipsToFriendsGrid(NpgsqlDataReader reader)
        {
            //Users.UserID, Users.UserName, Date, Likes, Text
            this.FriendsTipsGrid.Items.Add(new TipHelper()
            {
                UserID = reader.GetString(0),
                UserName = reader.GetString(1),
                CreationDate = reader.GetDateTime(2),
                Likes = reader.GetInt32(3),
                Text = reader.GetString(4),
                BusinessID = reader.GetString(5)
            });
        }

        private void LikeTipButton_Click(object sender, RoutedEventArgs e)
        {
            TipHelper SelectedTip = ((TipHelper)this.AllTipsGrid.SelectedItem);
            if (SelectedTip != null)
            {
                string sqlcall = @"UPDATE Tips
                                    SET Likes = Likes + 1
                                    WHERE Tips.UserID='" + SelectedTip.UserID + @"'
                                    AND Tips.BusinessID='" + SelectedTip.BusinessID + @"'
                                    AND Tips.Date='" + SelectedTip.CreationDate + "';";

                this.ExecuteNonQuery(sqlcall);

                this.AllTipsGrid.Items.Clear();
                this.FriendsTipsGrid.Items.Clear();

                ExecuteQuery(
                    @"SELECT Users.UserID, Users.UserName, Tips.Date, Tips.Likes, Tips.Text, Tips.BusinessID
                        FROM Tips, Users
                        WHERE Users.UserID=Tips.UserID
                        AND BusinessID='" + this.busi.BusinessID + @"'
                        ORDER BY Date DESC;",
                    AddTipsToAllGrid);

                ExecuteQuery(
                    @"SELECT Users.UserID, Users.UserName, Tips.Date, Tips.Likes, Tips.Text, Tips.BusinessID
                        FROM Tips, Users, Friends
                        WHERE Tips.BusinessID='" + this.busi.BusinessID + @"'
                        AND Friends.User01='" + this.acct.ID + @"'
                        AND Friends.User02=Users.UserID
                        AND Users.UserID=Tips.UserID
                        ORDER BY Date DESC;",
                    AddTipsToFriendsGrid);
            }
        }


        private void AddNewTipButton_Click(object sender, RoutedEventArgs e)
        {
            if ((this.AddNewTipTextBox.Text == "") || (this.AddNewTipTextBox.Text == "Enter new Tip text here."))
            {
                MessageBox.Show("Looks like you forgot to type a tip!");
            }
            else
            {
                var cs = GetConnectionString();

                using (var con = new NpgsqlConnection(cs))
                {
                    con.Open();

                    using (var cmd = new NpgsqlCommand()) {
                        cmd.Connection = con;
                        int initialLikes = 0;
                        string likes = initialLikes.ToString();
                        StringBuilder temp = new StringBuilder("INSERT INTO Tips(BusinessID, UserID, Date, Likes, Text) VALUES('");
                        temp.Append(this.busi.BusinessID.ToString());
                        temp.Append("', '");
                        temp.Append(this.acct.ID.ToString()); // this is returning null... I think I have a solution.
                        temp.Append("', '");
                        temp.Append(DateTime.Now.ToString("MM-dd-yy HH:mm:ss"));
                        temp.Append("', 0, '");
                        temp.Append(AddNewTipTextBox.Text.ToString());
                        temp.Append("');");
                        cmd.CommandText = temp.ToString();
                        string sqlstr = temp.ToString();
                            //"INSERT INTO Tips(BusinessID, UserID, Date, Likes, Text) VALUES(\'" + this.busi.BusinessID.ToString() + "\', \'" + this.acct.ID.ToString() + "\', \'" + DateTime.Now.ToString("MM-dd-yy HH:mm:ss") + "\', " + likes + ", \'" + AddNewTipTextBox.Text.ToString() + "\');";
                        // BusinessID                         // UserID                              // Date                                   // # Likes, initially 0.         // Text of tip
                        cmd.CommandText = sqlstr.ToString();
                        cmd.ExecuteNonQuery();
                        //ExecuteQuery(sqlstr, AddTipsToAllGrid);
                    }
                }

                this.AllTipsGrid.Items.Clear();
                this.FriendsTipsGrid.Items.Clear();

                ExecuteQuery(
                    @"SELECT Users.UserID, Users.UserName, Tips.Date, Tips.Likes, Tips.Text, Tips.BusinessID
                        FROM Tips, Users
                        WHERE Users.UserID=Tips.UserID
                        AND BusinessID='" + this.busi.BusinessID + @"'
                        ORDER BY Date DESC;",
                    AddTipsToAllGrid);

                ExecuteQuery(
                    @"SELECT Users.UserID, Users.UserName, Tips.Date, Tips.Likes, Tips.Text, Tips.BusinessID
                        FROM Tips, Users, Friends
                        WHERE Tips.BusinessID='" + this.busi.BusinessID + @"'
                        AND Friends.User01='" + this.acct.ID + @"'
                        AND Friends.User02=Users.UserID
                        AND Users.UserID=Tips.UserID
                        ORDER BY Date DESC;",
                    AddTipsToFriendsGrid);

                //reset textbox text to initial / default text.
                this.AddNewTipTextBox.Text = "Enter new Tip text here.";
            }
        }

        public void TipBoxRemoveDefaultText(object sender, RoutedEventArgs e)
        {
            if (this.AddNewTipTextBox.Text == "Enter new Tip text here.")
            {
                this.AddNewTipTextBox.Text = "";
            }
        }
        
        public void TipBoxAddDefaultText(object sender, RoutedEventArgs e)
        {
            if (this.AddNewTipTextBox.Text == "")
            {
                this.AddNewTipTextBox.Text = "Enter new Tip text here.";
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ((MainWindow)this.Owner).ReloadContext();
        }
    }
}