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

        public TipsWindow(string uid, string bid)
        {
            InitializeComponent();
            SetBusinessTipsColumns();

            ExecuteQuery(
                @"SELECT Users.UserID, Users.UserName, Date, Likes, Text
                FROM Tips, Users
                WHERE Users.UserID=Tips.UserID
                AND BusinessID='" + bid + "';",
                AddTipsToAllGrid);

            ExecuteQuery(
                @"SELECT Users.UserID, Users.UserName, Tips.Date, Tips.Likes, Tips.Text 
                FROM Tips, Users, Friends
                WHERE Tips.BusinessID='" + bid + @"'
                AND Friends.User01='" + uid + @"'
                AND Friends.User02=Users.UserID
                AND Users.UserID=Tips.UserID;",
                AddTipsToFriendsGrid);
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
                Name = reader.GetString(1),
                Likes = reader.GetInt32(2),
                Stars = reader.GetFloat(3),
                CreationDate = reader.GetDateTime(4)
            };
        }

        private void AddBusiness(NpgsqlDataReader reader)
        {
            this.busi = new Business()
            {
                BusinessID = reader.GetString(0)
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
                Text = reader.GetString(4)
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
                Text = reader.GetString(4)
            });
        }
    }
}