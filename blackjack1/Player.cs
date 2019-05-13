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
    class Player
    {
        //VARIABLES
        //In tokens, first list is tokens of 1000, then 500, 100, 50, 20, 10
        protected List<Card> hand;
        protected List<List<Token>> tokens;
        protected double money;
        protected Rectangle tokenRectangle;

        //CONSTRUCTOR
        public Player(double money, Texture2D tokenTexture)
        {
            this.money = money;
            this.hand = new List<Card>();
            this.tokens = new List<List<Token>>();
            this.SetTokensFromMoney(tokenTexture);
            this.tokenRectangle = new Rectangle(-50, 630, 490, 320);
        }

        //GETTERS SETTERS
        public Rectangle GetTokenRectangle()
        {
            return tokenRectangle;
        }
        public List<List<Token>> GetTokenLists()
        {
            return tokens;
        }
        public double GetMoney()
        {
            return money;
        }
        //Get the value from each token and set the total variable
        public void SetMoneyFromTokens()
        {
            double total = 0;
            foreach (List<Token> tokenList in this.GetTokenLists())
            {
                foreach (Token token in tokenList)
                {
                    if (tokenList.Count > 0)
                        total += token.GetValue();
                }
            }
            this.money = total;
        }
        public List<Card> GetHand()
        {
            return hand;
        }
        public void SetMoney(double money)
        {
            this.money = money;
        }
        public void SetHand(List<Card> hand)
        {
            this.hand = hand;
        }
        //Get the value from each card and return the lowest total hand value.
        public int GetHandValue()
        {
            int total = 0;
            GetHand().ForEach(card => total += card.GetValue());
            foreach (Card card in GetHand())
            {
                //If any ace in hand with a busted score, ace changes value
                if (card.GetValue() == 11 & total > 21)
                    total -= 10;
            }
            return total;
        }

        //Convert money into tokens
        public void SetTokensFromMoney(Texture2D texture)
        {
            double total = this.GetMoney();
            this.tokens = new List<List<Token>>();
            this.CreateTokenList(10, ref total, texture);
            this.CreateTokenList(20, ref total, texture);
            this.CreateTokenList(50, ref total, texture);
            this.CreateTokenList(1000, ref total, texture);
            this.CreateTokenList(500, ref total, texture);
            this.CreateTokenList(100, ref total, texture);
            while(total - 10 >= 0)
            {
                this.CreateTokenList(2, 50, ref total, texture);
                this.CreateTokenList(1, 20, ref total, texture);
                this.CreateTokenList(0, 10, ref total, texture);
            }
        }

        //Add a token in the proper token list
        public void SetTokensFromToken(Token token)
        {
            switch (token.GetValue())
            {
                case 10:
                    this.tokens[0].Add(new Token(10, token.GetTexture()));
                    break;
                case 20:
                    this.tokens[1].Add(new Token(20, token.GetTexture()));
                    break;
                case 50:
                    this.tokens[2].Add(new Token(50, token.GetTexture()));
                    break;
                case 1000:
                    this.tokens[3].Add(new Token(1000, token.GetTexture()));
                    break;
                case 500:
                    this.tokens[4].Add(new Token(500, token.GetTexture()));
                    break;
                case 100:
                    this.tokens[5].Add(new Token(100, token.GetTexture()));
                    break;
            }
        }

        //Give enough tokens to not bet too much at once.
        public void CreateTokenList(int value, ref double total, Texture2D texture)
        {
            this.tokens.Add(new List<Token>());
            int size = tokens.Count - 1;
            if (value > 50)
            {
                while (total > 3 * value)
                {
                    this.tokens[size].Add(new Token(value, texture));
                    total -= value;
                }
            }
            else
            {
                int amount = 0;
                while (total - value >= 0 & amount < 4)
                {
                    this.tokens[size].Add(new Token(value, texture));
                    total -= value;
                    ++amount;
                }
            }
        }
        public void CreateTokenList(int index, int value, ref double total, Texture2D texture)
        {
            while (total - value >= 0)
            {
                this.tokens[index].Add(new Token(value, texture));
                total -= value;
            }
        }

        //DEBUG
        public string ShowHandInOutput()
        {
            string display = "Hand : ";
            GetHand().ForEach(card => display += card + " ");
            return display;
        }

        //GAMEPLAY
        public virtual void Update(GameTime gameTime, Deck deck, MouseState state, MouseState previousState, Sprite passButton, Sprite placeBetsButton, Bet betBox, ref bool selfTurn, ref bool opponentTurn)
        {
            //Player draws cards by clicking on deck
            if (deck.isClicked(state, previousState))
            {
                this.DrawCards(1, deck);
                Console.WriteLine(this.ShowHandInOutput());
            }
            //Player passes turn by tapping passButton or by having 6 cards or by having 21 or more
            if (passButton.isClicked(state, previousState) || GetHand().Count == 7 || GetHandValue() >= 21)
            {
                this.PassTurn(ref selfTurn, ref opponentTurn);
            }
            //Player can drag tokens around at the beggining of their turn
            if (!placeBetsButton.GetClicked())
            {
                //If a player's token is dragged inside the bet box, it's added to the bet, removed from player's tokens and the totals are updated
                foreach (List<Token> tokenList in this.GetTokenLists())
                {
                    if (tokenList.Count > 0)
                    {
                        Token lastToken = tokenList[tokenList.Count - 1];
                        if (lastToken.DragToken(this, betBox, state, previousState) == 1)
                        {
                            betBox.SetTokens(lastToken);
                            tokenList.Remove(lastToken);
                            this.SetMoneyFromTokens();
                            betBox.SetTotalFromTokens();
                        }
                    }
                }
                //If a token from the bet is dragged to the player's token, it's added to the player's tokens, removed from bet and the totals are updated
                foreach (List<Token> tokenList in betBox.GetTokenLists())
                {
                    if (tokenList.Count > 0)
                    {
                        Token lastToken = tokenList[tokenList.Count - 1];
                        if (lastToken.DragToken(this, betBox, state, previousState) == 2)
                        {
                            this.SetTokensFromToken(lastToken);
                            tokenList.Remove(lastToken);
                            betBox.SetTotalFromTokens();
                            this.SetMoneyFromTokens();
                        }
                    }
                }
                //Player confirms the bets (not empty and multiple of 20) by clicking on the placeBetsButton
                if (betBox.GetTotal() != 0 & betBox.GetTotal()%20 == 0)
                    placeBetsButton.isClicked(state, previousState);
            }
        }

        //DISPLAY ON SCREEN
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont info, SpriteFont tinyInfo, Sprite passButton, Sprite placeBetsButton, bool selfTurn)
        {
            //Player's hand
            if(this.GetHand().Count != 0)
                this.GetHand().ForEach(card => card.Draw(spriteBatch));
            //Player's score
            if (placeBetsButton.GetClicked())
                spriteBatch.DrawString(info, "Score : " + this.GetHandValue(), new Vector2(720, 560), Color.White);
            //Stand button
            if (selfTurn)
            {
                if (!placeBetsButton.GetClicked())
                {
                    placeBetsButton.Draw(spriteBatch);
                    spriteBatch.DrawString(tinyInfo, "Must be a multiple of 20", new Vector2(57, 290), Color.White);
                }
                else
                {
                    passButton.Draw(spriteBatch);
                    spriteBatch.DrawString(info, "Hit", new Vector2(1368, 100), Color.Black);
                }
            }

            //Player's tokens
            foreach(List<Token> tokenList in this.GetTokenLists())
            {
                foreach (Token token in tokenList)
                    token.Draw(spriteBatch);
            }
            //Player's money
            spriteBatch.DrawString(info, "Money : " + this.GetMoney(), new Vector2(340, 830), Color.White);
        }

        //Give cards to the player, place them in the player slot and remove them from the deck
        public virtual void DrawCards(int numberOfCards, Deck deck)
        {

            for (int i=0;i<numberOfCards;i++)
            {
                Card card = deck.GetCards()[0];
                var size = GetHand().Count();
                GetHand().Add(card);
                card.SetDestinationRectangle(new Rectangle(738 + (size * 40), 630, 125, 181));
                deck.GetCards().Remove(card);
            }
        }

        public void PassTurn(ref bool selfTurn, ref bool opponentTurn)
        {
            selfTurn = false;
            opponentTurn = true;
        }

        public void DiscardHand()
        {
            this.GetHand().Clear();
        }
        
    }
}
