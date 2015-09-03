using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Robotica.Entity;
using Robotica.Entity.Component;

namespace Robotica
{
    public enum GameState
    {
        MENU,
        SELECTION,
        PLAYING,
        OPTIONS,
        CHOOSING_PLAYER
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont font;
        private float scale = 1.0f;
        private bool direction = false;
        private Color currentColor = Color.White;
        private Color backgroundColor = Color.Black;
        private Random rand = new Random();
        private int splash = 0;
        private List<string> SplashStrings = new List<string>();
        public static Texture2D GUISprites;
        public static Texture2D PlayerSprites;
        public static Texture2D RoomBack;
        public static Texture2D Shadow;
        public static Texture2D Lock;
        public static Texture2D Missing;
        public static Texture2D Weapon;
        public GameState gameState = GameState.MENU;
        public AudioEngine Audio;
        public SoundBank SoundEffectsBank;
        public WaveBank WaveEffectsBank;
        public Dictionary<Keys, bool> DebounceList = new Dictionary<Keys, bool>();
        public int PlayerSprite = 0;
        public EntityPlayer Player;
        public List<EntityBase> Entities; 
        public static Random Random = new Random();
        public List<string> Options = new List<string>
        {
            "NEW GAME",
            "OPTIONS",
            "EXIT"
        };

        public Dictionary<string, bool> Characters = new Dictionary<string,bool>
        {
            {"GREEN", true},
            {"TEAL", false},
            {"BLUE", false},
            {"PURPLE", false},
            {"RED", false},
            {"ORANGE", false},
            {"YELLOW", false},
            {"GREY", false}
        }; 
        public int Selection = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 896;
            graphics.PreferredBackBufferHeight = 640;
            graphics.ApplyChanges();
            Entities = new List<EntityBase>();
            Content.RootDirectory = "Content";
            RegisterDebounce(Keys.Up, true);
            RegisterDebounce(Keys.Down, true);
            RegisterDebounce(Keys.Left, true);
            RegisterDebounce(Keys.Right, true);
            RegisterDebounce(Keys.Escape, true);
            RegisterDebounce(Keys.Enter, true);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        public void RegisterDebounce(Keys key, bool firstState = false)
        {
            if (DebounceList.ContainsKey(key)) return;
            DebounceList.Add(key, firstState);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Main");
            GUISprites = Content.Load<Texture2D>("robotica_sprites_new");
            PlayerSprites = Content.Load<Texture2D>("robotica_player_new_bright");
            Shadow = Content.Load<Texture2D>("robotica_shadow");
            RoomBack = Content.Load<Texture2D>("robotica_green");
            Lock = Content.Load<Texture2D>("lock");
            Missing = Content.Load<Texture2D>("missing");
            Weapon = Content.Load<Texture2D>("robotica_weapon");
            Audio = new AudioEngine("Content\\Robotica.xgs");
            SoundEffectsBank = new SoundBank(Audio, "Content\\Sound Bank.xsb");
            WaveEffectsBank = new WaveBank(Audio, "Content\\Wave Bank.xwb");
            for (var i = 0; i < 16; i++)
            {
                Entities.Add(new EntityWeapon(new Vector2(128 + i % 4 * 64, 128 + (i / 4) * 64)));
            }
            try
            {
                var reader = new FileStream("./Config/splash.txt", FileMode.Open);
                var sb = new StringBuilder();
                var c = '\0';
                while (reader.Position < reader.Length)
                {
                    while ((c = (char)reader.ReadByte()) != '\n' && reader.Position != reader.Length)
                    {
                        sb.Append(c);
                    }
                    SplashStrings.Add(sb.ToString().Trim());
                    sb.Clear();
                }
                splash = rand.Next(SplashStrings.Count);
            }
            catch (Exception)
            {
                SplashStrings.Add("YOU REALLY SHOULD HAVE THAT FILE!");
            }
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            var keyboard = Keyboard.GetState();
            // TODO: Add your update logic here
            switch (gameState)
            {
                case GameState.PLAYING:
                {
                    //Ugly little hack to keep animations uniform
                    HoverComponent.UpdateAnimation();
                    Player.Update(gameTime);
                    foreach (var e in Entities)
                    {
                        e.Update(gameTime);
                    }
                    break;
                }
                case GameState.MENU:
                {
                    if (keyboard.GetPressedKeys().Length > 0)
                    {
                        if (keyboard.IsKeyDown(Keys.Escape)) return;
                        gameState = GameState.SELECTION;
                        SoundEffectsBank.PlayCue("Selection");
                    }
                    break;
                }
                case GameState.CHOOSING_PLAYER:
                {
                    if (keyboard.IsKeyDown(Keys.Left) && !DebounceList[Keys.Left])
                    {
                        DebounceList[Keys.Left] = true;
                        PlayerSprite = (PlayerSprite + 1 >= Characters.Keys.Count ? 0 : PlayerSprite + 1);
                        SoundEffectsBank.PlayCue("Selection");
                    }
                    if (keyboard.IsKeyDown(Keys.Right) && !DebounceList[Keys.Right])
                    {
                        DebounceList[Keys.Right] = true;
                        PlayerSprite = (PlayerSprite - 1 < 0 ? Characters.Keys.Count - 1 : PlayerSprite - 1);
                        SoundEffectsBank.PlayCue("Selection");
                    }
                    if (keyboard.IsKeyDown(Keys.Enter) && !DebounceList[Keys.Enter])
                    {
                        DebounceList[Keys.Enter] = true;
                        gameState = GameState.PLAYING;
                        Player = new EntityPlayer(PlayerSprites, PlayerSprite, Shadow, 57);
                    }
                    if (keyboard.IsKeyDown(Keys.Escape) && !DebounceList[Keys.Escape])
                    {
                        DebounceList[Keys.Escape] = true;
                        gameState = GameState.SELECTION;
                        SoundEffectsBank.PlayCue("AntiSelection");
                    }
                    if (DebounceList[Keys.Left] && keyboard.IsKeyUp(Keys.Left)) DebounceList[Keys.Left] = false;
                    if (DebounceList[Keys.Right] && keyboard.IsKeyUp(Keys.Right)) DebounceList[Keys.Right] = false;
                    if (DebounceList[Keys.Enter] && keyboard.IsKeyUp(Keys.Enter)) DebounceList[Keys.Enter] = false;
                    if (DebounceList[Keys.Escape] && keyboard.IsKeyUp(Keys.Escape)) DebounceList[Keys.Escape] = false;
                    break;
                }
                case GameState.SELECTION:
                {
                    if (keyboard.IsKeyDown(Keys.Up) && ! DebounceList[Keys.Up])
                    {
                        DebounceList[Keys.Up] = true;
                        Selection = (Selection - 1 < 0 ? Options.Count - 1 : Selection - 1);
                        SoundEffectsBank.PlayCue("Selection");
                    }
                    if (keyboard.IsKeyDown(Keys.Down) && !DebounceList[Keys.Down])
                    {
                        DebounceList[Keys.Down] = true;
                        Selection = (Selection + 1 >= Options.Count ? 0 : Selection + 1);
                        SoundEffectsBank.PlayCue("Selection");
                    }
                    if (keyboard.IsKeyDown(Keys.Enter) && !DebounceList[Keys.Enter])
                    {
                        DebounceList[Keys.Enter] = true;
                        switch(Selection)
                        {
                            case 0:
                            {
                                currentColor = Color.White;
                                backgroundColor = Color.Black;
                                gameState = GameState.CHOOSING_PLAYER;
                                break;
                            }
                            case 1:
                            {
                                gameState = GameState.OPTIONS;
                                break;
                            }
                            case 2:
                            {
                                Exit();
                                break;
                            }
                        }
                    }
                    if (DebounceList[Keys.Up] && keyboard.IsKeyUp(Keys.Up)) DebounceList[Keys.Up] = false;
                    if (DebounceList[Keys.Down] && keyboard.IsKeyUp(Keys.Down)) DebounceList[Keys.Down] = false;
                    if (DebounceList[Keys.Enter] && keyboard.IsKeyUp(Keys.Enter)) DebounceList[Keys.Enter] = false;
                    break;
                }
            }

            base.Update(gameTime);
        }

        public void RenderFrivolity()
        {
            if (!direction) scale += 0.0125f;
            else scale -= 0.0125f;
            if (scale <= 0.5f)
            {
                direction = false;
                currentColor = new Color(rand.Next(255), rand.Next(255), rand.Next(255));
                backgroundColor = new Color(255 - currentColor.R, 255 - currentColor.G, 255 - currentColor.B);
            }
            if (scale >= 1.0f) direction = true;
            // TODO: Add your drawing code here
            spriteBatch.DrawString(font, "ROBOTICA", new Vector2(Window.ClientBounds.Width / 2, 100), currentColor,
                0.0f, font.MeasureString("ROBOTICA") / 2, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, SplashStrings[splash], new Vector2(Window.ClientBounds.Width / 2, 150), currentColor,
                0.0f, font.MeasureString(SplashStrings[splash]) / 2, 0.5f, SpriteEffects.None, 0);
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.Default, null);
            switch (gameState)
            {
                case GameState.MENU:
                {
                    RenderFrivolity();
                    spriteBatch.DrawString(font, "PRESS ANY KEY TO CONTINUE", new Vector2(Window.ClientBounds.Width / 2, 400), currentColor, 0.0f, font.MeasureString("PRESS ANY KEY TO CONTINUE") / 2, 0.5f, SpriteEffects.None, 0);
                    break;
                }
                case GameState.SELECTION:
                {
                    RenderFrivolity();
                    spriteBatch.DrawString(font, Options[(Selection - 1) < 0 ? Options.Count - 1 : (Selection - 1)], new Vector2(Window.ClientBounds.Width / 2, 300), currentColor, 0.0f, font.MeasureString(Options[(Selection - 1) < 0 ? Options.Count - 1 : (Selection - 1)])/2, 0.5f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(font, Options[Selection], new Vector2(Window.ClientBounds.Width / 2, 350), currentColor, 0.0f, font.MeasureString(Options[Selection]) / 2, 0.75f, SpriteEffects.None, 0.0f);
                    spriteBatch.DrawString(font, Options[(Selection + 1) >= Options.Count ? 0 : (Selection + 1)], new Vector2(Window.ClientBounds.Width / 2, 400), currentColor, 0.0f, font.MeasureString(Options[(Selection + 1) >= Options.Count ? 0 : (Selection + 1)]) / 2, 0.5f, SpriteEffects.None, 0.0f);
                    break;
                }
                case GameState.CHOOSING_PLAYER:
                {
                    spriteBatch.DrawString(font, "CHOOSE YOUR CHARACTER", new Vector2(Window.ClientBounds.Width / 2, 400), Color.White, 0.0f,font.MeasureString("CHOOSE YOUR CHARACTER") / 2, 1.0f, SpriteEffects.None, 0);
                    if (!Characters.ElementAt(PlayerSprite).Value)
                    {
                        spriteBatch.Draw(PlayerSprites, new Vector2(Window.ClientBounds.Width/2, 200),
                            new Rectangle(0, PlayerSprite*64, 64, 64), Color.DarkGray, 0.0f, new Vector2(32, 32), 1.0f,
                            SpriteEffects.None, 0);
                        spriteBatch.Draw(Lock, new Vector2(Window.ClientBounds.Width/2, 200), null, Color.White, 0.0f,
                            new Vector2(32, 32), 0.75f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, "LOCKED", new Vector2(Window.ClientBounds.Width/2, 250),
                            Color.White, 0.0f, font.MeasureString("LOCKED")/2, 0.5f, SpriteEffects.None, 0);

                    }
                    else
                    {
                        spriteBatch.Draw(PlayerSprites, new Vector2(Window.ClientBounds.Width / 2, 200), new Rectangle(0, PlayerSprite * 64, 64, 64), Color.White, 0.0f, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, Characters.ElementAt(PlayerSprite).Key, new Vector2(Window.ClientBounds.Width / 2, 250), Color.White, 0.0f, font.MeasureString(Characters.ElementAt(PlayerSprite).Key) / 2, 0.5f, SpriteEffects.None, 0);
                    }
                    break;
                }
                case GameState.PLAYING:
                {
                    for (var x = 0; x < Window.ClientBounds.Width/64; x++)
                    {
                        for (var y = 0; y < Window.ClientBounds.Height/64; y++)
                        {
                            var sourceRectangle =
                                new Rectangle((x == 0 ? 0 : (x == ((Window.ClientBounds.Width/64) - 1) ? 128 : 64)), (y == 0 ? 0 : (y == ((Window.ClientBounds.Height/64) - 1)) ? 128 : 64), 64, 64);
                            spriteBatch.Draw(RoomBack, new Vector2(x*64, y*64), sourceRectangle, Color.White);  
                        }
                    }
                    foreach (var e in Entities)
                    {
                        e.Render(spriteBatch, null);
                    }
                    Player.Render(spriteBatch, null);
                    for (var i = 0; i < 4; i++)
                    {
                        spriteBatch.Draw(GUISprites, new Vector2(64 + i * 64, 32), new Rectangle(i * 64, 64, 64, 64), Color.White, 0.0f, new Vector2(32, 32), 0.5f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, Player.ShooterComponent.AmmoCounts[i] + "/" + Player.ShooterComponent.MaxAmmoCounts[i], new Vector2(72 + i * 68, 64), Color.White, 0.0f, font.MeasureString(Player.ShooterComponent.AmmoCounts[i] + "/" + Player.ShooterComponent.MaxAmmoCounts[i]), 0.325f, SpriteEffects.None, 0);
                    }
                    spriteBatch.Draw(GUISprites, new Vector2(Window.ClientBounds.Width - 96, 32), new Rectangle(0, 256, 64, 64), Color.White, 0.0f, new Vector2(32, 32), 1.0f, SpriteEffects.None, 0);
                    break;

                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
