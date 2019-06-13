using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack1
{
    class Card : Sprite
    {
        //GETTERS SETTERS
        public string Color { get; }
        public string Number { get; }
        public int Value { get; set; }
        public Rectangle FlippedSourceRectangle { get; set; }
        public Vector2 Scale { get; set; }

        //CONSTRUCTOR
        //By default, a card is a flipped card in the deck slot
        public Card(string number, string color, int value, Texture2D texture) : base(texture, new Rectangle(1338, 18, 125, 181), new Rectangle(0, 724, 125, 181))
        {
            Color = color;
            Number = number;
            Value = value;
            FlippedSourceRectangle = new Rectangle(0, 724, 125, 181);
            //Scale = new Vector2(1, 1);
        }

        //DISPLAY
        public override string ToString()
        {
            return Number + Color + " " + Value;
        }

        //Flip the card by getting the flipped sprite and replacing it with the current sprite
        public void FlipCard()
        {
            //Scale -= new Vector2(0.1f, 0.1f);
            //Console.WriteLine(Scale);
            Rectangle temp;
            temp = SourceRectangle;
            SourceRectangle = FlippedSourceRectangle;
            FlippedSourceRectangle = temp;

        }
    }
}
