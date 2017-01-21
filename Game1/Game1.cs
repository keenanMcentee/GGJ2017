using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D whiteSq;
        Map mapOne;
        Random rnd = new Random();
        enum GameState { Play, Start, GameOver, Exit };
        GameState currentState = GameState.Start;
        MouseState mouse;
        MouseState pastMouse;
        KeyboardState keyboard;
        KeyboardState oldKeyboard;
        Camera camera;
        const int windowWidth = 800;
        const int windowHeight = 600;
        Button[] m_buttons;
        Color superDarkGray;

        SpriteFont font;
        Player player;
        int level = 1;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            superDarkGray = new Color(25, 25, 25);
            // TODO: Add your initialization logic here
            player = new Player();
            mapOne = new Map();
            camera = new Camera(GraphicsDevice.Viewport);
            Tiles.Content = Content;
            currentState = GameState.Play;
            if (level == 1)
            {
                LoadLevelOne();
            }
            IsMouseVisible = true;
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
            font = Content.Load<SpriteFont>("font");
            // TODO: use this.Content to load your game content here
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
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            if (currentState == GameState.Play)
            {
                player.Update(gameTime);
                foreach (CollisionTiles tile in mapOne.CollisionTiles)
                {
                    player.CollisionWorld(tile.Rectangle, mapOne.Width, mapOne.Height, mapOne.MapArray);

                }
            }
            // TODO: Add your update logic here
            oldKeyboard = Keyboard.GetState();
            pastMouse = Mouse.GetState();
            camera.Update(player.Position, mapOne.Width, mapOne.Height);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(superDarkGray);
            
            if (currentState == GameState.Play)
            {
                

                
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform);

                mapOne.Draw(spriteBatch);
                player.Draw(spriteBatch, graphics);

                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Limbo", new Vector2(100, 100), Color.White);
                spriteBatch.End();
            }
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        public void LoadLevelOne()
        {
            //enemySquares = new List<EnemySq>();
            //movingBlocks = new List<MovingBlock>();
            mapOne = new Map();
            Tiles.Content = Content;
            camera = new Camera(GraphicsDevice.Viewport);
            player = new Player();
            mapOne.Generate(new int[,] {    
                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            { 0, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            { 1, 0, 0, 1, 1, 0, 1, 0, 1, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 , 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1},
                                            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 , 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},

            }, 64);
            GenerateLevel();
        }
        /// <summary>
        /// Method for generating in the level content.
        /// </summary>
        public void GenerateLevel()
        {
            player.Load(Content, mapOne.MapArray);
            for (int x = 0; x < mapOne.MapArray.GetLength(1); x++)
            {
                for (int y = 0; y < mapOne.MapArray.GetLength(0); y++)
                {
                    if (mapOne.MapArray[y, x] == 8)
                    {
                        //enemySquares.Add(new EnemySq(new Vector2(x, y), 16));
                    }
                    else if (mapOne.MapArray[y, x] == 6)
                    {
                        //movingBlocks.Add(new MovingBlock(new Vector2(x, y), Content));
                    }
                }
            }
            /*foreach (EnemySq a in enemySquares)
            {
                a.Load(Content, rnd);
            }*/
        }
    }
}
