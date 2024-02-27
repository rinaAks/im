using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //в этой задаче мы смотрим развитие процесса во времени

        //константы для предметной области
        //const double dt = 0.01; //шаг по времени
        const double g = 9.81;
        const double C = 0.15;
        const double ro = 1.29;

        double height, angle, speed, size, weight, dt;

        private void textbDistance_TextChanged(object sender, EventArgs e)
        {

        }

        double t, x, y, vx, vy; //параметры модели. t - "модельное время", время в полёте. будем перебирать
        double vx_old, vy_old, root;
        double maxY = 0, endSpeed;

        double cosA, sinA, beta, k; //вспомогательные параметры, чтобы не считать cos и sin в цикле
        private void btStart_Click(object sender, EventArgs e)
        {
            //после запуска считываем значения из Edit-ов
            height = (double)edHeight.Value;
            angle = (double)edAngle.Value;
            speed = (double)edSpeed.Value;
            size = (double)edSize.Value;
            weight = (double)edWeight.Value;
            dt = (double)edTimeStep.Value;

            cosA = Math.Cos(angle * Math.PI / 180);
            sinA = Math.Sin(angle * Math.PI / 180);

            beta = 0.5 * C * size * ro;
            k = beta / weight;

            t = 0;
            x = 0; y = height;
            vx = speed*cosA; vy = speed * sinA;

            //chart1.Series[0].Points.Clear();
            chart1.Series[0].Points.AddXY(x, y);

            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            vx_old = vx; vy_old = vy;
            root = Math.Sqrt(vx * vx + vy * vy);

            t = t + dt; //изменение времени, здесь равномерное

            vx = vx_old - k * vx_old * root * dt;
            vy = vy_old - (g + k * vy_old * root) * dt;

            x = x + vx * dt;
            y = y + vy * dt;
            if (y > maxY) maxY = y;
            endSpeed = Math.Sqrt(vx * vx + vy * vy);

            chart1.Series[0].Points.AddXY(x, y);

            if (y <= 0) 
            {
                textbEndSpeed.Text = endSpeed.ToString();
                textbMaxHeight.Text = maxY.ToString();
                textbDistance.Text = x.ToString();
                timer1.Stop();
            }
        }
    }
}
