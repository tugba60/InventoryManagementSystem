using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e) //login
        {
            this.IsMdiContainer = true;
            LoginScreen loginScreen = new LoginScreen();
            loginScreen.MdiParent = this;
            cleanScreen();
            loginScreen.Show();
        }

        private void button2_Click(object sender, EventArgs e) // sign up
        {
            this.IsMdiContainer = true;
            SignUpScreen signUpScreen = new SignUpScreen();
            signUpScreen.MdiParent = this;
            cleanScreen();
            signUpScreen.Show();
        }

        private void cleanScreen()
        {
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            
            button2.Visible = false;
            button1.Visible = false;
        }

        private void goToLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;

            button2.Visible = true;
            button1.Visible = true;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
