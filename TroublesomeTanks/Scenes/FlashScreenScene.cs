using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TroublesomeTanks.Scenes
{
    public class FlashScreenScene : IScene
    {
        Texture2D mLogoTexture = null;
        SpriteBatch mSpriteBatch = null;
        Rectangle mRectangle;
        float mSecondsLeft;
        public FlashScreenScene() {
            IGame game = TroublesomeTanks.Instance();
            mLogoTexture = game.CM().Load<Texture2D>("selogo");
            mSpriteBatch = new SpriteBatch(game.GDM().GraphicsDevice);
            int screenWidth = game.GDM().GraphicsDevice.Viewport.Width;
            int screenHeight = game.GDM().GraphicsDevice.Viewport.Height;
            int height = screenHeight / 2;
            int width = (int)(mLogoTexture.Width * (float)height / mLogoTexture.Height);
            int x = (screenWidth - width) / 2;
            int y = (screenHeight - height) / 2;
            mRectangle = new Rectangle(x, y, width, height);
            mSecondsLeft = DGS.SECONDS_TO_DISPLAY_FLASH_SCREEN;
            LoadContent();
        }

        static public bool sLoadingDone;

        void LoadContent()
        {
            var loading = Task.Factory.StartNew(() =>
            {
                // Load all your content here.
                // NOTE: Be sure to catch any exceptions in here and handle them.
                loadAllAssets();
                sLoadingDone = true;
            });
        }

        private void loadAllAssets()
        {
            IGame game = TroublesomeTanks.Instance();
            game.CM().Load<Texture2D>("selogo");
            game.CM().Load<Texture2D>("white_pixel");
            game.CM().Load<Texture2D>("circle");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_bw_01");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_bw_02");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_bw_03");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_bw_04");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_bw_05");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_colour_01");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_colour_02");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_colour_03");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_colour_04");
            game.CM().Load<Texture2D>("avatars/avatar_engineer_colour_05");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_bw_01");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_bw_02");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_bw_03");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_bw_04");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_bw_05");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_colour_01");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_colour_02");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_colour_03");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_colour_04");
            game.CM().Load<Texture2D>("avatars/avatar_winterjack_colour_05");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_bw_01");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_bw_02");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_bw_03");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_bw_04");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_bw_05");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_colour_01");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_colour_02");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_colour_03");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_colour_04");
            game.CM().Load<Texture2D>("avatars/avatar_yeti_colour_05");
            game.CM().Load<Texture2D>("avatars/avatar_robo_bw_01");
            game.CM().Load<Texture2D>("avatars/avatar_robo_bw_02");
            game.CM().Load<Texture2D>("avatars/avatar_robo_bw_03");
            game.CM().Load<Texture2D>("avatars/avatar_robo_bw_04");
            game.CM().Load<Texture2D>("avatars/avatar_robo_bw_05");
            game.CM().Load<Texture2D>("avatars/avatar_robo_colour_01");
            game.CM().Load<Texture2D>("avatars/avatar_robo_colour_02");
            game.CM().Load<Texture2D>("avatars/avatar_robo_colour_03");
            game.CM().Load<Texture2D>("avatars/avatar_robo_colour_04");
            game.CM().Load<Texture2D>("avatars/avatar_robo_colour_05");
            game.CM().Load<Texture2D>("countdown/ready");
            game.CM().Load<Texture2D>("countdown/one");
            game.CM().Load<Texture2D>("countdown/two");
            game.CM().Load<Texture2D>("countdown/three");
            game.CM().Load<Texture2D>("countdown/four");
            game.CM().Load<Texture2D>("countdown/five");
            game.CM().Load<Texture2D>("vertical_battery_bar");
            game.CM().Load<Texture2D>("vertical_battery_border");
            game.CM().Load<Texture2D>("vertical_battery_layer");
            game.CM().Load<Texture2D>("horizontal_power_bar");
            game.CM().Load<Texture2D>("horizontal_power_border");
            game.CM().Load<Texture2D>("horizontal_power_layer");
            game.CM().Load<Texture2D>("tracks_icon");
            game.CM().Load<Texture2D>("gun_icon");
            game.CM().Load<Texture2D>("fire_icon");
            game.CM().Load<Texture2D>("charge_icon");
            game.CM().Load<Texture2D>("battery_icon");
            game.CM().Load<Texture2D>("cannon");
            game.CM().Load<Texture2D>("cannonFire");
            game.CM().Load<Texture2D>("background_01");            
            game.CM().Load<Texture2D>("background_06");
            game.CM().Load<Texture2D>("healthbars/heart_colour");
            game.CM().Load<Texture2D>("healthbars/heart_bw");
            game.CM().Load<Texture2D>("block");
            game.CM().Load<Texture2D>("menu_play_white");
            game.CM().Load<Texture2D>("menu_play_dark");
            game.CM().Load<Texture2D>("menu_quit_white");
            game.CM().Load<Texture2D>("menu_quit_dark");
            game.CM().Load<Texture2D>("menu_white");
            game.CM().Load<Texture2D>("menu_title");
            game.CM().Load<Texture2D>("track");
            game.CM().Load<Texture2D>("Tank-B-05");
            game.CM().Load<Texture2D>("Tank track B-L");
            game.CM().Load<Texture2D>("Tank track B-R");
            game.CM().Load<Texture2D>("Tank track W-L");
            game.CM().Load<Texture2D>("Tank track W-R");
            game.CM().Load<SoundEffect>("Music/Music_start");
            game.CM().Load<SoundEffect>("Music/Music_intro");
            game.CM().Load<SoundEffect>("Music/Music_loopable");
            game.CM().Load<SoundEffect>("Sounds/Tank_Gun");
            game.CM().Load<SoundEffect>("Sounds/Tank_Clang1");
            game.CM().Load<SoundEffect>("Sounds/Tank_Clang2");
            game.CM().Load<SoundEffect>("Sounds/Tank_Clang3");
            game.CM().Load<SoundEffect>("Sounds/Tank_Tracks");

        }

        public void Update(float pSeconds)
        {
            mSecondsLeft -= pSeconds;
            
            if(mSecondsLeft<= 0.0f && sLoadingDone)
            {
                IGame game = TroublesomeTanks.Instance();
                game.SM().Transition(new StartScene());
                
            }
        }
        public void Draw(float pSeconds)
        {
            TroublesomeTanks.Instance().GDM().GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();

            mSpriteBatch.Draw(mLogoTexture, mRectangle, Color.White);

            mSpriteBatch.End();
        }
    }
}
