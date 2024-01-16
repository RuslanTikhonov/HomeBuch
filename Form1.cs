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
using System.Globalization;
using System.IO;
using System.Windows.Navigation;

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

        private SqlConnection sqlConnectionC = null;
        private SqlCommandBuilder sqlCommandBuilderC = null;
        private SqlDataAdapter sqlDataAdapterC = null;
        private DataSet dataSetC = null;
        private SqlCommandBuilder sqlBuilderC = null;
        private bool newRowAddingC = false;


        public static void LoadDataGread()
        {

        }



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
            catch (Exception ex)
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

            sqlConnectionC = new SqlConnection(ConfigurationManager.ConnectionStrings["BuchDB"].ConnectionString);
            sqlConnectionC.Open();
            LoadDataCrypto();


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
            ReloadDataCrypto();
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

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                
                    double result1, result2, res, res2, res3;
                    result1 = Math.Round(Convert.ToDouble(dataGridView1[2, i].Value, CultureInfo.InvariantCulture), 2);
                    result2 = Math.Round(Convert.ToDouble(dataGridView1[3, i].Value, CultureInfo.InvariantCulture), 2);
                    res = result1 + result2;
                    res2 = Math.Round(Convert.ToDouble(dataGridView1[4, i].Value, CultureInfo.InvariantCulture), 2);
                    res3 = Math.Round((res / res2), 2);
                    dataGridView1.Rows[i].Cells[5].Value = res3;
                
            }
        }


        //Блок крипто


        private void LoadDataCrypto()
        {
            try
            {
                sqlDataAdapterC = new SqlDataAdapter("SELECT *, 'Delete' AS [Command] FROM Crypto", sqlConnectionR);
                sqlBuilderC = new SqlCommandBuilder(sqlDataAdapterR);
                sqlBuilderC.GetInsertCommand();
                sqlBuilderC.GetUpdateCommand();
                sqlBuilderC.GetDeleteCommand();
                dataSetC = new DataSet();
                sqlDataAdapterC.Fill(dataSetC, "Crypto");
                dataGridView3.DataSource = dataSetC.Tables["Crypto"];
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCellC = new DataGridViewLinkCell();
                    dataGridView3[3, i] = linkCellC;
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

        private void ReloadDataCrypto()
        {
            try
            {
                dataSetC.Tables["Crypto"].Clear();
                sqlDataAdapterC.Fill(dataSetC, "Crypto");
                dataGridView3.DataSource = dataSetC.Tables["Crypto"];
                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCellC = new DataGridViewLinkCell();
                    dataGridView3[3, i] = linkCellC;
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


        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3)
                {
                    string taskC = dataGridView3.Rows[e.RowIndex].Cells[6].Value.ToString();

                    if (taskC == "Delete")
                    {
                        if (MessageBox.Show("Удалить строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndexC = e.RowIndex;
                            dataGridView3.Rows.RemoveAt(rowIndexC);
                            dataSetC.Tables["Crypto"].Rows[rowIndexC].Delete();
                            sqlDataAdapterC.Update(dataSetC, "Crypto");
                        }
                    }

                    else if (taskC == "Insert")
                    {
                        int rowIndexC = dataGridView3.Rows.Count - 2;
                        DataRow rowC = dataSetC.Tables["Crypto"].NewRow();

                        rowC["Month"] = dataGridView3.Rows[rowIndexC].Cells["Month"].Value;
                        rowC["Rashod"] = dataGridView3.Rows[rowIndexC].Cells["Rashod"].Value;


                        dataSetC.Tables["Crypto"].Rows.Add(rowC);
                        dataSetC.Tables["Crypto"].Rows.RemoveAt(dataSetC.Tables["Crypto"].Rows.Count - 1);
                        dataGridView3.Rows.RemoveAt(dataGridView3.Rows.Count - 2);
                        dataGridView3.Rows[e.RowIndex].Cells[3].Value = "Delete";

                        sqlDataAdapterC.Update(dataSetC, "Crypto");
                        newRowAddingC = false;
                    }
                    else if (taskC == "Update")
                    {
                        int rC = e.RowIndex;

                        dataSetC.Tables["Crypto"].Rows[rC]["Month"] = dataGridView3.Rows[rC].Cells["Month"].Value;
                        dataSetC.Tables["Crypto"].Rows[rC]["Rashod"] = dataGridView3.Rows[rC].Cells["Rashod"].Value;


                        sqlDataAdapterC.Update(dataSetC, "Rashod");
                        dataGridView3.Rows[e.RowIndex].Cells[3].Value = "Delete";

                    }
                    ReloadDataCrypto();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAddingC == false)
                {
                    int rowIndexC = dataGridView3.SelectedCells[0].RowIndex;
                    DataGridViewRow editingRowC = dataGridView3.Rows[rowIndexC];
                    DataGridViewCell linkCellC = new DataGridViewLinkCell();
                    dataGridView3[3, rowIndexC] = linkCellC;
                    editingRowC.Cells["Command"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView3_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAddingC == false)
                {
                    newRowAddingC = true;
                    int lastRowC = dataGridView3.Rows.Count - 2;
                    DataGridViewRow rowC = dataGridView3.Rows[lastRowC];
                    DataGridViewLinkCell linkCellC = new DataGridViewLinkCell();
                    dataGridView3[3, lastRowC] = linkCellC;
                    rowC.Cells["Command"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 crypto = new Form2();
            crypto.Show();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            RashodGraph graf = new RashodGraph();
            graf.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DochodGrapg graf = new DochodGrapg();
            graf.Show();
        }
    }

}
