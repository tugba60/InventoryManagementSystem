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
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private string username, storedhash;
        private string connectionString = "Server = LAPTOP-OKO0VKK3; Database = MaintenanceInventoryDB; Trusted_Connection=True;";
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null || textBox2.Text == null)
            {
                MessageBox.Show("Please, enter your username and your password", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                username = textBox1.Text;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "SELECT PasswordHash FROM Users WHERE UserName=@username";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        storedhash = reader.GetString(0);
                    }
                }
                bool loginSuccess = VerifyPassword(textBox2.Text, storedhash);

                if (loginSuccess)
                {
                    MessageBox.Show("Login successful!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ManageProduct manageProduct = new ManageProduct(username, this);
                    Logger.AddLog(manageProduct.getuserid(username), "Login", "Users", manageProduct.getuserid(username), "User logged in successfully.");
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "SELECT Role_ FROM Users WHERE UserName=@username";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@username", username);

                        bool userRole;
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            userRole = Convert.ToBoolean(reader["Role_"]);
                            con.Close();
                            if (userRole) //true-1-admin
                            {
                                username = textBox1.Text.Trim();
                                forAdminScreen forAdminScreen = new forAdminScreen(username,this);
                                forAdminScreen.MdiParent = this.MdiParent;
                                forAdminScreen.Show();
                                this.Close();
                            }
                            else
                            {
                                username = textBox1.Text.Trim();
                                forUsersScreen forUsersScreen = new forUsersScreen(username,this);
                                forUsersScreen.MdiParent = this.MdiParent;
                                forUsersScreen.Show();
                                this.Close();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ManageProduct manageProduct = new ManageProduct(username, this);
                    Logger.AddLog(manageProduct.getuserid(username), "Login Failed", "Users", manageProduct.getuserid(username), "User failed to log in.");
                }
            }
        }

        public bool VerifyPassword(string enteredPassword, string storedhash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedhash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes,0,salt,0,16);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for(int i = 0; i < 20; i++)
                if (hashBytes[i+16] != hash[i])
                    return false;
                
            return true;
            
        }
        
    }
}
