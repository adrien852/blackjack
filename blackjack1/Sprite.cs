using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack1
{
    //A sprite can be clicked and dragged.
    class Sprite
    {
        //VARIABLES
        public Rectangle DestinationRectangle { get; set; }
        public Rectangle SourceRectangle { get; set; }
        public Rectangle BeforeDragDestinationRectangle { get; set; }
        public bool Clicked { get; set; }
        public Texture2D Texture { get; }

        //CONSTRUCTOR
        public Sprite(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle)
        {
            Texture = texture;
            Clicked = false;
            DestinationRectangle = destinationRectangle;
            SourceRectangle = sourceRectangle;
        }
        
        //DISPLAY ON SCREEN
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Texture, destinationRectangle: DestinationRectangle, color: Color.White, sourceRectangle: SourceRectangle);
        }

        public bool IsClicked(MouseState state, MouseState previousState)
        {
            if(DestinationRectangle.Contains(state.X, state.Y))
            {
                if (state.LeftButton == ButtonState.Pressed & previousState.LeftButton != ButtonState.Pressed)
                {
                    Clicked = true;
                    return true;
                }
            }
            return false;
        }

        public bool IsHolded(MouseState state, MouseState previousState)
        {
            if (Clicked & state.LeftButton == ButtonState.Pressed & previousState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool IsReleased(MouseState state, MouseState previousState)
        {
            if (state.LeftButton == ButtonState.Released & previousState.LeftButton == ButtonState.Pressed)
            {
                Clicked = false;
                return true;
            }
            return false;
        }
    }
}
