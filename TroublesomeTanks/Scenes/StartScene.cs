﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;


namespace TroublesomeTanks.Scenes
{
    
    public class StartScene : IScene
    {
        ButtonList mButtonList = null;
        Texture2D mBackgroundTexture = null;
        Rectangle mBackgroundRectangle;

        Texture2D mForgroundTexture = null;

        Texture2D mTitleTexture = null;
        Rectangle mTitleRectangle;

        SpriteBatch mSpriteBatch = null;
        private IController mController0;
        private IController mController1;
        private IController mController2;
        private IController mController3;
        List<IController> mControllers;
        float mSecondsLeft;
        public StartScene() {
            TroublesomeTanks game = (TroublesomeTanks)TroublesomeTanks.Instance();
            mControllers = new List<IController>();
            mController0 = TroublesomeTanks.Instance().Controller0();
            if (mController0 != null)
            {
                mControllers.Add(mController0);
            }
            mController1 = TroublesomeTanks.Instance().Controller1();
            if (mController1 != null)
            {
                mControllers.Add(mController1);
            }
            mController2 = TroublesomeTanks.Instance().Controller2();
            if (mController2 != null)
            {
                mControllers.Add(mController2);
            }
            mController3 = TroublesomeTanks.Instance().Controller3();
            if (mController3 != null)
            {
                mControllers.Add(mController3);
            }
            
            
            
            mSpriteBatch = new SpriteBatch(game.GDM().GraphicsDevice);
            int screenWidth = game.GDM().GraphicsDevice.Viewport.Width;
            int screenHeight = game.GDM().GraphicsDevice.Viewport.Height;

            mBackgroundTexture = game.CM().Load<Texture2D>("background_01");         
            mBackgroundRectangle = new Rectangle(0, 0, screenWidth, screenHeight);


            mForgroundTexture = game.CM().Load<Texture2D>("menu_white");

            mTitleTexture = game.CM().Load<Texture2D>("menu_title");
            mTitleRectangle = new Rectangle((screenWidth / 2) - (644 / 2), (screenHeight / 2) - (128 / 2), 644, 128);

            mButtonList = new ButtonList();
                        
            Texture2D startGameButtonTexture = game.CM().Load<Texture2D>("menu_play_white");
            Texture2D startGameButtonTexturePressed = game.CM().Load<Texture2D>("menu_play_dark");
            
            Rectangle startGameButtonRectangle = 
                new Rectangle(
                    ((int)((screenWidth - startGameButtonTexture.Width) / 2) - (int)(startGameButtonTexture.Width * 0.75f)), 
                    (screenHeight) / 2 + startGameButtonTexture.Height, 
                    startGameButtonTexture.Width, 
                    startGameButtonTexture.Height);
            
            Button startGameButton = new Button(startGameButtonTexture,startGameButtonTexturePressed, startGameButtonRectangle, Color.Red, StartGame);
            startGameButton.Selected = true;
            mButtonList.Add(startGameButton);
            
            Texture2D exitGameButtonTexture = game.CM().Load<Texture2D>("menu_quit_white");
            Texture2D exitGameButtonTexturePressed = game.CM().Load<Texture2D>("menu_quit_dark");
            
            Rectangle exitGameButtonRectangle =
                new Rectangle((screenWidth - exitGameButtonTexture.Width) / 2 + (int)(startGameButtonTexture.Width * 0.75f),
                    (screenHeight) / 2 + exitGameButtonTexture.Width,
                    exitGameButtonTexture.Width, 
                    exitGameButtonTexture.Height);
            Button exitGameButton = new Button(exitGameButtonTexture, exitGameButtonTexturePressed, exitGameButtonRectangle, Color.Red, ExitGame);
            exitGameButton.Selected = false;
            mButtonList.Add(exitGameButton);
            mSecondsLeft = 0.1f;
            game.ReplaceCurrentMusicInstance("Music/Music_start", true);
            
            
         

         
        }



        private void ExitGame()
        {
            IGame game = TroublesomeTanks.Instance();
            game.SM().Transition(null);
        }

        private void StartGame()
        {
            IGame game = TroublesomeTanks.Instance();
            game.SM().Transition(new PlayerSelectionScene(), false);
        }

        public void Update(float pSeconds)
        {
            Escape();
            foreach (IController controller in mControllers)
            {
                controller.UpdateController(pSeconds);
                mSecondsLeft -= pSeconds;

                if (controller.IsPressedWithCharge(Control.TRACKS_LEFT))
                {
                    if (mSecondsLeft <= 0.0f)
                    {
                        //mSoundEffects["Button_Push"].Play();
                        mButtonList.SelectPreviousButton();
                        mSecondsLeft = 1f;
                    }
                }
                if (controller.IsPressed(Control.TRACKS_RIGHT))
                {
                    if (mSecondsLeft <= 0.0f)
                    {
                        //mSoundEffects["Button_Push"].Play();
                        mButtonList.SelectNextButton();
                        mSecondsLeft = 1f;
                    }
                }


                if (controller.IsPressed(Control.POWER3))
                {
                    if (mSecondsLeft <= 0.0f)
                    {
                        mButtonList.PressSelectedButton();
                        mSecondsLeft = 0.1f;
                    }
                }
                /*if (controller.IsPressed(Control.RECHARGE))
                {
                    if (mSecondsLeft <= 0.0f)
                    {
                        mButtonList.PressSelectedButton();
                        mSecondsLeft = 0.1f;
                    }
                }*/

            }
        }
        public void Escape()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                TroublesomeTanks.Instance().SM().Transition(null);
            }
        }
        public void Draw(float pSeconds)
        {
            TroublesomeTanks.Instance().GDM().GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();
            Color backColour = Color.White;
            
            mSpriteBatch.Draw(mBackgroundTexture, mBackgroundRectangle, backColour);
            mSpriteBatch.Draw(mForgroundTexture, mBackgroundRectangle, backColour);

            mSpriteBatch.Draw(mTitleTexture, mTitleRectangle, backColour);

            mButtonList.Draw(mSpriteBatch);

            mSpriteBatch.End();
        }
    }
}
