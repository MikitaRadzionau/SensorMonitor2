using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;   

namespace SensorMonitor2
{
    public partial class Form1 : Form
    {
        SerialPort port;
        List<double> tempValues = new List<double>();
        List<double> humValues = new List<double>();

        public Form1()
        {
            InitializeComponent();

            // Загружаем COM-порты
            cmbPorts.Items.AddRange(SerialPort.GetPortNames());

            // Включаем двойную буферизацию для picGraph
            typeof(PictureBox).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(picGraph, true);
        }

        // Обработчик кнопки
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cmbPorts.SelectedItem == null) return;

            try
            {
                port = new SerialPort(cmbPorts.SelectedItem.ToString(), 9600);
                port.DataReceived += Port_DataReceived;
                port.Open();
                btnStart.Text = "Подключено";
                btnStart.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string line = port.ReadLine().Trim();
            if (line == "ERROR") return;

            string[] parts = line.Split(',');
            if (parts.Length == 2)
            {
                if (double.TryParse(parts[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double temp) &&
                    double.TryParse(parts[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double hum))
                {
                    this.Invoke(new Action(() =>
                    {
                        tempValues.Add(temp);
                        humValues.Add(hum);

                        if (tempValues.Count > 50) tempValues.RemoveAt(0);
                        if (humValues.Count > 50) humValues.RemoveAt(0);

                        DrawGraph();
                    }));
                }
            }
        }

        private void DrawGraph()
        {
            if (picGraph.Width == 0 || picGraph.Height == 0) return;

            Bitmap bmp = new Bitmap(picGraph.Width, picGraph.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                if (tempValues.Count < 2) return;

                Pen gridPen = new Pen(Color.LightGray, 1);
                for (int i = 0; i < picGraph.Width; i += 40) g.DrawLine(gridPen, i, 0, i, picGraph.Height);

                float xStep = (float)picGraph.Width / 50;
                float yScale = (float)picGraph.Height / 100;

                // Красная линия (Температура)
                Pen tempPen = new Pen(Color.Red, 2);
                for (int i = 0; i < tempValues.Count - 1; i++)
                {
                    float x1 = i * xStep;
                    float y1 = picGraph.Height - (float)(tempValues[i] * yScale);
                    float x2 = (i + 1) * xStep;
                    float y2 = picGraph.Height - (float)(tempValues[i + 1] * yScale);
                    g.DrawLine(tempPen, x1, y1, x2, y2);
                }

                // Синяя линия (Влажность)
                Pen humPen = new Pen(Color.Blue, 2);
                for (int i = 0; i < humValues.Count - 1; i++)
                {
                    float x1 = i * xStep;
                    float y1 = picGraph.Height - (float)(humValues[i] * yScale);
                    float x2 = (i + 1) * xStep;
                    float y2 = picGraph.Height - (float)(humValues[i + 1] * yScale);
                    g.DrawLine(humPen, x1, y1, x2, y2);
                }
            }
            picGraph.Image = bmp;
        }
    }
}