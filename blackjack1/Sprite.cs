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
        protected Texture2D texture;
        protected Rectangle destinationRectangle;
        protected Rectangle sourceRectangle;
        protected bool clicked;
        protected Rectangle beforeDragDestinationRectangle;
        
        //CONSTRUCTOR
        public Sprite(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle)
        {
            this.texture = texture;
            this.clicked = false;
            this.destinationRectangle = destinationRectangle;
            this.sourceRectangle = sourceRectangle;
        }

        //GETTERS SETTERS
        public Rectangle GetDestinationRectangle()
        {
            return destinationRectangle;
        }
        public void SetDestinationRectangle(Rectangle rect)
        {
            destinationRectangle = rect;
        }
        public Rectangle GetSourceRectangle()
        {
            return sourceRectangle;
        }
        public void SetSourceRectangle(Rectangle rect)
        {
            sourceRectangle = rect;
        }
        public Rectangle GetBeforeDragDestinationRectangle()
        {
            return beforeDragDestinationRectangle;
        }
        public void SetBeforeDragDestinationRectangle(Rectangle before)
        {
            this.beforeDragDestinationRectangle = before;
        }
        public bool GetClicked()
        {
            return clicked;
        }
        public void SetClicked(bool clicked)
        {
            this.clicked = clicked;
        }

        //DISPLAY ON SCREEN
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: texture, destinationRectangle: destinationRectangle, color: Color.White, sourceRectangle: sourceRectangle);
        }

        public bool isClicked(MouseState state, MouseState previousState)
        {
            if(GetDestinationRectangle().Contains(state.X, state.Y))
            {
                if (state.LeftButton == ButtonState.Pressed & previousState.LeftButton != ButtonState.Pressed)
                {
                    this.SetClicked(true);
                    return true;
                }
            }
            return false;
        }

        public bool isHolded(MouseState state, MouseState previousState)
        {
            if (this.GetClicked() & state.LeftButton == ButtonState.Pressed & previousState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool isReleased(MouseState state, MouseState previousState)
        {
            if (state.LeftButton == ButtonState.Released & previousState.LeftButton == ButtonState.Pressed)
            {
                this.SetClicked(false);
                return true;
            }
            return false;
        }
    }
}
