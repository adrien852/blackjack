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
    //Dealer is a special player with an AI, who can shuffle the deck and is the last one to play. His hand is displayed on top of the screen.
    class Dealer : Player
    {
        //VARIABLES
        private static Random rng = new Random();

        //CONSTRUCTOR
        public Dealer(double money, Texture2D tokenTexture) : base(money, tokenTexture)
        {
        }

        //GAMEPLAY (Artificial intelligence)
        public void Update(GameTime gameTime, Player player, Deck deck, Sprite placeBetsButton, ref bool selfTurn, ref bool startTurn)
        {

            //At the beginning of the turn, flip the last card in his hand
            if (startTurn)
            {
                var lastCardIndex = Hand.Count - 1;
                Hand[lastCardIndex].FlipCard();
                startTurn = false;
            }
            //If his total hand value is under 17, keep drawing cards. Else, pass the turn
            else if (GetHandValue() < 17)
                DrawCards(1, deck);
            else
                PassTurn(ref selfTurn);
        }

        //DISPLAY ON SCREEN
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont score, bool selfTurn)
        {
            //Dealer's hand
            Hand.ForEach(card => card.Draw(spriteBatch));
            //Dealer's score
            if (selfTurn)
            {
                spriteBatch.DrawString(score, "Score : " + GetHandValue(), new Vector2(720, 300), Color.White);
            }
            //If not dealer's turn, only first card value
            else
            {
                if (Hand.Count != 0)
                    spriteBatch.DrawString(score, "Score : " + Hand[0].Value, new Vector2(720, 300), Color.White);
            }
        }

        //Fisher-Yates shuffle
        public void Shuffle(Deck deck)
        {
            List<Card> cards = deck.Cards;
            int currentPosition = cards.Count;
            Card temp;
            while (currentPosition > 1)
            {
                currentPosition--;
                int newPosition = rng.Next(currentPosition + 1);
                temp = cards[newPosition];
                cards[newPosition] = cards[currentPosition];
                cards[currentPosition] = temp;
            }
        }

        //Give cards to Dealer, place them on dealer slot and remove them from deck
        public override void DrawCards(int numberOfCards, Deck deck)
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                Card card = deck.Cards[0];
                var size = Hand.Count();
                Hand.Add(card);
                card.DestinationRectangle = new Rectangle(737 + (size * 130), 90, 125, 181);
                deck.Cards.Remove(card);
            }
        }

        public void PassTurn(ref bool selfTurn)
        {
            selfTurn = false;
        }
    }
}
