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
        //VARIABLES
        private string color;
        private string number;
        private int value;
        private Rectangle flippedSourceRectangle;

        //CONSTRUCTOR
        //By default, a card is a flipped card in the deck slot
        public Card(string number, string color, int value, Texture2D texture) : base(texture, new Rectangle(1338, 18, 125, 181), new Rectangle(0, 724, 125, 181))
        {
            this.color = color;
            this.number = number;
            this.value = value;
            this.flippedSourceRectangle = new Rectangle(0, 724, 125, 181);
        }

        //GETTERS SETTERS
        public string GetColor()
        {
            return this.color;
        }
        public string GetNumber()
        {
            return this.number;
        }
        public int GetValue()
        {
            return this.value;
        }
        public void SetValue(int value)
        {
            this.value = value;
        }

        //DISPLAY
        public override string ToString()
        {
            return this.GetNumber() + this.GetColor() + " " + this.GetValue();
        }

        //Flip the card by getting the flipped sprite and replacing it with the current sprite
        public void FlipCard()
        {
            Rectangle temp;
            temp = GetSourceRectangle();
            this.SetSourceRectangle(flippedSourceRectangle);
            flippedSourceRectangle = temp;
        }
    }
}
