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
        //public delegate void AsyncCall(NpgsqlDataReader reader); //Was an attempt as asyncronous data gathering

        private User UserAcct { get; set; }

        public partial class Business
        {
            public string BusinessID { get; set; }
            public string BusinessName { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
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

        public class TipHelper
        {
            //UserName, BusinessName, City, Text, Date
            public string UserName { get; set; }
            public string BusinessName { get; set; }
            public string City { get; set; }
            public string Text { get; set; }
            public DateTime CreationDate { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            // User Page Tab initialization
            this.UserAcct = null;
            CreateUserIDColumns();
            CreateFriendsColumns();
            CreateFriendsTipsColumns();

            //Business Search Tab initialization
            this.StateComboBox.Items.Add("State");
            this.StateComboBox.SelectedIndex = 0;
            ExecuteQuery("SELECT DISTINCT State FROM Businesses ORDER BY State ASC", AddStateToStateComboBox);
            CreateBusinessSearchColumns();
        }

        // not a great way of building a connection. unsafe to show user businessname and password.
        private string GetConnectionString()
        {
            // need to update this for everyone's personal machines
            //                  ---------------------------------------------------------------------
            //                                       v                                              v
            return "Host = localhost; Username = postgres; Database = milestone2; password= 'SegaSaturn'";
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


        //////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////// USER TAB STUFFS/////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////
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

            fname.Binding = new Binding("UserName");
            fbusi.Binding = new Binding("BusinessName");
            fcity.Binding = new Binding("City");
            ftext.Binding = new Binding("Text");
            fdate.Binding = new Binding("CreationDate");

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
                this.UserIDGrid.Items.Clear();
                string uname = this.UserNameTextBox.Text;
                string sqlcall = "SELECT UserID, UserName FROM Users WHERE UserName = '" + uname + "'";

                ExecuteQuery(sqlcall, AddItemsToUserIDGrid);
            }
            else if (b.Content.Equals("Log In") &&
                this.UserIDGrid.SelectedItem != null)
            {
                this.UserAcct = null;

                b.Content = "Log Out";
                var cell = this.UserIDGrid.SelectedCells[1];
                var content = ((TextBlock)cell.Column.GetCellContent(cell.Item)).Text;
                string sqlcall = "SELECT * FROM Users WHERE UserID = '" + content.ToString() + "'";

                ExecuteQuery(sqlcall, AttemptLogIn);

                if (this.UserAcct != null)
                {
                    sqlcall = "SELECT UserID, UserName, TotalLikes, AvgStarRating, CreationDate FROM Users, Friends WHERE (User01='" + this.UserAcct.ID + "') AND (User02=UserID);";
                    ExecuteQuery(sqlcall, AddFriends);

                    sqlcall = @"SELECT UserName, BusinessName, City, Text, Date
                                FROM Users, Businesses, Friends, (
	                                SELECT Tips.UserID, Tips.BusinessID, Tips.Text, Tips.Date
	                                FROM Tips
	                                LEFT JOIN Tips AS NewerDate
	                                ON Tips.UserID = NewerDate.UserID
	                                AND NewerDate.Date > Tips.Date
	                                WHERE NewerDate.UserID IS NULL
	                                ORDER BY Tips.UserID
                                ) AS FilteredTips
                                WHERE (Users.UserID = FilteredTips.UserID)
                                AND (Businesses.BusinessID = FilteredTips.BusinessID)
                                AND (Friends.User02 = FilteredTips.UserID)
                                AND Friends.User01 = '" + this.UserAcct.ID + @"'
                                ORDER BY Date DESC;";
                    ExecuteQuery(sqlcall, AddFriendTips);
                }
            }
            else if (b.Content.Equals("Log Out"))
            {
                this.UserIDGrid.Items.Clear();
                this.FriendsGrid.Items.Clear();
                this.FriendsTipsGrid.Items.Clear();

                this.UserInfoNameTextBox.Text = "";
                this.UserInfoStarsTextBox.Text = "";
                this.UserInfoFansTextBox.Text = "";
                this.UserInfoCreationTextBox.Text = "";
                this.UserInfoFunnyTextBox.Text = "";
                this.UserInfoCoolTextBox.Text = "";
                this.UserInfoUsefulTextBox.Text = "";
                this.UserInfoTipsTextBox.Text = "";
                this.UserInfoLikesTextBox.Text = "";
                this.UserInfoLatTextBox.Text = "";
                this.UserInfoLongTextBox.Text = "";

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
            this.UserIDGrid.Items.Add(new User() { ID = reader.GetString(0), Name = reader.GetString(1) });
        }

        private void AttemptLogIn(NpgsqlDataReader reader)
        {
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

        private void AddFriends(NpgsqlDataReader reader)
        {
            //SELECT UserID, UserName, TotalLikes, AvgStarRating, CreationDate
            User friend = new User()
            {
                ID = reader.GetString(0),
                Name = reader.GetString(1),
                Likes = reader.GetInt32(2),
                Stars = reader.GetFloat(3),
                CreationDate = reader.GetDateTime(4)
            };

            this.FriendsGrid.Items.Add(friend);
        }

        private void AddFriendTips(NpgsqlDataReader reader)
        {

            //UserName, BusinessName, City, Text, Date
            TipHelper helper = new TipHelper()
            {
                UserName = reader.GetString(0),
                BusinessName = reader.GetString(1),
                City = reader.GetString(2),
                Text = reader.GetString(3),
                CreationDate = reader.GetDateTime(4)
            };

            this.FriendsTipsGrid.Items.Add(helper);
        }

        private void UserIDGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If I need it.
        }

        private void UserInfoEditButton_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            if (this.UserAcct != null)
            {
                if (b.Content.Equals("Edit"))
                {
                    b.Content = "Update";
                    this.UserInfoNameTextBox.IsEnabled = true;
                    this.UserInfoLatTextBox.IsEnabled = true;
                    this.UserInfoLongTextBox.IsEnabled = true;

                    this.UserInfoNameTextBox.IsReadOnly = false;
                    this.UserInfoLatTextBox.IsReadOnly = false;
                    this.UserInfoLongTextBox.IsReadOnly = false;
                }
                else if (b.Content.Equals("Update"))
                {
                    b.Content = "Edit";

                    this.UserInfoNameTextBox.IsEnabled = false;
                    this.UserInfoLatTextBox.IsEnabled = false;
                    this.UserInfoLongTextBox.IsEnabled = false;

                    this.UserInfoNameTextBox.IsReadOnly = true;
                    this.UserInfoLatTextBox.IsReadOnly = true;
                    this.UserInfoLongTextBox.IsReadOnly = true;

                    float outlat, outlong;
                    float.TryParse(this.UserInfoLatTextBox.Text, out outlat);
                    float.TryParse(this.UserInfoLongTextBox.Text, out outlong);

                    if (this.UserInfoNameTextBox.Text.Equals(this.UserAcct.Name) == false ||
                        outlat != this.UserAcct.Latitude ||
                        outlong != this.UserAcct.Longitude)
                    {
                        this.UserAcct.Name = this.UserInfoNameTextBox.Text;
                        this.UserAcct.Latitude = outlat;
                        this.UserAcct.Longitude = outlong;

                        string sqlcall = @"UPDATE Users
                        SET
                            UserName = '" + this.UserAcct.Name + @"',
                            Latitude = " + this.UserAcct.Latitude.ToString() + @",
                            Longitude = " + this.UserAcct.Longitude.ToString() + @"
                        WHERE UserID = '" + this.UserAcct.ID + "';";
                        
                        ExecuteQuery(sqlcall, null);
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////// BUSINESS TAB STUFFS/////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////
        private void CreateBusinessSearchColumns()
        {
            // Name, Addr, City, State, Distance, Stars, Tip Count, Checkins
            DataGridTextColumn bname = new DataGridTextColumn();
            DataGridTextColumn baddr = new DataGridTextColumn();
            DataGridTextColumn bcity = new DataGridTextColumn();
            DataGridTextColumn bstate = new DataGridTextColumn();
            DataGridTextColumn bdist = new DataGridTextColumn();
            DataGridTextColumn bstars = new DataGridTextColumn();
            DataGridTextColumn btipcount = new DataGridTextColumn();
            DataGridTextColumn bcheckins = new DataGridTextColumn();

            bname.Binding = new Binding("BusinessName");
            baddr.Binding = new Binding("Street");
            bcity.Binding = new Binding("City");
            bstate.Binding = new Binding("State");
            //bdist.Binding = new Binding("Distance");
            bstars.Binding = new Binding("StarRating");
            btipcount.Binding = new Binding("NumTips");
            bcheckins.Binding = new Binding("NumCheckIns");

            bname.Header = "Name";
            baddr.Header = "Address";
            bcity.Header = "City";
            bstate.Header = "State";
            bdist.Header = "Distance";
            bstars.Header = "Avg. Stars";
            btipcount.Header = "Tip Count";
            bcheckins.Header = "Check Ins";

            this.SearchResultsGrid.Columns.Add(bname);
            this.SearchResultsGrid.Columns.Add(baddr);
            this.SearchResultsGrid.Columns.Add(bcity);
            this.SearchResultsGrid.Columns.Add(bstate);
            this.SearchResultsGrid.Columns.Add(bdist);
            this.SearchResultsGrid.Columns.Add(bstars);
            this.SearchResultsGrid.Columns.Add(btipcount);
            this.SearchResultsGrid.Columns.Add(bcheckins);
        }

        private void AddStateToStateComboBox(NpgsqlDataReader reader)
        {
            this.StateComboBox.Items.Add(reader.GetString(0));
        }

        private void AddZipCodeToListBox(NpgsqlDataReader reader)
        {
            this.ZipCodeListBox.Items.Add(reader.GetInt32(0));
        }

        private void AddCityToListBox(NpgsqlDataReader reader)
        {
            this.CityListBox.Items.Add(reader.GetString(0));
        }

        private void AddCategoryToListBox(NpgsqlDataReader reader)
        {
            this.CategoriesListBox.Items.Add(reader.GetString(0));
        }

        private void AddBusinessesToSearchResults(NpgsqlDataReader reader)
        {
            /*
            public string BusinessID { get; set; }
            public string BusinessName { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public int ZipCode { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
            public bool IsOpen { get; set; }
            public int ReviewCount { get; set; }
            public float StarRating { get; set; }
            public int NumCheckIns { get; set; }
            public int NumTips { get; set; }
            Businesses.BusinessID, BusinessName, Street, City, State, StarRating, NumTips, NumCheckIn
             */
            this.SearchResultsGrid.Items.Add(
                new Business
                {
                    BusinessID = reader.GetString(0),
                    BusinessName = reader.GetString(1),
                    Street = reader.GetString(2),
                    City = reader.GetString(3),
                    State = reader.GetString(4),
                    ZipCode = reader.GetInt32(5),
                    StarRating = (float)reader.GetDouble(6),
                    NumTips = reader.GetInt32(7),
                    NumCheckIns = reader.GetInt32(8)
                }
            );
        }

        private void StateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;

            this.CityListBox.Items.Clear();
            this.ZipCodeListBox.Items.Clear();
            this.CategoriesListBox.Items.Clear();

            if (box.SelectedIndex == 0)
            {
                ExecuteQuery("SELECT DISTINCT City FROM Businesses ORDER BY City ASC", AddCityToListBox);
                ExecuteQuery("SELECT DISTINCT ZipCode FROM Businesses ORDER BY ZipCode ASC", AddZipCodeToListBox);
                ExecuteQuery("SELECT DISTINCT Category FROM BusinessCategories ORDER BY Category ASC", AddCategoryToListBox);
            }
            else
            {
                string state = box.SelectedItem.ToString();
                ExecuteQuery("SELECT DISTINCT City FROM Businesses WHERE State='" + state + "' ORDER BY City ASC;", AddCityToListBox);
                ExecuteQuery("SELECT DISTINCT ZipCode FROM Businesses WHERE State='" + state + "' ORDER BY ZipCode ASC;", AddZipCodeToListBox);
                ExecuteQuery("SELECT DISTINCT Category FROM Businesses, BusinessCategories WHERE State='" + state +
                    "' AND Businesses.BusinessID = BusinessCategories.BusinessID ORDER BY Category ASC;", AddCategoryToListBox);
            }
        }

        private void AddHoursToSelectedBusiness(NpgsqlDataReader reader)
        {
            StringBuilder str = new StringBuilder();

        }

        private void CityListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void SelectedAttributesSearchButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sqlcall = new StringBuilder("SELECT Businesses.BusinessID, BusinessName, Street, City, State, ZipCode, StarRating, NumTips, NumCheckIns FROM Businesses");

            if (this.CategoriesListBox.SelectedItems.Count > 0)
            {
                sqlcall.Append(", BusinessCategories");
            }

            if (
                this.StateComboBox.SelectedIndex != 0 ||
                this.CityListBox.SelectedItems.Count > 0 ||
                this.ZipCodeListBox.SelectedItems.Count > 0 ||
                this.CategoriesListBox.SelectedItems.Count > 0
                )
            {
                sqlcall.Append(" WHERE ");
                if (this.StateComboBox.SelectedIndex != 0)
                {
                    sqlcall.Append("State='" + this.StateComboBox.SelectedItem.ToString() + "'");
                }

                if (this.CityListBox.SelectedItems.Count > 0)
                {
                    if (this.StateComboBox.SelectedIndex != 0)
                    {
                        sqlcall.Append(" AND ");
                    }

                    sqlcall.Append("City='" + this.CityListBox.SelectedItem.ToString() + "'");
                }

                if (this.ZipCodeListBox.SelectedItems.Count > 0)
                {

                    if (this.StateComboBox.SelectedIndex != 0 ||
                        this.CityListBox.SelectedItems.Count > 0)
                    {
                        sqlcall.Append(" AND ");
                    }

                    sqlcall.Append("ZipCode=" + this.ZipCodeListBox.SelectedItem.ToString());
                }

                if (this.CategoriesListBox.SelectedItems.Count > 0)
                {

                    if (this.StateComboBox.SelectedIndex != 0 ||
                        this.CityListBox.SelectedItems.Count > 0 ||
                        this.ZipCodeListBox.SelectedItems.Count > 0)
                    {
                        sqlcall.Append(" AND ");
                    }
                    sqlcall.Append("(");
                    foreach (var item in this.CategoriesListBox.SelectedItems)
                    {
                        sqlcall.Append("Category='" + item.ToString() + "' OR ");
                    }

                    sqlcall.Remove(sqlcall.Length - 4, 4);
                    sqlcall.Append(") AND Businesses.BusinessID = BusinessCategories.BusinessID");
                }
            }

            sqlcall.Append(" ORDER BY BusinessName ASC;");

            ExecuteQuery(sqlcall.ToString(), AddBusinessesToSearchResults);
        }

        private void SearchResultsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Business selected  = (Business)(this.SearchResultsGrid.SelectedItem);

            this.SelectedBusinessNameTextBox.Text = selected.BusinessName;
            string address = selected.Street + ", " + selected.City + ", " + selected.State + " " + selected.ZipCode.ToString();
            this.SelectedBusinessAddrTextBox.Text = address;
            
            DayOfWeek today = DateTime.Today.DayOfWeek;
            string sqlcall = "SELECT OpeningTime, ClosingTime FROM BusinessHours " +
                "WHERE BusinessID='" + selected.BusinessID + "' " +
                "AND Day='" + today.ToString() + "';";

            ExecuteQuery(sqlcall, AddHoursToSelectedBusiness);
        }
    }
}
