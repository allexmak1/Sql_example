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

            // filter data greed
            SqlDataAdapter adapter = new SqlDataAdapter(
                "SELECT * FROM Products",
                _connection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];

            //filter for list View
            button3_Click(sender, null);
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
            string[] row = null;
            rows = new List<string[]>();
            listView1.Items.Clear();
            SqlDataReader sqlDataReader = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName, QuantityPerUnit, UnitPrice FROM Products",
                    _connection);
                sqlDataReader = sqlCommand.ExecuteReader();
                ListViewItem item = null;
                while(sqlDataReader.Read()) {
                    //item = new ListViewItem(new string[] { 
                    //    Convert.ToString(sqlDataReader["ProductName"]),
                    //    Convert.ToString(sqlDataReader["QuantityPerUnit"]),
                    //    Convert.ToString(sqlDataReader["UnitPrice"])
                    //});
                    //listView1.Items.Add(item);

                    // тут для фильтра заполняем list
                    row = new string[]
                    {
                        Convert.ToString(sqlDataReader["ProductName"]),
                        Convert.ToString(sqlDataReader["QuantityPerUnit"]),
                        Convert.ToString(sqlDataReader["UnitPrice"])
                    };
                    rows.Add(row);
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
            RefreshList(rows);
        }

        //filter data greed
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '%{textBox4.Text}%'";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock <= 10";
                    break;
                case 1:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 10 AND UnitsInStock <= 50";
                    break;
                case 2:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 50";
                    break;
                default:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";
                    break;
            }
        }

        //filter list View
        private List<string[]> rows = null;
        private List<string[]> fieredList = null;

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            fieredList = rows.Where((x) => 
                x[0].ToLower().Contains(textBox5.Text.ToLower())).ToList();
            RefreshList(fieredList);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    fieredList = rows.Where((x) =>
                        Double.Parse(x[2]) <= 10).ToList();
                    RefreshList(fieredList);
                    break;
                case 1:
                    fieredList = rows.Where((x) =>
                        Double.Parse(x[2]) >= 10 && Double.Parse(x[2]) <= 50).ToList();
                    RefreshList(fieredList);
                    break;
                case 2:
                    fieredList = rows.Where((x) =>
                        Double.Parse(x[2]) >= 50).ToList();
                    RefreshList(fieredList);
                    break;
                default:
                    RefreshList(rows);
                    break;
            }
        }
        private void RefreshList(List<string[]> list)
        {
            listView1.Items.Clear();
            foreach (string[] row in list)
            {
                listView1.Items.Add(new ListViewItem(row));
            }
        }
    }   
}
