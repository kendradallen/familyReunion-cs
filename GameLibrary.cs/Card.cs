using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    public class Card
    {
        #region Fields
        private byte _storyPoints;
        #endregion
        #region Properties
        public Suit Suit { get; set; }
        public Face Face { get; set; }
        public byte StoryPoints
        {
            get { return _storyPoints; }
            set
            {
                switch (Face)
                {
                    case Face.Ace:
                        _storyPoints = 11;
                        break;
                    case Face.Two:
                        _storyPoints = 2;
                        break;
                    case Face.Three:
                        _storyPoints = 3;
                        break;
                    case Face.Four:
                        _storyPoints = 4;
                        break;
                    case Face.Five:
                        _storyPoints = 5;
                        break;
                    case Face.Six:
                        _storyPoints = 6;
                        break;
                    case Face.Seven:
                        _storyPoints = 7;
                        break;
                    case Face.Eight:
                        _storyPoints = 8;
                        break;
                    case Face.Nine:
                        _storyPoints = 9;
                        break;
                    case Face.Ten:
                        _storyPoints = 10;
                        break;
                    case Face.Jack:
                        _storyPoints = 0;
                        break;
                    case Face.Queen:
                        _storyPoints = 0;
                        break;
                    case Face.King:
                        _storyPoints = 0;
                        break;
                    case Face.Joker:
                        _storyPoints = 0;
                        break; 
                }
            }
        }
        #endregion
        #region CTOR
        public Card(Suit suit, Face face)
        {
            Suit = suit;
            Face = face;
            StoryPoints = _storyPoints;
        }
        #endregion
        #region Methods
        public override string ToString()
        {
            #region Joker logic. Uncomment out to play with jokers
            if (Face == Face.Joker)
            {
                return string.Format((Suit == Suit.Hearts ? "Red" : "Black") + " Joker");
            }
            #endregion
            return string.Format($"{Face} of {Suit}");
        }
        #endregion

    }
}
