using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Crud_Project
{
    public partial class Form1 : Form
    {
        private static string connection = "Data Source=MJDT-0286;Initial Catalog=CRUD;Integrated Security=True";
        SqlConnection con = new SqlConnection(connection);
        public Form1()
        {
            InitializeComponent();
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick; // Subscribe to double-click event

        }


        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.cRUDOperationTableAdapter.Fill(this.cRUDDataSet.CRUDOperation);
            LoadData();
        }

        private  async void Button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            try
            {
                await Task.Run(() => InsertData());
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                progressBar1.Visible = false;
            }
        }

        private void InsertData()
        {

            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "INSERT INTO CRUDOperation VALUES ('farhan', 'atif', '12345678', 'malir', 'male')";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlDataAdapter adapt = new SqlDataAdapter("SELECT * FROM CRUDOperation", con);
                DataTable dt = new DataTable();
                adapt.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoResizeColumns(); 
                }

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DeleteData(id); 
                    LoadData(); 
                    MessageBox.Show("Record deleted successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select a record to delete.");
            }
        }

        private void DeleteData(int id)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "DELETE FROM CRUDOperation WHERE Id=@id"; 
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                 DialogResult dialogResult = MessageBox.Show("Do you want to edit this record?", "Confirm Edit", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                
                    string name = textBox1.Text; 
                    string address = textBox2.Text; // Get new address from a TextBox
                    string salary = textBox3.Text; // Get new salary from a TextBox

                    EditData(id, name, address, salary); // Call your edit method
                    LoadData(); // Refresh the DataGridView
                    MessageBox.Show("Record updated successfully.");
                }
            }
            else
            {
                MessageBox.Show("Please select a record to edit.");
            }
        }

        private void EditData(int id, string name, string address, string salary)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                string query = "UPDATE Teacher SET Name=@name, Address=@address, Salary=@salary WHERE Id=@id"; // Adjust according to your table structure
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@salary", salary);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {

        if (e.RowIndex >= 0) // Ensure it's not a header row
        {
            // Get the ID from the selected row (assuming it's in the first column)
            int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

            // Confirm edit
            DialogResult dialogResult = MessageBox.Show("Do you want to edit this record?", "Confirm Edit", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string name = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); // Get current name
                string address = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(); // Get current address
                int salary = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value); // Get current salary

                    textBox1.Text = name;
                    textBox2.Text = address;
                    textBox3.Text = salary.ToString();

                    textBox1.Focus();
            }
        }
    }

    }


}
