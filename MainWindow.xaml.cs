using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
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

        int dealercardvalue;
        int playercardvalue;
        string playercard1;
        string playercard2;
        string dealercard1;
        string name;
        Random rnd = new Random();
        bool isplayer = true;
        private void HitButton_Click(object sender, RoutedEventArgs e)
        {
            int playercardvalue1;
            playercard1 = Randomcard(true, out playercardvalue1);
            playercardvalue += playercardvalue1;
            PlayerScore.Text = Convert.ToString(playercardvalue);
            PlayerCards.Text += $"\n{playercard1}";

            //Checkaceplayer();

            if (playercardvalue > 21)
            {
                Lose();
            }
            
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Resetclient();
            PlayButton.Visibility = Visibility.Collapsed;
            if (HitButton.Visibility == Visibility.Collapsed && StandButton.Visibility == Visibility.Collapsed && SplitButton.Visibility == Visibility.Collapsed && DoubleButton.Visibility == Visibility.Collapsed)
            {
                HitButton.Visibility = Visibility.Visible;
                StandButton.Visibility = Visibility.Visible;
                SplitButton.Visibility = Visibility.Visible;
                DoubleButton.Visibility = Visibility.Visible; 
            }
            if (Result.Visibility == Visibility.Collapsed)
            {
                Result.Visibility = Visibility.Visible;
            }

            int playercardvalue1;
            int playercardvalue2;
            int dealercardvalue1;
            playercard1 = Randomcard(true, out playercardvalue1);
            playercard2 = Randomcard(true, out playercardvalue2);
            dealercard1 = Randomcard(true, out dealercardvalue1);
            //Checkaceplayer();
            //Checkacedealer();
            playercardvalue = playercardvalue1 + playercardvalue2;
            dealercardvalue = dealercardvalue1;
            PlayerScore.Text = Convert.ToString(playercardvalue);
            PlayerCards.Text = $"{playercard1}\n{playercard2}";
            DealerScore.Text = Convert.ToString(dealercardvalue1);
            DealerCards.Text = dealercard1;

            
        }
        private void StandButton_Click(object sender, RoutedEventArgs e)
        {
            do
            {
                int dealercardvalue1;
                dealercard1 = Randomcard(true, out dealercardvalue1);
                dealercardvalue += dealercardvalue1;
                DealerScore.Text = Convert.ToString(dealercardvalue);
                DealerCards.Text += $"\n{dealercard1}";

            } while (dealercardvalue <= 17);

            //Checkacedealer();

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
                    Cardvalue = 1;
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
        }
        //private void Checkaceplayer()
        //{
        //    string ace = "Ace";
        //    bool doesitcontainace = playercard1.Contains(ace);
        //    bool doesitcontainace2 = playercard2.Contains(ace);

        //    if (doesitcontainace)
        //    {
        //        playercardvalue -= 10;
        //        PlayerScore.Text = Convert.ToString(playercardvalue);
        //    }
        //    else if (doesitcontainace2)
        //    {
        //        playercardvalue -= 10;
        //        PlayerScore.Text = Convert.ToString(playercardvalue);
        //    }
        //}
        //private void Checkacedealer()
        //{
        //   string ace = "Ace";
        //    bool doesitcontainace = dealercard1.Contains(ace);
        //    //bool doesitcontainace2 = dealercard2.Contains(ace);

        //    if (doesitcontainace)
        //    {
        //        dealercardvalue -= 10;
        //        DealerScore.Text = Convert.ToString(dealercardvalue);
        //    }
        //    //else if (doesitcontainace2)
        //    //{
        //    //    playercardvalue -= 10;
        //    //    DealerScore.Text = Convert.ToString(dealercardvalue);
        //    //}
        //}
        private void Resetclient()
        {
            dealercardvalue = 0;
            playercardvalue = 0;
            playercard1 = "0";
            playercard2 = "0";
            dealercard1 = "0";
            PlayerCards.Text = "";
            DealerCards.Text = "";
            DealerScore.Text = "0";
            PlayerScore.Text = "0";
            Result.Text = "Started";
            Result.Foreground = Brushes.Black;
        } 
    }
}

