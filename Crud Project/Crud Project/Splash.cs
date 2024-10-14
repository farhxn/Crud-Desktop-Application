using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crud_Project
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            timer1.Interval = 3000; // Set timer for 10 seconds
            timer1.Tick += Timer1_Tick;
            timer1.Start(); // Start the timer when the splash screen loads
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop(); 
            Form1 mainForm = new Form1(); 
            mainForm.Show(); 
            this.Hide(); 
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
