using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TrettioEtt
{
    public enum Suit { Hjärter, Ruter, Spader, Klöver };

    class Program
    {

        static void Main(string[] args)
        {
            Console.WindowWidth = 120;
            Game game = new Game();

            List<Player> players = new List<Player>();


            players.Add(new BasicPlayer());
            players.Add(new FabianPlayer());
            players.Add(new FabianFaresPlayer());
            players.Add(new FabianPlayerTest());
            players.Add(new HackerMan());
            Console.WriteLine("Vilka två spelare skall mötas?");
            for (int i = 1; i <= players.Count; i++)
            {
                Console.WriteLine(i + ": {0}", players[i - 1].Name);
            }
            int p1 = int.Parse(Console.ReadLine());
            int p2 = int.Parse(Console.ReadLine());
            Player player1 = players[p1 - 1];
            Player player2 = players[p2 - 1];
            player1.Game = game;
            player1.PrintPosition = 0;
            player2.Game = game;
            player2.PrintPosition = 9;
            game.Player1 = player1;
            game.Player2 = player2;
            Console.WriteLine("Hur många spel skall spelas?");
            int numberOfGames = int.Parse(Console.ReadLine());
            Console.WriteLine("Skriva ut första spelet? (y/n)");
            string print = Console.ReadLine();
            Console.Clear();
            if (print == "y")
                game.Printlevel = 2;
            else
                game.Printlevel = 0;
            game.initialize(true);
            game.PlayAGame(true);
            Console.Clear();
            bool player1starts = true;

            for (int i = 1; i < numberOfGames; i++)
            {
                game.Printlevel = 0;
                player1starts = !player1starts;
                game.initialize(false);
                game.PlayAGame(player1starts);

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, 3);
                Console.Write(player1.Name + ":");
                Console.ForegroundColor = ConsoleColor.Green;

                Console.SetCursorPosition((player1.Wongames * 100 / numberOfGames) + 15, 3);
                Console.Write("█");
                Console.SetCursorPosition((player1.Wongames * 100 / numberOfGames) + 16, 3);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(player1.Wongames);

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(0, 5);
                Console.Write(player2.Name + ":");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition((player2.Wongames * 100 / numberOfGames) + 15, 5);
                Console.Write("█");
                Console.SetCursorPosition((player2.Wongames * 100 / numberOfGames) + 16, 5);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(player2.Wongames);

            }
            Console.SetCursorPosition(25, 7);
            Console.Write(player1.Name);
            Console.SetCursorPosition(45, 7);
            Console.WriteLine(player2.Name);
            Console.WriteLine("          Vunna spel:");
            Console.WriteLine("            Skillnad:");
            Console.WriteLine("Antal ronder i snitt:");
            Console.WriteLine("    Egna knackningar:");
            Console.WriteLine("         Andel vunna:");
            Console.WriteLine("  Knackpoäng i snitt:");
            Console.WriteLine("            Antal 31:");
            Console.SetCursorPosition(25 + player1.Name.Length / 2, 8);
            Console.Write(player1.Wongames);
            Console.SetCursorPosition(25 + player1.Name.Length / 2, 9);
            int diff = player1.Wongames - player2.Wongames;
            if (diff > 0)
            {
                Console.Write("+" + diff);
            }
            Console.SetCursorPosition(25 + player1.Name.Length / 2, 10);
            double avgRounds1 = Math.Round((double)player1.StoppedRounds / (double)player1.StoppedGames, 2);
            Console.Write(avgRounds1);
            Console.SetCursorPosition(25 + player1.Name.Length / 2, 11);
            Console.Write(player1.KnackWins + player2.DefWins);
            Console.SetCursorPosition(25 + player1.Name.Length / 2, 12);
            double winPercent1 = Math.Round((double)player1.KnackWins * 100 / (player1.KnackWins + player2.DefWins), 1);
            Console.Write(winPercent1 + " %");
            Console.SetCursorPosition(25 + player1.Name.Length / 2, 13);
            double knackAvg = Math.Round((double)player1.KnackTotal / (player1.KnackWins + player2.DefWins), 1);
            Console.Write(knackAvg);
            Console.SetCursorPosition(25 + player1.Name.Length / 2, 14);
            Console.Write(player1.TrettiettWins);

            Console.SetCursorPosition(45 + player2.Name.Length / 2, 8);
            Console.Write(player2.Wongames);
            Console.SetCursorPosition(45 + player2.Name.Length / 2, 9);
            int diff2 = player2.Wongames - player1.Wongames;
            if (diff2 > 0)
            {
                Console.Write("+" + diff2);
            }
            Console.SetCursorPosition(45 + player2.Name.Length / 2, 10);
            double avgRounds2 = Math.Round((double)player2.StoppedRounds / (double)player2.StoppedGames, 2);
            Console.Write(avgRounds2);

            Console.SetCursorPosition(45 + player2.Name.Length / 2, 11);
            Console.Write(player2.KnackWins + player1.DefWins);
            Console.SetCursorPosition(45 + player2.Name.Length / 2, 12);
            double winPercent2 = Math.Round((double)player2.KnackWins * 100 / (player2.KnackWins + player1.DefWins), 1);
            Console.Write(winPercent2 + " %");
            Console.SetCursorPosition(45 + player2.Name.Length / 2, 13);
            double knackAvg2 = Math.Round((double)player2.KnackTotal / (player2.KnackWins + player1.DefWins), 1);
            Console.Write(knackAvg2);
            Console.SetCursorPosition(45 + player2.Name.Length / 2, 14);
            Console.Write(player2.TrettiettWins);
            Console.ReadLine();
            Console.ReadLine();

        }

    }
    class Card
    {
        public int Value { get; private set; } //Kortets värde enligt reglerna i Trettioett, t.ex. dam = 10
        public Suit Suit { get; private set; }
        private int Id; //Typ av kort, t.ex dam = 12

        public Card(int id, Suit suit)
        {
            Id = id;
            Suit = suit;
            if (id == 1)
            {
                Value = 11;
            }
            else if (id > 9)
            {
                Value = 10;
            }
            else
            {
                Value = id;
            }
        }

        public void PrintCard()
        {
            string cardname = "";
            if (Id == 1)
            {
                cardname = "ess ";
            }
            else if (Id == 10)
            {
                cardname = "tio ";
            }
            else if (Id == 11)
            {
                cardname = "knekt ";
            }
            else if (Id == 12)
            {
                cardname = "dam ";
            }
            else if (Id == 13)
            {
                cardname = "kung ";
            }
            if (Suit == Suit.Hjärter)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (Suit == Suit.Ruter)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (Suit == Suit.Spader)
                Console.ForegroundColor = ConsoleColor.Gray;
            else if (Suit == Suit.Klöver)
                Console.ForegroundColor = ConsoleColor.Green;

            Console.Write(" " + Suit + " " + cardname);
            if (cardname == "")
            {
                Console.Write(Id + " ");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

    }


    class Game
    {

        List<Card> CardDeck = new List<Card>();
        List<Card> DiscardPile = new List<Card>();
        public bool Lastround;
        int Cardnumber;
        public int Printlevel;
        public int Discardnumber;
        public Player Player1 { private get; set; }
        public Player Player2 { private get; set; }
        Random RNG = new Random();
        public int NbrOfRounds;

        public Game()
        {

        }

        public void initialize(bool firstGame)
        {
            Lastround = false;
            Player1.lastTurn = false;
            Player2.lastTurn = false;
            Cardnumber = -1;
            Discardnumber = 52;
            CardDeck = new List<Card>();
            DiscardPile = new List<Card>();
            Player1.Hand = new List<Card>();
            Player2.Hand = new List<Card>();

            int id;
            int suit;
            for (int i = 0; i < 52; i++)
            {
                id = i % 13 + 1;
                suit = i % 4;
                CardDeck.Add(new Card(id, (Suit)suit));
            }
            Shuffle();
            for (int i = 0; i < 3; i++)
            {
                Player1.Hand.Add(DrawCard());
                Player2.Hand.Add(DrawCard());
            }
            if (firstGame)
            {
                if (Score(Player1) > 10 || Score(Player2) > 10)
                {
                    //Console.WriteLine("Omgiv. Scores: " + Score(Player1) + " , " + Score(Player2));
                    //Console.ReadKey();
                    initialize(true);

                }
            }


            Discard(DrawCard());
        }

        public void printHand(Player player)
        {
            Console.SetCursorPosition(0, player.PrintPosition);
            Console.WriteLine(player.Name + " har ");
            for (int i = 0; i < player.Hand.Count; i++)
            {
                player.Hand[i].PrintCard();
                Console.WriteLine();
            }
        }

        private int playARound(Player player, Player otherPlayer)
        {
            player.UpCard = DiscardPile.Last();
            if (Printlevel > 1)
            {
                printHand(player);
                Console.SetCursorPosition(4, 6);
                Console.Write("På skräphögen ligger ");
                DiscardPile.Last().PrintCard();

            }
            otherPlayer.OpponentsLatestCard = null;
            if (NbrOfRounds > 1 && !Lastround && player.Knacka(NbrOfRounds))
            {
                if (Printlevel > 1)
                {
                    Console.SetCursorPosition(20, player.PrintPosition + 2);
                    Console.Write(player.Name + " knackar!");
                }
                return Score(player);
            }
            else if (player.TaUppKort(DiscardPile.Last()))
            {

                player.Hand.Add(PickDiscarded());
                otherPlayer.OpponentsLatestCard = player.Hand.Last();
                if (Printlevel > 1)
                {
                    Console.SetCursorPosition(20, player.PrintPosition + 2);
                    Console.Write(player.Name + " plockar ");
                    player.Hand.Last().PrintCard();
                    Console.Write(" från skräphögen.");
                }
            }
            else
            {
                player.Hand.Add(DrawCard());
                if (Printlevel > 1)
                {
                    Console.SetCursorPosition(20, player.PrintPosition + 2);
                    Console.Write(player.Name + " drar ");
                    player.Hand.Last().PrintCard();
                }
            }
            Card discardcard = player.KastaKort();

            UpdateHand(player, discardcard);
            if (Printlevel > 1)
            {
                Console.SetCursorPosition(20, player.PrintPosition + 3);
                Console.Write(player.Name + " kastar bort ");
                discardcard.PrintCard();
                Console.Write("       Tryck ENTER");
                Console.ReadLine();
                Console.Clear();
                printHand(player);

            }

            Discard(discardcard);
            if (Score(player) == 31)
            {
                return 31;
            }
            else
            {
                return 0;
            }
        }

        private void UpdateHand(Player player, Card discardcard)
        {
            player.Hand.Remove(discardcard);

        }

        public int Score(Player player) //Reurnerar spelarens poäng av bästa färg. Uppdaterar player.bestsuit.
        {
            int[] suitScore = new int[4];
            if (player.Hand[0].Value == 11 && player.Hand[1].Value == 11 && player.Hand[2].Value == 11)
            {
                return 31;
            }

            for (int i = 0; i < player.Hand.Count; i++)
            {
                if (player.Hand[i] != null)
                    suitScore[(int)player.Hand[i].Suit] += player.Hand[i].Value;
            }
            int max = 0;

            for (int i = 0; i < 4; i++)
            {
                if (suitScore[i] > max)
                {
                    max = suitScore[i];
                    player.BestSuit = (Suit)i;
                }
            }
            return max;

        }

        public int SuitScore(List<Card> hand, Suit suit) //Reurnerar handens poäng av en viss färg
        {
            int sum = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] != null && hand[i].Suit == suit)
                {
                    sum += hand[i].Value;
                }

            }
            return sum;


        }

        public int HandScore(List<Card> hand, Card excluded) //Reurnerar handens poäng av bästa färg. Undantar ett kort från beräkningen (null för att ta med alla kort)
        {
            int[] suitScore = new int[4];
            int aces = 0;
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] != null && hand[i] != excluded)
                {
                    suitScore[(int)hand[i].Suit] += hand[i].Value;
                    if (hand[i].Value == 11)
                    {
                        aces++;
                    }
                }

            }
            if (aces == 3)
                return 31;
            int max = 0;

            for (int i = 0; i < 4; i++)
            {
                if (suitScore[i] > max)
                {
                    max = suitScore[i];
                }
            }
            return max;
        }

        public void PlayAGame(bool player1starts)

        {

            NbrOfRounds = 0;

            Player playerInTurn, playerNotInTurn, temp;

            if (player1starts)

            {

                playerInTurn = Player1;

                playerNotInTurn = Player2;

            }

            else

            {

                playerInTurn = Player2;

                playerNotInTurn = Player1;

            }

            while (Cardnumber < 51 && NbrOfRounds < 100)

            {

                NbrOfRounds++;

                int result = playARound(playerInTurn, playerNotInTurn);

                if (result == 31)

                {

                    if (Printlevel > 1)

                        printHand(playerNotInTurn);


                    playerNotInTurn.OpponentLatestScore = Score(playerInTurn);
                    playerInTurn.OpponentLatestScore = Score(playerNotInTurn);
                    playerInTurn.SpelSlut(true);

                    playerInTurn.TrettiettWins++;
                    playerInTurn.StoppedGames++;
                    playerInTurn.StoppedRounds += NbrOfRounds;
                    playerNotInTurn.SpelSlut(false);

                    if (Printlevel > 0)

                    {

                        Console.SetCursorPosition(15, playerInTurn.PrintPosition + 5);

                        Console.Write(playerInTurn.Name + " fick 31 och vann spelet!");

                        Console.ReadLine();

                    }

                    break;

                }

                else if (result > 0)

                {
                    playerInTurn.KnackTotal += result;
                    Lastround = true;
                    playerNotInTurn.lastTurn = true;

                    playARound(playerNotInTurn, playerInTurn);

                    if (Printlevel > 1)

                    {

                        printHand(playerInTurn);

                        printHand(playerNotInTurn);

                    }





                    if (Printlevel > 0)

                    {

                        Console.SetCursorPosition(15, playerInTurn.PrintPosition + 5);

                        Console.Write(playerInTurn.Name + " knackade och har " + Score(playerInTurn) + " poäng");

                        Console.SetCursorPosition(15, playerNotInTurn.PrintPosition + 5);

                        Console.Write(playerNotInTurn.Name + " har " + Score(playerNotInTurn) + " poäng");

                    }

                    if (Score(playerInTurn) > Score(playerNotInTurn))

                    {
                        playerNotInTurn.OpponentLatestScore = Score(playerInTurn);
                        playerInTurn.OpponentLatestScore = Score(playerNotInTurn);
                        playerInTurn.SpelSlut(true);
                        playerInTurn.KnackWins++;
                        playerInTurn.StoppedGames++;
                        playerInTurn.StoppedRounds += NbrOfRounds;
                        playerNotInTurn.SpelSlut(false);

                        if (Printlevel > 0)

                        {

                            Console.SetCursorPosition(15, playerInTurn.PrintPosition + 6);

                            Console.WriteLine(playerInTurn.Name + " vann!");

                            Console.ReadLine();

                        }

                        break;

                    }

                    else

                    {
                        playerNotInTurn.OpponentLatestScore = Score(playerInTurn);
                        playerInTurn.OpponentLatestScore = Score(playerNotInTurn);
                        playerInTurn.SpelSlut(false);

                        playerNotInTurn.SpelSlut(true);
                        playerNotInTurn.DefWins++;
                        playerInTurn.StoppedGames++;
                        playerInTurn.StoppedRounds += NbrOfRounds;
                        //playerNotInTurn.WinRounds += NbrOfRounds;

                        if (Printlevel > 0)

                        {

                            Console.SetCursorPosition(15, playerNotInTurn.PrintPosition + 6);

                            Console.WriteLine(playerNotInTurn.Name + " vann!");

                            Console.ReadLine();

                        }

                        break;

                    }

                }

                else

                {

                    temp = playerNotInTurn;

                    playerNotInTurn = playerInTurn;

                    playerInTurn = temp;

                }



            }

            if (Cardnumber >= 51 || NbrOfRounds >= 100)

            {

                if (Printlevel > 0)

                {

                    Console.SetCursorPosition(0, 20);

                    Console.WriteLine("Korten tog slut utan att någon spelare vann.");

                    Console.ReadLine();

                }

                playerInTurn.SpelSlut(false);

                playerNotInTurn.SpelSlut(false);



            }



        }

        private void Discard(Card card)
        {
            Discardnumber--;
            DiscardPile.Add(card);
        }

        private Card DrawCard()
        {
            Cardnumber++;
            Card card = CardDeck.First();
            CardDeck.RemoveAt(0);
            return card;
        }

        private Card PickDiscarded()
        {
            Card card = DiscardPile.Last();
            Discardnumber++;
            return card;
        }



        private void Shuffle()
        {
            for (int i = 0; i < 200; i++)
            {
                switchCards();
            }
        }

        private void switchCards()
        {
            int card1 = RNG.Next(CardDeck.Count);
            int card2 = RNG.Next(CardDeck.Count);
            Card temp = CardDeck[card1];
            CardDeck[card1] = CardDeck[card2];
            CardDeck[card2] = temp;
        }
    }

    abstract class Player
    {
        public string Name;
        // Dessa variabler får ej ändras

        public List<Card> Hand = new List<Card>();  // Lista med alla kort i handen. Bara de tre första platserna har kort i sig när rundan börjar, ett fjärde läggs till när man tar ett kort
        public Game Game;
        public Suit BestSuit; // Den färg med mest poäng. Uppdateras varje gång game.Score anropas
        public int Wongames;
        public int TrettiettWins;
        public int KnackWins;
        public int DefWins;
        public int KnackTotal;
        public int StoppedGames;
        public int StoppedRounds;
        public int PrintPosition;
        public Card UpCard; //Det kort som ligger över på skräphögen
        public int OpponentLatestScore;  //Motståndarens senaste slutpoäng
        public bool lastTurn; // True om motståndaren har knackat, annars false.
        public Card OpponentsLatestCard; // Det senaste kortet motståndaren tog. Null om kortet drogs från högen.

        public abstract bool Knacka(int round);
        public abstract bool TaUppKort(Card card);
        public abstract Card KastaKort();
        public abstract void SpelSlut(bool wonTheGame);
    }



    class BasicPlayer : Player //Denna spelare fungerar exakt som MyPlayer. Ändra gärna i denna för att göra tester.
    {
        public BasicPlayer()
        {
            Name = "BasicPlayer";
        }

        public override bool Knacka(int round)
        {
            if (Game.Score(this) >= 30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool TaUppKort(Card card)
        {
            if (card.Value == 11 || (card.Value == 10 && card.Suit == BestSuit))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public override Card KastaKort()
        {
            Game.Score(this);
            Card worstCard = Hand.First();
            for (int i = 1; i < Hand.Count; i++)
            {
                if (Hand[i].Value < worstCard.Value)
                {
                    worstCard = Hand[i];
                }
            }
            return worstCard;

        }

        public override void SpelSlut(bool wonTheGame)
        {
            if (wonTheGame)
            {
                Wongames++;
            }

        }
    }


    class FabianPlayer : Player
    {
        private List<int> OpponentKnackValues = new List<int>();
        private bool OpponentHasKnackat = false;
        private int OpponentKnackatGånger = 0;
        public FabianPlayer()
        {
            Name = "FabianPlayer";
        }

        public override bool Knacka(int round)
        {
            int whenToKnacka = 24;
            if (/*CalculateOpponentKnackValue() >= whenToKnacka &&*/ OpponentKnackatGånger >= 100)
            {
                if(CalculateOpponentKnackValue() >= 29)
                {
                    whenToKnacka = 28;
                }
                else if(CalculateOpponentKnackValue() <= 15)
                {
                    whenToKnacka = 16;
                }
                else
                {
                    whenToKnacka = (CalculateOpponentKnackValue() - 3);
                }
            }
            if (Game.Score(this) >= whenToKnacka)
            {
                return true;
            }
            else if ((Game.Score(this) >= 20 && round == 3) || (Game.Score(this) >= 20 && round == 2))
            {
                return true;
            }
            else if ((Game.Score(this) >= 22 && round <= 8))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool TaUppKort(Card card)
        {
            if (lastTurn)
            {
                OpponentHasKnackat = true;
            }
            else
            {
                OpponentHasKnackat = false;
            }
            Hand.Add(card);
            int lowestCardInHandValue = 12;
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Value < card.Value)
                {
                    lowestCardInHandValue = card.Value;
                }
            }
            if (card.Value == 11 || (card.Value == 10 && card.Suit == BestSuit) || (card.Value >= lowestCardInHandValue && card.Suit == BestSuit))
            {
                Hand.RemoveAt(Hand.Count - 1);
                return true;
            }
            else
            {
                Hand.RemoveAt(Hand.Count - 1);
                return false;
            }

        }

        public override Card KastaKort()
        {
            Game.Score(this);
            Card worstCard = Hand.First();
            int numberOfAce = 0;
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Value == 11)
                {
                    numberOfAce++;
                }
            }
            bool[] aceAndDressed = new bool[3];
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Suit == BestSuit && Hand[i].Value == 11)
                {
                    aceAndDressed[0] = true;
                }
                else if (Hand[i].Suit == BestSuit && Hand[i].Value == 10)
                {
                    aceAndDressed[1] = true;
                }
                if (aceAndDressed[0] && aceAndDressed[1])
                {
                    aceAndDressed[2] = true;
                }
            }
            int howManyOfBestSuit = 0;
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Suit == BestSuit)
                {
                    howManyOfBestSuit++;
                }
            }
            for (int i = 1; i < Hand.Count; i++)
            {
                if (howManyOfBestSuit == 4 && worstCard.Value > Hand[i].Value)
                {
                    worstCard = Hand[i];
                }
                else if (howManyOfBestSuit == 3 && Hand[i].Suit != BestSuit)
                {
                    worstCard = Hand[i];
                }
                else if (aceAndDressed[2] && Hand[i].Suit != BestSuit)
                {
                    worstCard = Hand[i];
                }
                // Om motstånndaren plockar upp ett kort från skräphögen är det kortet den andra spelarens bestsuit
                // kolla om ess har blivit kastad/ , testa igen
                //else if (numberOfAce == 2)
                //{
                //   if (Hand[i].Value != 11 && Hand[i].Value < worstCard.Value)
                //   {
                //        worstCard = Hand[i];
                //    }
                //}
                else if (Hand[i].Value < worstCard.Value && Hand[i].Suit != BestSuit && Hand[i].Value != 11)
                {
                    worstCard = Hand[i];
                }
            }
            return worstCard;

        }

        public override void SpelSlut(bool wonTheGame)
        {
            //Debug.WriteLine("OpponentKnackatGånger: " + OpponentKnackatGånger);
            //Debug.WriteLine("Average KnackPoäng: " + CalculateOpponentKnackValue());
            if (OpponentHasKnackat)
            {
                OpponentKnackValues.Add(OpponentLatestScore);
                OpponentKnackatGånger++;
            }
            if (wonTheGame)
            {
                Wongames++;
            }

        }
        private int CalculateOpponentKnackValue()
        {
            double tempSum = OpponentKnackValues.Sum();
            int opponentKnackValueReal = 0;
            if (OpponentKnackValues.Count == 0)
            {

            }
            else
            {
                opponentKnackValueReal = Convert.ToInt32(Math.Round(tempSum /= OpponentKnackValues.Count));
            }
            return opponentKnackValueReal;
        }
    }
    class FabianPlayerTest : Player //Denna spelare fungerar exakt som MyPlayer. Ändra gärna i denna för att göra tester.
    {
        private List<int> OpponentKnackValues = new List<int>();
        private bool OpponentHasKnackat = false;
        private int OpponentKnackatGånger = 0;
        public FabianPlayerTest()
        {
            Name = "FabianPlayerTest";
        }

        public override bool Knacka(int round)
        {
            int whenToKnacka = 24;
            if (CalculateOpponentKnackValue() >= whenToKnacka && OpponentKnackatGånger == 100)
            {
                if (CalculateOpponentKnackValue() >= 29)
                {
                    whenToKnacka = 28;
                }
                else if (CalculateOpponentKnackValue() <= 15)
                {
                    whenToKnacka = 16;
                }
                else
                {
                    whenToKnacka = (CalculateOpponentKnackValue() - 1);
                }
            }
            if (Game.Score(this) >= whenToKnacka)
            {
                return true;
            }
            else if ((Game.Score(this) >= 20 && round == 3) || (Game.Score(this) >= 20 && round == 2))
            {
                return true;
            }
            else if ((Game.Score(this) >= 22 && round <= 8))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool TaUppKort(Card card)
        {
            if (lastTurn)
            {
                OpponentHasKnackat = true;
            }
            else
            {
                OpponentHasKnackat = false;
            }
            Hand.Add(card);
            int lowestCardInHandValue = 12;
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Value < card.Value)
                {
                    lowestCardInHandValue = card.Value;
                }
            }
            if (card.Value == 11 || (card.Value == 10 && card.Suit == BestSuit) || (card.Value >= lowestCardInHandValue && card.Suit == BestSuit))
            {
                Hand.RemoveAt(Hand.Count - 1);
                return true;
            }
            else
            {
                Hand.RemoveAt(Hand.Count - 1);
                return false;
            }

        }

        public override Card KastaKort()
        {
            Game.Score(this);
            Card worstCard = Hand.First();
            int numberOfAce = 0;
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Value == 11)
                {
                    numberOfAce++;
                }
            }
            bool[] aceAndDressed = new bool[3];
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Suit == BestSuit && Hand[i].Value == 11)
                {
                    aceAndDressed[0] = true;
                }
                else if (Hand[i].Suit == BestSuit && Hand[i].Value == 10)
                {
                    aceAndDressed[1] = true;
                }
                if (aceAndDressed[0] && aceAndDressed[1])
                {
                    aceAndDressed[2] = true;
                }
            }
            int howManyOfBestSuit = 0;
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Suit == BestSuit)
                {
                    howManyOfBestSuit++;
                }
            }
            for (int i = 1; i < Hand.Count; i++)
            {
                if (howManyOfBestSuit == 4 && worstCard.Value > Hand[i].Value)
                {
                    worstCard = Hand[i];
                }
                else if (howManyOfBestSuit == 3 && Hand[i].Suit != BestSuit)
                {
                    worstCard = Hand[i];
                }
                else if (aceAndDressed[2] && Hand[i].Suit != BestSuit)
                {
                    worstCard = Hand[i];
                }
                // Om motstånndaren plockar upp ett kort från skräphögen är det kortet den andra spelarens bestsuit
                // kolla om ess har blivit kastad/ , testa igen
                //else if (numberOfAce == 2)
                //{
                //   if (Hand[i].Value != 11 && Hand[i].Value < worstCard.Value)
                //   {
                //        worstCard = Hand[i];
                //    }
                //}
                else if (Hand[i].Value < worstCard.Value && Hand[i].Suit != BestSuit && Hand[i].Value != 11)
                {
                    worstCard = Hand[i];
                }
            }
            return worstCard;

        }

        public override void SpelSlut(bool wonTheGame)
        {
            Debug.WriteLine("OpponentKnackatGånger: " + OpponentKnackatGånger);
            Debug.WriteLine("Average KnackPoäng: " + CalculateOpponentKnackValue());
            if (OpponentHasKnackat)
            {
                OpponentKnackValues.Add(OpponentLatestScore);
                OpponentKnackatGånger++;
            }
            if (wonTheGame)
            {
                Wongames++;
            }

        }
        private int CalculateOpponentKnackValue()
        {
            double tempSum = OpponentKnackValues.Sum();
            int opponentKnackValueReal = 0;
            if (OpponentKnackValues.Count == 0)
            {

            }
            else
            {
                opponentKnackValueReal = Convert.ToInt32(Math.Round(tempSum /= OpponentKnackValues.Count));
            }
            return opponentKnackValueReal;
        }
    }

    class FabianFaresPlayer : Player //Denna spelare fungerar exakt som MyPlayer. Ändra gärna i denna för att göra tester.
    {
        private List<Card> OpponentCards = new List<Card>();
        public FabianFaresPlayer()
        {
            Name = "FabianFaresPlayer";
        }

        public override bool Knacka(int round)
        {

            if (Game.Score(this) >= 25)
            {
                return true;
            }
            else if ((Game.Score(this) >= 20 && round == 3) || (Game.Score(this) >= 20 && round == 2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool TaUppKort(Card card)
        {
            int lowestCardInHandValue = 12;
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Value < card.Value)
                {
                    lowestCardInHandValue = card.Value;
                }
            }
            if (card.Value == 11 || (card.Value == 10 && card.Suit == BestSuit) || (card.Value >= lowestCardInHandValue && card.Suit == BestSuit))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public override Card KastaKort()
        {
            Game.Score(this);
            Card worstCard = Hand.First();
            int numberOfAce = 0;
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Value == 11)
                {
                    numberOfAce++;
                }
            }
            bool[] aceAndDressed = new bool[3];
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Suit == BestSuit && Hand[i].Value == 11)
                {
                    aceAndDressed[0] = true;
                }
                else if (Hand[i].Suit == BestSuit && Hand[i].Value == 10)
                {
                    aceAndDressed[1] = true;
                }
                if (aceAndDressed[0] && aceAndDressed[1])
                {
                    aceAndDressed[2] = true;
                }
            }
            int howManyOfBestSuit = 0;
            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Suit == BestSuit)
                {
                    howManyOfBestSuit++;
                }
            }
            for (int i = 1; i < Hand.Count; i++)
            {
                if (howManyOfBestSuit == 4 && worstCard.Value > Hand[i].Value)
                {
                    worstCard = Hand[i];
                }
                else if (howManyOfBestSuit == 3 && Hand[i].Suit != BestSuit)
                {
                    worstCard = Hand[i];
                }
                else if (aceAndDressed[2] && Hand[i].Suit != BestSuit)
                {
                    worstCard = Hand[i];
                }
                else if (numberOfAce == 2)
                {
                    if (Hand[i].Value != 11 && Hand[i].Value < worstCard.Value)
                    {
                        worstCard = Hand[i];
                    }
                }
                else if (Hand[i].Value < worstCard.Value && Hand[i].Suit != BestSuit && Hand[i].Value != 11)
                {
                    worstCard = Hand[i];
                }
            }
            return worstCard;

        }

        public override void SpelSlut(bool wonTheGame)
        {
            if (wonTheGame)
            {
                Wongames++;
            }

        }
    }
    class HackerMan : Player
    {
        List<Card> OpponentCards = new List<Card>();
        List<int> OpponentKnockingScore = new List<int>();
        List<int> KnockingScore = new List<int>();

        int GameNumber = 0;
        bool ShouldUseFirst = false;

        public HackerMan()
        {
            Name = "HackerMan";
        }

        //Called when the enemy knocks
        public override bool Knacka(int round)
        {
            if (Game.Score(this) >= 23)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Called when the AI takes up a card
        public override bool TaUppKort(Card card)
        {
            //If the card isn't null
            if (card != null)
            {
                //Check if the hand is one suit
                if (IsOneSuit())
                {
                    //If the cards suit is the best suit
                    if (card.Suit == BestSuit)
                    {
                        //Temp value
                        int lowestValue = 11;

                        //Go through the hand
                        for (int i = 0; i < Hand.Count; i++)
                        {
                            //If the hands lowest card is lower than the lowestValue
                            if (Hand[i].Value < lowestValue)
                            {
                                //Change the lowestValue
                                lowestValue = Hand[i].Value;
                            }
                        }

                        //If the value is less than the lowest value
                        if (card.Value > lowestValue)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (card.Value == 11)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    List<Card> cards = GetNonBestSuitCards();

                    if (card.Suit == cards[0].Suit)
                    {
                        int sumNonBest = 0;

                        for (int i = 0; i < cards.Count; i++)
                        {
                            sumNonBest += cards[0].Value;
                        }

                        sumNonBest += card.Value;

                        if (sumNonBest > Game.SuitScore(Hand, BestSuit))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //Called when the AI should throw a cards
        public override Card KastaKort()
        {
            //If all the cards in the hand is the same suit
            if (IsOneSuit())
            {
                Card lowestCard = null;
                int lowestCardValue = 11;

                //Go through the hand
                for (int i = 0; i < Hand.Count; i++)
                {
                    //If the current cards value is less than the last lowest value
                    if (Hand[i].Value < lowestCardValue)
                    {
                        //Set the values
                        lowestCard = Hand[i];
                        lowestCardValue = Hand[i].Value;
                    }
                }

                //Return the worst card
                return lowestCard;
            }
            else
            {
                //Temp values
                Card throwawayCard = null;
                int lowestCardValue = 12;

                //Go through the hand
                for (int i = 0; i < Hand.Count; i++)
                {
                    //If it's not the best suit and the value is less than the lowestCardValue
                    if (Hand[i].Suit != BestSuit && Hand[i].Value < lowestCardValue)
                    {
                        lowestCardValue = Hand[i].Value;
                        throwawayCard = Hand[i];
                    }
                }

                if (throwawayCard == null)
                {
                    Console.Write("t");
                }
                return throwawayCard;
            }
        }

        //Called every time a game has ended
        public override void SpelSlut(bool wonTheGame)
        {
            GameNumber++;

            if (wonTheGame)
            {
                Wongames++;
            }

            if (lastTurn)
            {
                OpponentKnockingScore.Add(OpponentLatestScore);
            }

            //Clear the collecting cache
            OpponentCards.Clear();
        }

        //Check if the player has full hand of one suit
        private bool IsOneSuit()
        {
            Game.Score(this);

            int numCards = 0;

            //Go through all the cards
            for (int i = 0; i < Hand.Count; i++)
            {
                //If the suit if the current card is the best suit
                if (Hand[i].Suit == BestSuit)
                {
                    //Increase the temp num
                    numCards++;
                }
            }

            //If the amount of cards is equal to the amount of cards that is of the best suit
            if (numCards == Hand.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Get a list of the cards of the suit that isn't best
        private List<Card> GetNonBestSuitCards()
        {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < Hand.Count; i++)
            {
                if (Hand[i].Suit != BestSuit)
                {
                    cards.Add(Hand[i]);
                }
            }

            int[] sums = new int[4];

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].Suit == Suit.Hjärter)
                {
                    sums[0] += cards[i].Value;
                }
                else if (cards[i].Suit == Suit.Klöver)
                {
                    sums[1] += cards[i].Value;
                }
                else if (cards[i].Suit == Suit.Ruter)
                {
                    sums[2] += cards[i].Value;
                }
                else if (cards[i].Suit == Suit.Spader)
                {
                    sums[3] += cards[i].Value;
                }
            }

            int maxIndex = sums.ToList().IndexOf(sums.Max());

            Suit bestSuit;

            if (maxIndex == 0)
            {
                bestSuit = Suit.Hjärter;
            }
            else if (maxIndex == 1)
            {
                bestSuit = Suit.Klöver;
            }
            else if (maxIndex == 2)
            {
                bestSuit = Suit.Ruter;
            }
            else if (maxIndex == 3)
            {
                bestSuit = Suit.Spader;
            }
            else
            {
                bestSuit = Suit.Hjärter;
            }

            List<Card> returnCards = new List<Card>();

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].Suit == bestSuit)
                {
                    returnCards.Add(cards[i]);
                }
            }

            return returnCards;
        }

        //Gets the average of the opponents knocking score
        private int GetOpponentKnockingAverage()
        {
            //Get the sum and the average
            int sum = OpponentKnockingScore.Sum();
            int average = 0;

            if (OpponentKnockingScore.Count != 0)
            {
                average = sum / OpponentKnockingScore.Count;


            }
            else
            {
                average = 20;
            }

            return average;
        }

        private int GetKnockingAverage()
        {
            int sum = 0;

            for (int i = 0; i < KnockingScore.Count; i++)
            {
                sum += KnockingScore[i];
            }

            int average = sum / KnockingScore.Count;

            return average;
        }

        //Gets the probable collecting suit of the opponent
        Suit GetPropableOpponentCollectingSuit()
        {
            //int array to hold the amounts
            int[] amounts = new int[4];

            //Go through all the cards that the opponent has taken from the throwaway pile
            for (int i = 0; i < OpponentCards.Count; i++)
            {
                //Add to the correct amount
                if (OpponentCards[i].Suit == Suit.Hjärter)
                {
                    amounts[0]++;
                }
                else if (OpponentCards[i].Suit == Suit.Klöver)
                {
                    amounts[1]++;
                }
                else if (OpponentCards[i].Suit == Suit.Ruter)
                {
                    amounts[2]++;
                }
                else if (OpponentCards[i].Suit == Suit.Spader)
                {
                    amounts[3]++;
                }
            }

            //Get the index of the highest occuring number
            int maxIndex = amounts.ToList().IndexOf(amounts.Max());

            //Return the right suit based on the number
            if (maxIndex == 0)
            {
                return Suit.Hjärter;
            }
            else if (maxIndex == 1)
            {
                return Suit.Klöver;
            }
            else if (maxIndex == 2)
            {
                return Suit.Ruter;
            }
            else if (maxIndex == 3)
            {
                return Suit.Spader;
            }
            else
            {
                return Suit.Hjärter;
            }
        }
    }
}