using UnityEngine;
namespace Bhabbhi
{
    [System.Serializable]
    public class Card
    {
        public string Name;
        public ValuesEnum Value;
        public SuitsEnum Suit;
        public Sprite CardImage;

        public Card(int suit, int value)
        {
            Value = (ValuesEnum)value;
            Suit = (SuitsEnum)suit;
            Name = Value.ToString() + "-" + Suit.ToString();
            //CardImage = 
        }

        public string ValueToSymbol(int value)
        {
            switch (value)
            {
                case 11:
                    return "J";
                case 12:
                    return "Q";
                case 13:
                    return "K";
                case 14:
                    return "A";
                default:
                    return value.ToString();
            }
        }

        public void SetName()
        {
            Name = Value.ToString() + "-" + Suit.ToString();
        }
    }

    public enum ValuesEnum
    {
        Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10, Jack = 11, Queen = 12, King = 13, Ace = 14
    }

    public enum SuitsEnum
    {
        Club, Spade, Heart, Diamond, Undefined
    }
}