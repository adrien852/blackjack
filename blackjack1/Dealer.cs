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
            TokenRectangle = new Rectangle(-50, -50, 490, 320);
        }

        //GAMEPLAY (Artificial intelligence)
        public void Update(GameTime gameTime, Player player, Deck deck, ref bool selfTurn, ref bool startTurn)
        {
            //If player already busted, dealer does not play
            if (player.GetHandValue() <= 21 || (player.GetStandbyHandValue() <= 21 && player.StandbyHand.Count >= 2))
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
            else
                PassTurn(ref selfTurn);
        }

        //DISPLAY ON SCREEN
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Animation animation, SpriteFont info, bool selfTurn, bool startTurn)
        {
            //Dealer's hand
            Hand.ForEach(card => card.Draw(spriteBatch));
            //Dealer's tokens
            foreach (List<Token> tokenList in TokenLists)
            {
                foreach (Token token in tokenList)
                    token.Draw(spriteBatch);
            }
            //Dealer's money
            spriteBatch.DrawString(info, "Money : " + Money, new Vector2(350, 20), Color.White);
            //Dealer's score
            if (selfTurn & !startTurn)
            {
                spriteBatch.DrawString(info, "Score : " + GetHandValue(), new Vector2(720, 300), Color.White);
            }
            //If not dealer's turn, only first card value
            else
            {
                if (Hand.Count != 0)
                    spriteBatch.DrawString(info, "Score : " + Hand[0].Value, new Vector2(720, 300), Color.White);
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

        //Add a token in the proper token list
        public override void SetTokensFromToken(Token token)
        {
            switch (token.Value)
            {
                case 10:
                    token.DestinationRectangle = new Rectangle(20, 0, 100, 99);
                    TokenLists[0].Add(token);
                    break;
                case 20:
                    token.DestinationRectangle = new Rectangle(120, 0, 100, 99);
                    TokenLists[1].Add(token);
                    break;
                case 50:
                    token.DestinationRectangle = new Rectangle(220, 0, 100, 99);
                    TokenLists[2].Add(token);
                    break;
                case 1000:
                    token.DestinationRectangle = new Rectangle(220, 100, 100, 99);
                    TokenLists[3].Add(token);
                    break;
                case 500:
                    token.DestinationRectangle = new Rectangle(120, 100, 100, 99);
                    TokenLists[4].Add(token);
                    break;
                case 100:
                    token.DestinationRectangle = new Rectangle(20, 100, 100, 99);
                    TokenLists[5].Add(token);
                    break;
            }
        }
    }
}
