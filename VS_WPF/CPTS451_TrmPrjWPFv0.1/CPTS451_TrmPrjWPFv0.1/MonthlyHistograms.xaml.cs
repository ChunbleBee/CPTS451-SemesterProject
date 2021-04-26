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
    /// Interaction logic for MonthlyHistograms.xaml
    /// </summary>
    public partial class MonthlyHistograms : Window
    {
        private Business busi { get; set; }

        public MonthlyHistograms(string uid, string bid)
        {
            InitializeComponent();
            busi = new Business() { BusinessID = bid };
            this.SetMonthlyCheckIns();
        }

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

        private void SetMonthlyCheckIns()
        {

            string bid = this.busi.BusinessID;

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=1
                            AND Businesses.BusinessID='" + bid + "';", SetJanuaryCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=2
                            AND Businesses.BusinessID='" + bid + "';", SetFebruaryCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=3
                            AND Businesses.BusinessID='" + bid + "';", SetMarchCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=4
                            AND Businesses.BusinessID='" + bid + "';", SetAprilCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=5
                            AND Businesses.BusinessID='" + bid + "';", SetMayCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=6
                            AND Businesses.BusinessID='" + bid + "';", SetJuneCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=7
                            AND Businesses.BusinessID='" + bid + "';", SetJulyCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=8
                            AND Businesses.BusinessID='" + bid + "';", SetAugustCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=9
                            AND Businesses.BusinessID='" + bid + "';", SetSeptemberCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=10
                            AND Businesses.BusinessID='" + bid + "';", SetOctoberCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=11
                            AND Businesses.BusinessID='" + bid + "';", SetNovemberCheckIns);

            ExecuteQuery(@"SELECT COUNT(*) FROM Businesses, CheckIns
                            WHERE Businesses.BusinessID=CheckIns.BusinessID
                            AND EXTRACT(MONTH FROM CheckIns.CheckInDate)=12
                            AND Businesses.BusinessID='" + bid + "';", SetDecemberCheckIns);
        }

        private void SetJanuaryCheckIns(NpgsqlDataReader reader)
        {
            int numJanCheckIns = reader.GetInt32(0);
            numJanCheckIns = Math.Min(numJanCheckIns, 90);

            double newheight = ((double)numJanCheckIns) / 90.0;

            this.JanRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetFebruaryCheckIns(NpgsqlDataReader reader)
        {
            int numFebCheckIns = reader.GetInt32(0);
            numFebCheckIns = Math.Min(numFebCheckIns, 90);

            double newheight = ((double)numFebCheckIns) / 90.0;

            this.FebRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetMarchCheckIns(NpgsqlDataReader reader)
        {
            int numMarCheckIns = reader.GetInt32(0);
            numMarCheckIns = Math.Min(numMarCheckIns, 90);

            double newheight = ((double)numMarCheckIns) / 90.0;

            this.MarRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetAprilCheckIns(NpgsqlDataReader reader)
        {
            int numAprCheckIns = reader.GetInt32(0);
            numAprCheckIns = Math.Min(numAprCheckIns, 90);

            double newheight = ((double)numAprCheckIns) / 90.0;

            this.AprRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetMayCheckIns(NpgsqlDataReader reader)
        {
            int numMayCheckIns = reader.GetInt32(0);
            numMayCheckIns = Math.Min(numMayCheckIns, 90);

            double newheight = ((double)numMayCheckIns) / 90.0;

            this.MayRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetJuneCheckIns(NpgsqlDataReader reader)
        {
            int numJunCheckIns = reader.GetInt32(0);
            numJunCheckIns = Math.Min(numJunCheckIns, 90);

            double newheight = ((double)numJunCheckIns) / 90.0;

            this.JunRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetJulyCheckIns(NpgsqlDataReader reader)
        {
            int numJulCheckIns = reader.GetInt32(0);
            numJulCheckIns = Math.Min(numJulCheckIns, 90);

            double newheight = ((double)numJulCheckIns) / 90.0;

            this.JulRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetAugustCheckIns(NpgsqlDataReader reader)
        {
            int numAugCheckIns = reader.GetInt32(0);
            numAugCheckIns = Math.Min(numAugCheckIns, 90);

            double newheight = ((double)numAugCheckIns) / 90.0;

            this.AugRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetSeptemberCheckIns(NpgsqlDataReader reader)
        {
            int numSepCheckIns = reader.GetInt32(0);
            numSepCheckIns = Math.Min(numSepCheckIns, 90);

            double newheight = ((double)numSepCheckIns) / 90.0;

            this.SepRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetOctoberCheckIns(NpgsqlDataReader reader)
        {
            int numOctCheckIns = reader.GetInt32(0);
            numOctCheckIns = Math.Min(numOctCheckIns, 90);

            double newheight = ((double)numOctCheckIns) / 90.0;

            this.OctRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetNovemberCheckIns(NpgsqlDataReader reader)
        {
            int numNovCheckIns = reader.GetInt32(0);
            numNovCheckIns = Math.Min(numNovCheckIns, 90);

            double newheight = ((double)numNovCheckIns) / 90.0;

            this.NovRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void SetDecemberCheckIns(NpgsqlDataReader reader)
        {
            int numDecCheckIns = reader.GetInt32(0);
            numDecCheckIns = Math.Min(numDecCheckIns, 90);

            double newheight = ((double)numDecCheckIns) / 90.0;

            this.DecRect.RenderTransform = new ScaleTransform(1.0, newheight);
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime now = DateTime.Now;
            string sqlcall = "INSERT INTO CheckIns (BusinessID, CheckInDate, CheckInTime) VALUES ('" +
                this.busi.BusinessID + "', '" +
                now.Date.ToString().Replace(" 12:00:00 AM","") + "', '" +
                now.TimeOfDay.ToString() + "');";

            this.ExecuteNonQuery(sqlcall);
            this.SetMonthlyCheckIns();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ((MainWindow)this.Owner).ReloadContext();
        }
    }
}
