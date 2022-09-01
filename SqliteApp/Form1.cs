using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqliteApp
{
    public partial class Form1 : Form
    {
        SQLiteConnection conn;
        string databaseName = @"sample.db";        
        public Form1()
        {
            InitializeComponent();

            changeCultureInfo("en-US");

            #region SQLite
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + databaseName) == false)
            {
                string createTable = @"CREATE TABLE IF NOT EXISTS 
                                        [Mytable] (
                                        [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
                                        [Name] NVARCHAR(250) NULL, 
                                        [Gender] NVARCHAR(5) NULL, 
                                        [Age] INT NULL,
                                        [Images] BLOB NULL, 
                                        [ImagesText] TEXT NULL,     
                                        [TimeStamp] NVARCHAR(20) NULL)";

                SQLiteConnection.CreateFile(databaseName);
                conn = new SQLiteConnection("Data Source=" + databaseName);
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    conn.Open();
                    cmd.CommandText = createTable;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }


            conn = new SQLiteConnection("Data Source=" + databaseName);
            conn.Open();
            #endregion
        }
        private void changeCultureInfo(string lang)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture; 
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            string base64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(@"1.jpg"));
            byte[] newByte = System.IO.File.ReadAllBytes(@"1.jpg");
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            
            string query = String.Format("INSERT INTO Mytable(Name, Gender, Age, Images, ImagesText, TimeStamp) VALUES ('{0}', '{1}', {2}, '{3}', '{4}' ,'{5}')", "Name1", "Male", 30, newByte, base64, timeStamp);
            SQLiteCommand command = new SQLiteCommand(query, conn);
            command.ExecuteNonQuery();            

            viewSqlite();
        }

        private void viewSqlite()
        {
            listBox1.Items.Clear();
            SQLiteDataAdapter da = new SQLiteDataAdapter("SELECT * FROM Mytable", conn);
            DataTable dt = new DataTable("dt");
            da.Fill(dt);
            da = null;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn.State != ConnectionState.Closed)
                conn.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ///
        }
    }
}
