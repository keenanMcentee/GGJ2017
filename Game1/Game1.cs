using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
        Texture2D limboBg1, limboBg2;
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
        List<Enemy> enemies;
        SpriteFont font;
        SoundEffect bgMusic;
        SoundEffectInstance bgMusicInst;
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
            enemies = new List<Enemy>();
            superDarkGray = new Color(64, 64, 64);
            // TODO: Add your initialization logic here
            player = new Player();
            mapOne = new Map();
            camera = new Camera(GraphicsDevice.Viewport);
            Tiles.Content = Content;
            currentState = GameState.Play;
            bgMusic = Content.Load<SoundEffect>("bgMusic");
            bgMusicInst = bgMusic.CreateInstance();
            //bgMusicInst.Volume = 0.5f;
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
            limboBg1 = Content.Load<Texture2D>("limboBg");

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
            {
                bgMusicInst.Stop();
                Initialize();

            }
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            if (currentState == GameState.Play)
            {
                foreach (Enemy a in enemies)
                {
                    a.Update();
                    foreach (CollisionTiles tile in mapOne.CollisionTiles)
                    {
                        a.Collision(tile.Rectangle, mapOne.Width, mapOne.Height);
                    }
                    player.CollisionEnemy(a, rnd);
                }
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
                
                foreach (Enemy a in enemies)
                {
                    a.Draw(spriteBatch);
                }


                spriteBatch.End();
                
            }
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        public void LoadLevelOne()
        {
            enemies = new List<Enemy>();
            //movingBlocks = new List<MovingBlock>();
            mapOne = new Map();
            Tiles.Content = Content;
            camera = new Camera(GraphicsDevice.Viewport);
            player = new Player();
            mapOne.Generate(new int[,] {
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
{ 0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,1},
{ 0,0,0,0,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,1},
{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1},
{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1},
{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1},
{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1},
{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1},
{ 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,0,0,0,11},
{2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,0,0,0,5},
{ 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,0,0,0,5},
{ 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,0,0,0,5},
{ 2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,4,0,0,0,11},
{ 4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,11},
{ 4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,11},
{ 4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,11},
{ 4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11},
{ 4,0,0,0,0,0,0,2,2,2,0,0,0,0,0,0,2,2,0,0,0,0,0,0,11},
{ 4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11},
{ 4,0,0,0,7,8,0,0,0,0,0,7,2,2,8,0,0,1001,0,7,8,0,0,0,11},
{ 4,0,0,0,2,2,6,6,6,6,6,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
{ 4,0,0,0,11,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
{ 4,0,0,0,5,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
{ 4,0,0,0,15,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12},
{ 14,0,0,0,15,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12},
{ 14,0,0,0,15,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12},
{ 14,0,0,0,0,20,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12},
{ 14,0,0,0,0,0,20,13,12,12,12,12,12,13,13,13,13,0,13,13,0,13,13,12,12},
{ 14,0,0,0,0,0,0,0,20,13,13,13,13,0,0,0,0,0,0,0,0,0,0,12,12},
{14,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15},
{14,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15},
{14,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15},
{14,0,0,0,0,17,18,0,0,0,0,17,18,0,0,0,0,17,18,0,0,0,0,0,15},
{14,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15},
{14,0,0,12,0,1003,0,0,12,12,0,0,1003,0,12,12,0,1003,0,0,12,0,0,0,15},
{12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,16,16,16,16,12,0,0,0,15},
{12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,0,0,0,15},
{12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,12,24,0,0,0,15},
{22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,24,0,0,0,31},
{22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,24,0,0,0,25},
{22,22,22,22,22,22,22,22,22,23,23,23,22,22,22,22,22,22,22,22,22,0,0,0,25},
{22,23,23,23,23,23,23,23,23,0,0,0,23,23,23,23,23,23,23,23,23,0,0,0,25},
{22,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,25},
{22,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,25},
{22,0,0,0,0,0,0,0,0,0,22,0,0,0,0,0,0,0,0,0,0,0,0,0,25},
{22,0,0,0,0,0,0,0,0,0,22,0,0,22,32,32,22,0,0,0,0,0,0,0,25},
{22,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,25},
{22,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,27,32,28,0,0,0,0,25},
{22,0,0,0,1000,0,22,22,0,0,0,0,0,0,0,0,27,22,22,22,28,0,0,0,25},
{22,0,0,0,22,22,22,22,32,32,32,32,32,32,32,32,22,22,22,22,22,22,22,22,22},
{22,0,0,0,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22,22},
{41,0,0,0,35,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43},
{41,0,0,0,35,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43},
{41,0,0,0,35,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43},
{41,0,0,0,35,43,43,43,43,43,0,0,0,0,0,0,0,0,43,43,43,43,43,43,43},
{41,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,43},
{41,0,0,0,0,0,0,0,0,0,0,0,43,43,43,43,0,0,0,0,0,0,0,0,43},
{41,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,43},
{41,0,0,0,0,0,0,0,0,0,43,0,0,0,0,0,0,43,43,43,0,0,0,0,43},
{41,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,35},
{41,0,0,0,43,43,43,0,0,0,0,0,43,43,43,0,0,0,0,0,0,0,0,0,35},
{41,0,0,0,0,0,0,43,0,1005,0,43,0,0,0,0,0,0,0,43,43,0,0,0,35},
{41,0,0,0,0,0,0,43,43,43,43,43,0,0,0,0,0,0,0,43,43,0,0,0,35},
{43,43,43,43,42,42,42,42,42,42,42,42,42,42,42,42,42,42,42,43,43,0,0,0,35},
{43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,0,0,0,52},
{43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,43,0,0,0,52},
{54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,0,0,0,52},
{54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,0,0,0,52},
{54,54,54,44,44,44,44,44,44,44,44,44,44,44,44,44,44,44,44,54,50,0,0,0,52},
{54,54,54,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,50,0,0,0,0,46},
{54,54,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,52},
{45,0,0,0,0,53,0,0,1007,0,0,53,0,0,1007,0,0,53,0,0,1007,0,0,0,46},
{45,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,52},
{45,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,46},
{45,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,54},
{45,0,0,0,0,53,0,0,0,0,53,53,1007,0,1007,0,0,53,0,0,0,54,54,54,54},
{45,0,0,0,46,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54},
{54,0,0,0,46,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54,54},
{54,0,0,0,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55},
{57,0,0,0,58,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55},
{57,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,58},
{57,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,58},
{57,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,58},
{57,0,0,0,0,0,0,0,0,0,0,0,60,0,0,0,0,0,0,0,0,61,0,0,58},
{57,0,0,0,0,0,0,0,0,0,0,0,0,63,55,55,55,55,55,55,62,0,0,0,58},
{57,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,58},
{57,0,0,0,0,0,0,0,0,0,60,0,0,0,0,0,0,0,0,0,0,0,0,0,58},
{57,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,58},
{57,0,0,61,0,0,0,60,0,0,0,1009,0,0,1009,0,0,1009,0,0,0,0,0,0,58},
{55,55,55,55,55,55,55,55,59,65,65,65,59,59,59,65,59,59,59,55,57,0,0,0,58},
{55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,57,0,0,0,58},
{55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,55,57,0,0,0,58},
{66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,68,0,0,0,75},
{66,67,67,67,67,67,67,67,67,67,67,67,66,67,67,67,67,67,67,67,73,0,0,0,69},
{66,0,0,0,0,0,0,0,0,0,0,0,66,0,0,0,0,0,0,0,66,0,0,0,69},
{66,0,0,0,0,66,0,66,0,0,0,0,66,0,0,0,0,0,0,0,66,0,0,0,75},
{66,0,0,0,0,66,66,66,0,66,66,66,66,0,0,66,66,0,0,0,66,0,0,0,75},
{66,0,0,0,0,66,66,66,0,0,0,0,66,0,0,66,0,0,0,0,66,0,0,0,75},
{66,0,0,66,66,66,66,66,0,0,0,0,66,0,0,66,0,0,66,66,66,0,0,0,75},
{66,0,0,0,0,66,66,66,66,0,0,0,66,0,0,66,0,0,66,0,0,0,0,0,69},
{66,66,0,0,0,66,66,66,66,66,0,0,0,0,0,66,0,66,66,0,0,0,0,0,69},
{66,66,66,0,0,66,66,0,0,0,66,0,0,0,0,66,0,0,0,0,0,0,0,0,69},
{66,0,0,0,0,66,66,0,0,0,0,0,0,0,0,66,0,0,0,0,66,0,0,0,75},
{66,0,0,0,0,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66},
{76,0,0,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66},
{76,0,0,0,76,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66,66},
{85,0,0,0,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76},
{85,0,0,0,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76},
{85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,79},
{85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,79},
{85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,79},
{85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,79},
{85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,79},
{85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,79},
{85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,79},
{85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,79},
{85,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,79},
{76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76,76}



            }, 64);
            GenerateLevel();
        }
        /// <summary>
        /// Method for generating in the level content.
        /// </summary>
        public void GenerateLevel()
        {


            bgMusicInst.Play();
            player.Load(Content, mapOne.MapArray);
            for (int x = 0; x < mapOne.MapArray.GetLength(1); x++)
            {
                for (int y = 0; y < mapOne.MapArray.GetLength(0); y++)
                {
                    if (mapOne.MapArray[y, x] == 1001)
                    {
                        enemies.Add(new Enemy(new Vector2(x, y), 64, 1001) );
                    }
                    else if (mapOne.MapArray[y, x] == 1003)
                        {
                            enemies.Add(new Enemy(new Vector2(x, y), 64, 1003));
                        }
                    else if (mapOne.MapArray[y, x] == 1005)
                    {
                        enemies.Add(new Enemy(new Vector2(x, y), 64, 1005));
                    }
                    else if (mapOne.MapArray[y, x] == 1007)
                    {
                        enemies.Add(new Enemy(new Vector2(x, y), 64, 1007));
                    }
                    else if (mapOne.MapArray[y, x] == 1009)
                    {
                        enemies.Add(new Enemy(new Vector2(x, y), 64, 1009));
                    }
                    //else if (mapOne.MapArray[y, x] == 1003)
                    //{
                    //    enemies.Add(new Enemy(new Vector2(x, y), 64, 1003));
                    //}
                    //else if (mapOne.MapArray[y, x] == 6)
                    //{
                    //    //movingBlocks.Add(new MovingBlock(new Vector2(x, y), Content));
                    //}
                }
            }
            foreach (Enemy a in enemies)
            {
                a.Load(Content, rnd);
            }
        }
    }
}
