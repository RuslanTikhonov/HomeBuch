using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace HomeBuch
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlCommandBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;
        private SqlCommandBuilder sqlBuilder = null;
        private bool newRowAdding = false;

        private SqlConnection sqlConnectionR = null;
        private SqlCommandBuilder sqlCommandBuilderR = null;
        private SqlDataAdapter sqlDataAdapterR = null;
        private DataSet dataSetR = null;
        private SqlCommandBuilder sqlBuilderR = null;
        private bool newRowAddingR = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM Zarplata", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Zarplata");
                dataGridView1.DataSource = dataSet.Tables["Zarplata"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
        }


        private void LoadDataR()
        {
            try
            {
                sqlDataAdapterR = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM Rashod", sqlConnectionR);
                sqlBuilderR = new SqlCommandBuilder(sqlDataAdapterR);
                sqlBuilderR.GetInsertCommand();
                sqlBuilderR.GetUpdateCommand();
                sqlBuilderR.GetDeleteCommand();
                dataSetR = new DataSet();
                sqlDataAdapterR.Fill(dataSetR, "Rashod");
                dataGridView2.DataSource = dataSetR.Tables["Rashod"];
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCellR = new DataGridViewLinkCell();
                    dataGridView2[3, i] = linkCellR;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConnectionR.Close();
            }
        }


        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Zarplata"].Clear();
                sqlDataAdapter.Fill(dataSet, "Zarplata");
                dataGridView1.DataSource = dataSet.Tables["Zarplata"];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void ReloadDataRashod()
        {
            try
            {
                dataSetR.Tables["Rashod"].Clear();
                sqlDataAdapterR.Fill(dataSetR, "Rashod");
                dataGridView2.DataSource = dataSetR.Tables["Rashod"];
                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCellR = new DataGridViewLinkCell();
                    dataGridView2[3, i] = linkCellR;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                sqlConnectionR.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "buchDBDataSet.Zarplata". При необходимости она может быть перемещена или удалена.
            //this.zarplataTableAdapter.Fill(this.buchDBDataSet.Zarplata);
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BuchDB"].ConnectionString);
            sqlConnection.Open();
            LoadData();
            sqlConnectionR = new SqlConnection(ConfigurationManager.ConnectionStrings["BuchDB"].ConnectionString);
            sqlConnectionR.Open();
            LoadDataR();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DataTable datatable1 = dataGridView1.DataSource as DataTable;
            //DataRow row = datatable1.NewRow();
            ////инициализация некоторых полей, если это необходимо
            ////row["Field"]=someValue;
            //datatable1.Rows.Add(row);
            ReloadData();
            ReloadDataRashod();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 6)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);
                            dataSet.Tables["Zarplata"].Rows[rowIndex].Delete();
                            sqlDataAdapter.Update(dataSet, "Zarplata");
                        }
                    }

                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;
                        DataRow row = dataSet.Tables["Zarplata"].NewRow();
                        
                        row["Month"] = dataGridView1.Rows[rowIndex].Cells["Month"].Value;
                        row["Zarplata"] = dataGridView1.Rows[rowIndex].Cells["Zarplata"].Value;
                        row["Avans"] = dataGridView1.Rows[rowIndex].Cells["Avans"].Value;
                        row["Chasi"] = dataGridView1.Rows[rowIndex].Cells["Chasi"].Value;
                        row["Stavka"] = dataGridView1.Rows[rowIndex].Cells["Stavka"].Value;

                        dataSet.Tables["Zarplata"].Rows.Add(row);
                        dataSet.Tables["Zarplata"].Rows.RemoveAt(dataSet.Tables["Zarplata"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Zarplata");
                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["Zarplata"].Rows[r]["Month"] = dataGridView1.Rows[r].Cells["Month"].Value;
                        dataSet.Tables["Zarplata"].Rows[r]["Zarplata"] = dataGridView1.Rows[r].Cells["Zarplata"].Value;
                        dataSet.Tables["Zarplata"].Rows[r]["Avans"] = dataGridView1.Rows[r].Cells["Avans"].Value;
                        dataSet.Tables["Zarplata"].Rows[r]["Chasi"] = dataGridView1.Rows[r].Cells["Chasi"].Value;
                        dataSet.Tables["Zarplata"].Rows[r]["Stavka"] = dataGridView1.Rows[r].Cells["Stavka"].Value;

                        sqlDataAdapter.Update(dataSet, "Zarplata");
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = "Delete";

                    }
                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;
                    int lastRow = dataGridView1.Rows.Count - 2;
                    DataGridViewRow row = dataGridView1.Rows[lastRow];
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, lastRow] = linkCell;
                    row.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex; 
                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];
                    DataGridViewCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, rowIndex] = linkCell;
                    editingRow.Cells["Command"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    string taskR = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();

                    if (taskR == "Delete")
                    {
                        if (MessageBox.Show("Удалить строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndexR = e.RowIndex;
                            dataGridView2.Rows.RemoveAt(rowIndexR);
                            dataSetR.Tables["Rashod"].Rows[rowIndexR].Delete();
                            sqlDataAdapterR.Update(dataSetR, "Rashod");
                        }
                    }

                    else if (taskR == "Insert")
                    {
                        int rowIndexR = dataGridView2.Rows.Count - 2;
                        DataRow rowR = dataSetR.Tables["Rashod"].NewRow();

                        rowR["Month"] = dataGridView2.Rows[rowIndexR].Cells["Month"].Value;
                        rowR["Rashod"] = dataGridView2.Rows[rowIndexR].Cells["Rashod"].Value;


                        dataSetR.Tables["Rashod"].Rows.Add(rowR);
                        dataSetR.Tables["Rashod"].Rows.RemoveAt(dataSetR.Tables["Rashod"].Rows.Count - 1);
                        dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 2);
                        dataGridView2.Rows[e.RowIndex].Cells[3].Value = "Delete";

                        sqlDataAdapterR.Update(dataSetR, "Rashod");
                        newRowAddingR = false;
                    }
                    else if (taskR == "Update")
                    {
                        int rR = e.RowIndex;

                        dataSetR.Tables["Rashod"].Rows[rR]["Month"] = dataGridView2.Rows[rR].Cells["Month"].Value;
                        dataSetR.Tables["Rashod"].Rows[rR]["Rashod"] = dataGridView2.Rows[rR].Cells["Rashod"].Value;


                        sqlDataAdapterR.Update(dataSetR, "Rashod");
                        dataGridView2.Rows[e.RowIndex].Cells[3].Value = "Delete";

                    }
                    ReloadDataRashod();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAddingR == false)
                {
                    int rowIndexR = dataGridView2.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRowR = dataGridView2.Rows[rowIndexR];
                    DataGridViewCell linkCellR = new DataGridViewLinkCell();
                    dataGridView2[3, rowIndexR] = linkCellR;
                    editingRowR.Cells["Command"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView2_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAddingR == false)
                {
                    newRowAddingR = true;
                    int lastRowR = dataGridView2.Rows.Count - 2;
                    DataGridViewRow rowR = dataGridView2.Rows[lastRowR];
                    DataGridViewLinkCell linkCellR = new DataGridViewLinkCell();
                    dataGridView2[3, lastRowR] = linkCellR;
                    rowR.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
