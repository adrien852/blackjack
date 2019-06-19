using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace blackjack1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState previousState;
        TimeSpan lastAction;
        Texture2D background;
        Texture2D cards;
        Texture2D token;
        Texture2D stand;
        Texture2D allin;
        Texture2D placeBets;
        Texture2D doubleBet;
        SpriteFont info;
        SpriteFont tinyInfo;
        SpriteFont bigInfo;
        Animation animation;
        Animation previousAnimation;
        Deck mainDeck;
        Sprite allinButton;
        Sprite standButton;
        Sprite placeBetsButton;
        Sprite doubleBetButton;
        Bet betBox;
        Player player1;
        Dealer dealer1;
        bool AITurn;
        bool playerTurn;
        bool firstTurn;
        bool startDealerTurn;
        bool firstCards;
        bool firstDealerAction;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            AITurn = false;
            playerTurn = false;
            firstTurn = true;
            firstCards = false;
            IsMouseVisible = true;
            firstDealerAction = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            previousState = Mouse.GetState();
            background = Content.Load<Texture2D>("bg"); //Load background file
            cards = Content.Load<Texture2D>("card_set"); //Load card set file
            token = Content.Load<Texture2D>("tokens");
            stand = Content.Load<Texture2D>("stand");
            allin = Content.Load<Texture2D>("allin");
            placeBets = Content.Load<Texture2D>("bet");
            info = Content.Load<SpriteFont>("Score");
            tinyInfo = Content.Load<SpriteFont>("tinyInfo");
            bigInfo = Content.Load<SpriteFont>("bigInfo");
            doubleBet = Content.Load<Texture2D>("doubleBet");
            allinButton = new Sprite(allin, new Rectangle(330, 630, 170, 87), new Rectangle(0, 0, 170, 87));
            standButton = new Sprite(stand, new Rectangle(1315, 200, 170, 87), new Rectangle(0, 0, 170, 87));
            doubleBetButton = new Sprite(doubleBet, new Rectangle(1315, 280, 170, 87), new Rectangle(0, 0, 170, 87));
            placeBetsButton = new Sprite(placeBets, new Rectangle(330, 730, 170, 87), new Rectangle(0, 0, 170, 87));
            animation = new Animation();
            betBox = new Bet();
            player1 = new Player(2000, token);
            dealer1 = new Dealer(5000, token);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Exit game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //If not enough money to play, can't play
            if (player1.Money + betBox.Total != 0 && player1.Money + betBox.Total != 10)
            {
                MouseState state = Mouse.GetState();
                //Player's turn
                if (playerTurn)
                {
                    player1.Update(gameTime, mainDeck, state, previousState, doubleBetButton, allinButton, standButton, placeBetsButton, betBox, ref playerTurn, ref AITurn);
                    if (placeBetsButton.Clicked & !firstCards)
                    {
                        player1.DrawCards(2, mainDeck);
                        dealer1.DrawCards(2, mainDeck);
                        dealer1.Hand[dealer1.Hand.Count - 1].FlipCard();
                        firstCards = true;
                    }
                }
                //Dealer's turn (AI)
                else if (AITurn)
                {
                    //First action is delayed
                    if (firstDealerAction & player1.GetHandValue() > 21)
                    {
                        lastAction = gameTime.TotalGameTime;
                        firstDealerAction = false;
                    }
                    //One second between each action
                    if (lastAction + TimeSpan.FromMilliseconds(1500) < gameTime.TotalGameTime)
                    {
                        dealer1.Update(gameTime, player1, mainDeck, placeBetsButton, ref AITurn, ref startDealerTurn);
                        lastAction = gameTime.TotalGameTime;
                    }
                }
                //Loop back for a new turn
                else
                {
                    //Compare hands, share bets, discard hands, reset deck, shuffle deck, draw new cards and reset settings
                    //If first turn, no winner and no hands to discard
                    if (!firstTurn)
                    {
                        int playerScore = player1.GetHandValue();
                        int dealerScore = dealer1.GetHandValue();
                        ShareBets(Winner(playerScore, dealerScore));
                        player1.SetTokensFromMoney();
                        dealer1.SetTokensFromMoney();
                        player1.DiscardHand();
                        dealer1.DiscardHand();
                    }
                    mainDeck = new Deck(52, cards);
                    betBox = new Bet();
                    dealer1.Shuffle(mainDeck);
                    playerTurn = true;
                    AITurn = false;
                    firstTurn = false;
                    startDealerTurn = true;
                    firstDealerAction = true;
                    firstCards = false;
                    placeBetsButton.Clicked = false;
                }
                previousState = state;
                previousAnimation = animation;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            spriteBatch.Begin();
            spriteBatch.Draw(background, destinationRectangle: new Rectangle(0, 0, 1600, 900), color: Color.White); //Background
            //If not enough money left, "YOU LOOSE" message
            if (player1.Money + betBox.Total <= 10)
                spriteBatch.DrawString(bigInfo, "YOU LOOSE !", new Vector2(600, 560), Color.Red);
            //If dealer has no money left, "YOU WIN" message
            else if (dealer1.Money <= 0)
                spriteBatch.DrawString(bigInfo, "YOU WIN !", new Vector2(600, 560), Color.Red);
            else
            {
                mainDeck.Draw(spriteBatch); //Deck
                betBox.Draw(spriteBatch, info); //Bet box
                player1.Draw(gameTime, spriteBatch, info, tinyInfo, doubleBetButton, allinButton, standButton, placeBetsButton, playerTurn); //Player drawing
                dealer1.Draw(gameTime, spriteBatch, animation, info, AITurn, startDealerTurn); //Dealer drawing
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //Return 0 if draw, return 1 if player wins, return 2 if dealer wins, return 3 if player wins by blackjack
        protected int Winner(int playerScore, int dealerScore)
        {
            if (playerScore <= 21)
            {
                if (playerScore == 21 & player1.Hand.Count == 2)
                {
                    //If blackjack for player but not for dealer
                    if (!(dealerScore == 21 & dealer1.Hand.Count == 2))
                        return 3;
                    //If blackjacks on both sides
                    if (dealerScore == 21 & dealer1.Hand.Count == 2)
                        return 0;
                }
                //If blackjack for dealer but not for player
                else if (dealerScore == 21 & dealer1.Hand.Count == 2)
                    return 2;
                //If player better than dealer and did not bust
                else if (playerScore > dealerScore)
                    return 1;

                else if (playerScore < dealerScore)
                {
                    //If dealer better than player and did not bust
                    if (dealerScore <= 21)
                        return 2;
                    //If dealer busted but not player
                    else
                        return 1;
                }
                //If same score
                else if (playerScore == dealerScore)
                    return 0;
            }
            //If player busted but not dealer
            else if (dealerScore <= 21)
                return 2;
            //If both busted
            return 2;
        }

        protected void ShareBets(int result)
        {
            switch (result)
            {
                //If draw, get back the bet
                case 0:
                    player1.Money += betBox.Total;
                    break;
                //If win, get twice the bet
                case 1:
                    player1.Money += betBox.Total * 2;
                    dealer1.Money -= betBox.Total;
                    break;
                //If lose, get nothing
                case 2:
                    dealer1.Money += betBox.Total;
                    break;
                //If win by blackjack, get two-and-a-half times the bet
                case 3:
                    player1.Money += betBox.Total * 2.5;
                    dealer1.Money -= betBox.Total * 1.5;
                    break;
                default:
                    break;
            }
        }
    }
}
