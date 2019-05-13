using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack1
{
    class Deck : Sprite
    {
        //VARIABLES
        public int NumberOfCards { get; }
        public List<Card> Cards { get; }

        //CONSTRUCTOR
        public Deck(int numberOfCards, Texture2D texture) : base(texture, new Rectangle(1338, 18, 125, 181), new Rectangle(0, 724, 125, 181))
        {
            NumberOfCards = numberOfCards;
            Cards = new List<Card>();
            if (numberOfCards == 52) //If deck of 52 cards, traditional settings
            {
                string[] colors = new string[] { "H", "D", "C", "S" }; //Diamonds, Clubs, Spades, Hearts
                int i;
                int j;
                //For each card of each number of each color, create a card, its sprite and its properties and add it in the deck 
                for (i = 0; i < 4; ++i)
                {
                    for (j = 1; j <= 13; ++j)
                    {
                        if (j <= 10)
                        {
                            Card card;
                            if(j == 1) 
                            {
                                card = new Card($"{j}", colors[i], 11, texture); //If Ace, value of 11 by default
                            }
                            else
                            {
                                card = new Card($"{j}", colors[i], j, texture);
                            }
                            card.SourceRectangle = new Rectangle(125 * (j - 1), 181 * i, 125, 181);
                            Cards.Add(card);
                        }
                        else
                        {
                            switch (j)
                            {
                                case 11:
                                    var card = new Card("J", colors[i], 10, texture);
                                    card.SourceRectangle = new Rectangle(125 * (j - 1), 181 * i, 125, 181);
                                    Cards.Add(card);
                                    break;
                                case 12:
                                    card = new Card("Q", colors[i], 10, texture);
                                    card.SourceRectangle = new Rectangle(125 * (j - 1), 181 * i, 125, 181);
                                    Cards.Add(card);
                                    break;
                                case 13:
                                    card = new Card("K", colors[i], 10, texture);
                                    card.SourceRectangle = new Rectangle(125 * (j - 1), 181 * i, 125, 181);
                                    Cards.Add(card);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Deck needs to contain 52 cards to work.");
            }
        }

        //DEBUG
        public void ShowDeckInOutput()
        {
            string display = "Deck : ";
            Cards.ForEach(item => display += item + " ");
            Console.WriteLine(display);
        }
    }
}
