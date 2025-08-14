using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class SignUpScreen : Form
    {
        public enum AccountStatus
        {
            Active,
            Inactive,
            Suspended
        } 
        public SignUpScreen()
        {
            InitializeComponent();
        }
        public string username, passwordhash, eMail, accountStatus;
        int role;
        DateTime createDate;
        
        private string connectionString = "Server = LAPTOP-OKO0VKK3; Database = MaintenanceInventoryDB; Trusted_Connection=True;";
        private void button1_Click(object sender, EventArgs e)
        {
            if (isAvailable(textBox1.Text.Trim()) && MailisTrue(textBox3.Text.Trim()) && !string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                username = textBox1.Text.Trim();
                passwordhash = getPasswordHash(textBox2.Text.Trim());
                eMail = textBox3.Text.Trim();
                DateTime now = DateTime.Now;
                createDate = now;
                AccountStatus status = AccountStatus.Active;
                accountStatus=status.ToString();
                role = 0;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    try
                    {
                        string query = "INSERT INTO Users(UserName, eMail, PasswordHash, Role_, CreateDate, AccountStatus) VALUES (@username,@eMail,@passwordhash, @role, @createDate, @accountStatus)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@eMail", eMail);
                        cmd.Parameters.AddWithValue("@passwordhash", passwordhash);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.Parameters.AddWithValue("@createDate", createDate);
                        cmd.Parameters.AddWithValue("@accountStatus", accountStatus);

                        cmd.ExecuteReader();

                        MessageBox.Show("New account has created successfully.");
                        Logger.AddLog(0, "Sign Up", "Users", 0, $"New user {username} has been created successfully.");
                        forUsersScreen forUsersScreen = new forUsersScreen(username,this);
                        forUsersScreen.MdiParent = this.MdiParent;
                        forUsersScreen.Show();
                        this.Close();
                    }catch (Exception ex)
                    {
                        MessageBox.Show($"Find an error:{ex.Message}");
                    }
                }
            }
            
        }

        private void SignUpScreen_Load(object sender, EventArgs e)
        {
            warningLabel.Visible = false;
        }

        public bool isAvailable(string userName) // unique username check
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT COUNT(*) FROM Users WHERE UserName=@username";
                SqlCommand cmd = new SqlCommand(query,con); 

                cmd.Parameters.AddWithValue("@username", userName);

                int count = (int)cmd.ExecuteScalar(); // return integer

                if (count > 0)
                {
                    return false; // this username is already taken
                }

                return true;
            }
        }
        public bool MailisTrue(string mail) // email address format and null or ampty check
        {
            if (string.IsNullOrEmpty(mail))
            {
                warningLabel.Visible = true;
                warningLabel.Text = "Please enter your E-Mail";
                warningLabel.ForeColor = Color.Red;
                return false;
            }
            if(mail.EndsWith("@gmail.com"))
                return true;
            return false;
        }
        public string getPasswordHash(string password)
        {
            // Salt oluştur (16 byte)
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Hash üret (PBKDF2)
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20); // 160-bit

            // Salt + Hash birleştir
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Base64 formatında döndür
            return Convert.ToBase64String(hashBytes);

        }
    }
}
