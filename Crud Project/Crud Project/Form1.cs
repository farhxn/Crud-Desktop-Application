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
        private static string connection = "Data Source=FARHAN;Initial Catalog=crudoperation;Integrated Security=True;";
        SqlConnection con = new SqlConnection(connection);
        private String FirstName, LastName, Address, Contact, Gender;
        public Form1()
        {
            InitializeComponent();
            FirstName = textBox1.Text;
            LastName = textBox2.Text;
            Address = textBox3.Text;
            Contact = textBox4.Text;
            Gender = textBox5.Text;
            dataGridView1.CellDoubleClick += DataGridView1_CellDoubleClick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.cRUDOperationTableAdapter1.Fill(this.crudoperationDataSet1.CRUDOperation);
            LoadData();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (InsertData())
                {
                    SetInputsEmpty();
                    LoadData();
                    MessageBox.Show("Thank you :) \n\nData Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private bool InsertData()
        {
            if (!ValidateInputs())
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string firstName = textBox1.Text;
            string lastName = textBox2.Text;
            string address = textBox3.Text;
            string contact = textBox4.Text;
            string gender = textBox5.Text;


            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                string query = "INSERT INTO CRUDOperation (FirstName, LastName, Contact, Address, Gender) VALUES (@FirstName, @LastName, @Contact, @Address, @Gender)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                     cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Gender", gender);

                    cmd.ExecuteNonQuery();
                }

            }
            return true;
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

                DialogResult dialogResult = MessageBox.Show("Do you want to edit this record?", "Confirm Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {

                    string name = textBox1.Text;
                    string lastName = textBox2.Text; 
                    string contact = textBox3.Text; 
                    string address = textBox4.Text; 
                    string gender = textBox5.Text;

                    EditData(id, name, lastName, contact,address,gender);
                    LoadData();
                }
            }
            else
            {
                MessageBox.Show("Please select a record to edit.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SetInputsEmpty();
        }

        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].Cells[0].Value != DBNull.Value)
            {
                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);

                DialogResult dialogResult = MessageBox.Show("Do you want to edit this record?", "Confirm Edit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    string firstname = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); 
                    string lastName = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(); 
                    string contact = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(); 
                    string gender = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(); 
                    string address = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString(); 

                    textBox1.Text = firstname;
                    textBox2.Text = lastName;
                    textBox3.Text = contact;
                    textBox4.Text = address;
                    textBox5.Text = gender;

                    textBox1.Focus();
                }
            }
        }

        private void EditData(int id, string firstName, string lastName, string contact, string address, string gender)
        {
            using (SqlConnection con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    string query = "UPDATE CRUDOperation SET FirstName = @FirstName, LastName = @LastName, Contact = @Contact, Address = @Address, Gender = @Gender WHERE Id = @Id"; 

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                      
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Contact", contact);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Gender", gender);

                        cmd.ExecuteNonQuery();
                    }
                    SetInputsEmpty();
                    MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private bool ValidateInputs()
        {
            return !string.IsNullOrWhiteSpace(textBox1.Text) &&
                   !string.IsNullOrWhiteSpace(textBox2.Text) &&
                   !string.IsNullOrWhiteSpace(textBox3.Text) &&
                   !string.IsNullOrWhiteSpace(textBox4.Text) &&
                   !string.IsNullOrWhiteSpace(textBox5.Text);
        }

        private void SetInputsEmpty()
        {
                    textBox1.Text = "";
                   textBox2.Text = "";
                    textBox3.Text= "";
                   textBox4.Text= "";
                   textBox5.Text = "";
        }
    }


}
