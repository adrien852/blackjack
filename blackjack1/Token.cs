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
    class Token : Sprite
    {
        int value;

        //CONSTRUCTOR
        public Token(int value, Texture2D texture) : base(texture, new Rectangle(200, 730, 100, 99), new Rectangle(50, 50, 100, 99))
        {
            this.value = value;
            //Set the token sprite matching its value
            switch (value)
            {
                case 10:
                    this.SetDestinationRectangle(new Rectangle(20, 700, 100, 99));
                    this.SetSourceRectangle(new Rectangle(0, 0, 100, 99));
                    break;
                case 20:
                    this.SetDestinationRectangle(new Rectangle(120, 700, 100, 99));
                    this.SetSourceRectangle(new Rectangle(100, 0, 100, 99));
                    break;
                case 50:
                    this.SetDestinationRectangle(new Rectangle(220, 700, 100, 99));
                    this.SetSourceRectangle(new Rectangle(200, 0, 100, 99));
                    break;
                case 100:
                    this.SetDestinationRectangle(new Rectangle(20, 800, 100, 99));
                    this.SetSourceRectangle(new Rectangle(0, 99, 100, 99));
                    break;
                case 500:
                    this.SetDestinationRectangle(new Rectangle(120, 800, 100, 99));
                    this.SetSourceRectangle(new Rectangle(100, 99, 100, 99));
                    break;
                case 1000:
                    this.SetDestinationRectangle(new Rectangle(220, 800, 100, 99));
                    this.SetSourceRectangle(new Rectangle(202, 99, 100, 99));
                    break;
                default:
                    break;
            }
            this.beforeDragDestinationRectangle = destinationRectangle;
        }

        //GETTERS SETTERS
        public int GetValue()
        {
            return value;
        }
        public Texture2D GetTexture()
        {
            return this.texture;
        }

        //Return 1 if the token is placed inside bet box, return 2 if placed in the player's token, return 0 if placed in neither, return -1 if not clicked or not released yet
        public int DragToken(Player player, Bet betBox, MouseState state, MouseState previousState)
        {
            //If clicked, memorize the initial position in case of wrong placement
            if (this.isClicked(state, previousState))
                this.SetBeforeDragDestinationRectangle(this.GetDestinationRectangle());
            //Hold click on token
            if (this.isHolded(state, previousState))
                this.SetDestinationRectangle(new Rectangle(new Point(state.X - 50, state.Y - 50), new Point(100, 99)));
            //Drop token in bet box or in player's tokens or token go back in position
            if (this.isReleased(state, previousState) & betBox.GetDestinationRectangle().Contains(this.GetDestinationRectangle()) & !player.GetTokenRectangle().Contains(this.GetDestinationRectangle()))
                return 1;

            if (this.isReleased(state, previousState) & !betBox.GetDestinationRectangle().Contains(this.GetDestinationRectangle()) & player.GetTokenRectangle().Contains(this.GetDestinationRectangle()))
                return 2;

            if (this.isReleased(state, previousState) & !betBox.GetDestinationRectangle().Contains(this.GetDestinationRectangle()) & !player.GetTokenRectangle().Contains(this.GetDestinationRectangle()))
            {
                this.SetDestinationRectangle(this.GetBeforeDragDestinationRectangle());
                return 0;
            }
            return -1;
        }
    }
}
