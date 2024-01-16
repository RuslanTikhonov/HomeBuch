using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeBuch
{
    public partial class DochodGrapg : Form
    {
        public DochodGrapg()
        {
            InitializeComponent();
        }

        private void DochodGrapg_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "buchDBDataSet3.Zarplata". При необходимости она может быть перемещена или удалена.
            this.zarplataTableAdapter.Fill(this.buchDBDataSet3.Zarplata);

            //Рисуем график доходов
            cartesianChart1.LegendLocation = LegendLocation.Bottom;

            SeriesCollection series = new SeriesCollection();

            ChartValues<decimal> dochod = new ChartValues<decimal>();

            List<string> dates = new List<string>();
            foreach (var dochodRow in this.buchDBDataSet3.Zarplata)
            {
                dochod.Add(dochodRow.Zarplata);

                dates.Add(dochodRow.Month);
            }
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisX.Add(new Axis()
            {
                Title = "Даты",
                Labels = dates
            });

            LineSeries rashodLine = new LineSeries();
            rashodLine.Title = "Доход";
            rashodLine.Values = dochod;

            series.Add(rashodLine);

            cartesianChart1.Series = series;
        }
    }
}
