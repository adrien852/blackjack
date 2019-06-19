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
        public Rectangle DestinationRectangle { get; }
        public List<List<Token>> TokenLists { get; }
        public int Total { get; set; }

        //CONSTRUCTOR
        public Bet()
        {
            DestinationRectangle = new Rectangle(0, 240, 440, 420);
            TokenLists = new List<List<Token>>();
            for (int i = 0; i < 6; ++i)
                TokenLists.Add(new List<Token>());
        }
        
        //Add a token in the proper token list
        public void SetTokensFromToken(Token token)
        {
            switch (token.Value)
            {
                case 1000:
                    token.DestinationRectangle = new Rectangle(250, 450, 100, 99);
                    TokenLists[0].Add(token);
                    break;
                case 500:
                    token.DestinationRectangle = new Rectangle(150, 450, 100, 99);
                    TokenLists[1].Add(token);
                    break;
                case 100:
                    token.DestinationRectangle = new Rectangle(50, 450, 100, 99);
                    TokenLists[2].Add(token);
                    break;
                case 50:
                    token.DestinationRectangle = new Rectangle(250, 350, 100, 99);
                    TokenLists[3].Add(token);
                    break;
                case 20:
                    token.DestinationRectangle = new Rectangle(150, 350, 100, 99);
                    TokenLists[4].Add(token);
                    break;
                case 10:
                    token.DestinationRectangle = new Rectangle(50, 350, 100, 99);
                    TokenLists[5].Add(token);
                    break;
            }
        }

        //Get the value from each token and set the total variable
        public void SetTotalFromTokens()
        {
            int total = 0;
            foreach (List<Token> tokenList in TokenLists)
            {
                foreach (Token token in tokenList)
                {
                    if (tokenList.Count > 0)
                        total += token.Value;
                }
            }
            Total = total;
        }

        //Get total and recreate tokens


        //DISPLAY ON SCREEN
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont score)
        {
            //Bet's tokens
            foreach (List<Token> tokenList in TokenLists)
            {
                foreach (Token token in tokenList)
                    token.Draw(spriteBatch);
            }

            //Bet's total
            if(Total != 0)
                spriteBatch.DrawString(score, "Bet : " + Total, new Vector2(130, 600), Color.White);
        }
    }
}
