using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GameLibrary
{
    public class Player
    {
        //todo add player name property
        #region Fields
        #endregion
        #region Properties
        public byte PlayerNumber { get; set; }
        public List<Card> Hand { get; set; }
        public byte Tokens { get; set; }
        public byte HandBreak { get; set; }
        public bool HasPassed { get; set; }
        public byte Score { get; set; }
        public byte IncreaseBreak { get; set; }
        public string Name { get; set; }
        #endregion

        #region CTOR
        public Player() { }
        public Player(byte playerNumber, byte handBreak, byte increaseBreak)
        {
            Hand = new List<Card>();
            PlayerNumber = playerNumber;
            Tokens = 10;
            HandBreak = handBreak;
            HasPassed = false;
            Score = 0;
            IncreaseBreak = increaseBreak;
            Name = "Player" + PlayerNumber;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return string.Format(Name);
        }

        public string PublicInfo()
        {
            return string.Format($"{ToString()}:\n\tCurrent Handsize: {Hand.Count}\n\tTokens:{Tokens}\n");
        }

        public void PrivateInfo()
        {
            if (Score > HandBreak)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.Write($"Score: {Score}\tCurrent Break: {HandBreak}\t");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Tokens: {Tokens}\nYour Hand:\n{ShowHand()}");
        }

        public string ShowHand()
        {
            string yourhand = "";
            foreach (Card c in Hand)
            {
                yourhand += $"\t{c}\n";
            }
            return yourhand;
        }

        public void DrawCard(List<Card> deck)
        {
            Random rand = new Random();
            Thread.Sleep(12);
            int index = rand.Next(0, deck.Count());
            Card choosenCard = deck[index];

            Hand.Add(choosenCard);
            if (choosenCard.StoryPoints == 0)
            {
                HandBreak += IncreaseBreak; 
            }
            else
            {
                Score += choosenCard.StoryPoints;
            }
            deck.RemoveAt(index);
        }

        public bool FullFamily()
        {
            if (Hand.Exists(x => x.Face == Face.King) && Hand.Exists(x => x.Face == Face.Queen) && Hand.Exists(x => x.Face == Face.Jack))
            {
                return true;
            }
            #region Joker logic. Must umcomment to play with jokers
            switch (Hand.Count(x => x.Face == Face.Joker))
            {
                case 2:
                    if (Hand.Exists(x => x.Face == Face.King || x.Face == Face.Queen || x.Face == Face.Jack))
                    {
                        return true;
                    }
                    break;
                case 1:
                    if (Hand.Exists(x => x.Face == Face.King) && Hand.Exists(x => x.Face == Face.Queen)
                        || Hand.Exists(x => x.Face == Face.King) && Hand.Exists(x => x.Face == Face.Jack)
                        || Hand.Exists(x => x.Face == Face.Queen) && Hand.Exists(x => x.Face == Face.Jack))
                    {
                        return true;
                    }
                    break;
                case 0:
                    break;
                default:
                    Console.WriteLine("Only dirty cheaters ( or testing coders get here)");
                    break;
            }
            #endregion
            return false;
        }

        #endregion
    }
}
