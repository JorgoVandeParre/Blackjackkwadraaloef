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
            ResetClient();
            oneSecondTimer.Interval = TimeSpan.FromMilliseconds(1000);
            oneSecondTimer.Tick += GiveCard_Tick;
            twoSecondTimer.Interval = TimeSpan.FromMilliseconds(2000);
            twoSecondTimer.Tick += GiveCard2_Tick;
            threeSecondTimer.Interval = TimeSpan.FromMilliseconds(3000);
            threeSecondTimer.Tick += GiveCard3_Tick;
            fourSecondTimer.Interval = TimeSpan.FromMilliseconds(4000);
            fourSecondTimer.Tick += GiveCard4_Tick;
            fiveSecondTimer.Interval = TimeSpan.FromMilliseconds(5000);
            fiveSecondTimer.Tick += Play_Tick;
            standTimer.Interval = TimeSpan.FromMilliseconds(1000);
            standTimer.Tick += StandGiveCard_Tick;

            DispatcherTimer timeNow = new DispatcherTimer();
            timeNow.Interval = TimeSpan.FromSeconds(1);
            timeNow.Tick += Timer_Tick;
            timeNow.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LiveTime.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        bool split1IsPlaying = true;
        bool split2IsPlaying = true;
        int playerCardValue1;
        int playerCardValue2;
        int dealerCardValue1;
        int dealerCardValue2;
        private int availMoney = 100;
        private int playerAces = 1;
        private int dealerAces = 1;
        private int moneyWagered;
        private int dealerCardValue;
        private int playerCardValue;
        private string playerCard1 = "empty";
        private string playerCard2 = "empty";
        private string playerCard3 = "empty";
        private string dealerCard1 = "empty";
        private string dealerCard2 = "empty";
        private string name;
        private Random rnd = new Random();
        DispatcherTimer oneSecondTimer = new DispatcherTimer();
        DispatcherTimer twoSecondTimer = new DispatcherTimer();
        DispatcherTimer threeSecondTimer = new DispatcherTimer();
        DispatcherTimer fourSecondTimer = new DispatcherTimer();
        DispatcherTimer fiveSecondTimer = new DispatcherTimer();
        DispatcherTimer standTimer = new DispatcherTimer();

        private void HitButton_Click(object sender, RoutedEventArgs e)
        {
            Hit();  
        }
        private void Hit()
        {

            if (PlayerCards.Visibility == Visibility.Visible)
            {
                int playerCardValue3;
                playerCard3 = RandomCard(true, out playerCardValue3);
                playerCardValue += playerCardValue3;
                PlayerScore.Text = Convert.ToString(playerCardValue);
                PlayerCards.Text += $"\n{playerCard3}";

                CheckAcePlayer();

                if (playerCardValue > 21)
                {
                    Lose();
                }
            }
            else if (!Split2.IsEnabled)
            {
                int playerCardValue3;
                playerCard3 = RandomCard(true, out playerCardValue3);
                playerCardValue1 += playerCardValue3;
                PlayerScore2.Text = Convert.ToString(playerCardValue1);
                Split1.Text += $"\n{playerCard3}";
                CheckAcePlayer();
                if (playerCardValue2 > 21 && split2IsPlaying == true)
                {
                    Lose();
                }

            }
            else if (!Split1.IsEnabled)
            {
                int playerCardValue3;
                playerCard3 = RandomCard(true, out playerCardValue3);
                playerCardValue2 += playerCardValue3;
                PlayerScore1.Text = Convert.ToString(playerCardValue2);
                Split2.Text += $"\n{playerCard3}";
                CheckAcePlayer();
                if (playerCardValue1 > 21 && split1IsPlaying == true)
                {
                    Lose();
                }
            }

        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }
        private void Play()
        {
            if (!CheckMoney())
            {
                return;
            }

            PlayerScore.Text = "0";
            DealerScore.Text = "0";
            PlayerCards.Text = "";
            DealerCards.Text = "";
            SplitResult1.Text = "";
            SplitResult2.Text = "";
            PlayerScore1.Text = "";
            PlayerScore2.Text = "";
            split2IsPlaying = true;
            split1IsPlaying = true;
            Result.Foreground = Brushes.Black;
            Result.Text = "Started";
            PlayerCards.Visibility = Visibility.Visible;
            Split1.Visibility = Visibility.Collapsed;
            Split2.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Collapsed;
            Result.Visibility = Visibility.Visible;
            SecondGrid.Visibility = Visibility.Collapsed;
            ThirdGrid.Visibility = Visibility.Collapsed;
            MoneyBox.Visibility = Visibility.Collapsed;
            Wager.Visibility = Visibility.Collapsed;
            NewGameBTTN.Visibility = Visibility.Collapsed;
            SplitResult1.Visibility = Visibility.Collapsed;
            SplitResult2.Visibility = Visibility.Collapsed;
            FourthGrid.Visibility = Visibility.Collapsed;
            PlayerScore.Visibility = Visibility.Visible;

            oneSecondTimer.Start();
            twoSecondTimer.Start();
            threeSecondTimer.Start();
            fourSecondTimer.Start();
            fiveSecondTimer.Start();
            
        }
        private void GiveCard_Tick(object sender, EventArgs e)
        {
            playerCard1 = RandomCard(true, out playerCardValue1);            
            PlayerScore.Text = Convert.ToString(playerCardValue1);
            PlayerCards.Text = $"{playerCard1}";
            oneSecondTimer.Stop();
        }
        private void GiveCard2_Tick(object sender, EventArgs e)
        {
            playerCard2 = RandomCard(true, out playerCardValue2);            
            PlayerScore.Text = Convert.ToString(playerCardValue1 + playerCardValue2);
            PlayerCards.Text = $"{playerCard1}\n{playerCard2}";
            playerCardValue = playerCardValue1 + playerCardValue2;
            twoSecondTimer.Stop();

        }
        private void GiveCard3_Tick(object sender, EventArgs e)
        {
            dealerCard1 = RandomCard(false, out dealerCardValue1);
            dealerCardValue = dealerCardValue1;
            DealerScore.Text = Convert.ToString(dealerCardValue);
            DealerCards.Text = dealerCard1;
            threeSecondTimer.Stop();



        }
        private void GiveCard4_Tick(object sender, EventArgs e)
        {
            dealerCard2 = RandomCard(false, out dealerCardValue2);
            dealerCardValue += dealerCardValue2;
            DealerScore.Text = Convert.ToString(dealerCardValue);
            DealerCards.Text = $"{dealerCard1}\n{dealerCard2}";


            PlayButton.Visibility = Visibility.Collapsed;
            HitButton.Visibility = Visibility.Visible;
            StandButton.Visibility = Visibility.Visible;
            SplitButton.Visibility = Visibility.Visible;
            DoubleButton.Visibility = Visibility.Visible;
            fourSecondTimer.Stop();
        }
        private void Play_Tick(object sender, EventArgs e)
        {
            CheckAcePlayer();
            CheckAceDealer();


            if (/*playercardvalue1 == playercardvalue2 &&*/ moneyWagered * 2 <= availMoney)
            {
                SplitButton.IsEnabled = true;
            }
            if (moneyWagered * 2 <= availMoney)
            {
                DoubleButton.IsEnabled = true;
            }
        }
        private void StandGiveCard_Tick(object sender, EventArgs e)
        {


            if (!(dealerCardValue <= 17))
            {
                if (dealerCardValue == 21 && playerCardValue == 21)
                {
                    Push();
                }
                else if (dealerCardValue > 21)
                {
                    Win();
                }
                else if (dealerCardValue == 21 && playerCardValue != 21)
                {
                    Lose();
                }
                else if (dealerCardValue > playerCardValue)
                {
                    Lose();
                }
                else if (playerCardValue > dealerCardValue)
                {
                    Win();
                }
                else if (dealerCardValue == playerCardValue)
                {
                    Push();
                }
                standTimer.Stop();
            }
            else
            {
                int dealercardvalue2;
                dealerCard2 = RandomCard(false, out dealercardvalue2);
                dealerCardValue += dealercardvalue2;
                DealerScore.Text = Convert.ToString(dealerCardValue);
                DealerCards.Text += $"\n{dealerCard2}";

            }
            
        }
        private void StandButton_Click(object sender, RoutedEventArgs e)
        {
            Stand();
        }
        private void Stand()
        {
            
            standTimer.Start();

        }
        private string RandomCard(bool isplayer, out int Cardvalue)
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
                        playerAces++;
                    }
                    else
                    {
                        dealerAces++;
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
        SecondGrid.Visibility = Visibility.Visible;
        ThirdGrid.Visibility = Visibility.Visible;
        MoneyBox.Visibility = Visibility.Visible;
        Wager.Visibility = Visibility.Visible;
        NewGameBTTN.Visibility = Visibility.Visible;
        MoneyBox.Text = "0";
            dealerCardValue = 0;
            dealerCardValue1 = 0;
            dealerCardValue2 = 0;
            playerCardValue = 0;
            playerCardValue1 = 0;
            playerCardValue2 = 0;
            playerAces = 1;
            dealerAces = 1;
            playerCard1 = "";
            playerCard2 = "";
            playerCard3 = "";
            dealerCard1 = "";
            dealerCard2 = "";

            moneyWagered *= 2;
        availMoney += moneyWagered;
        availablemoney.Text = availMoney.ToString();
        moneyWagered = 0;

           

        }
        private void Lose()
        {

            if (PlayerCards.Visibility == Visibility.Visible)
            {
                Result.Foreground = Brushes.Red;
                Result.Text = "You lost";
                HitButton.Visibility = Visibility.Collapsed;
                StandButton.Visibility = Visibility.Collapsed;
                SplitButton.Visibility = Visibility.Collapsed;
                DoubleButton.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Visible;
                SecondGrid.Visibility = Visibility.Visible;
                ThirdGrid.Visibility = Visibility.Visible;
                MoneyBox.Visibility = Visibility.Visible;
                Wager.Visibility = Visibility.Visible;
                NewGameBTTN.Visibility = Visibility.Visible;
                MoneyBox.Text = "0";
                dealerCardValue = 0;
                dealerCardValue1 = 0;
                dealerCardValue2 = 0;
                playerCardValue = 0;
                playerCardValue1 = 0;
                playerCardValue2 = 0;
                playerAces = 1;
                dealerAces = 1;
                playerCard1 = "";
                playerCard2 = "";
                playerCard3 = "";
                dealerCard1 = "";
                dealerCard2 = "";


                availMoney -= moneyWagered;
                availablemoney.Text = availMoney.ToString();
                moneyWagered = 0;

            }
            else if (!Split1.IsEnabled == true)
            {
                SplitResult1.Foreground = Brushes.Red;
                SplitResult1.Text += "You lost";
                SplitResult1.Visibility = Visibility.Visible;
                split1IsPlaying = false;
                if (split2IsPlaying == false)
                {
                    availMoney -= moneyWagered;
                    availablemoney.Text = availMoney.ToString();
                    moneyWagered = 0;
                    MoneyBox.Text = "0";
                    dealerCardValue = 0;
                    dealerCardValue1 = 0;
                    dealerCardValue2 = 0;
                    playerCardValue = 0;
                    playerCardValue1 = 0;
                    playerCardValue2 = 0;
                    playerAces = 1;
                    dealerAces = 1;
                    playerCard1 = "";
                    playerCard2 = "";
                    playerCard3 = "";
                    dealerCard1 = "";
                    dealerCard2 = "";

                    HitsplitButton.Visibility = Visibility.Collapsed;
                    HitsplitsecondButton.Visibility = Visibility.Collapsed;
                    StandsplitButton.Visibility = Visibility.Collapsed;
                    StandsplitsecondButton.Visibility = Visibility.Collapsed;
                    PlayButton.Visibility = Visibility.Visible;
                    SecondGrid.Visibility = Visibility.Visible;
                    ThirdGrid.Visibility = Visibility.Visible;
                    MoneyBox.Visibility = Visibility.Visible;
                    Wager.Visibility = Visibility.Visible;
                    NewGameBTTN.Visibility = Visibility.Visible;

                }
                else
                {
                    moneyWagered /= 2;
                }
            }
            else if (!Split2.IsEnabled == true)
            {
                moneyWagered /= 2;
                SplitResult2.Foreground = Brushes.Red;
                SplitResult2.Text += "You lost";
                SplitResult2.Visibility = Visibility.Visible;
                split2IsPlaying = false;
                if (split1IsPlaying == false)
                {
                    availMoney -= moneyWagered;
                    availablemoney.Text = availMoney.ToString();
                    moneyWagered = 0;
                    MoneyBox.Text = "0";
                    dealerCardValue = 0;
                    dealerCardValue1 = 0;
                    dealerCardValue2 = 0;
                    playerCardValue = 0;
                    playerCardValue1 = 0;
                    playerCardValue2 = 0;
                    playerAces = 1;
                    dealerAces = 1;
                    playerCard1 = "";
                    playerCard2 = "";
                    playerCard3 = "";
                    dealerCard1 = "";
                    dealerCard2 = "";

                    HitsplitButton.Visibility = Visibility.Collapsed;
                    HitsplitsecondButton.Visibility = Visibility.Collapsed;
                    StandsplitButton.Visibility = Visibility.Collapsed;
                    StandsplitsecondButton.Visibility = Visibility.Collapsed;
                    PlayButton.Visibility = Visibility.Visible;
                    SecondGrid.Visibility = Visibility.Visible;
                    ThirdGrid.Visibility = Visibility.Visible;
                    MoneyBox.Visibility = Visibility.Visible;
                    Wager.Visibility = Visibility.Visible;
                    NewGameBTTN.Visibility = Visibility.Visible;
                }
                else
                {
                    moneyWagered /= 2;
                }
            }

            if (availMoney <= 0)
            {
                MessageBox.Show("You went bankrupt!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SecondGrid.Visibility = Visibility.Collapsed;
                ThirdGrid.Visibility = Visibility.Collapsed;
                MoneyBox.Visibility = Visibility.Collapsed;
                Wager.Visibility = Visibility.Collapsed;
                PlayButton.Visibility = Visibility.Collapsed;
                HitButton.Visibility = Visibility.Collapsed;
                DoubleButton.Visibility = Visibility.Collapsed;
                SplitButton.Visibility = Visibility.Collapsed;
                SplitButton.IsEnabled = false;
                StandButton.Visibility = Visibility.Collapsed;
                NewGameBTTN.Visibility = Visibility.Visible;
                Split1.Visibility = Visibility.Collapsed;
                Split2.Visibility = Visibility.Collapsed;
                FourthGrid.Visibility = Visibility.Collapsed;
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
            SecondGrid.Visibility = Visibility.Visible;
            ThirdGrid.Visibility = Visibility.Visible;
            MoneyBox.Visibility = Visibility.Visible;
            Wager.Visibility = Visibility.Visible;
            NewGameBTTN.Visibility = Visibility.Visible;
            MoneyBox.Text = "0";
            dealerCardValue = 0;
            dealerCardValue1 = 0;
            dealerCardValue2 = 0;
            playerCardValue = 0;
            playerCardValue1 = 0;
            playerCardValue2 = 0;
            playerAces = 1;
            dealerAces = 1;
            playerCard1 = "";
            playerCard2 = "";
            playerCard3 = "";
            dealerCard1 = "";
            dealerCard2 = "";
            moneyWagered = 0;
        }
        private void CheckAcePlayer()
        {
            
            if (playerAces > 1 && playerCardValue > 21)
            {
                playerCardValue -= 10;
                PlayerScore.Text = Convert.ToString(playerCardValue);
                playerAces--;
            }
        }
        private void CheckAceDealer()
        {
            if (dealerAces > 1 && dealerCardValue > 21)
            {
                dealerCardValue -= 10;
                DealerScore.Text = Convert.ToString(dealerCardValue);
            } 
        }
        private void ResetClient()
        {
            dealerCardValue = 0;
            playerCardValue = 0;
            playerAces = 1;
            dealerAces = 1;
            playerCard1 = "";
            playerCard2 = "";
            playerCard3 = "";
            dealerCard1 = "";
            dealerCard2 = "";
            PlayerCards.Text = "";
            DealerCards.Text = "";
            DealerScore.Text = "0";
            PlayerScore.Text = "0";
            Result.Text = "Started";
            availablemoney.Text = "100";
            split1IsPlaying = true;
            split2IsPlaying = true;
            moneyWagered = 0;
            availMoney = 100;
            Result.Foreground = Brushes.Black;
            SplitResult1.Foreground = Brushes.Black;
            SplitResult2.Foreground = Brushes.Black;
            SplitResult2.Visibility = Visibility.Collapsed;
            SplitResult1.Visibility = Visibility.Collapsed;
            PlayerScore.Visibility = Visibility.Visible;
            PlayerCards.Visibility = Visibility.Visible;
            SecondGrid.Visibility = Visibility.Visible;
            ThirdGrid.Visibility = Visibility.Visible;
            MoneyBox.Visibility = Visibility.Visible;
            Wager.Visibility = Visibility.Visible;
            PlayButton.Visibility = Visibility.Visible;
            HitButton.Visibility = Visibility.Collapsed;
            DoubleButton.Visibility = Visibility.Collapsed;
            DoubleButton.IsEnabled = false;
            SplitButton.Visibility = Visibility.Collapsed;
            SplitButton.IsEnabled = false;
            StandButton.Visibility = Visibility.Collapsed;
            NewGameBTTN.Visibility = Visibility.Visible;
            Split1.Visibility = Visibility.Collapsed;
            Split2.Visibility = Visibility.Collapsed;
            FourthGrid.Visibility = Visibility.Collapsed;
            HitsplitButton.Visibility = Visibility.Collapsed;
            HitsplitsecondButton.Visibility = Visibility.Collapsed;
            StandsplitButton.Visibility = Visibility.Collapsed;
            StandsplitsecondButton.Visibility = Visibility.Collapsed;

        }
        private bool CheckMoney()
        {
            availablemoney.Text = Convert.ToString(availMoney);

            if (moneyWagered > availMoney)
            {
                MessageBox.Show("Do not exceed available money", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                moneyWagered = 0;
                MoneyBox.Text = Convert.ToString(moneyWagered);
                return false;
            }
            else if (moneyWagered < 0.1*availMoney)
            {
                MessageBox.Show("Money wagered has to be above 10%", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                moneyWagered = 0;
                MoneyBox.Text = Convert.ToString(moneyWagered);
                return false;
            }
            else
            {
                return true;
            }
        }
        private void PlusHundred_Click(object sender, RoutedEventArgs e)
        {
            moneyWagered += 100;

            if (!CheckMoney())
            {
                moneyWagered -= 100;
            }

            MoneyBox.Text = Convert.ToString(moneyWagered);
        }
        private void MinusHundred_Click(object sender, RoutedEventArgs e)
        {
            moneyWagered -= 100;

            if (!CheckMoney())
            {
                moneyWagered += 100;
            }

            MoneyBox.Text = Convert.ToString(moneyWagered);
        }
        private void PlusFifty_Click(object sender, RoutedEventArgs e)
        {
            moneyWagered += 50;

            if (!CheckMoney())
            {
                moneyWagered -= 50;
            }

            MoneyBox.Text = Convert.ToString(moneyWagered);
        }
        private void MinusFifty_Click(object sender, RoutedEventArgs e)
        {
            moneyWagered -= 50;

            if (!CheckMoney())
            {
                moneyWagered += 50;
            }

            MoneyBox.Text = Convert.ToString(moneyWagered);
        }
        private void PlusTen_Click(object sender, RoutedEventArgs e)
        {
            moneyWagered += 10;

            if (!CheckMoney())
            {
                moneyWagered -= 10;
            }

            MoneyBox.Text = Convert.ToString(moneyWagered);
        }
        private void MinusTen_Click(object sender, RoutedEventArgs e)
        {
            moneyWagered -= 10;

            if (!CheckMoney())
            {
                moneyWagered += 10;
            }

            MoneyBox.Text = Convert.ToString(moneyWagered);
        }
        private void DoubleButton_Click(object sender, RoutedEventArgs e)
        {
            moneyWagered *= 2;
            Hit();
            Stand();
        }
        private void SplitButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerCards.Visibility = Visibility.Collapsed;
            PlayerScore.Visibility = Visibility.Collapsed;
            Split1.Visibility = Visibility.Visible;
            Split2.Visibility = Visibility.Visible;
            FourthGrid.Visibility = Visibility.Visible;
            SplitButton.Visibility = Visibility.Collapsed;
            DoubleButton.Visibility = Visibility.Collapsed;
            HitButton.Visibility = Visibility.Collapsed;
            StandButton.Visibility = Visibility.Collapsed;
            HitsplitButton.Visibility = Visibility.Visible;
            StandsplitButton.Visibility = Visibility.Visible;
            Split1.IsEnabled = true;

            moneyWagered *= 2;

            PlayerScore1.Text = Convert.ToString(playerCardValue1);
            PlayerScore2.Text = Convert.ToString(playerCardValue2);

            Split1.Text = playerCard2;
            Split2.Text = playerCard1;
            



        }
        private void NewGameBTTN_Click(object sender, RoutedEventArgs e)
        {
            ResetClient();
        }
        private void HitSplitButton_Click(object sender, RoutedEventArgs e)
        {
            Hit();
            if (split2IsPlaying == true)
            {
                Split1.IsEnabled = false;
                Split2.IsEnabled = true;
                HitsplitButton.Visibility = Visibility.Collapsed;
                HitsplitsecondButton.Visibility = Visibility.Visible;
                StandsplitButton.Visibility = Visibility.Collapsed;
                StandsplitsecondButton.Visibility = Visibility.Visible;
            }
        }
        private void HitSplitSecondButton_Click(object sender, RoutedEventArgs e)
        {
            Hit();
            if (split1IsPlaying == true)
            {
                Split1.IsEnabled = true;
                Split2.IsEnabled = false;
                HitsplitButton.Visibility = Visibility.Visible;
                HitsplitsecondButton.Visibility = Visibility.Collapsed;
                StandsplitButton.Visibility = Visibility.Visible;
                StandsplitsecondButton.Visibility = Visibility.Collapsed;
            }
        }
        private void StandSplitButton_Click(object sender, RoutedEventArgs e)
        {
            if (split2IsPlaying == true)
            {
                Split1.IsEnabled = false;
                Split2.IsEnabled = true;
                HitsplitButton.Visibility = Visibility.Collapsed;
                HitsplitsecondButton.Visibility = Visibility.Visible;
                StandsplitButton.Visibility = Visibility.Collapsed;
                StandsplitsecondButton.Visibility = Visibility.Visible;
            }
            Stand();
        }
        private void StandSplitSecondButton_Click(object sender, RoutedEventArgs e)
        {
            if (split1IsPlaying == true)
            {
                Split1.IsEnabled = true;
                Split2.IsEnabled = false;
                HitsplitButton.Visibility = Visibility.Visible;
                HitsplitsecondButton.Visibility = Visibility.Collapsed;
                StandsplitButton.Visibility = Visibility.Visible;
                StandsplitsecondButton.Visibility = Visibility.Collapsed;
            }
            Stand();
        }

  
    }
}

