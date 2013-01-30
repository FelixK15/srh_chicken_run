using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Media;

namespace HühnerRennen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Random r = new Random();
        private DispatcherTimer m_timer = new DispatcherTimer();
        private Chicken[] m_chickens = new Chicken[4];
        private Player[] m_players = new Player[4];
        private Chicken m_winnerChicken = null;
        private Chicken m_curBidChicken = null;
        private SoundPlayer m_soundPlayer = null;

        public MainWindow()
        {
            ChickenResources.LoadResources();
            InitializeComponent();
            _InternalInitialize();
            _UpdateData();
        }

        private void _DrawTrack()
        {
            cvGame.Children.Clear();

            //Track
            Rectangle track = new Rectangle();
            track.Width = cvGame.Width;
            track.Height = 240;

            track.Fill = new SolidColorBrush(Color.FromRgb(200, 20, 20));

            cvGame.Children.Add(track);
            Canvas.SetLeft(track, 0);
            Canvas.SetTop(track, 40);

            //Finish Line
            Rectangle finishLine = new Rectangle();
            finishLine.Width = 2;
            finishLine.Height = cvGame.Height;

            finishLine.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            cvGame.Children.Add(finishLine);
            Canvas.SetLeft(finishLine, cvGame.Width - 80);
            Canvas.SetTop(finishLine, 0);

            foreach (Chicken c in m_chickens)
            {
                Label lbName = new Label();
                lbName.Content = c.Name;

                cvGame.Children.Add(c.Graphic);
                Canvas.SetLeft(c.Graphic, c.X);
                Canvas.SetTop(c.Graphic, c.Y);

                cvGame.Children.Add(lbName);
                Canvas.SetLeft(lbName, c.X);
                Canvas.SetTop(lbName, c.Y - 10);
            }
        }

        private void _ResetTrack()
        {
            int x = 20;
            int y = 50;

            foreach (Chicken c in m_chickens)
            {
                c.X = x;
                c.Y = y;
                c.Speed = r.Next(3) + 3;
                y += 50;
            }
        }

        private void _CheckForWinner()
        {
            foreach (Chicken c in m_chickens)
            {
                if (c.X > (cvGame.Width - 80))
                {
                    m_timer.Stop();
                    m_winnerChicken = c;
                    _AfterRace();
                    break;
                }
            }
        }

        private void _AfterRace()
        {
            string winString = "";
            string playerWinner = "";

            //Alten Sound anhalten.
            m_soundPlayer.Stop();

            //Diese Funktion wird aufgerufen, nachdem ein Rennen gewonnen wurde.
            Thread.Sleep(2000); //Warte etwas, damit nicht SOFORT unterbrochen wird.

            //Neuen Sound laden und abspielen.
            m_soundPlayer = new SoundPlayer("winner.wav");
            m_soundPlayer.Play();

            m_winnerChicken.WonRaces = m_winnerChicken.WonRaces + 1;
            winString += m_winnerChicken.Name + " hat das Rennen gewonnen!\n";

            foreach(Chicken c in m_chickens)
            {
                if(c != m_winnerChicken)
                {
                    c.LostRaces = c.LostRaces + 1;
                }
            }

            //Finde Spieler, die für das Winner Chicken gewettet haben.
            foreach (Player p in m_players)
            {
                //Wenn der Spieler für das Winner Chicken gewettet hat, rechne den Wetteinsatz * 2 auf das Geld des Spielers.
                if (p.Chickens.ContainsKey(m_winnerChicken))
                {
                    playerWinner += p.Name + " ";
                    double money = 0;
                    p.Chickens.TryGetValue(m_winnerChicken, out money);

                    p.Money = p.Money + (money * 2);
                }
                
                p.Chickens.Clear();
            }

            if (playerWinner != "")
            {
                MessageBox.Show(winString + playerWinner + "hat/haben auf das richtige Huhn gesetzt!","Wir haben einen Gewinner!");
            }
            else
            {
                MessageBox.Show(winString + "Leider hat keiner auf das richtige Huhn gesetzt.", "Verloren.");
            }

            tcChickens.IsEnabled = true;
            tcPlayers.IsEnabled = true;
            bnStartRennen.IsEnabled = true;

            _UpdateData();
            _ResetTrack();
            _DrawTrack();
        }

        private void _InternalInitialize()
        {
            //Timer wird gesetzt und Objekte initialisiert.

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

            m_chickens[0] = new Chicken(Chicken.ChickenType.RED_CHICKEN);
            m_chickens[1] = new Chicken(Chicken.ChickenType.BLUE_CHICKEN);
            m_chickens[2] = new Chicken(Chicken.ChickenType.GREEN_CHICKEN);
            m_chickens[3] = new Chicken(Chicken.ChickenType.VIOLET_CHICKEN);

            m_chickens[0].Name = "Karl";
            m_chickens[1].Name = "Agathe";
            m_chickens[2].Name = "Sören";
            m_chickens[3].Name = "Kentucky";

            m_players[0] = new Player();
            m_players[1] = new Player();
            m_players[2] = new Player();
            m_players[3] = new Player();

            m_players[0].Money = 300.0;
            m_players[1].Money = 300.0;
            m_players[2].Money = 300.0;
            m_players[3].Money = 300.0;

            m_curBidChicken = m_chickens[0];

            cvGame.Background = new SolidColorBrush(Color.FromRgb(10, 220, 10));

            _ResetTrack();
            _DrawTrack();
        }

        private void _UpdateData()
        {
            //Diese Funktion updated die Daten in der Maske (Geld der Spieler etc.)
            lbGuthabenPl1.Content = m_players[0].Money.ToString() + " $";
            lbGewinnPl1.Content = m_players[0].WonMoney.ToString() + " $";
            lbVerlustPl1.Content = m_players[0].LostMoney.ToString() + " $";

            lbGuthabenPl2.Content = m_players[1].Money.ToString() + " $";
            lbGewinnPl2.Content = m_players[1].WonMoney.ToString() + " $";
            lbVerlustPl2.Content = m_players[1].LostMoney.ToString() + " $";

            lbGuthabenPl3.Content = m_players[2].Money.ToString() + " $";
            lbGewinnPl3.Content = m_players[2].WonMoney.ToString() + " $";
            lbVerlustPl3.Content = m_players[2].LostMoney.ToString() + " $";

            lbGuthabenPl4.Content = m_players[3].Money.ToString() + " $";
            lbGewinnPl4.Content = m_players[3].WonMoney.ToString() + " $";
            lbVerlustPl4.Content = m_players[3].LostMoney.ToString() + " $";

            lbC1GesetztesGeld.Content = m_chickens[0].BidMoney.ToString() + " $";
            lbC1GewonneneRennen.Content = m_chickens[0].WonRaces.ToString();
            lbC1VerloreneRennen.Content = m_chickens[0].LostRaces.ToString();

            lbC2GesetztesGeld.Content = m_chickens[1].BidMoney.ToString() + " $";
            lbC2GewonneneRennen.Content = m_chickens[1].WonRaces.ToString();
            lbC2VerloreneRennen.Content = m_chickens[1].LostRaces.ToString();

            lbC3GesetztesGeld.Content = m_chickens[2].BidMoney.ToString() + " $";
            lbC3GewonneneRennen.Content = m_chickens[2].WonRaces.ToString();
            lbC3VerloreneRennen.Content = m_chickens[2].LostRaces.ToString();

            lbC4GesetztesGeld.Content = m_chickens[3].BidMoney.ToString() + " $";
            lbC4GewonneneRennen.Content = m_chickens[3].WonRaces.ToString();
            lbC4VerloreneRennen.Content = m_chickens[3].LostRaces.ToString();

            txWetteC1Pl1.Text = "0";
            txWetteC1Pl2.Text = "0";
            txWetteC1Pl3.Text = "0";
            txWetteC1Pl4.Text = "0";

            txWetteC2Pl1.Text = "0";
            txWetteC2Pl2.Text = "0";
            txWetteC2Pl3.Text = "0";
            txWetteC2Pl4.Text = "0";

            txWetteC3Pl1.Text = "0";
            txWetteC3Pl2.Text = "0";
            txWetteC3Pl3.Text = "0";
            txWetteC3Pl4.Text = "0";

            txWetteC4Pl1.Text = "0";
            txWetteC4Pl2.Text = "0";
            txWetteC4Pl3.Text = "0";
            txWetteC4Pl4.Text = "0";
        }

        private void _CheckData()
        {
            //Die Wetteinsätze werden verrechnet.
            foreach (Player p in m_players)
            {
                
                var enumurator = p.Chickens.GetEnumerator();
                Chicken c = null;
                while(enumurator.MoveNext())
                {
                    c = enumurator.Current.Key;
                    //Wetteinsatz wird dem Spieler hinzugefügt.
                    p.Money = p.Money - enumurator.Current.Value;
                    //Und dem Huhn hinzugefügt.
                    c.BidMoney = c.BidMoney + enumurator.Current.Value;
                }
            }
        }

        private void bnStartRennen_Click(object sender, RoutedEventArgs e)
        {
            //Bevor das Rennen los geht werden die Daten uebernommen und der Timer gestartet.
            _CheckData();
            _ResetTrack();

            tcChickens.IsEnabled = false;
            tcPlayers.IsEnabled = false;
            bnStartRennen.IsEnabled = false;

            m_soundPlayer = new SoundPlayer("chicken.wav");
            m_soundPlayer.PlayLooping();
            m_timer.Start();
        }

        private void txWette_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Dieser Eventhandler stellt sicher, dass nur Zahlen in den jeweiligen Wetteinsatz Textboxen eingetragen werden koennen.
            foreach (char c in e.Text)
            {
                if (!Char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

        private void txWette_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox source = (TextBox)e.Source;
            String sourceTag = (String)source.Tag;
            double content = 0.0;

            //Setze Geld nur wenn auch was eingegeben wurde.
            if (source.Text != "")
            {
                content = Convert.ToDouble(source.Text);
            }
           
            Player player = null;

            //Ermittle player anhand von Tag.
            if (sourceTag == "Pl1")
            {
                player = m_players[0];
            }
            else if (sourceTag == "Pl2")
            {
                player = m_players[1];
            }
            else if (sourceTag == "Pl3")
            {
                player = m_players[2];
            }
            else
            {
                player = m_players[3];
            }

            if (player == null)
            {
                return;
            }

            //Wenn nichts gesetzt wurde, Chicken aus dem Player Dictionary entfernen.
            if (content == 0.0)
            {
                player.Chickens.Remove(m_curBidChicken);
            }
            else
            {
                //Checke, ob der Spieler genug Geld hat.
                if (player.Money < content)
                {
                    MessageBox.Show(player.Name + " hat nicht genug Geld um " + content + "$ zu setzen.");
                    source.Text = player.Money.ToString();
                    content = player.Money;
                }

                if (player.Chickens.ContainsKey(m_curBidChicken))
                {
                    player.Chickens.Remove(m_curBidChicken);
                }

                //Füge das aktuelle Chicken dem Spieler mit dem Geld hinzu, wieviel er gewettet hat.
                player.Chickens.Add(m_curBidChicken, content);
            }

        }

        private void TabControl_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            TabControl source = (TabControl)e.Source;
            TabItem currentItem = (TabItem)source.SelectedItem;
            string tag = (string)currentItem.Tag;

            if (tag == "C1")
            {
                m_curBidChicken = m_chickens[0];
            }
            else if (tag == "C2")
            {
                m_curBidChicken = m_chickens[1];
            }
            else if (tag == "C3")
            {
                m_curBidChicken = m_chickens[2];
            }
            else
            {
                m_curBidChicken = m_chickens[3];
            }
        }

        private void txName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox source = (TextBox)e.Source;
            string text = source.Text;
            string tag = (string)source.Tag;
            Player player = null;

            if (tag == "Pl1")
            {
                player = m_players[0];
            }
            else if (tag == "Pl2")
            {
                player = m_players[1];
            }
            else if (tag == "Pl3")
            {
                player = m_players[2];
            }
            else
            {
                player = m_players[3];
            }

            if (player != null)
            {
                player.Name = text;
            }
        }
    }
}
