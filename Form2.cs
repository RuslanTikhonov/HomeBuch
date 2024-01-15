using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HomeBuch
{
    public partial class Form2 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlCommandBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private SqlCommandBuilder sqlBuilder = null;
        private bool newRowAdding = false;
        //private SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BuchDB"].ConnectionString);

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double average_price  = 0;
            average_price = Convert.ToDouble(total_price.Text) / Convert.ToDouble(price.Text);
            SqlCommand command = new SqlCommand("INSERT INTO [Crypto] (coin, quantity, average_price, total_cost) VALUES (@coin, @quantity, @average_price, @total_cost)", sqlConnection);
            command.Parameters.AddWithValue("coin", comboBox1.Text);
            command.Parameters.AddWithValue("quantity", price.Text);
            command.Parameters.AddWithValue("average_price", total_price.Text);
            command.Parameters.AddWithValue("total_cost", average_price);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
            sqlConnection.Close();
            
        }


        private void Form2_Load(object sender, EventArgs e)
        {
           

            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BuchDB"].ConnectionString);
            sqlConnection.Open();
            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Подключено");
            }
        }

    }
}

