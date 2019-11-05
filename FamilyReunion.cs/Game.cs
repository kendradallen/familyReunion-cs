using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLibrary;
using System.Threading;

namespace FamilyReunion
{
    class Game
    {
        static void Main(string[] args)
        {

            #region Game Variables
            //todo figure out how to make pot a variable that the functions could change if they returned void.
            byte pot = 0;
            byte baseHandBreak = 18;
            //byte baseTokens = 10; //changed to be property set in player
            bool playGame = true;
            bool jokers = false;
            byte increaseBreak = 3;
            #endregion
            Console.WriteLine("Welcome to Family Reunion!");
            Console.WriteLine("Press space to read instructions or anything else to move forward to the game.");
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.Spacebar:
                    Console.WriteLine("The family reunion is coming up and you are trying to have the best story to tell. But if your adventures are too wild no one will believe you. I'm sure if your family backed up your tale, people wouldn't question it.");
                    Console.WriteLine("Family reunion is a multi-player betting game similar to blackjack in that your goal is to get the highest number possible in your hand without going over the limit.");
                    Console.WriteLine($"In this game however, face cards do not increase your score' instead they increase your hand's break point. Each player's break point starts at {baseHandBreak}, but for every face card this increases by {increaseBreak}. This represents the family backing up your story.");
                    Console.WriteLine("If you have a full family backing - i.e. have a king, queen, and jack in your hand - no one will be able to trump your story unless they also have a full family backing their story.");
                    Console.WriteLine("In every game, each player begins with a hand of 2 cards. They also must pay an initial bet of 1 token. There are then four rounds of gathering stories where you have the opportunity to gahter more stories (draw a card). This will cost 1 token and once passed, you cannot particapate in the gathering of stories for the rest of the game. The fourth round, cost twice as must in order to participate in.");
                    Console.WriteLine("The hands are then revealed. If two or more players have a full family backing then their story points are compared. Otherwise ties between scores are broken by who has more family backing. In the case of ties, the pot will be split between the winnners and any remainders will be left in the pot for the next game.");
                    Console.WriteLine("You may optionally play with Jokers. They act as a any kind of face card.");
                    Console.ReadKey();
                    Console.Clear();
                    break;
                default:
                    break;
            }
            Console.WriteLine("Press J to play with jokers. (under testing)\nAnything else for regular play ");
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.J:
                    jokers = true;
                    break;
                default:
                    jokers = false;
                    break;
            }
            Console.WriteLine("How many players?\nType a number between 2 and 6");
            byte playerNumber = Byte.Parse(Console.ReadLine());
            List<Player> Players = new List<Player> { };
            //todo make 1 person version

            for (byte i = 1; i <= playerNumber; i++)
            {
                Players.Add(new Player(i, baseHandBreak, increaseBreak));
            }

            List<Player> currentPlayers = Players;


            do
            {
                //build deck
                List<Card> deck = new List<Card> { };
                foreach (Suit i in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Face f in Enum.GetValues(typeof(Face)))
                    {
                        deck.Add(new Card(i, f));
                    }

                    #region Joker logic. 
                    if (i == Suit.Diamonds)
                    {
                        deck.RemoveAll(x => x.Face == Face.Joker);
                    }
                    #endregion
                }
                //joker removal step
                if (!jokers)
                {
                    deck.RemoveAll(x => x.Face == Face.Joker);
                }

                //draw initial hand
                foreach (Player p in currentPlayers)
                {
                    p.DrawCard(deck);
                    Thread.Sleep(39);
                    p.DrawCard(deck);
                    Thread.Sleep(21);
                    p.Tokens -= 1;
                    pot += 1;
                }
                

                for (byte r = 1; r <= 4; r++)
                {
                    Console.Clear();
                    Console.WriteLine($"Round {r}!");
                    if (r == 4)
                    {
                        Console.WriteLine("The Final round! Cost to gather stories is doubled");
                    }
                    Console.ReadKey();
                    foreach (Player p in currentPlayers)
                    {
                        bool reload = true;
                        if (p.HasPassed)
                        {
                            Console.WriteLine($"It would be {p}'s turn, but they have passed!");
                            reload = false;
                        }
                        else
                        {
                            Console.WriteLine($"{p}'s turn!\nPass the game to them!\nPress anykey to start your turn {p}!\n");
                            Console.ReadKey();
                            Console.Clear();
                            Console.WriteLine("\t\tROUND {0}\nPot: {1}\n", r, pot);
                            p.PrivateInfo();
                            Console.WriteLine("Would you like to\n1) draw or \n2) pass?");
                        }
                        while (reload)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.NumPad1:
                                case ConsoleKey.D1:
                                    pot += Hit(p, deck, pot, r);
                                    reload = false;
                                    Console.ReadKey();
                                    break;
                                case ConsoleKey.NumPad2:
                                case ConsoleKey.D2:
                                    p.HasPassed = true;
                                    reload = false;
                                    break;
                                default:
                                    Console.WriteLine("Didn't quite catch that. Please try again.\n");
                                    break;
                            }
                        }
                        Console.Clear();
                    }
                    if (currentPlayers.TrueForAll(x => x.HasPassed))
                    {
                        Console.WriteLine("All players have passed. Proceeding to the end of game.");
                        Console.ReadKey();
                        break;
                    }

                }
                Console.Clear();
                Console.WriteLine("Time to reveal your hands and tell your stories! Everyone gather round the screen");
                //reveal hands
                foreach (Player p in currentPlayers)
                {
                    Console.WriteLine("{0}'s hand\n", p);
                    Console.WriteLine(p.ShowHand()); 
                }
                Console.ReadKey();

                pot = RoundEnd(Players, pot);

                currentPlayers.RemoveAll(x => x.Tokens <= 0);

                if (currentPlayers.Count == 1)
                {
                    Console.WriteLine("Contratulations {0}. You have all the tokens!\n" +
                        "Thanks for playing everyone!", currentPlayers[0]);
                    playGame = false;
                }
                else if (currentPlayers.Count == 0)
                {
                    Console.WriteLine("So..... everyone went broke.... that's special....\nUh... thanks for playing. Bye now");
                    playGame = false;
                }
                //again?
                else
                {
                    Console.WriteLine("Continue playing or take your metaphorical loses and stop?\nPress escape to stop early or anything else to continue");
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Escape:
                            playGame = false;
                            Console.WriteLine("Thanks for playing!");
                            break;
                        default:
                            Console.WriteLine("Alright more fun! Here's the current scores:");
                            foreach (Player p in Players)
                            {
                                Console.WriteLine($"{p}" + (p.Tokens > 0 ? $": {p.Tokens.ToString()}" : " is broke"));
                            }
                            if (pot != 0)
                            {
                                Console.WriteLine("And the pot still has {0} in it", pot);
                            }
                            break;
                    }

                    //todo reset everything
                    foreach (Player p in currentPlayers)
                    {
                        p.HandBreak = baseHandBreak;
                        p.Score = 0;
                        p.Hand = new List<Card> { };
                        p.HasPassed = false;
                    }
                }
                Console.ReadKey();
            } while (playGame);
        }

        public static byte Hit(Player person, List<Card> deck, byte pot, byte round)
        {
            byte cost = 1;
            if (round == 4)
            {
                cost *= 2;
            }
            if (person.Tokens < cost)
            {
                Console.WriteLine("Sorry dude but you can't afford the cost. You can't draw anymore.");
                return 0;
            }
            else
            {
                person.DrawCard(deck);
                person.Tokens -= cost;
                //pot += cost;
                Console.WriteLine("Your new card:\n" + person.Hand[person.Hand.Count() - 1]);
                return cost;
            }
        }

        public static byte RoundEnd(List<Player> players, byte pot)
        {
            List<Player> winners = new List<Player> { };
            bool escape = false;
            int counter = 1;    //if counter reaches 3 then everyone lost
            List<Player> qualifiers = players.FindAll(x => x.FullFamily());
            do
            {
                qualifiers.RemoveAll(x => x.Score != qualifiers.Max(y => y.Score)); //      <-------remove anyone without a score equal to the current highest score in qualifiers

                switch (qualifiers.Count)
                {
                    //first case is for whenever qualifiers is empty. The first time this happens is because no one has a full family. 
                    //The only reason this would occur twice is due to no one having full family and EVERYONE busting
                    case int n when (n == 0):
                        qualifiers = players.FindAll(x => x.Score <= x.HandBreak); //           <--------change qualifiers from full families to non-busted hands
                        counter++;
                        break;
                    case int n when (n == 1): //                  <------------whenever there is no tie at current stage (on either full families or non-fulls
                        winners.Add(qualifiers[0]);
                        escape = true;
                        break;
                    case int n when (n > 1):    //there was a tie scenario
                        switch (counter)
                        {
                            case 1: //              <--------when there are multiple people with full families and highest score
                                for (int i = 0; i < qualifiers.Count; i++)
                                {
                                    winners.Add(qualifiers[i]);
                                }
                                break;
                            case 2:
                                qualifiers.RemoveAll(x => x.HandBreak != qualifiers.Max(y => y.HandBreak)); //          <--------and remove any of the max scorers without highest number of family members
                                for (int i = 0; i < qualifiers.Count; i++)
                                {
                                    winners.Add(qualifiers[i]);
                                }
                                break;
                        }
                        escape = true;
                        break;
                }
            } while (!escape && counter <= 2);

            if (winners.Count >1)
            {
                for (byte i = 0; i < winners.Count; i++)
                {
                    Console.Write(winners[i] + (i + 1 == winners.Count ? " " : ", ") + (i + 2 == winners.Count ? "and " : ""));
                }
                Console.WriteLine("have tied!");
                Console.WriteLine("The pot will be divided amongst the {0} of you. You each get {1} tokens!", winners.Count, pot / (winners.Count));
                foreach (Player w in winners)
                {
                    w.Tokens += (byte)(pot / winners.Count);
                }
                return (byte)(pot % winners.Count);
            }
            else if (winners.Count == 1)
            {
                Console.WriteLine("{0} wins the pot of {1} tokens!", winners[0], pot);
                winners[0].Tokens += pot;
                return 0;
            }
            else
            {
                Console.WriteLine("No one won. Too bad!");
                return pot;
            }

        }

    }
}
