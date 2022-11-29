using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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

namespace Blackjackkwadraaloef
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Resetclient();
            
        }
        int playercardvalue1;
        int playercardvalue2;
        int dealercardvalue1;
        private int availmoney = 100;
        private int playeraces = 1;
        private int dealeraces = 1;
        private int moneywagered;
        private int dealercardvalue;
        private int playercardvalue;
        private string playercard1 = "empty";
        private string playercard2 = "empty";
        private string playercard3 = "empty";
        private string dealercard1 = "empty";
        private string dealercard2 = "empty";
        private string name;
        private Random rnd = new Random();
        DispatcherTimer onesecondtimer = new DispatcherTimer();
        DispatcherTimer twosecondtimer = new DispatcherTimer();
        DispatcherTimer threesecondtimer = new DispatcherTimer();

        private void HitButton_Click(object sender, RoutedEventArgs e)
        {
            int playercardvalue3;
            playercard3 = Randomcard(true, out playercardvalue3);
            playercardvalue += playercardvalue3;
            PlayerScore.Text = Convert.ToString(playercardvalue);
            PlayerCards.Text += $"\n{playercard3}";

            Checkaceplayer();

            if (playercardvalue > 21)
            {
   
                Lose();
            }
            
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckMoney())
            {
                return;
            }

            PlayerCards.Text = "";
            DealerCards.Text = "";
            Result.Foreground = Brushes.Black;
            Result.Text = "Started";
            PlayButton.Visibility = Visibility.Collapsed;
            HitButton.Visibility = Visibility.Visible;
            StandButton.Visibility = Visibility.Visible;
            SplitButton.Visibility = Visibility.Visible;
            DoubleButton.Visibility = Visibility.Visible;
            Result.Visibility = Visibility.Visible;
            secondgrid.Visibility = Visibility.Collapsed;
            thirdgrid.Visibility = Visibility.Collapsed;
            Moneybox.Visibility = Visibility.Collapsed;
            Wager.Visibility = Visibility.Collapsed;
            newgamebttn.Visibility = Visibility.Collapsed;

            onesecondtimer.Interval = TimeSpan.FromMilliseconds(1000);
            onesecondtimer.Tick += GiveCard_Tick;
            onesecondtimer.Start();
            twosecondtimer.Interval = TimeSpan.FromMilliseconds(2000);
            twosecondtimer.Tick += GiveCard2_Tick;
            twosecondtimer.Start();
            threesecondtimer.Interval = TimeSpan.FromMilliseconds(3000);
            threesecondtimer.Tick += GiveCard3_Tick;
            threesecondtimer.Start();
            
            Checkaceplayer();
            Checkacedealer();

            dealercardvalue = dealercardvalue1;
            DealerScore.Text = Convert.ToString(dealercardvalue1);
            DealerCards.Text = dealercard1;

            if (playercardvalue1 == playercardvalue2)
            {
                SplitButton.IsEnabled = true;
            }
            
        }
        private void GiveCard_Tick(object sender, EventArgs e)
        {
            playercard1 = Randomcard(true, out playercardvalue1);            
            PlayerScore.Text = Convert.ToString(playercardvalue1);
            PlayerCards.Text = $"{playercard1}";
            onesecondtimer.Stop();
        }
        private void GiveCard2_Tick(object sender, EventArgs e)
        {
            playercard2 = Randomcard(true, out playercardvalue2);            
            PlayerScore.Text = Convert.ToString(playercardvalue1 + playercardvalue2);
            PlayerCards.Text = $"{playercard1}\n{playercard2}";
            playercardvalue = playercardvalue1 + playercardvalue2;
            twosecondtimer.Stop();

        }
        private void GiveCard3_Tick(object sender, EventArgs e)
        {
            dealercard1 = Randomcard(false, out dealercardvalue1);
            dealercardvalue = dealercardvalue1;
            DealerScore.Text = Convert.ToString(dealercardvalue1);
            DealerCards.Text = dealercard1;
            threesecondtimer.Stop();


        }
        private void StandButton_Click(object sender, RoutedEventArgs e)
        {
            do
            {
                int dealercardvalue2;
                dealercard2 = Randomcard(false, out dealercardvalue2);
                dealercardvalue += dealercardvalue2;
                DealerScore.Text = Convert.ToString(dealercardvalue);
                DealerCards.Text += $"\n{dealercard2}";

            } while (dealercardvalue <= 17);

            Checkacedealer();

            if (dealercardvalue == 21 && playercardvalue == 21)
            {
                Push();
            }
            else if (dealercardvalue > 21)
            {
                Win();
            }   
            else if (dealercardvalue == 21 && playercardvalue != 21)
            {
                Lose();
            }
            else if (dealercardvalue > playercardvalue)
            {
                Lose();
            }
            else if (playercardvalue > dealercardvalue)
            {
                Win();
            }
            else if (dealercardvalue == playercardvalue)
            {
                Push();
            }
        }
        private string Randomcard(bool isplayer, out int Cardvalue)
        {
            int getal = rnd.Next(1, 5);

            string CardSuit = "empty";

            switch (getal)
            {
                case 1:
                    CardSuit = "Hearts";
                    break;
                case 2:
                    CardSuit = "Clubs";
                    break;
                case 3:
                    CardSuit = "Spades";
                    break;
                case 4:
                    CardSuit = "Diamonds";
                    break;
            }

            Cardvalue = 0;
            name = "";
            int random1 = rnd.Next(1, 13);

            switch (random1)
            {
                case 1:
                    name = "Ace of ";
                    Cardvalue = 11;
                    if (isplayer == true)
                    {
                        playeraces++;
                    }
                    else
                    {
                        dealeraces++;
                    }
                    break;
                case 10:
                    name = "Jack of ";
                    Cardvalue = 10;
                    break;
                case 11:
                    name = "Queen of ";
                    Cardvalue = 10;
                    break;
                case 12:
                    name = "King of ";
                    Cardvalue = 10;
                    break;
                case 2:
                    name = "2 ";
                    Cardvalue = 2;
                    break;
                case 3:
                    name = "3 ";
                    Cardvalue = 3;
                    break;
                case 4:
                    name = "4 ";
                    Cardvalue = 4;
                    break;
                case 5:
                    name = "5 ";
                    Cardvalue = 5;
                    break;
                case 6:
                    name = "6 ";
                    Cardvalue = 6;
                    break;
                case 7:
                    name = "7 ";
                    Cardvalue = 7;
                    break;
                case 8:
                    name = "8 ";
                    Cardvalue = 8;
                    break;
                case 9:
                    name = "9 ";
                    Cardvalue = 9;
                    break;
             
            }


            return name + CardSuit;
        }
        private void Win()
        {
        Result.Foreground = Brushes.Green;
        Result.Text = "Winner!";
        HitButton.Visibility = Visibility.Collapsed;
        StandButton.Visibility = Visibility.Collapsed;
        SplitButton.Visibility = Visibility.Collapsed;
        DoubleButton.Visibility = Visibility.Collapsed;
        PlayButton.Visibility = Visibility.Visible;
        secondgrid.Visibility = Visibility.Visible;
        thirdgrid.Visibility = Visibility.Visible;
        Moneybox.Visibility = Visibility.Visible;
        Wager.Visibility = Visibility.Visible;
        newgamebttn.Visibility = Visibility.Visible;
        Moneybox.Text = "0";
         
        moneywagered *= 2;
        availmoney += moneywagered;
        availablemoney.Text = availmoney.ToString();
        moneywagered = 0;

           

        }
        private void Lose()
        {
            Result.Foreground = Brushes.Red;
            Result.Text = "You lost";
            HitButton.Visibility = Visibility.Collapsed;
            StandButton.Visibility = Visibility.Collapsed;
            SplitButton.Visibility = Visibility.Collapsed;
            DoubleButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            secondgrid.Visibility = Visibility.Visible;
            thirdgrid.Visibility = Visibility.Visible;
            Moneybox.Visibility = Visibility.Visible;
            Wager.Visibility = Visibility.Visible;
            newgamebttn.Visibility = Visibility.Visible;
            Moneybox.Text = "0";
            


            availmoney -= moneywagered;
            availablemoney.Text = availmoney.ToString();
            moneywagered = 0;

            if (availmoney <= 0)
            {
                MessageBox.Show("You went bankrupt!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                secondgrid.Visibility = Visibility.Collapsed;
                thirdgrid.Visibility = Visibility.Collapsed;
                Moneybox.Visibility = Visibility.Collapsed;
                Wager.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Collapsed;
                HitButton.Visibility = Visibility.Collapsed;
                DoubleButton.Visibility = Visibility.Collapsed;
                SplitButton.Visibility = Visibility.Collapsed;
                SplitButton.IsEnabled = false;
                StandButton.Visibility = Visibility.Collapsed;
                newgamebttn.Visibility = Visibility.Visible;
                split1.Visibility = Visibility.Collapsed;
                split2.Visibility = Visibility.Collapsed;
                Fourthgrid.Visibility = Visibility.Collapsed;
            }
        }
        private void Push()
        {
            Result.Foreground = Brushes.Orange;
            Result.Text = "Push!";
            HitButton.Visibility = Visibility.Collapsed;
            StandButton.Visibility = Visibility.Collapsed;
            SplitButton.Visibility = Visibility.Collapsed;
            DoubleButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
            secondgrid.Visibility = Visibility.Visible;
            thirdgrid.Visibility = Visibility.Visible;
            Moneybox.Visibility = Visibility.Visible;
            Wager.Visibility = Visibility.Visible;
            newgamebttn.Visibility = Visibility.Visible;
            Moneybox.Text = "0";
            

            moneywagered = 0;
        }
        private void Checkaceplayer()
        {
            
            if (playeraces > 1 && playercardvalue > 21)
            {
                playercardvalue -= 10;
                PlayerScore.Text = Convert.ToString(playercardvalue);
                playeraces--;
            }
        }
        private void Checkacedealer()
        {
            if (dealeraces > 1 && dealercardvalue > 21)
            {
                dealercardvalue -= 10;
                DealerScore.Text = Convert.ToString(dealercardvalue);
            } 
        }
        private void Resetclient()
        {
            dealercardvalue = 0;
            playercardvalue = 0;
            playeraces = 1;
            dealeraces = 1;
            playercard1 = "";
            playercard2 = "";
            playercard3 = "";
            dealercard1 = "";
            dealercard2 = "";
            PlayerCards.Text = "";
            DealerCards.Text = "";
            DealerScore.Text = "0";
            PlayerScore.Text = "0";
            Result.Text = "Started";
            availablemoney.Text = "100";
            moneywagered = 0;
            availmoney = 100;
            Result.Foreground = Brushes.Black;
            secondgrid.Visibility = Visibility.Visible;
            thirdgrid.Visibility = Visibility.Visible;
            Moneybox.Visibility = Visibility.Visible;
            Wager.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Visible;
            HitButton.Visibility = Visibility.Collapsed;
            DoubleButton.Visibility = Visibility.Collapsed;
            SplitButton.Visibility = Visibility.Collapsed;
            SplitButton.IsEnabled = false;
            StandButton.Visibility = Visibility.Collapsed;
            newgamebttn.Visibility = Visibility.Visible;
            split1.Visibility = Visibility.Collapsed;
            split2.Visibility = Visibility.Collapsed;
            Fourthgrid.Visibility = Visibility.Collapsed;
            HitsplitButton.Visibility = Visibility.Collapsed;
            HitsplitsecondButton.Visibility = Visibility.Collapsed;
            StandsplitButton.Visibility = Visibility.Collapsed;
            StandsplitsecondButton.Visibility = Visibility.Collapsed;
        }
        private bool CheckMoney()
        {
            availablemoney.Text = Convert.ToString(availmoney);

            if (moneywagered > availmoney)
            {
                MessageBox.Show("Do not exceed available money", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                moneywagered = 0;
                Moneybox.Text = Convert.ToString(moneywagered);
                return false;
            }
            else if (moneywagered < 0.1*availmoney)
            {
                MessageBox.Show("Money wagered has to be above 10%", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                moneywagered = 0;
                Moneybox.Text = Convert.ToString(moneywagered);
                return false;
            }
            else
            {
                return true;
            }
        }
        private void plushundred_Click(object sender, RoutedEventArgs e)
        {
            moneywagered += 100;

            if (!CheckMoney())
            {
                moneywagered -= 100;
            }

            Moneybox.Text = Convert.ToString(moneywagered);
        }
        private void minushundred_Click(object sender, RoutedEventArgs e)
        {
            moneywagered -= 100;

            if (!CheckMoney())
            {
                moneywagered += 100;
            }

            Moneybox.Text = Convert.ToString(moneywagered);
        }
        private void plusfifty_Click(object sender, RoutedEventArgs e)
        {
            moneywagered += 50;

            if (!CheckMoney())
            {
                moneywagered -= 50;
            }

            Moneybox.Text = Convert.ToString(moneywagered);
        }
        private void minusfifty_Click(object sender, RoutedEventArgs e)
        {
            moneywagered -= 50;

            if (!CheckMoney())
            {
                moneywagered += 50;
            }

            Moneybox.Text = Convert.ToString(moneywagered);
        }
        private void plusten_Click(object sender, RoutedEventArgs e)
        {
            moneywagered += 10;

            if (!CheckMoney())
            {
                moneywagered -= 10;
            }

            Moneybox.Text = Convert.ToString(moneywagered);
        }
        private void minusten_Click(object sender, RoutedEventArgs e)
        {
            moneywagered -= 10;

            if (!CheckMoney())
            {
                moneywagered += 10;
            }

            Moneybox.Text = Convert.ToString(moneywagered);
        }
        private void DoubleButton_Click(object sender, RoutedEventArgs e)
        {
            
            
            moneywagered *= 2;

            int playercardvalue1;
            playercard1 = Randomcard(true, out playercardvalue1);
            playercardvalue += playercardvalue1;
            PlayerScore.Text = Convert.ToString(playercardvalue);
            PlayerCards.Text += $"\n{playercard1}";

            Checkaceplayer();

            if (playercardvalue > 21)
            {
                Lose();
            }

            do
            {
                int dealercardvalue1;
                dealercard1 = Randomcard(false, out dealercardvalue1);
                dealercardvalue += dealercardvalue1;
                DealerScore.Text = Convert.ToString(dealercardvalue);
                DealerCards.Text += $"\n{dealercard1}";

            } while (dealercardvalue <= 17);

            Checkacedealer();

            if (dealercardvalue == 21 && playercardvalue == 21)
            {
                Push();
            }
            else if (dealercardvalue > 21)
            {
                Win();
            }
            else if (dealercardvalue == 21 && playercardvalue != 21)
            {
                Lose();
            }
            else if (dealercardvalue > playercardvalue)
            {
                Lose();
            }
            else if (playercardvalue > dealercardvalue)
            {
                Win();
            }
            else if (dealercardvalue == playercardvalue)
            {
                Push();
            }
        }
        private void SplitButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerCards.Visibility = Visibility.Collapsed;
            PlayerScore.Visibility = Visibility.Collapsed;
            split1.Visibility = Visibility.Visible;
            split2.Visibility = Visibility.Visible;
            Fourthgrid.Visibility = Visibility.Visible;
            SplitButton.Visibility = Visibility.Collapsed;
            DoubleButton.Visibility = Visibility.Collapsed;
            HitButton.Visibility = Visibility.Collapsed;
            StandButton.Visibility = Visibility.Collapsed;
            HitsplitButton.Visibility = Visibility.Visible;
            StandsplitButton.Visibility = Visibility.Visible;
            split1.IsEnabled = true;

            moneywagered *= 2;

            PlayerScore1.Text = Convert.ToString(playercardvalue1);
            PlayerScore2.Text = Convert.ToString(playercardvalue2);

            split1.Text = playercard1;
            split2.Text = playercard2;
            



        }
        private void newgamebttn_Click(object sender, RoutedEventArgs e)
        {
            Resetclient();
        }

        private void HitsplitButton_Click(object sender, RoutedEventArgs e)
        {
            split1.IsEnabled = false;
            split2.IsEnabled = true;
            HitsplitButton.Visibility = Visibility.Collapsed;
            HitsplitsecondButton.Visibility = Visibility.Visible;
            StandsplitButton.Visibility = Visibility.Collapsed;
            StandsplitsecondButton.Visibility = Visibility.Visible;

            int playercardvalue3;
            playercard3 = Randomcard(true, out playercardvalue3);
            playercardvalue += playercardvalue3;
            PlayerScore1.Text = Convert.ToString(playercardvalue);
            split1.Text += $"\n{playercard3}";

            Checkaceplayer();

            if (playercardvalue > 21)
            {

                Lose();
            }
        }

        private void HitsplitsecondButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StandsplitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StandsplitsecondButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

