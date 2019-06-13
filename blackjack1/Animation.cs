using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack1
{
    class Animation
    {
        /*private Card previousCard;
        private Vector2 cardScale;
        private bool bouncing;
        private bool initAnimation;

        public Animation()
        {
            cardScale = new Vector2(1, 1);
            bouncing = false;
            initAnimation = true;
        }
        public void FlipAnimation(Card card, SpriteBatch spriteBatch)
        {
            if (initAnimation == true)
            {
                previousCard = card;
                initAnimation = false;
            }
            if (card.SourceRectangle != previousCard.SourceRectangle)
            {
                if (bouncing == false)
                {
                    spriteBatch.Draw(texture: previousCard.Texture, destinationRectangle: previousCard.DestinationRectangle, color: Color.White, scale: cardScale, origin: new Vector2(62.5f, 90.5f));
                    cardScale.X -= 0.1f;
                }
                if (cardScale == Vector2.Zero)
                {
                    bouncing = true;
                }
                if (bouncing == true)
                {
                    spriteBatch.Draw(texture: card.Texture, destinationRectangle: card.DestinationRectangle, color: Color.White, scale: cardScale, origin: new Vector2(62.5f, 90.5f));
                    cardScale.X += 0.1f;
                }
                if (cardScale == Vector2.One)
                {
                    bouncing = false;
                    previousCard = card;
                }
            }
        }*/
    }
}
