using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class forAdminScreen : Form
    {
        public string Username;
        public string connectionString = "Server = LAPTOP-OKO0VKK3; Database = MaintenanceInventoryDB; Trusted_Connection=True;";
        private Form _previousForm;
        public forAdminScreen(string username,Form previous)
        {
            Username = username;
            _previousForm = previous;
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.MdiParent = this.MdiParent;
            loginScreen.Show();
        }

        private void manageProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageProduct manageProduct = new ManageProduct(Username,this);
            manageProduct.MdiParent = this.MdiParent;
            manageProduct.Show();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageUsersScreen manageUsersScreen = new ManageUsersScreen(Username,this);
            manageUsersScreen.MdiParent = this.MdiParent;
            manageUsersScreen.Show();
        }

        private void forAdminScreen_Load(object sender, EventArgs e)
        {
            label1.Text = "Welcome, " + Username + "!";
            listView1.Visible = false;
            groupBox1.Visible = false;
        }

        private void viewProductReportsToolStripMenuItem_Click(object sender, EventArgs e) // Table Transavtions
        {
            listView1.Visible = true;

            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.View = View.Details;

            listView1.Columns.Add("TransactionId", 50);
            listView1.Columns.Add("Productid", 100);
            listView1.Columns.Add("Userid", 100);
            listView1.Columns.Add("TransactionType", 100);
            listView1.Columns.Add("Amount", 100);
            listView1.Columns.Add("TransactionDate", 100);
            listView1.Columns.Add("Note", 100);

            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "SELECT * FROM Transactions";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //listview e yaz
                    ListViewItem item = new ListViewItem(reader["TransactionId"].ToString());
                    item.SubItems.Add(reader["Productid"].ToString());
                    item.SubItems.Add(reader["Userid"].ToString());
                    item.SubItems.Add(reader["TransactionType"].ToString());
                    item.SubItems.Add(reader["Amount"].ToString());
                    item.SubItems.Add(reader["TransactionDate"].ToString());
                    item.SubItems.Add(reader["Note"].ToString());

                    listView1.Items.Add(item);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void viewInventoryToolStripMenuItem_Click(object sender, EventArgs e) // Table Products
        {
            listView1.Visible = true;

            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.View = View.Details;

            listView1.Columns.Add("ID", 50);
            listView1.Columns.Add("ProductName", 100);
            listView1.Columns.Add("ProductCode", 100);
            listView1.Columns.Add("Category", 100);
            listView1.Columns.Add("BrandModel", 100);
            listView1.Columns.Add("Quantity", 100);
            listView1.Columns.Add("Unit", 100);
            listView1.Columns.Add("EntryDate", 100);
            listView1.Columns.Add("WarrantyEndDate", 100);
            listView1.Columns.Add("Location_", 100);
            listView1.Columns.Add("Status_", 100);
            listView1.Columns.Add("Description_", 100);
            listView1.Columns.Add("AddedByUserID", 100);

            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //listview e yaz
                    ListViewItem item = new ListViewItem(reader["ProductId"].ToString());
                    item.SubItems.Add(reader["ProductName"].ToString());
                    item.SubItems.Add(reader["ProductCode"].ToString());
                    item.SubItems.Add(reader["Category"].ToString());
                    item.SubItems.Add(reader["BrandModel"].ToString());
                    item.SubItems.Add(reader["Quantity"].ToString());
                    item.SubItems.Add(reader["Unit"].ToString());
                    item.SubItems.Add(reader["EntryDate"].ToString());
                    item.SubItems.Add(reader["WarrantyEndDate"].ToString());
                    item.SubItems.Add(reader["Location_"].ToString());
                    item.SubItems.Add(reader["Status_"].ToString());
                    item.SubItems.Add(reader["Description_"].ToString());
                    item.SubItems.Add(reader["AddedByUserID"].ToString());
                    listView1.Items.Add(item);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void maintenanceRepairReportsToolStripMenuItem_Click(object sender, EventArgs e)//Table MainTenance
        {
            listView1.Visible = true;

            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.View = View.Details;

            listView1.Columns.Add("MainTenanceId", 50);
            listView1.Columns.Add("ProductId", 100);
            listView1.Columns.Add("PerformedByUserId", 100);
            listView1.Columns.Add("ProcessDate", 100);
            listView1.Columns.Add("Description_", 100);
            listView1.Columns.Add("StatusAfter", 100);

            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "SELECT * FROM MainTenance";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //listview e yaz
                    ListViewItem item = new ListViewItem(reader["MainTenanceId"].ToString());
                    item.SubItems.Add(reader["ProductId"].ToString());
                    item.SubItems.Add(reader["PerformedByUserId"].ToString());
                    item.SubItems.Add(reader["ProcessDate"].ToString());
                    item.SubItems.Add(reader["Description_"].ToString());
                    item.SubItems.Add(reader["StatusAfter"].ToString());

                    listView1.Items.Add(item);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void userManagementToolStripMenuItem_Click(object sender, EventArgs e) // Table Users
        {
            listView1.Visible = true;

            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("UserId");
            listView1.Columns.Add("UserName");
            listView1.Columns.Add("eMail");
            listView1.Columns.Add("Role_");
            listView1.Columns.Add("CreateDate");
            listView1.Columns.Add("AccountStatus");

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

        private void loginHistoryToolStripMenuItem_Click(object sender, EventArgs e) //Logs
        {
            listView1.Visible = true;
            listView1.View = View.Details;
            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("LogId");
            listView1.Columns.Add("UserId");
            listView1.Columns.Add("Action");
            listView1.Columns.Add("TableAffected");
            listView1.Columns.Add("RecordId");
            listView1.Columns.Add("logDescription_");
            listView1.Columns.Add("ActionDate");

            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "SELECT * FROM Logs";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //listview e yaz
                    ListViewItem item = new ListViewItem(reader["LogId"].ToString());
                    item.SubItems.Add(reader["UserId"].ToString());
                    item.SubItems.Add(reader["Action_"].ToString());
                    item.SubItems.Add(reader["TableAffected"].ToString());
                    item.SubItems.Add(reader["RecordId"].ToString());
                    item.SubItems.Add(reader["logDescription_"].ToString());
                    item.SubItems.Add(reader["ActionDate"].ToString());

                    listView1.Items.Add(item);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int indexer=0;
        private void arkaPlanRengiDeğiştirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (indexer)
            {
                case 0:
                    this.BackColor = Color.Blue;
                    break;
                case 1:
                    this.BackColor = Color.GreenYellow;
                    break;
                case 2:
                    this.BackColor = Color.Red;
                    break;
                case 3:
                    this.BackColor = Color.Orange;
                    break;
                case 4:
                    this.BackColor = Color.Pink;
                    break;
                case 5:
                    this.BackColor = Color.Purple;
                    break;
                default:
                    this.BackColor = Color.Black;
                    break;
            }
            indexer++;
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Visible = false;
            groupBox1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newpassword = textBox1.Text;
            SignUpScreen signUpScreen = new SignUpScreen();
            string passwordHash = signUpScreen.getPasswordHash(newpassword);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    string query = "UPDATE Users SET PasswordHash = @passwordHash WHERE UserName = @username";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@username", Username);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Password has been changed successfully.");
                    ManageProduct manageProduct = new ManageProduct(Username, this);
                    Logger.AddLog(manageProduct.getuserid(Username), "Update", "Users", manageProduct.getuserid(Username), $"Password has been changed successfully.");
                    groupBox1.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
