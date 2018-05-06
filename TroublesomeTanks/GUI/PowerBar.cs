using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TroublesomeTanks.GUI
{
    public class PowerBar
    {
        private static Texture2D mPowerBorder, mPowerBar, mPowerLayer;
        private Rectangle mBoundsRectangle;
        private Rectangle mDrawRectangle;
        private Rectangle mPowerBarDrawRectangle;
        private JackIcon mJackIcon;
        private JackIcon mChargeIcon;

        public static void SetupStaticTextures(Texture2D pPowerBorder, Texture2D pPowerBar, Texture2D pPowerLayer)
        {
            mPowerBorder = pPowerBorder;
            mPowerBar = pPowerBar;
            mPowerLayer = pPowerLayer;
        }

        public PowerBar(Rectangle pBoundsRectangle)
        {
            mBoundsRectangle = pBoundsRectangle;
            PrepareDrawRectangle();
            PrepareJackIcon();
            PrepareChargeIcon();

        }
        private void PrepareDrawRectangle()
        {
            int padding = 5;
            int chargeIconHeight = mBoundsRectangle.Height;
            int chargeIconWidth = (int)(chargeIconHeight * 0.5f);
            int powerBarHeight = mBoundsRectangle.Height - padding * 2;
            int powerBarWidth = mBoundsRectangle.Width - padding * 2 - chargeIconWidth;
            int powerBarLeft = mBoundsRectangle.Left + padding + chargeIconWidth;
            int powerBarTop = mBoundsRectangle.Top + padding;
            mDrawRectangle = new Rectangle(powerBarLeft, powerBarTop, powerBarWidth, powerBarHeight);
        }

        private void PrepareJackIcon()
        {
            Vector2 jackIconPosition = new Vector2(mDrawRectangle.X + mDrawRectangle.Width * 0.5f, mDrawRectangle.Y + mDrawRectangle.Height * 0.5f);
            int jackIconWidth = 0;
            int padding = 5;
            int jackIconHeight = mDrawRectangle.Height - padding * 2;
            mJackIcon = new JackIcon(jackIconPosition, jackIconWidth, jackIconHeight);
        }
        private void PrepareChargeIcon()
        {
            int padding = 5;
            int chargeIconHeight = mDrawRectangle.Height;
            int chargeIconWidth = (int)(chargeIconHeight * 0.5f);

            Vector2 chargeIconPosition = new Vector2(mBoundsRectangle.X + chargeIconWidth * 0.5f, mBoundsRectangle.Y + chargeIconHeight * 0.5f + padding);
            
            mChargeIcon = new JackIcon(chargeIconPosition, chargeIconWidth, chargeIconHeight);
        }
        private void UpdatePowerBarRectangle(float pCharge)
        {
            int powerBarWidth = mDrawRectangle.Width;
            int powerBarHeight = mDrawRectangle.Height;
            int width = (int)(powerBarWidth * ((pCharge / DGS.MAX_CHARGE)));
            mPowerBarDrawRectangle = new Rectangle(mDrawRectangle.X, mDrawRectangle.Y, width, mDrawRectangle.Height);
        }
        public void Draw(SpriteBatch pSpriteBatch, float pCharge, Color pBarColour, bool pEnoughCharge, bool pDrawChargeIcon, ControlGroup pJackIcon, bool pControlSwapMode)
        {
            UpdatePowerBarRectangle(pCharge);
            pSpriteBatch.Draw(mPowerLayer, mDrawRectangle, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            if (pCharge > 0)
            {
                pSpriteBatch.Draw(mPowerBar, mPowerBarDrawRectangle, null, pBarColour, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
                if (pEnoughCharge)
                {
                    pSpriteBatch.Draw(mPowerBar, mPowerBarDrawRectangle, null, pBarColour, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
                }
                else
                {
                    pSpriteBatch.Draw(mPowerBar, mDrawRectangle, null, Color.Red, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
                    pSpriteBatch.Draw(mPowerBar, mPowerBarDrawRectangle, null, pBarColour, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
                }
            }
            else if (pCharge == 0)
            {
                pSpriteBatch.Draw(mPowerBar, mDrawRectangle, null, Color.Red, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);

            }
            pSpriteBatch.Draw(mPowerBorder, mDrawRectangle, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
            if (pDrawChargeIcon)
            {
                mChargeIcon.Draw(pSpriteBatch, ControlGroup.BATTERY, Color.White);
            }
            Color jackColour = Color.White;
            if (pControlSwapMode)
            {
                jackColour = Color.Green;
            }
            mJackIcon.Draw(pSpriteBatch, pJackIcon, jackColour);

        }

    }
}
