using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class ManageUsersScreen : Form
    {
        public string Username;
        private Form _previousForm;
        public ManageUsersScreen(string username, Form previousForm)
        {
            Username = username;
            InitializeComponent();
            _previousForm = previousForm;
        }

        private int choise = 0;
        public string connectionString = "Server = LAPTOP-OKO0VKK3; Database = MaintenanceInventoryDB; Trusted_Connection=True;";
        private void ManageUsersScreen_Load(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            listView1.View = View.Details;
            // Clear existing items and columns in the ListView
            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("UserId");
            listView1.Columns.Add("UserName");
            listView1.Columns.Add("eMail");
            listView1.Columns.Add("Role_");
            listView1.Columns.Add("CreateDate");
            listView1.Columns.Add("AccountStatus");

            getUsers();
        }
        private void getUsers()
        {
            listView1.Items.Clear();
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "SELECT UserId,UserName,eMail,Role_,CreateDate,AccountStatus FROM Users";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //listview e yaz
                    ListViewItem item = new ListViewItem(reader["UserId"].ToString());
                    item.SubItems.Add(reader["UserName"].ToString());
                    item.SubItems.Add(reader["eMail"].ToString());
                    item.SubItems.Add(reader["Role_"].ToString());
                    item.SubItems.Add(reader["CreateDate"].ToString());
                    item.SubItems.Add(reader["AccountStatus"].ToString());

                    listView1.Items.Add(item);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) //add new user
        {
            choise = 1; //add new user
            groupBox1.Enabled = true;
            label3.Text = "Password:";
            textBox2.Text = "123456";
            textBox2.Enabled = false;
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dateTimePicker1.Enabled = false;
        }
        private void button2_Click(object sender, EventArgs e) //edit user
        {
            if(listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a user from the list to edit.");
                return;
            }

            choise = 2;//edit user

            groupBox1.Enabled = true;
            textBox1.Enabled = false;//username
            textBox2.Enabled = false;//passwordhash
            textBox3.Enabled = false;//email
            dateTimePicker1.Enabled = false;
            string pHash = textBox2.Text;
            textBox2.Text = "****".ToString();

            string selectedUserName = listView1.SelectedItems[0].SubItems[1].Text;
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "SELECT UserName, eMail, Role_, CreateDate FROM Users WHERE UserName = @username";
                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@username", selectedUserName);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    textBox1.Text = reader["UserName"].ToString();
                    textBox3.Text = reader["eMail"].ToString();
                    if(reader["Role_"].ToString() == "0")
                    {
                        checkBox1.Checked = true; // Admin
                        checkBox2.Checked = false; // User
                    }
                    else if (reader["Role_"].ToString() == "1")
                    {
                        checkBox1.Checked = false; // Admin
                        checkBox2.Checked = true; // User
                    }
                    else
                    {
                        checkBox1.Checked = false;
                        checkBox2.Checked = false;
                    }
                    dateTimePicker1.Text = reader["CreateDate"].ToString();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e) //Save Button
        {
            if (choise == 2)
            {
                editUser();
                getUsers();
            }
            else if (choise == 1)
            {
                addNewUser();
                getUsers();
            }
            else
            {
                MessageBox.Show("Please select an action to perform.");
            }
        }

        private void button4_Click(object sender, EventArgs e) //previous screen
        {
            this.Close();
            _previousForm.Show();
        }

        public void editUser()
        {
            string selectedUserName = listView1.SelectedItems[0].SubItems[1].Text;
            int role_ = 1;
            string status = comboBox1.SelectedItem.ToString();
            if (checkBox1.Checked)
            {
                role_ = 0;
            }
            else if (checkBox2.Checked)
            {
                role_ = 1;
            }
            else
            {
                MessageBox.Show("You must select a choise");
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "UPDATE Users SET Role_=@role_, AccountStatus=@status WHERE UserName=@username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@role_", role_);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@username", selectedUserName);
                cmd.ExecuteReader();

                con.Close();
                MessageBox.Show("This process is successful!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ManageProduct manageProduct = new ManageProduct(Username, this);
                Logger.AddLog(manageProduct.getuserid(Username), "Edit User", "Users", manageProduct.getuserid(selectedUserName), $"User {selectedUserName} has been edited successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void addNewUser()
        {
            string username = textBox1.Text;
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter a username.");
                return;
            }

            string password = textBox2.Text;
            SignUpScreen signUpScreen = new SignUpScreen();
            string passwordhash = signUpScreen.getPasswordHash(password); // Assuming this method exists in SignUpScreen

            string email = textBox3.Text;
            int role_ = 0; // Default role is User
            if (checkBox2.Checked)
            {
                role_ = 1; // If User checkbox is checked, set role to Admin
            }
            else if (checkBox1.Checked)
            {
                role_ = 0; // If Admin checkbox is checked, set role to User
            }
            else
            {
                MessageBox.Show("You must select a role.");
                return;
            }
            string accountStatus = "Active"; // Default account status

            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "INSERT INTO Users(UserName, eMail, PasswordHash, Role_, CreateDate, AccountStatus) VALUES (@username, @eMail, @passwordhash, @role_, @createDate, @accountStatus)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@eMail", email);
                cmd.Parameters.AddWithValue("@passwordhash", passwordhash);
                cmd.Parameters.AddWithValue("@role_", role_);
                cmd.Parameters.AddWithValue("@createDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@accountStatus", accountStatus);
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("New user has been created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ManageProduct manageProduct = new ManageProduct(Username, this);
                Logger.AddLog(manageProduct.getuserid(Username), "Add User", "Users", manageProduct.getuserid(username), $"New user {username} has been created successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
