﻿using Microsoft.Xna.Framework;
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
        //GETTERS SETTERS
        public int Value { get; }

        //CONSTRUCTOR
        public Token(int value, Texture2D texture) : base(texture, new Rectangle(200, 730, 100, 99), new Rectangle(50, 50, 100, 99))
        {
            Value = value;
            //Set the token sprite matching its value
            switch (value)
            {
                case 10:
                    SourceRectangle = new Rectangle(0, 0, 100, 99);
                    break;
                case 20:
                    SourceRectangle = new Rectangle(100, 0, 100, 99);
                    break;
                case 50:
                    SourceRectangle = new Rectangle(200, 0, 100, 99);
                    break;
                case 100:
                    SourceRectangle = new Rectangle(0, 99, 100, 99);
                    break;
                case 500:
                    SourceRectangle = new Rectangle(100, 99, 100, 99);
                    break;
                case 1000:
                    SourceRectangle = new Rectangle(202, 99, 100, 99);
                    break;
                default:
                    break;
            }
            BeforeDragDestinationRectangle = DestinationRectangle;
        }

        //Return 1 if the token is placed inside bet box, return 2 if placed in the player's token, return 0 if placed in neither, return -1 if not clicked or not released yet
        public int DragToken(Player player, Bet betBox, MouseState state, MouseState previousState)
        {
            //If clicked, memorize the initial position in case of wrong placement
            if (IsClicked(state, previousState))
                BeforeDragDestinationRectangle = DestinationRectangle;
            //Hold click on token
            if (IsHolded(state, previousState))
                DestinationRectangle = new Rectangle(new Point(state.X - 50, state.Y - 50), new Point(100, 99));
            //Drop token in bet box or in player's tokens or token go back in position
            if (IsReleased(state, previousState) & betBox.DestinationRectangle.Contains(DestinationRectangle) & !player.TokenRectangle.Contains(DestinationRectangle))
                return 1;

            if (IsReleased(state, previousState) & !betBox.DestinationRectangle.Contains(DestinationRectangle) & player.TokenRectangle.Contains(DestinationRectangle))
                return 2;

            if (IsReleased(state, previousState) & !betBox.DestinationRectangle.Contains(DestinationRectangle) & !player.TokenRectangle.Contains(DestinationRectangle))
            {
                DestinationRectangle = BeforeDragDestinationRectangle;
                return 0;
            }
            return -1;
        }
    }
}
