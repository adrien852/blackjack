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
        public Rectangle TokenRectangle { get; }
        public List<List<Token>> TokenLists { get; set; }
        public double Money { get; set; }
        public List<Card> Hand { get; set; }

        //CONSTRUCTOR
        public Player(double money, Texture2D tokenTexture)
        {
            Money = money;
            Console.WriteLine(Money);
            Hand = new List<Card>();
            TokenLists = new List<List<Token>>();
            SetTokensFromMoney(tokenTexture);
            TokenRectangle = new Rectangle(-50, 630, 490, 320);
        }

        //Get the value from each token and set the total variable
        public void SetMoneyFromTokens()
        {
            double total = 0;
            foreach (List<Token> tokenList in TokenLists)
            {
                foreach (Token token in tokenList)
                {
                    if (tokenList.Count > 0)
                        total += token.Value;
                }
            }
            Money = total;
        }

        //Get the value from each card and return the lowest total hand value.
        public int GetHandValue()
        {
            int total = 0;
            Hand.ForEach(card => total += card.Value);
            foreach (Card card in Hand)
            {
                //If any ace in hand with a busted score, ace changes value
                if (card.Value == 11 & total > 21)
                    total -= 10;
            }
            return total;
        }

        //Convert money into tokens
        public void SetTokensFromMoney(Texture2D texture)
        {
            double total = Money;
            TokenLists = new List<List<Token>>();
            CreateTokenList(10, ref total, texture);
            CreateTokenList(20, ref total, texture);
            CreateTokenList(50, ref total, texture);
            CreateTokenList(1000, ref total, texture);
            CreateTokenList(500, ref total, texture);
            CreateTokenList(100, ref total, texture);
            while (total - 10 >= 0)
            {
                CreateTokenList(2, 50, ref total, texture);
                CreateTokenList(1, 20, ref total, texture);
                CreateTokenList(0, 10, ref total, texture);
            }
        }

        //Add a token in the proper token list
        public void SetTokensFromToken(Token token)
        {
            switch (token.Value)
            {
                case 10:
                    TokenLists[0].Add(new Token(10, token.Texture));
                    break;
                case 20:
                    TokenLists[1].Add(new Token(20, token.Texture));
                    break;
                case 50:
                    TokenLists[2].Add(new Token(50, token.Texture));
                    break;
                case 1000:
                    TokenLists[3].Add(new Token(1000, token.Texture));
                    break;
                case 500:
                    TokenLists[4].Add(new Token(500, token.Texture));
                    break;
                case 100:
                    TokenLists[5].Add(new Token(100, token.Texture));
                    break;
            }
        }

        //Give enough tokens to not bet too much at once.
        public void CreateTokenList(int value, ref double total, Texture2D texture)
        {
            TokenLists.Add(new List<Token>());
            int size = TokenLists.Count - 1;
            if (value > 50)
            {
                while (total > 3 * value)
                {
                    TokenLists[size].Add(new Token(value, texture));
                    total -= value;
                }
            }
            else
            {
                int amount = 0;
                while (total - value >= 0 & amount < 4)
                {
                    TokenLists[size].Add(new Token(value, texture));
                    total -= value;
                    ++amount;
                }
            }
        }
        public void CreateTokenList(int index, int value, ref double total, Texture2D texture)
        {
            while (total - value >= 0)
            {
                TokenLists[index].Add(new Token(value, texture));
                total -= value;
            }
        }

        //DEBUG
        public string ShowHandInOutput()
        {
            string display = "Hand : ";
            Hand.ForEach(card => display += card + " ");
            return display;
        }

        //GAMEPLAY
        public virtual void Update(GameTime gameTime, Deck deck, MouseState state, MouseState previousState, Sprite allinButton, Sprite passButton, Sprite placeBetsButton, Bet betBox, ref bool selfTurn, ref bool opponentTurn)
        {
            //Player draws cards by clicking on deck
            if (deck.IsClicked(state, previousState))
            {
                DrawCards(1, deck);
                Console.WriteLine(ShowHandInOutput());
            }
            //Player passes turn by tapping passButton or by having 6 cards or by having 21 or more
            if (passButton.IsClicked(state, previousState) || Hand.Count == 7 || GetHandValue() >= 21)
            {
                PassTurn(ref selfTurn, ref opponentTurn);
            }
            //Player can drag tokens around at the beggining of their turn
            if (!placeBetsButton.Clicked)
            {
                //If a player's token is dragged inside the bet box, it's added to the bet, removed from player's tokens and the totals are updated
                foreach (List<Token> tokenList in TokenLists)
                {
                    if (tokenList.Count > 0)
                    {
                        Token lastToken = tokenList[tokenList.Count - 1];
                        if (lastToken.DragToken(this, betBox, state, previousState) == 1)
                        {
                            betBox.SetTokens(lastToken);
                            tokenList.Remove(lastToken);
                            SetMoneyFromTokens();
                            betBox.SetTotalFromTokens();
                        }
                    }
                }
                //If a token from the bet is dragged to the player's tokens, it's added to the player's tokens, removed from bet and the totals are updated
                foreach (List<Token> tokenList in betBox.TokenLists)
                {
                    if (tokenList.Count > 0)
                    {
                        Token lastToken = tokenList[tokenList.Count - 1];
                        if (lastToken.DragToken(this, betBox, state, previousState) == 2)
                        {
                            SetTokensFromToken(lastToken);
                            tokenList.Remove(lastToken);
                            betBox.SetTotalFromTokens();
                            SetMoneyFromTokens();
                        }
                    }
                }
                //Player can bet all tokens by tapping Allin button
                if (allinButton.IsClicked(state, previousState))
                {
                    Allin(betBox);
                }
                //Player confirms the bets (not empty and multiple of 20) by clicking on the placeBetsButton
                if (betBox.Total != 0 & betBox.Total % 20 == 0)
                    placeBetsButton.IsClicked(state, previousState);
            }
        }

        //DISPLAY ON SCREEN
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont info, SpriteFont tinyInfo, Sprite allinButton, Sprite passButton, Sprite placeBetsButton, bool selfTurn)
        {
            //Player's hand
            if (Hand.Count != 0)
                Hand.ForEach(card => card.Draw(spriteBatch));
            //Player's score
            if (placeBetsButton.Clicked)
                spriteBatch.DrawString(info, "Score : " + GetHandValue(), new Vector2(720, 560), Color.White);
            //Stand and Allin buttons
            if (selfTurn)
            {
                if (!placeBetsButton.Clicked)
                {
                    allinButton.Draw(spriteBatch);
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
            foreach (List<Token> tokenList in TokenLists)
            {
                foreach (Token token in tokenList)
                    token.Draw(spriteBatch);
            }
            //Player's money
            spriteBatch.DrawString(info, "Money : " + Money, new Vector2(340, 830), Color.White);
        }

        //Give cards to the player, place them in the player slot and remove them from the deck
        public virtual void DrawCards(int numberOfCards, Deck deck)
        {

            for (int i = 0; i < numberOfCards; i++)
            {
                Card card = deck.Cards[0];
                var size = Hand.Count();
                Hand.Add(card);
                card.DestinationRectangle = new Rectangle(738 + (size * 40), 630, 125, 181);
                deck.Cards.Remove(card);
            }
        }

        public void PassTurn(ref bool selfTurn, ref bool opponentTurn)
        {
            selfTurn = false;
            opponentTurn = true;
        }

        public void DiscardHand()
        {
            Hand.Clear();
        }

        public void Allin(Bet betBox)
        {
            foreach (List<Token> tokenList in TokenLists)
            {
                while(tokenList.Count > 0)
                {
                    betBox.SetTokens(tokenList[0]);
                    tokenList.Remove(tokenList[0]);
                }
            }
            SetMoneyFromTokens();
            betBox.SetTotalFromTokens();
        }
    }
}