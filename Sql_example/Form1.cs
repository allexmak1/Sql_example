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
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Sql_example
{
    public partial class Form1 : Form
    {
        private SqlConnection _connection = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlName"].ConnectionString);
            _connection.Open();
            if (_connection.State != ConnectionState.Open)
            {
                MessageBox.Show("sql не подключили!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command= new SqlCommand(
                //$"INSERT INTO [Table1] (Name, Phone, Data) VALUES (N'{textBox1.Text}', N'{textBox2.Text}', '{textBox3.Text}')",
                $"INSERT INTO [Table1] (Name, Phone, Data) VALUES (@Name, @Phone, @Data)",
                _connection);
            command.Parameters.AddWithValue("Name", textBox1.Text);
            command.Parameters.AddWithValue("Phone", textBox2.Text);
            DateTime date = DateTime.Now;
            if (textBox3.Text == null)
            {
                date = DateTime.Parse(textBox3.Text);
            }
            command.Parameters.AddWithValue("Data", $"{date.Month}/{date.Day}/{date.Hour}");

            if(command.ExecuteNonQuery() < 1)
                MessageBox.Show("даннык не добавленны");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT * FROM Products WHERE UnitPrice > 100",
                _connection);
            DataSet dataSet = new DataSet();    
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
        }


        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SqlDataAdapter adapter = new SqlDataAdapter(
                comboBox1.Text,
                _connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SqlDataReader sqlDataReader = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products",
                    _connection);
                sqlDataReader = sqlCommand.ExecuteReader();
                ListViewItem item = null;
                while(sqlDataReader.Read()) {
                    item = new ListViewItem(new string[] { 
                        Convert.ToString(sqlDataReader["ProductName"]),
                        Convert.ToString(sqlDataReader["QuantityPerUnit"]),
                        Convert.ToString(sqlDataReader["UnitPrice"])
                    });
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }
            }
        }
    }   
}
