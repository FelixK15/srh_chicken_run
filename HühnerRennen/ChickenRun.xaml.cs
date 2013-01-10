using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace HühnerRennen
{
    /// <summary>
    /// Interaction logic for ChickenRun.xaml
    /// </summary>
    public partial class ChickenRun : Window
    {
        private static Random r = new Random();
        private DispatcherTimer m_timer;
        private Chicken[] m_chickens;
        public ChickenRun()
        {
            ChickenResources.LoadResources();

            m_chickens = new Chicken[4];
            m_timer = new DispatcherTimer();

            m_timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            m_timer.Tick += delegate(object sender, EventArgs e)
            {
                foreach (Chicken c in m_chickens)
                {
                    c.CalculateSpeed();
                }

                _DrawTrack();
                _CheckForWinner();
            };

            m_timer.Start();

            m_chickens[0] = new Chicken(Chicken.ChickenType.RED_CHICKEN);
            m_chickens[1] = new Chicken(Chicken.ChickenType.BLUE_CHICKEN);
            m_chickens[2] = new Chicken(Chicken.ChickenType.GREEN_CHICKEN);
            m_chickens[3] = new Chicken(Chicken.ChickenType.VIOLET_CHICKEN);


            int x = 20;
            int y = 50;

            foreach (Chicken c in m_chickens)
            {
                c.X = x;
                c.Y = y;
                c.Speed = r.Next(3) + 3;
                y += 50;
            }

            InitializeComponent();

            cvTrack.Background = new SolidColorBrush(Color.FromRgb(10,220,10));
        }

        private void _DrawTrack()
        {
            cvTrack.Children.Clear();

            //Track
            Rectangle track = new Rectangle();
            track.Width = cvTrack.Width;
            track.Height = 240;

            track.Fill = new SolidColorBrush(Color.FromRgb(200, 20, 20));
            
            cvTrack.Children.Add(track);
            Canvas.SetLeft(track, 0);
            Canvas.SetTop(track, 40);

            //Finish Line
            Rectangle finishLine = new Rectangle();
            finishLine.Width = 2;
            finishLine.Height = cvTrack.Height;

            finishLine.Fill = new SolidColorBrush(Color.FromRgb(255,255,255));

            cvTrack.Children.Add(finishLine);
            Canvas.SetLeft(finishLine, cvTrack.Width - 80);
            Canvas.SetTop(finishLine, 0);

            foreach (Chicken c in m_chickens)
            {
                cvTrack.Children.Add(c.Graphic);
                Canvas.SetLeft(c.Graphic, c.X);
                Canvas.SetTop(c.Graphic, c.Y);
            }
        }

        private void _CheckForWinner()
        {
            foreach (Chicken c in m_chickens)
            {
                if (c.X > (cvTrack.Width - 80))
                {
                    m_timer.Stop();
                }
            }
        }
    }
}
