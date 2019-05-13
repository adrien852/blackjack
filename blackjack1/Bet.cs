using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blackjack1
{
    class Bet
    {
        //VARIABLES
        private Rectangle destinationRectangle;
        private List<List<Token>> tokenLists;
        private int total;

        //CONSTRUCTOR
        public Bet()
        {
            this.destinationRectangle = new Rectangle(0, 240, 440, 420);
            this.tokenLists = new List<List<Token>>();
            for (int i = 0; i < 6; ++i)
                tokenLists.Add(new List<Token>());
        }

        //GETTERS SETTERS
        public Rectangle GetDestinationRectangle()
        {
            return destinationRectangle;
        }
        public List<List<Token>> GetTokenLists()
        {
            return tokenLists;
        }
        //Add a token in the proper token list
        public void SetTokens(Token token)
        {
            switch (token.GetValue())
            {
                case 1000:
                    token.SetDestinationRectangle(new Rectangle(250, 450, 100, 99));
                    this.tokenLists[0].Add(token);
                    break;
                case 500:
                    token.SetDestinationRectangle(new Rectangle(150, 450, 100, 99));
                    this.tokenLists[1].Add(token);
                    break;
                case 100:
                    token.SetDestinationRectangle(new Rectangle(50, 450, 100, 99));
                    this.tokenLists[2].Add(token);
                    break;
                case 50:
                    token.SetDestinationRectangle(new Rectangle(250, 350, 100, 99));
                    this.tokenLists[3].Add(token);
                    break;
                case 20:
                    token.SetDestinationRectangle(new Rectangle(150, 350, 100, 99));
                    this.tokenLists[4].Add(token);
                    break;
                case 10:
                    token.SetDestinationRectangle(new Rectangle(50, 350, 100, 99));
                    this.tokenLists[5].Add(token);
                    break;
            }
        }

        //Get the value from each token and set the total variable
        public void SetTotalFromTokens()
        {
            int total = 0;
            foreach (List<Token> tokenList in this.GetTokenLists())
            {
                foreach (Token token in tokenList)
                {
                    if (tokenList.Count > 0)
                        total += token.GetValue();
                }
            }
            this.total = total;
        }
        public int GetTotal()
        {
            return total;
        }

        //DISPLAY ON SCREEN
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont score)
        {
            //Bet's tokens
            foreach (List<Token> tokenList in this.GetTokenLists())
            {
                foreach (Token token in tokenList)
                    token.Draw(spriteBatch);
            }

            //Bet's total
            if(total != 0)
                spriteBatch.DrawString(score, "Bet : " + this.GetTotal(), new Vector2(130, 600), Color.White);
        }
    }
}
