using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using System.Globalization;

namespace HomeBuch
{
    public partial class RashodGraph : Form
    {
        public RashodGraph()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "buchDBDataSet2.Rashod". При необходимости она может быть перемещена или удалена.
            this.rashodTableAdapter.Fill(this.buchDBDataSet2.Rashod);


            //Рисуем график расходов
            cartesianChart1.LegendLocation = LegendLocation.Bottom;

            SeriesCollection series = new SeriesCollection();

            ChartValues<double> rashod = new ChartValues<double>();

            List<string> dates = new List<string>();
            foreach (var rashodRow in this.buchDBDataSet2.Rashod)
            {
                rashod.Add(rashodRow.Rashod);

                dates.Add(rashodRow.Month);
            }
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisX.Add(new Axis()
            {
                Title = "Даты",
                Labels = dates
            });

            LineSeries rashodLine = new LineSeries();
            rashodLine.Title = "Расход";
            rashodLine.Values = rashod;

            series.Add(rashodLine);

            cartesianChart1.Series = series;
        }
    }
}
