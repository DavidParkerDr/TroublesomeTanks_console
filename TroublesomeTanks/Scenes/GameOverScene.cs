using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TroublesomeTanks.Scenes
{
    public class GameOverScene : IScene
    {
        private List<Player> mPlayers;
        Texture2D mBackgroundTexture = null;
        SpriteBatch mSpriteBatch = null;
        Rectangle mRectangle;
        float mSecondsLeft;
        int mWinner;
        public GameOverScene(Texture2D pBackgroundTexture, List<Player> pPlayers, int pWinner)
        {
            TroublesomeTanks game = (TroublesomeTanks)TroublesomeTanks.Instance();
            mBackgroundTexture = pBackgroundTexture;
            mSpriteBatch = new SpriteBatch(game.GDM().GraphicsDevice);
            int screenWidth = game.GDM().GraphicsDevice.Viewport.Width;
            int screenHeight = game.GDM().GraphicsDevice.Viewport.Height;
            int height = screenHeight / 2;
            int width = (int)(mBackgroundTexture.Width * (float)height / mBackgroundTexture.Height);
            int x = (screenWidth - width) / 2;
            int y = (screenHeight - height) / 2;
            mRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            mSecondsLeft = DGS.SECONDS_TO_DISPLAY_GAMEOVER_SCREEN;
            game.ReplaceCurrentMusicInstance("Music/Music_start", true);
            mPlayers = pPlayers;
            mWinner = pWinner;
            RepositionGUIs();
        }
        public void RepositionGUIs()
        {
            if (mWinner != -1)
            {
                RepositionAsWinner();
            }
            RepositionAsLosers();
        }
        public void RepositionAsWinner()
        {
            TroublesomeTanks game = (TroublesomeTanks)TroublesomeTanks.Instance();
            float textureHeightOverWidth = (float)254 / (float)540;
            int textureHeight = game.GDM().GraphicsDevice.Viewport.Height / 2;
            int textureWidth = textureHeight;

            int left = (game.GDM().GraphicsDevice.Viewport.Width - textureWidth) / 2;
            int top = 0;// game.GDM().GraphicsDevice.Viewport.Height / 2 - textureHeight / 2;
            Rectangle newRectangle = new Rectangle(left, top, textureWidth, textureHeight);
            mPlayers[mWinner].Avatar.Reposition(newRectangle);

            textureHeight = textureWidth / 5;

            left = (game.GDM().GraphicsDevice.Viewport.Width - textureWidth) / 2;
            top = game.GDM().GraphicsDevice.Viewport.Height / 2;
            Rectangle healthRectangle = new Rectangle(left, top, textureWidth, textureHeight);
            mPlayers[mWinner].GUI.GetHealthBar().Reposition(healthRectangle);
        }

        public void RepositionAsLosers()
        {
            int offset = 0;
            int loserCount = mPlayers.Count;
            if (mWinner != -1)
            {
                loserCount -= 1;
            }
            TroublesomeTanks game = (TroublesomeTanks)TroublesomeTanks.Instance();

            int textureHeight = game.GDM().GraphicsDevice.Viewport.Height / 4;
            int textureWidth = textureHeight;
            int totalWidth = textureWidth * loserCount;
            offset = (game.GDM().GraphicsDevice.Viewport.Width - totalWidth) / 2;
            for (int i = 0; i < mPlayers.Count; i++)
            {
                if (i != mWinner)
                {
                    float textureHeightOverWidth = (float)254 / (float)540;
                    
                    int left = offset;
                    int top = game.GDM().GraphicsDevice.Viewport.Height / 2 + (game.GDM().GraphicsDevice.Viewport.Height / 2 - textureHeight) / 2;
                    
                    Rectangle newRectangle = new Rectangle(left, top, textureWidth, textureHeight);
                    mPlayers[i].Avatar.Reposition(newRectangle);
                    offset += textureWidth;
                }
            }
            
        }

        public void Update(float pSeconds)
        {
            mSecondsLeft -= pSeconds;
            if (mSecondsLeft <= 0.0f)
            {
                IGame game = TroublesomeTanks.Instance();
                game.SM().Transition(null);
            }
        }
        public void Draw(float pSeconds)
        {
            TroublesomeTanks.Instance().GDM().GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();

            mSpriteBatch.Draw(mBackgroundTexture, mRectangle, Color.White);
            for (int i = 0; i < mPlayers.Count; i++)
            {
                mPlayers[i].GUI.DrawAvatar(mSpriteBatch);
                if(mWinner == i)
                {
                    mPlayers[i].GUI.DrawHealthBar(mSpriteBatch);
                }
            }
            mSpriteBatch.End();
        }
    }
}
