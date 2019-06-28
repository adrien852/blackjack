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
        public Rectangle TokenRectangle { get; set; }
        public List<List<Token>> TokenLists { get; set; }
        public double Money { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> StandbyHand { get; set; }
        public Texture2D tokenTexture { get; set; }
        public bool isHandSwitched;
        public bool isHandSplit;

        //CONSTRUCTOR
        public Player(double money, Texture2D tokenTexture)
        {
            Money = money;
            Console.WriteLine(Money);
            Hand = new List<Card>();
            StandbyHand = new List<Card>();
            TokenLists = new List<List<Token>>();
            this.tokenTexture = tokenTexture;
            SetTokensFromMoney();
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
        
        public int GetStandbyHandValue()
        {
            int total = 0;
            StandbyHand.ForEach(card => total += card.Value);
            foreach (Card card in StandbyHand)
            {
                //If any ace in hand with a busted score, ace changes value
                if (card.Value == 11 & total > 21)
                    total -= 10;
            }
            return total;
        }

        //Convert money into tokens
        public void SetTokensFromMoney()
        {
            double total = Money;
            TokenLists = new List<List<Token>>();
            CreateTokenList(10, ref total);
            CreateTokenList(20, ref total);
            CreateTokenList(50, ref total);
            CreateTokenList(1000, ref total);
            CreateTokenList(500, ref total);
            CreateTokenList(100, ref total);
            while (total - 10 >= 0)
            {
                CreateTokenList(50, ref total);
                CreateTokenList(20, ref total);
                CreateTokenList(10, ref total);
            }
        }

        //Add a token in the proper token list
        public virtual void SetTokensFromToken(Token token)
        {
            switch (token.Value)
            {
                case 10:
                    token.DestinationRectangle = new Rectangle(20, 700, 100, 99);
                    TokenLists[0].Add(token);
                    break;
                case 20:
                    token.DestinationRectangle = new Rectangle(120, 700, 100, 99);
                    TokenLists[1].Add(token);
                    break;
                case 50:
                    token.DestinationRectangle = new Rectangle(220, 700, 100, 99);
                    TokenLists[2].Add(token);
                    break;
                case 1000:
                    token.DestinationRectangle = new Rectangle(220, 800, 100, 99);
                    TokenLists[3].Add(token);
                    break;
                case 500:
                    token.DestinationRectangle = new Rectangle(120, 800, 100, 99);
                    TokenLists[4].Add(token);
                    break;
                case 100:
                    token.DestinationRectangle = new Rectangle(20, 800, 100, 99);
                    TokenLists[5].Add(token);
                    break;
            }
        }

        //Give enough tokens to not bet too much at once.
        public void CreateTokenList(int value, ref double total)
        {
            TokenLists.Add(new List<Token>());
            if (value > 50)
            {
                while (total > 3 * value)
                {
                    SetTokensFromToken(new Token(value, tokenTexture));
                    total -= value;
                }
            }
            else
            {
                int amount = 0;
                while (total - value >= 0 & amount < 4)
                {
                    SetTokensFromToken(new Token(value, tokenTexture));
                    total -= value;
                    ++amount;
                }
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
        public virtual void Update(GameTime gameTime, Deck deck, MouseState state, MouseState previousState, Sprite splitButton, Sprite doubleBetButton, Sprite allinButton, Sprite passButton, Sprite placeBetsButton, Bet betBox, ref bool selfTurn, ref bool opponentTurn)
        {
            //Player draws cards by clicking on deck
            if (placeBetsButton.Clicked & deck.IsClicked(state, previousState))
            {
                DrawCards(1, deck);
                Console.WriteLine(ShowHandInOutput());
            }
            //Player splits his hand by tapping the split button
            if (Money >= betBox.Total & splitButton.IsClicked(state, previousState))
                SplitHand(betBox);
            //Player passes turn (or switch hands if split) by tapping passButton or by having 6 cards or by having 21 or more
            if (passButton.IsClicked(state, previousState) || Hand.Count == 7 || GetHandValue() >= 21)
                PassTurn(ref selfTurn, ref opponentTurn);
            //Player double his bet and draw one last card by tapping the doubleBetButton
            if(Money >= betBox.Total & doubleBetButton.IsClicked(state, previousState))
                DoubleAndDraw(betBox, deck, ref selfTurn, ref opponentTurn);
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
                            betBox.SetTokensFromToken(lastToken);
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
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont info, SpriteFont tinyInfo, Sprite splitButton, Sprite doubleBetButton, Sprite allinButton, Sprite passButton, Sprite placeBetsButton, bool selfTurn)
        {
            //Player's hand
            if (Hand.Count != 0)
                Hand.ForEach(card => card.Draw(spriteBatch));
            //Player's standby hand
            if(StandbyHand.Count != 0)
                StandbyHand.ForEach(card => card.Draw(spriteBatch));
            //Player's score
            if (placeBetsButton.Clicked)
            {
                if (!isHandSwitched)
                    spriteBatch.DrawString(info, "Score : " + GetHandValue(), new Vector2(720, 560), Color.White);
                if (isHandSwitched)
                {
                    spriteBatch.DrawString(info, "Score : " + GetStandbyHandValue(), new Vector2(720, 560), Color.White);
                    spriteBatch.DrawString(info, "Score : " + GetHandValue(), new Vector2(972, 560), Color.White);
                }
            }
            //Buttons
            if (selfTurn)
            {
                //During the betting phase, only Allin and Bet buttons available
                if (!placeBetsButton.Clicked)
                {
                    allinButton.Draw(spriteBatch);
                    placeBetsButton.Draw(spriteBatch);

                    spriteBatch.DrawString(tinyInfo, "Must be a multiple of 20", new Vector2(57, 290), Color.White);
                }
                //During the playing phase, the deck is shown as clickable and Pass button available
                else
                {
                    passButton.Draw(spriteBatch);
                    //If 2 cards and not already split, can double bet
                    if (Hand.Count == 2 & !isHandSplit)
                    {
                        doubleBetButton.Draw(spriteBatch);
                        //And if both cards has same number, can split
                        if(Hand[0].Number == Hand[1].Number)
                            splitButton.Draw(spriteBatch);
                    }
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
                if(!isHandSwitched)
                    card.DestinationRectangle = new Rectangle(738 + (size * 40), 630, 125, 181);
                else
                    card.DestinationRectangle = new Rectangle(1000 + (size * 40), 630, 125, 181);
                deck.Cards.Remove(card);
            }
        }

        public void PassTurn(ref bool selfTurn, ref bool opponentTurn)
        {
            if (isHandSwitched)
            {
                selfTurn = false;
                opponentTurn = true;
                isHandSplit = false;
            }
            else if (isHandSplit)
            {
                SwitchHands();
                isHandSwitched = true;
            }
            else
            {
                selfTurn = false;
                opponentTurn = true;
            }
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
                    betBox.SetTokensFromToken(tokenList[0]);
                    tokenList.Remove(tokenList[0]);
                }
            }
            SetMoneyFromTokens();
            betBox.SetTotalFromTokens();
        }

        public void DoubleAndDraw(Bet betBox, Deck deck, ref bool selfTurn, ref bool opponentTurn)
        {
            DrawCards(1,deck);
            Money -= betBox.Total;
            SetTokensFromMoney();
            betBox.Total *= 2;
            PassTurn(ref selfTurn, ref opponentTurn);
        }

        public void SwitchHands()
        {
            List<Card> tempHand = Hand;
            Hand = StandbyHand;
            StandbyHand = tempHand;

        }

        public void SplitHand(Bet betBox)
        {
            Hand[1].DestinationRectangle = new Rectangle(1000, 630, 125, 181);
            StandbyHand.Add(Hand[1]);
            Hand.Remove(Hand[1]);
            isHandSplit = true;
            Money -= betBox.Total;
            SetTokensFromMoney();
            betBox.Total *= 2;
        }
    }
}