using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO.Ports;
using TroublesomeTanks.Scenes;

namespace TroublesomeTanks
{
    /// <summary>
    /// This interface specifies things that we want to get global access to (probs everything)
    /// </summary>
    public interface IGame
    {
        SoundManager GetSoundManager();
        SceneManager SM();
        ContentManager CM();
        GraphicsDeviceManager GDM();

        IController Controller0();
        IController Controller1();
        IController Controller2();
        IController Controller3();

        IController GetController(int pIndex);
        int NumControllers();


        void UpdateControllers(float pSeconds);
        void ResetControllers();

        void Exit();
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TroublesomeTanks : Game, IGame
    {
        private SoundManager mSoundManager;
        private GraphicsDeviceManager mGraphics;
        private SpriteBatch mBatch;
        private SceneManager mSceneManager;
        private List<IController> mControllers;
        private static IGame mGameInterface = null;
        private SoundEffectInstance mCurrentMusic;
        private string mCurrentMusicName;

        public static IGame Instance()
        {
            if(mGameInterface == null)
            {
                mGameInterface = new TroublesomeTanks();
            }
            return mGameInterface;
        }

        public IController Controller0()
        {
            if (mControllers.Count > 0)
            {
                return mControllers[0];
            }
            else
            {
                return null;
            }
        }
        public IController Controller1()
        {
            if (mControllers.Count > 1)
            {
                return mControllers[1];
            }
            else
            {
                return null;
            }
        }
        public IController Controller2()
        {
            if (mControllers.Count > 2)
            {
                return mControllers[2];
            }
            else
            {
                return null;
            }
        }
        public IController Controller3()
        {
            if (mControllers.Count > 3)
            {
                return mControllers[3];
            }
            else
            {
                return null;
            }
        }

        public IController GetController(int pIndex)
        {
            if(pIndex < NumControllers())
            {
                return mControllers[pIndex];
            }
            return null;
        }
        public int NumControllers()
        {
            return mControllers.Count;
        }

        public void UpdateControllers(float pSeconds)
        {
            foreach(IController controller in mControllers)
            {
                controller.UpdateController(pSeconds);
            }
        }
        public void ResetControllers()
        {
            foreach (IController controller in mControllers)
            {
                controller.ResetJacks();
                controller.ResetControls();
            }
        }
        public SoundManager GetSoundManager() { return mSoundManager; }
        public SceneManager SM() { return mSceneManager; }
        public ContentManager CM() { return Content; }
        public GraphicsDeviceManager GDM() { return mGraphics; }
        public TroublesomeTanks()
        {
            mGraphics               = new GraphicsDeviceManager(this);
            Content.RootDirectory   = "Content";
            mSceneManager           = new SceneManager();
            
                
            
    


            mSoundManager = new SoundManager();

            
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
            mGraphics.PreferredBackBufferHeight = DGS.SCREENHEIGHT;
            mGraphics.PreferredBackBufferWidth = DGS.SCREENWIDTH;
            mGraphics.IsFullScreen = DGS.IS_FULL_SCREEN;
            this.Window.Title = "TroubleSome Tanks";
            mGraphics.ApplyChanges();
            base.Initialize();
            mControllers = new List<IController>();
            Dictionary<Buttons, Control> buttonMap = new Dictionary<Buttons, Control>();
            buttonMap.Add(Buttons.LeftThumbstickUp, Control.TRACKS_FORWARDS);
            buttonMap.Add(Buttons.LeftThumbstickDown, Control.TRACKS_BACKWARDS);
            buttonMap.Add(Buttons.LeftThumbstickLeft, Control.TRACKS_LEFT);
            buttonMap.Add(Buttons.LeftThumbstickRight, Control.TRACKS_RIGHT);
            buttonMap.Add(Buttons.RightThumbstickLeft, Control.TURRET_LEFT);
            buttonMap.Add(Buttons.RightThumbstickRight, Control.TURRET_RIGHT);
            buttonMap.Add(Buttons.LeftTrigger, Control.FIRE);
            buttonMap.Add(Buttons.RightTrigger, Control.FIRE);
            buttonMap.Add(Buttons.X, Control.BATTERY);
            buttonMap.Add(Buttons.Y, Control.POWER1);
            buttonMap.Add(Buttons.B, Control.POWER2);
            buttonMap.Add(Buttons.A, Control.POWER3);

            Dictionary<Keys, Control> keyMap1 = new Dictionary<Keys, Control>();
            keyMap1.Add(Keys.W, Control.TRACKS_FORWARDS);
            keyMap1.Add(Keys.S, Control.TRACKS_BACKWARDS);
            keyMap1.Add(Keys.A, Control.TRACKS_LEFT);
            keyMap1.Add(Keys.D, Control.TRACKS_RIGHT);
            keyMap1.Add(Keys.V, Control.TURRET_LEFT);
            keyMap1.Add(Keys.B, Control.TURRET_RIGHT);
            keyMap1.Add(Keys.Space, Control.FIRE);
            keyMap1.Add(Keys.F, Control.BATTERY);
            keyMap1.Add(Keys.R, Control.POWER1);
            keyMap1.Add(Keys.T, Control.POWER2);
            keyMap1.Add(Keys.G, Control.POWER3);

            Dictionary<Keys, Control> keyMap2 = new Dictionary<Keys, Control>();
            keyMap2.Add(Keys.U, Control.TRACKS_FORWARDS);
            keyMap2.Add(Keys.J, Control.TRACKS_BACKWARDS);
            keyMap2.Add(Keys.H, Control.TRACKS_LEFT);
            keyMap2.Add(Keys.K, Control.TRACKS_RIGHT);
            keyMap2.Add(Keys.OemPeriod, Control.TURRET_LEFT);
            keyMap2.Add(Keys.OemQuestion, Control.TURRET_RIGHT);
            keyMap2.Add(Keys.Enter, Control.FIRE);
            keyMap2.Add(Keys.L, Control.BATTERY);
            keyMap2.Add(Keys.O, Control.POWER1);
            keyMap2.Add(Keys.P, Control.POWER2);
            keyMap2.Add(Keys.OemSemicolon, Control.POWER3);

            GamePadCapabilities capabilities1 = GamePad.GetCapabilities(PlayerIndex.One);
            GamePadCapabilities capabilities2 = GamePad.GetCapabilities(PlayerIndex.Two);
            GamePadCapabilities capabilities3 = GamePad.GetCapabilities(PlayerIndex.Three);
            GamePadCapabilities capabilities4 = GamePad.GetCapabilities(PlayerIndex.Four);
            if (capabilities1.IsConnected)
            {
                mControllers.Add(new XBoxController(buttonMap, PlayerIndex.One));
                if (capabilities2.IsConnected)
                {
                    mControllers.Add(new XBoxController(buttonMap, PlayerIndex.Two));
                    if (capabilities3.IsConnected)
                    {
                        mControllers.Add(new XBoxController(buttonMap, PlayerIndex.Three));
                        if (capabilities4.IsConnected)
                        {
                            mControllers.Add(new XBoxController(buttonMap, PlayerIndex.Four));
                        }
                        else
                        {
                            mControllers.Add(new KeyboardController(keyMap1));
                        }
                    }
                    else
                    {
                        mControllers.Add(new KeyboardController(keyMap1));
                        mControllers.Add(new KeyboardController(keyMap2));
                    }
                }
                else
                {
                    mControllers.Add(new KeyboardController(keyMap1));
                    mControllers.Add(new KeyboardController(keyMap2));
                }
            }
            else
            {
                mControllers.Add(new KeyboardController(keyMap1));
                mControllers.Add(new KeyboardController(keyMap2));
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mBatch = new SpriteBatch(GraphicsDevice);
            mSoundManager.Add("Music/Music_start");
            mSoundManager.Add("Music/Music_intro");
            mSoundManager.Add("Music/Music_loopable");
            mSoundManager.Add("Sounds/Tank_Gun");
            mSoundManager.Add("Sounds/Tank_Tracks");
            mSoundManager.Add("Sounds/Tank_Clang1");
            mSoundManager.Add("Sounds/Tank_Clang2");
            mSoundManager.Add("Sounds/Tank_Clang3");

             mSceneManager.Push(new FlashScreenScene());
            //mSceneManager.Push(new GameScene());
           // mSceneManager.Push(new PlayerSelectionScene());

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
            float seconds = 0.001f * gameTime.ElapsedGameTime.Milliseconds;
            mSceneManager.Update(seconds);
            
            base.Update(gameTime);
        }
        public SoundEffectInstance ReplaceCurrentMusicInstance(string pName, bool pLoopable)
        {
            if (mCurrentMusicName != pName)
            {
                if (mCurrentMusic != null)
                {
                    mCurrentMusic.Stop();
                }
                mCurrentMusicName = pName;
                SoundEffectInstance replacement = mSoundManager.GetSoundEffectInstance(pName);
                replacement.IsLooped = pLoopable;
                mCurrentMusic = replacement;
                mCurrentMusic.Play();
            }
            mCurrentMusic.IsLooped = pLoopable;

            return mCurrentMusic;
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            float seconds = 0.001f * gameTime.ElapsedGameTime.Milliseconds;
            mSceneManager.Draw(seconds);
            base.Draw(gameTime);
        }
    }
}
