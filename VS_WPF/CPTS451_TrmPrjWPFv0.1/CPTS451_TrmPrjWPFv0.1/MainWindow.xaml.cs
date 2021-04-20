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
        private NpgsqlConnection Connection { get; set; }
        private User UserAcct { get; set; }

        public partial class Business
        {
            public string BusinessID { get; set; } // for querying businesses in a state/city
            public string BusinessName { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public int ZipCode { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
            public bool IsOpen { get; set; }
            public int ReviewCount { get; set; }
            public float StarRating { get; set; }
            public int NumCheckIns { get; set; }
            public int NumTips { get; set; }
        }

        public class User
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public float Stars { get; set; }
            public int Fans { get; set; }
            public DateTime CreationDate { get; set; }
            public int FunnyRates {get; set; }
            public int CoolRates { get; set; }
            public int UsefulRates { get; set; }
            public int Tips { get; set; }
            public int Likes { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }
        public class Tip
        {
            public string BusinessID { get; set; }
            public string UserID { get; set; }
            DateTime CreationDate { get; set; }
            public int Likes { get; set; }
            public string Text { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Connection = new NpgsqlConnection(GetConnectionString());
            this.UserAcct = null;

            CreateUserIDColumns();
            CreateFriendsColumns();
            CreateFriendsTipsColumns();
        }

        // not a great way of building a connection. unsafe to show user businessname and password.
        private string GetConnectionString()
        {
            // need to update this for everyone's personal machines
            //                  ---------------------------------------------------------------------
            //                                       |                                              |
            //                                       v                                              v
            return "Host = localhost; Username = postgres; Database = milestone2; password= 'SegaSaturn'";
        }

        private void ExecuteQuery(string sqlstr, Action<NpgsqlDataReader> myf)
        {
            if (this.Connection.State == System.Data.ConnectionState.Closed)
            {
                this.Connection.Open();
            }
            var cmd = new NpgsqlCommand();
            cmd.Connection = this.Connection;
            cmd.CommandText = sqlstr;

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
                System.Windows.MessageBox.Show("SQL Error - " + er.Message.ToString());
            }

            this.Connection.Close();
        }

        private void CreateFriendsColumns()
        {
            DataGridTextColumn fnames = new DataGridTextColumn();
            DataGridTextColumn flikes = new DataGridTextColumn();
            DataGridTextColumn fstars = new DataGridTextColumn();
            DataGridTextColumn fdate = new DataGridTextColumn();

            fnames.Binding = new Binding("Name");
            flikes.Binding = new Binding("Stars");
            fstars.Binding = new Binding("Likes");
            fdate.Binding = new Binding("CreationDate");

            fnames.Header = "Name";
            flikes.Header = "Likes";
            fstars.Header = "Stars";
            fdate.Header = "Creation Date";

            this.FriendsGrid.Columns.Add(fnames);
            this.FriendsGrid.Columns.Add(flikes);
            this.FriendsGrid.Columns.Add(fstars);
            this.FriendsGrid.Columns.Add(fdate);

            //this.FriendsGrid.Items.Add(new User() { Name = "Taiya", Stars = 5.0F, Likes = 200000000, CreationDate = DateTime.Now });
        }

        private void CreateFriendsTipsColumns()
        {
            DataGridTextColumn fname = new DataGridTextColumn();
            DataGridTextColumn fbusi = new DataGridTextColumn();
            DataGridTextColumn fcity = new DataGridTextColumn();
            DataGridTextColumn ftext = new DataGridTextColumn();
            DataGridTextColumn fdate = new DataGridTextColumn();

            fname.Header = "Friend";
            fbusi.Header = "Business";
            fcity.Header = "City";
            ftext.Header = "Review";
            fdate.Header = "Date";

            this.FriendsTipsGrid.Columns.Add(fname);
            this.FriendsTipsGrid.Columns.Add(fbusi);
            this.FriendsTipsGrid.Columns.Add(fcity);
            this.FriendsTipsGrid.Columns.Add(ftext);
            this.FriendsTipsGrid.Columns.Add(fdate);
        }

        private void CreateUserIDColumns()
        {
            DataGridTextColumn uname = new DataGridTextColumn();
            DataGridTextColumn uid = new DataGridTextColumn();

            uname.Header = "User Name";
            uid.Header = "User ID";

            uname.Binding = new Binding("Name");
            uid.Binding = new Binding("ID");

            this.UserIDGrid.Columns.Add(uname);
            this.UserIDGrid.Columns.Add(uid);
        }

        private void UserNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbox = (TextBox)sender;
            if (tbox.Text.Equals("User Name"))
            {
                tbox.Text = "";
            }
            else if (this.UserNameButton.Content.Equals("Log In"))
            {
                tbox.Text = "";
                this.UserIDGrid.Items.Clear();
                this.UserNameButton.Content = "Search";
            }
        }

        private void UserNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbox = (TextBox)sender;
            if (tbox.Text.Equals(""))
            {
                tbox.Text = "User Name";
            }
        }

        private void UserNameButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            if (b.Content.Equals("Search") && this.UserNameTextBox.Text != "User Name")
            {
                b.Content = "Log In";
                string uname = this.UserNameTextBox.Text;
                string sqlcall = "SELECT UserID, UserName FROM Users WHERE UserName = '" + uname + "'";

                ExecuteQuery(sqlcall, AddItemsToUserIDGrid);
            }
            else if (b.Content.Equals("Log In") &&
                this.UserIDGrid.SelectedItem != null)
            {
                b.Content = "Log Out";
                var cell = this.UserIDGrid.SelectedCells[1];
                var content = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
                string sqlcall = "SELECT * FROM Users WHERE UserID = '" + content.ToString() + "'";

                ExecuteQuery(sqlcall, AttemptLogIn);
            }
            else if (b.Content.Equals("Log Out"))
            {
                this.UserIDGrid.Items.Clear();
                b.Content = "Search";
            }
        }

        private void LogUserIn(NpgsqlDataReader reader)
        {
            this.UserAcct = new User()
            {
                ID = reader.GetString(0),
                Name = reader.GetString(1)
            };
        }

        private void AddItemsToUserIDGrid(NpgsqlDataReader reader)
        {
            this.UserIDGrid.Items.Clear();
            this.UserIDGrid.Items.Add(new User() { ID = reader.GetString(0), Name = reader.GetString(1) });
        }

        private void AttemptLogIn(NpgsqlDataReader reader)
        {
            /*
             * 
                UserID          TEXT NOT NULL,
                CreationDate    TIMESTAMP NOT NULL DEFAULT NOW(),
                UserName        TEXT,
                TotalLikes      INTEGER DEFAULT (0),
                TipCount        INTEGER DEFAULT (0),
                FansRating      INTEGER,
                FunnyRating     INTEGER,
                CoolRating      INTEGER,
                AvgStarRating   DECIMAL (3, 2),
                Latitude        DECIMAL (8, 6),
                Longitude       DECIMAL (9, 6),
             */

            float lat = (reader.IsDBNull(9) == false) ? reader.GetFloat(9) : 0.0F;
            float lng = (reader.IsDBNull(10) == false) ? reader.GetFloat(10) : 0.0F;
            
            this.UserAcct = new User()
            {
                ID = reader.GetString(0),
                CreationDate = reader.GetDateTime(1),
                Name = reader.GetString(2),
                Likes = reader.GetInt32(3),
                Tips = reader.GetInt32(4),
                Fans = reader.GetInt32(5),
                FunnyRates = reader.GetInt32(6),
                CoolRates = reader.GetInt32(7),
                UsefulRates = 0,
                Stars = reader.GetFloat(8),
                Latitude = lat,
                Longitude = lng
            };

            this.UserInfoNameTextBox.Text = this.UserAcct.Name;
            this.UserInfoStarsTextBox.Text = this.UserAcct.Stars.ToString();
            this.UserInfoFansTextBox.Text = this.UserAcct.Fans.ToString();
            this.UserInfoCreationTextBox.Text = this.UserAcct.CreationDate.ToString();
            this.UserInfoFunnyTextBox.Text = this.UserAcct.FunnyRates.ToString();
            this.UserInfoCoolTextBox.Text = this.UserAcct.CoolRates.ToString();
            this.UserInfoUsefulTextBox.Text = this.UserAcct.UsefulRates.ToString();
            this.UserInfoTipsTextBox.Text = this.UserAcct.Tips.ToString();
            this.UserInfoLikesTextBox.Text = this.UserAcct.Likes.ToString();
            this.UserInfoLatTextBox.Text = this.UserAcct.Latitude.ToString();
            this.UserInfoLongTextBox.Text = this.UserAcct.Longitude.ToString();
        }

        private void UserIDGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If I need it.
        }
    }
}
