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
using static InventoryManagementSystem.SignUpScreen;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace InventoryManagementSystem
{
    public partial class forUsersScreen : Form
    {
        public string Username;
        private Form _previousForm;
        public forUsersScreen(string username, Form previousForm)
        {
            Username = username;
            InitializeComponent();
            _previousForm = previousForm;
        }
        public string connectionString = "Server = LAPTOP-OKO0VKK3; Database = MaintenanceInventoryDB; Trusted_Connection=True;";
        private void forUsersScreen_Load(object sender, EventArgs e)
        {
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Visible = false; // change password groupbox is invisible at the beginning
            // lblWelcome a isim yazdır.
            lblWelcome.Text = "Welcome, " + Username + "!";
            VerileriListele();
        }

        private void VerileriListele() // while loading form
        {
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
        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You must type any pozitive number!","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int unit = Convert.ToInt32(textBox1.Text);
                if (unit < 0)// negative
                {
                    MessageBox.Show("Please enter pozitive number or zero!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void trans_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an item from the list to perform a transaction.");
                return;
            }
            MessageBox.Show("Please enter information for stock transaction.","Information",MessageBoxButtons.OK, MessageBoxIcon.Information);
            groupBox2.Enabled = true;
            groupBox3.Enabled = false;
           
        }
        private void maintenance_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an item from the list to perform maintenance.");
                return;
            }
            MessageBox.Show("Please enter description and status for maintenance.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            groupBox3.Enabled = true;
            groupBox2.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e) //for transaction's button
        {
            byte type = 2;
            if (radioButton1.Checked)
            {
                type = 0; // stock out
            }
            else if (radioButton2.Checked)
            {
                type = 1; // stock in
            }
            else
            {
                MessageBox.Show("Please select a transaction type.");
                return;
            }
            int amount = textBox1.Text == "" ? 0 : Convert.ToInt32(textBox1.Text);
            string unit = comboBox1.SelectedItem.ToString();
            string note = textBox2.Text;
            int productId = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);
            int userid = getUserId(Username);
            if (userid == -1)
            {
                MessageBox.Show("User not found. Please check your username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime transactionDate = DateTime.Now;

            ExecuteDBTrans(productId, userid, type, amount, transactionDate, note);
        }
        private void button2_Click(object sender, EventArgs e) //for maintenance's button
        {
            int productId = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);
            int userid = getUserId(Username);
            if (userid == -1)
            {
                MessageBox.Show("User not found. Please check your username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime processDate = DateTime.Now;
            string description = textBox3.Text;
            string statusAfter = comboBox2.SelectedItem.ToString();

            ExecuteDBMain(productId,userid,processDate,description,statusAfter);
            
        }
        private int getUserId(string username)
        {
            int Id;
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "SELECT UserId FROM Users WHERE UserName=@username";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    Id = Convert.ToInt32(result);
                    return Id; // User ID found
                }
                return -1; // User not found
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1; // Error occurred
            }
           
        }
        private void ExecuteDBTrans(int productId, int userid, byte type, int amount, DateTime transactionDate, string note) //for transaction
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                string query = "INSERT INTO Transactions(Productid, Userid, TransactionType, Amount, TransactionDate, Note) VALUES (@productId,@userid,@type, @amount, @transactionDate, @note)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productId", productId);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@transactionDate", transactionDate);
                cmd.Parameters.AddWithValue("@note", note);

                cmd.ExecuteReader();
                conn.Close();
                MessageBox.Show("This process is successful!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ManageProduct manageProduct = new ManageProduct(Username, this);
                Logger.AddLog(manageProduct.getuserid(Username), "Transaction", "Products", productId, $"Transaction performed successfully.");
            }
            catch (Exception e) 
            {
                MessageBox.Show($"An error occurred while executing the database operation: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void ExecuteDBMain(int productid,int userid,DateTime date,string descrip,string status) //for maintenance
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                string query = "INSERT INTO MainTenance(ProductId, PerformedByUserId, ProcessDate, Description_, StatusAfter) VALUES (@productId,@userid,@date, @descrip, @status)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productId", productid);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@descrip", descrip);
                cmd.Parameters.AddWithValue("@status", status);

                cmd.ExecuteReader();
                conn.Close();
                MessageBox.Show("This process is successful!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ManageProduct manageProduct = new ManageProduct(Username, this);
                Logger.AddLog(manageProduct.getuserid(Username), "Maintenance", "Products", productid, $"Maintenance performed successfully.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occurred while executing the database operation: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        
        private void showTransactionsReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // listview i temizle
            listView1.Items.Clear();
            listView1.Columns.Clear();

            label1.Text = "Transactions Report";
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
        private void showMainTenanceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // listview i temizle
            listView1.Items.Clear();
            listView1.Columns.Clear();

            label1.Text = "Manintenance Report";
            listView1.View = View.Details;
            listView1.Columns.Add("ManinTenanceId", 50);
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
                    ListViewItem item = new ListViewItem(reader["ManinTenanceId"].ToString());
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

        private void exitToolStripMenuItem_Click_1(object sender, EventArgs e) //MyAccount->Exit
        {
            this.Close();
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.MdiParent = this.MdiParent;
            loginScreen.Show();
        }

        private void textBox3_TextChanged(object sender, EventArgs e){}

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e) // cahange password button
        {
            string newpassword = textBox4.Text;
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
                    groupBox4.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
