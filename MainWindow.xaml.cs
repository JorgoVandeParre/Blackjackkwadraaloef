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
        // i was not able to finish the split function because i poorly planned and underestimated the effort it took, i still have a half finished version of it if u want to use it

        /// <summary>
        /// <para>The client resets returning it to its begin state</para>
        /// <para>All timer tick get made</para>
        /// <para>The live clock found in app gets made and starts</para>
        /// </summary>
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
            standTimer.Interval = TimeSpan.FromMilliseconds(2000);
            standTimer.Tick += StandGiveCard_Tick;
            showSecondDealerCard.Interval = TimeSpan.FromMilliseconds(1000);
            showSecondDealerCard.Tick += ShowSecondDealerCard_Tick;

            DispatcherTimer timeNow = new DispatcherTimer();
            timeNow.Interval = TimeSpan.FromSeconds(1);
            timeNow.Tick += Timer_Tick;
            timeNow.Start();
        }
        /// <summary>
        /// <para>The Live clock Tick</para>
        /// </summary>
        private void Timer_Tick(object sender, EventArgs e)
        {
            LiveTime.Content = DateTime.Now.ToString("HH:mm:ss");
        }

        private bool isItPush = false; // checks if the result of the game was a push (used for updating the history)
        private bool didPlayerWin; // checks if player won (used for updating history)
        private bool isHiddenCard = false; // checks if the card being delt is the hidden card (used in randomcard methode to hide the image)
        private bool isDoubleDown = false; // gets toggled if player clicks on double down (used to change money wagered)
        private bool split1IsPlaying = true; // checks which hand is playing at the time in the split function
        private bool split2IsPlaying = true; // checks which hand is playing at the time in the split function
        private int playerCardValue1;
        private int playerCardValue2;
        private int dealerCardValue1;
        private int dealerCardValue2;
        private int availMoney = 100; // money the player has
        private int playerAces = 1;
        private int dealerAces = 1;
        private int moneyWagered; // how much money the player wagered
        private int dealerCardValue;
        private int playerCardValue;
        private int numberOfCardsInDeck = 52;
        private int historyUpdator = 1; // counts what round u are playing
        private string playerCard1 = "empty";
        private string playerCard2 = "empty";
        private string playerCard3 = "empty";
        private string dealerCard1 = "empty";
        private string dealerCard2 = "empty";
        private string plusOrMinus = ""; // used in the history feature this is determined by isitpush or didplayerwin so the history can output the right symbol
        private Random rnd = new Random(); 
        private DispatcherTimer oneSecondTimer = new DispatcherTimer(); // 1 second from play button adds first player card
        private DispatcherTimer twoSecondTimer = new DispatcherTimer(); // 2 seconds from play button adds second player card
        private DispatcherTimer threeSecondTimer = new DispatcherTimer(); // 3 seconds from play button adds first dealer card
        private DispatcherTimer fourSecondTimer = new DispatcherTimer(); // 4 seconds from play button adds second dealer card
        private DispatcherTimer fiveSecondTimer = new DispatcherTimer(); // 5 seconds from play button adds the option to use split/double down and also checks the aces on the board
        private DispatcherTimer standTimer = new DispatcherTimer(); // hands out dealer cards untill he gets 17 or higher then shows game result
        private DispatcherTimer showSecondDealerCard = new DispatcherTimer(); // flips over dealers second card that was still invisible after pressing stand
        private List<string> newDeck = new List<string>(){
                "Ace of Hearts",
                "Ace of Spades",
                "Ace of Clubs",
                "Ace of Diamonds",
                "Jack of Hearts",
                "Jack of Spades",
                "Jack of Clubs",
                "Jack of Diamonds",
                "King of Hearts",
                "King of Spades",
                "King of Clubs",
                "King of Diamonds",
                "Queen of Hearts",
                "Queen of Spades",
                "Queen of Clubs",
                "Queen of Diamonds",
                "Two of Hearts",
                "Two of Spades",
                "Two of Clubs",
                "Two of Diamonds",
                "Three of Hearts",
                "Three of Spades",
                "Three of Clubs",
                "Three of Diamonds",
                "Four of Hearts",
                "Four of Spades",
                "Four of Clubs",
                "Four of Diamonds",
                "Five of Hearts",
                "Five of Spades",
                "Five of Clubs",
                "Five of Diamonds",
                "Six of Hearts",
                "Six of Spades",
                "Six of Clubs",
                "Six of Diamonds",
                "Seven of Hearts",
                "Seven of Spades",
                "Seven of Clubs",
                "Seven of Diamonds",
                "Eight of Hearts",
                "Eight of Spades",
                "Eight of Clubs",
                "Eight of Diamonds",
                "Nine of Hearts",
                "Nine of Spades",
                "Nine of Clubs",
                "Nine of Diamonds",
                "Ten of Hearts",
                "Ten of Spades",
                "Ten of Clubs",
                "Ten of Diamonds",
            }; // makes a new deck used when the list deck becomes empty
        private List<string> deck = new List<string>(){
                "Ace of Hearts",
                "Ace of Spades",
                "Ace of Clubs",
                "Ace of Diamonds",
                "Jack of Hearts",
                "Jack of Spades",
                "Jack of Clubs",
                "Jack of Diamonds",
                "King of Hearts",
                "King of Spades",
                "King of Clubs",
                "King of Diamonds",
                "Queen of Hearts",
                "Queen of Spades",
                "Queen of Clubs",
                "Queen of Diamonds",
                "Two of Hearts",
                "Two of Spades",
                "Two of Clubs",
                "Two of Diamonds",
                "Three of Hearts",
                "Three of Spades",
                "Three of Clubs",
                "Three of Diamonds",
                "Four of Hearts",
                "Four of Spades",
                "Four of Clubs",
                "Four of Diamonds",
                "Five of Hearts",
                "Five of Spades",
                "Five of Clubs",
                "Five of Diamonds",
                "Six of Hearts",
                "Six of Spades",
                "Six of Clubs",
                "Six of Diamonds",
                "Seven of Hearts",
                "Seven of Spades",
                "Seven of Clubs",
                "Seven of Diamonds",
                "Eight of Hearts",
                "Eight of Spades",
                "Eight of Clubs",
                "Eight of Diamonds",
                "Nine of Hearts",
                "Nine of Spades",
                "Nine of Clubs",
                "Nine of Diamonds",
                "Ten of Hearts",
                "Ten of Spades",
                "Ten of Clubs",
                "Ten of Diamonds",
            }; // the deck where all the cards are
        private List<string> history = new List<string>()
        {
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
        }; // the history of past games are in here

        private void HitButton_Click(object sender, RoutedEventArgs e)
        {
            Hit();
        }
        /// <summary>
        /// <para>Firstly checks if the hit is used on split or not then gives 1 card to the player</para>
        /// </summary>
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
                DoubleButton.IsEnabled = false;
            }

        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Play();
        }
        /// <summary>
        /// <para>first it uses checkmoney method to see if the player used a valid amount of money then resets the app to a playable position and starts the ticks for handing out cards</para>
        /// </summary>
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

            oneSecondTimer.Start(); // related to GiveCard_Tick
            twoSecondTimer.Start(); // related to GiveCard2_Tick
            threeSecondTimer.Start(); // related to GiveCard3_Tick
            fourSecondTimer.Start(); // related to GiveCard4_Tick
            fiveSecondTimer.Start(); // related to Play_Tick

        }
        /// <summary>
        ///  <para>Gives the player his first card</para>
        /// </summary>
        private void GiveCard_Tick(object sender, EventArgs e)
        {
            playerCard1 = RandomCard(true, out playerCardValue1);
            PlayerScore.Text = Convert.ToString(playerCardValue1);
            PlayerCards.Text = $"{playerCard1}";
            oneSecondTimer.Stop();
        }
        /// <summary>
        ///  <para>Gives the player his second card</para>
        /// </summary>
        private void GiveCard2_Tick(object sender, EventArgs e)
        {
            playerCard2 = RandomCard(true, out playerCardValue2);
            PlayerScore.Text = Convert.ToString(playerCardValue1 + playerCardValue2);
            PlayerCards.Text = $"{playerCard1}\n{playerCard2}";
            playerCardValue = playerCardValue1 + playerCardValue2;
            twoSecondTimer.Stop();

        }
        /// <summary>
        ///  <para>Gives the dealer his first card</para>
        /// </summary>
        private void GiveCard3_Tick(object sender, EventArgs e)
        {
            dealerCard1 = RandomCard(false, out dealerCardValue1);
            dealerCardValue = dealerCardValue1;
            DealerScore.Text = Convert.ToString(dealerCardValue);
            DealerCards.Text = dealerCard1;
            threeSecondTimer.Stop();



        }
        /// <summary>
        ///  <para>Gives the dealer his second card</para>
        /// </summary>
        private void GiveCard4_Tick(object sender, EventArgs e)
        {
            isHiddenCard = true;
            dealerCard2 = RandomCard(false, out dealerCardValue2);
            dealerCardValue += dealerCardValue2;

            PlayButton.Visibility = Visibility.Collapsed;
            HitButton.Visibility = Visibility.Visible;
            StandButton.Visibility = Visibility.Visible;
            SplitButton.Visibility = Visibility.Visible;
            DoubleButton.Visibility = Visibility.Visible;
            fourSecondTimer.Stop();
        }
        /// <summary>
        ///  <para>Checks for aces and enables split and double down buttons based on cards</para>
        /// </summary>
        private void Play_Tick(object sender, EventArgs e)
        {
            CheckAcePlayer();
            CheckAceDealer();


            if (/*playerCardValue1 == playerCardValue2 &&*/ moneyWagered * 2 <= availMoney)
            {
                SplitButton.IsEnabled = true;
            }
            if (moneyWagered * 2 <= availMoney)
            {
                DoubleButton.IsEnabled = true;
            }
        }
  

        private void StandButton_Click(object sender, RoutedEventArgs e)
        {
            Stand();
        }
        /// <summary>
        /// <para>Shows the hidden second dealer card, then it gives the dealer cards until het gets 17 or higher then checks if the player won or lost</para>
        /// </summary>
        private void Stand()
        {
            showSecondDealerCard.Start(); // relates to ShowSecondDealerCard_Tick
            standTimer.Start(); // relates to StandGiveCard_Tick

        }
        private void ShowSecondDealerCard_Tick(object sender, EventArgs e)
        {
            DealerScore.Text = Convert.ToString(dealerCardValue);
            DealerCards.Text += $"\n{dealerCard2}";
            showSecondDealerCard.Stop();
        }
        /// <summary>
        /// <para>it gives the dealer cards until het gets 17 or higher then checks if the player won or lost</para>
        /// </summary>
        private void StandGiveCard_Tick(object sender, EventArgs e)
        {

            if (!(dealerCardValue <= 16))
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
        /// <summary>
        /// <para>gives a random card out of the deck</para>
        /// </summary>
        /// <param name="isPlayer">checks if it is the player using this methode or the dealer (usefull for adding aces and updating the image)</param>
        /// <param name="cardValue">returns the value of the card given so the code knows </param>
        /// <returns></returns>
        private string RandomCard(bool isPlayer, out int cardValue)
        {
            int index = rnd.Next(0, numberOfCardsInDeck);
            cardValue = 0;
            string card = "0";
            string imageName = "";
            string cardType = "";

            if (numberOfCardsInDeck < 1)
            {
                deck = newDeck;
                numberOfCardsInDeck = 52;
                MessageBox.Show("New deck will be dealt", "New deck", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            card = deck[index];
            deck.Remove(card);
            numberOfCardsInDeck--;
            deckNumber.Text = Convert.ToString(numberOfCardsInDeck);
            switch (card)
            {
                case "Ace of Hearts":
                    cardValue = 11;
                    cardType = "Harten";
                    imageName = "AH";
                    if (isPlayer == true)
                    {
                        playerAces++;
                    }
                    else
                    {
                        dealerAces++;
                    }
                    break;
                case "Ace of Spades":
                    cardValue = 11;
                    cardType = "Schoppen";
                    imageName = "AS";
                    if (isPlayer == true)
                    {
                        playerAces++;
                    }
                    else
                    {
                        dealerAces++;
                    }
                    break;
                case "Ace of Clubs":
                    cardValue = 11;
                    cardType = "Klaveren";
                    imageName = "AC";
                    if (isPlayer == true)
                    {
                        playerAces++;
                    }
                    else
                    {
                        dealerAces++;
                    }
                    break;
                case "Ace of Diamonds":
                    cardValue = 11;
                    cardType = "Ruiten";
                    imageName = "AD";
                    if (isPlayer == true)
                    {
                        playerAces++;
                    }
                    else
                    {
                        dealerAces++;
                    }
                    break;
                case "Jack of Hearts":
                    cardValue = 10;
                    cardType = "Harten";
                    imageName = "JH";
                    break;
                case "Jack of Spades":
                    cardValue = 10;
                    cardType = "Schoppen";
                    imageName = "JS";
                    break;
                case "Jack of Clubs":
                    cardValue = 10;
                    cardType = "Klaveren";
                    imageName = "JC";
                    break;
                case "Jack of Diamonds":
                    cardValue = 10;
                    cardType = "Ruiten";
                    imageName = "JD";
                    break;
                case "King of Hearts":
                    cardValue = 10;
                    cardType = "Harten";
                    imageName = "KH";
                    break;
                case "King of Spades":
                    cardValue = 10;
                    cardType = "Schoppen";
                    imageName = "KS";
                    break;
                case "King of Clubs":
                    cardValue = 10;
                    cardType = "Klaveren";
                    imageName = "KC";
                    break;
                case "King of Diamonds":
                    cardValue = 10;
                    cardType = "Ruiten";
                    imageName = "KD";
                    break;
                case "Queen of Hearts":
                    cardValue = 10;
                    cardType = "Harten";
                    imageName = "QH";
                    break;
                case "Queen of Spades":
                    cardValue = 10;
                    cardType = "Schoppen";
                    imageName = "QS";
                    break;
                case "Queen of Clubs":
                    cardValue = 10;
                    cardType = "Klaveren";
                    imageName = "QC";
                    break;
                case "Queen of Diamonds":
                    cardValue = 10;
                    cardType = "Ruiten";
                    imageName = "QD";
                    break;
                case "Two of Hearts":
                    cardValue = 2;
                    cardType = "Harten";
                    imageName = "2H";
                    break;
                case "Two of Spades":
                    cardValue = 2;
                    cardType = "Schoppen";
                    imageName = "2S";
                    break;
                case "Two of Clubs":
                    cardValue = 2;
                    cardType = "Klaveren";
                    imageName = "2C";
                    break;
                case "Two of Diamonds":
                    cardValue = 2;
                    cardType = "Ruiten";
                    imageName = "2D";
                    break;
                case "Three of Hearts":
                    cardValue = 3;
                    cardType = "Harten";
                    imageName = "3H";
                    break;
                case "Three of Spades":
                    cardValue = 3;
                    cardType = "Schoppen";
                    imageName = "3S";
                    break;
                case "Three of Clubs":
                    cardValue = 3;
                    cardType = "Klaveren";
                    imageName = "3C";
                    break;
                case "Three of Diamonds":
                    cardValue = 3;
                    cardType = "Ruiten";
                    imageName = "3D";
                    break;
                case "Four of Hearts":
                    cardValue = 4;
                    cardType = "Harten";
                    imageName = "4H";
                    break;
                case "Four of Spades":
                    cardValue = 4;
                    cardType = "Schoppen";
                    imageName = "4S";
                    break;
                case "Four of Clubs":
                    cardValue = 4;
                    cardType = "Klaveren";
                    imageName = "4C";
                    break;
                case "Four of Diamonds":
                    cardValue = 4;
                    cardType = "Ruiten";
                    imageName = "4D";
                    break;
                case "Five of Hearts":
                    cardType = "Harten";
                    imageName = "5H";
                    cardValue = 5;
                    break;
                case "Five of Spades":
                    cardValue = 5;
                    cardType = "Schoppen";
                    imageName = "5S";
                    break;
                case "Five of Clubs":
                    cardValue = 5;
                    cardType = "Klaveren";
                    imageName = "5C";
                    break;
                case "Five of Diamonds":
                    cardValue = 5;
                    cardType = "Ruiten";
                    imageName = "5D";
                    break;
                case "Six of Hearts":
                    cardValue = 6;
                    cardType = "Harten";
                    imageName = "6H";
                    break;
                case "Six of Spades":
                    cardValue = 6;
                    cardType = "Schoppen";
                    imageName = "6S";
                    break;
                case "Six of Clubs":
                    cardValue = 6;
                    cardType = "Klaveren";
                    imageName = "6C";
                    break;
                case "Six of Diamonds":
                    cardValue = 6;
                    cardType = "Ruiten";
                    imageName = "6D";
                    break;
                case "Seven of Hearts":
                    cardValue = 7;
                    cardType = "Harten";
                    imageName = "7H";
                    break;
                case "Seven of Spades":
                    cardValue = 7;
                    cardType = "Schoppen";
                    imageName = "7S";
                    break;
                case "Seven of Clubs":
                    cardValue = 7;
                    cardType = "Klaveren";
                    imageName = "7C";
                    break;
                case "Seven of Diamonds":
                    cardValue = 7;
                    cardType = "Ruiten";
                    imageName = "7D";
                    break;
                case "Eight of Hearts":
                    cardValue = 8;
                    cardType = "Harten";
                    imageName = "8H";
                    break;
                case "Eight of Spades":
                    cardValue = 8;
                    cardType = "Schoppen";
                    imageName = "8S";
                    break;
                case "Eight of Clubs":
                    cardValue = 8;
                    cardType = "Klaveren";
                    imageName = "8C";
                    break;
                case "Eight of Diamonds":
                    cardValue = 8;
                    cardType = "Ruiten";
                    imageName = "8D";
                    break;
                case "Nine of Hearts":
                    cardValue = 9;
                    cardType = "Harten";
                    imageName = "9H";
                    break;
                case "Nine of Spades":
                    cardValue = 9;
                    cardType = "Schoppen";
                    imageName = "9S";
                    break;
                case "Nine of Clubs":
                    cardValue = 9;
                    cardType = "Klaveren";
                    imageName = "9C";
                    break;
                case "Nine of Diamonds":
                    cardValue = 9;
                    cardType = "Ruiten";
                    imageName = "9D";
                    break;
                case "Ten of Hearts":
                    cardValue = 10;
                    cardType = "Harten";
                    imageName = "10H";
                    break;
                case "Ten of Spades":
                    cardValue = 10;
                    cardType = "Schoppen";
                    imageName = "10S";
                    break;
                case "Ten of Clubs":
                    cardValue = 10;
                    cardType = "Klaveren";
                    imageName = "10C";
                    break;
                case "Ten of Diamonds":
                    cardValue = 10;
                    cardType = "Ruiten";
                    imageName = "10D";
                    break;

            }

            if (isPlayer == true)
            {
                if (isDoubleDown == true)
                {
                    BitmapImage dubbelDown = new BitmapImage();
                    dubbelDown.BeginInit();
                    dubbelDown.UriSource = new Uri($"/kaarten/{cardType}/{imageName}.svg.png", UriKind.RelativeOrAbsolute);
                    dubbelDown.Rotation = Rotation.Rotate90;
                    dubbelDown.EndInit();

                    PlayerImage.Source = dubbelDown;
                }
                else
                {
                    PlayerImage.Source = new BitmapImage(new Uri($"/kaarten/{cardType}/{imageName}.svg.png", UriKind.RelativeOrAbsolute));
                }
            }
            else
            {
                if (isHiddenCard == true)
                {
                    DealerImage.Source = new BitmapImage(new Uri($"/kaarten/Naamloos.png", UriKind.RelativeOrAbsolute));
                    isHiddenCard = false;
                }
                else
                {
                    DealerImage.Source = new BitmapImage(new Uri($"/kaarten/{cardType}/{imageName}.svg.png", UriKind.RelativeOrAbsolute));
                }
            }

            return card;

        }
        /// <summary>
        /// <para>firstly uses a method to update isitplusorminus to know what the game result was, after that updates the last played game that displays in the bottom left</para>
        /// </summary>
        private void InsertLastGameResults()
        {
            CheckGameResultForHistory();
            lastPlayedGame.Foreground = Brushes.Green;
            lastPlayedGame.Text = $"{plusOrMinus}{moneyWagered} / {playerCardValue} / {dealerCardValue}";
        }
        /// <summary>
        /// <para>inserts last played game into the history bar found in the bottom middle of the screen</para>
        /// </summary>
        private void InsertAllGameResults()
        {
            CheckGameResultForHistory();
            ComboBoxItem lastGame = new ComboBoxItem();
            history.Insert(0, $"{historyUpdator}. {plusOrMinus}{moneyWagered} / {playerCardValue} / {dealerCardValue}");
            lastGame.Content = history[0];
            TenLastPlayedGames.Items.Insert(0, lastGame);
            if (history.Count > 10)
            {
                history.RemoveAt(10);
                TenLastPlayedGames.Items.RemoveAt(10);s
            }
            historyUpdator++;
        }
        /// <summary>
        /// <para>updates the plusorminus to determine what symbol gets used in the history</para>
        /// </summary>
        private void CheckGameResultForHistory()
        {
            if (isItPush == true)
            {
                plusOrMinus = "";
                moneyWagered = 0;
            }
            else if (didPlayerWin == true)
            {
                plusOrMinus = "+";
            }
            else if (didPlayerWin == false)
            {
                plusOrMinus = "-";
            }
        }
        /// <summary>
        /// <para>updates UI and history, also doubles money wagered and adds it to his bank.</para>
        /// </summary>
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
            isDoubleDown = false;
            MoneyBox.Text = "0";
            dealerCardValue1 = 0;
            dealerCardValue2 = 0;
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
            InsertLastGameResults();
            InsertAllGameResults();
            playerCardValue = 0;
            dealerCardValue = 0;
            availMoney += moneyWagered;
            availablemoney.Text = availMoney.ToString();
            moneyWagered = 0;



        }
        /// <summary>
        /// <para>updates UI and history, also makes the player lose all the money he wagered</para>
        /// </summary>
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
                isDoubleDown = false;
                MoneyBox.Text = "0";
                dealerCardValue1 = 0;
                dealerCardValue2 = 0;
                playerCardValue1 = 0;
                playerCardValue2 = 0;
                playerAces = 1;
                dealerAces = 1;
                playerCard1 = "";
                playerCard2 = "";
                playerCard3 = "";
                dealerCard1 = "";
                dealerCard2 = "";

                InsertLastGameResults();
                InsertAllGameResults();
                playerCardValue = 0;
                dealerCardValue = 0;
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
        /// <summary>
        /// <para>updates UI and history, also gives the money wagered back to the player</para>
        /// </summary>
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
            isDoubleDown = false;
            MoneyBox.Text = "0";
            dealerCardValue1 = 0;
            dealerCardValue2 = 0;
            playerCardValue1 = 0;
            playerCardValue2 = 0;
            playerAces = 1;
            dealerAces = 1;
            playerCard1 = "";
            playerCard2 = "";
            playerCard3 = "";
            dealerCard1 = "";
            dealerCard2 = "";
            InsertLastGameResults();
            InsertAllGameResults();
            playerCardValue = 0;
            dealerCardValue = 0;
            moneyWagered = 0;
        }
        /// <summary>
        /// <para>checks if the player has an ace on the board and if he has gone over 21 then changes the value of that ace if needed</para>
        /// </summary>
        private void CheckAcePlayer()
        {
            
            if (playerAces > 1 && playerCardValue > 21)
            {
                playerCardValue -= 10;
                PlayerScore.Text = Convert.ToString(playerCardValue);
                playerAces--;
            }
        }
        /// <summary>
        /// <para>checks if the dealer has an ace on the board and if he has gone over 21 then changes the value of that ace if needed</para>
        /// </summary>
        private void CheckAceDealer()
        {
            if (dealerAces > 1 && dealerCardValue > 21)
            {
                dealerCardValue -= 10;
                DealerScore.Text = Convert.ToString(dealerCardValue);
            } 
        }
        /// <summary>
        /// <para>completely resets the app</para>
        /// </summary>
        private void ResetClient()
        {
            dealerCardValue = 0;
            playerCardValue = 0;
            historyUpdator = 1;
            moneyWagered = 0;
            availMoney = 100;
            playerAces = 1;
            dealerAces = 1;
            playerCard1 = "";
            playerCard2 = "";
            playerCard3 = "";
            dealerCard1 = "";
            dealerCard2 = "";
            lastPlayedGame.Text = $"";
            PlayerCards.Text = "";
            DealerCards.Text = "";
            DealerScore.Text = "0";
            PlayerScore.Text = "0";
            Result.Text = "Started";
            availablemoney.Text = "100";
            split1IsPlaying = true;
            split2IsPlaying = true;
            lastPlayedGame.Foreground = Brushes.Black;
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
            history.Clear();
            List<string> deck = new List<string>(){
                "Ace of Hearts",
                "Ace of Spades",
                "Ace of Clubs",
                "Ace of Diamonds",
                "Jack of Hearts",
                "Jack of Spades",
                "Jack of Clubs",
                "Jack of Diamonds",
                "King of Hearts",
                "King of Spades",
                "King of Clubs",
                "King of Diamonds",
                "Queen of Hearts",
                "Queen of Spades",
                "Queen of Clubs",
                "Queen of Diamonds",
                "Two of Hearts",
                "Two of Spades",
                "Two of Clubs",
                "Two of Diamonds",
                "Three of Hearts",
                "Three of Spades",
                "Three of Clubs",
                "Three of Diamonds",
                "Four of Hearts",
                "Four of Spades",
                "Four of Clubs",
                "Four of Diamonds",
                "Five of Hearts",
                "Five of Spades",
                "Five of Clubs",
                "Five of Diamonds",
                "Six of Hearts",
                "Six of Spades",
                "Six of Clubs",
                "Six of Diamonds",
                "Seven of Hearts",
                "Seven of Spades",
                "Seven of Clubs",
                "Seven of Diamonds",
                "Eight of Hearts",
                "Eight of Spades",
                "Eight of Clubs",
                "Eight of Diamonds",
                "Nine of Hearts",
                "Nine of Spades",
                "Nine of Clubs",
                "Nine of Diamonds",
                "Ten of Hearts",
                "Ten of Spades",
                "Ten of Clubs",
                "Ten of Diamonds",
            };
            TenLastPlayedGames.Items.Clear();

        }
        /// <summary>
        /// <para>checks if the player entered a valid amount of money to play the game</para>
        /// </summary>
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
            DoubleDown();
        }
        /// <summary>
        /// <para>doubles wagared money then uses hit and stand function</para>
        /// </summary>
        private void DoubleDown()
        {
            moneyWagered *= 2;
            isDoubleDown = true;
            Hit();
            Stand();
        }
        /// <summary>
        /// <para>NOT Finished</para>
        /// </summary>
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
        /// <summary>
        /// <para>NOT Finished</para>
        /// </summary>
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
        /// <summary>
        /// <para>NOT Finished</para>
        /// </summary>
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
        /// <summary>
        /// <para>NOT Finished</para>
        /// </summary>
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
        /// <summary>
        /// <para>NOT Finished</para>
        /// </summary>
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

        /// <summary>
        /// <para>resets the client with a button</para>
        /// </summary>
        private void NewGameBTTN_Click(object sender, RoutedEventArgs e)
        {
            ResetClient();
        }


    }
}

