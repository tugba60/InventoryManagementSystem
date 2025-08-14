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
    public partial class ManageProduct : Form
    {
        string Username;
        private Form _previousForm;
        public ManageProduct(string username, Form previousForm)
        {
            Username = username;
            InitializeComponent();
            _previousForm = previousForm;
        }
        public string connectionString = "Server = LAPTOP-OKO0VKK3; Database = MaintenanceInventoryDB; Trusted_Connection=True;";
       
        private void ManageProduct_Load(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            listView1.View = View.Details;
            listView1.Items.Clear();
            listView1.Columns.Clear();

            listView1.Columns.Add("ProductId");
            listView1.Columns.Add("ProductName");
            listView1.Columns.Add("ProductCode");
            listView1.Columns.Add("Category");
            listView1.Columns.Add("BrandModel");
            listView1.Columns.Add("Quantity");
            listView1.Columns.Add("Unit");
            listView1.Columns.Add("EntryDate");
            listView1.Columns.Add("WarrantyEndDate");
            listView1.Columns.Add("Location_");
            listView1.Columns.Add("Status_");
            listView1.Columns.Add("Description_");
            listView1.Columns.Add("AddedByUserId");

            getProducts();
        }
        
        public void getProducts()
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string query = "SELECT ProductId,ProductName,ProductCode,Category, BrandModel, Quantity, Unit, EntryDate, WarrantyEndDate, Location_, Status_, Description_, AddedByUserId FROM Products";
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
                    item.SubItems.Add(reader["AddedByUserId"].ToString());

                    listView1.Items.Add(item);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void label6_Click(object sender, EventArgs e){}

        private void button2_Click(object sender, EventArgs e)//add new product
        {
            groupBox1.Enabled = true;
        }
        private void button3_Click(object sender, EventArgs e)//previous button
        {
            this.Close();
            _previousForm.Show();
        }
        private void button4_Click(object sender, EventArgs e) //remove product
        {
            groupBox1.Enabled = false;
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a user from the list to edit.");
                return;
            }
            int productId = Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    string query = "DELETE FROM Products WHERE ProductId = @productId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product has been removed successfully.");
                    Logger.AddLog(getuserid(Username), "Delete", "Products", productId, $"Product with ID {productId} has been deleted successfully.");
                    listView1.Items.Clear();
                    getProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) // OK button saves changes
        {
            string productname = textBox1.Text;
            string productcode = textBox2.Text;
            string category = comboBox1.SelectedItem.ToString();
            string brandmodel = textBox3.Text;
            int quantity;
            if (int.TryParse(textBox4.Text, out quantity)) { }
            string unit = comboBox2.SelectedItem.ToString();
            DateTime entrydate = dateTimePicker1.Value;
            DateTime warrantyenddate = dateTimePicker2.Value;
            string location = textBox5.Text;
            string status = comboBox3.SelectedItem.ToString();
            string description = textBox6.Text;
            int addedbyuserid = getuserid(Username);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Products(ProductName, ProductCode, Category, BrandModel, Quantity, Unit, EntryDate, WarrantyEndDate, Location_, Status_, Description_, AddedByUserId) " +
                               "VALUES (@productname, @productcode, @category, @brandmodel, @quantity, @unit, @entrydate, @warrantyenddate, @location, @status, @description, @addedbyuserid)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@productname", productname);
                cmd.Parameters.AddWithValue("@productcode", productcode);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@brandmodel", brandmodel);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@unit", unit);
                cmd.Parameters.AddWithValue("@entrydate", entrydate);
                cmd.Parameters.AddWithValue("@warrantyenddate", warrantyenddate);
                cmd.Parameters.AddWithValue("@location", location);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@addedbyuserid", addedbyuserid);
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product has been added successfully.");
                    Logger.AddLog(addedbyuserid, "Add", "Products", 0, $"Product '{productname}' has been added successfully.");
                    listView1.Items.Clear();
                    getProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public int getuserid(string username)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT UserId FROM Users WHERE UserName = @Username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    MessageBox.Show("User not found.");
                    return -1; // or handle as needed
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            forUsersScreen forUsersScreen = new forUsersScreen(Username, this);
            forUsersScreen.Show();
        }
    }
}
