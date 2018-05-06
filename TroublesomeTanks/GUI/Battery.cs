using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TroublesomeTanks.GUI
{
    public class Battery
    {
        private static Texture2D mVerticalBatteryBorder, mVerticalBatteryBar, mVerticalBatteryLayer;
        private Vector2 mPowerBorderPosition;
        private int mPowerBorderWidth, mPowerBorderHeight;
        private Rectangle mBoundsRectangle;
        private Rectangle mDrawRectangle;
        private Rectangle mPowerBarDrawRectangle;
        private JackIcon mBatteryIcon;

        public static void SetupStaticTextures(Texture2D pVerticalBatteryBorder, Texture2D pVerticalBatteryBar, Texture2D pHorizontalBatteryLayer)
        {
            mVerticalBatteryBorder = pVerticalBatteryBorder;
            mVerticalBatteryBar = pVerticalBatteryBar;
            mVerticalBatteryLayer = pHorizontalBatteryLayer;
        }

        public Battery(Rectangle pBoundsRectangle)
        {
            mBoundsRectangle = pBoundsRectangle;            
            PrepareDrawRectangle();
            PrepareBatteryIcon();
            
        }

        private void PrepareDrawRectangle()
        {
            int padding = 5;
            int batteryHeight = mBoundsRectangle.Height - padding * 2;
            int batteryWidth = mBoundsRectangle.Width - padding * 2;
            int batteryLeft = mBoundsRectangle.Left + padding;
            int batteryTop = mBoundsRectangle.Top + padding;
            mDrawRectangle = new Rectangle(batteryLeft, batteryTop, batteryWidth, batteryHeight);
        }

        private void PrepareBatteryIcon()
        {
            Vector2 batteryIconPosition = new Vector2(mDrawRectangle.X + mDrawRectangle.Width * 0.5f, mDrawRectangle.Y + mDrawRectangle.Height * 0.5f);
            int batteryIconWidth = 0;
            int batteryIconHeight = mDrawRectangle.Width;
            mBatteryIcon = new JackIcon(batteryIconPosition, batteryIconWidth, batteryIconHeight);
        }

        private void UpdatePowerBarRectangle(float pCharge)
        {
            int powerBarWidth = mDrawRectangle.Width;
            int powerBarHeight = mDrawRectangle.Height - 18;
            int height = (int)(powerBarHeight * ((pCharge / DGS.MAX_BATTERY_CHARGE)));
            mPowerBarDrawRectangle = new Rectangle(mDrawRectangle.X, mDrawRectangle.Y + mDrawRectangle.Height - height, powerBarWidth, height);
        }

        public void Draw(SpriteBatch pSpriteBatch, float pCharge, Color pBarColour, bool pDrawIcon)
        {
            UpdatePowerBarRectangle(pCharge);

            if (pCharge > 0)
            {
                pSpriteBatch.Draw(mVerticalBatteryLayer, mDrawRectangle, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
                pSpriteBatch.Draw(mVerticalBatteryBar, mPowerBarDrawRectangle, null, pBarColour, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
                pSpriteBatch.Draw(mVerticalBatteryBorder, mDrawRectangle, null, Color.White, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
                if (pDrawIcon)
                {
                    mBatteryIcon.Draw(pSpriteBatch,ControlGroup.BATTERY, Color.White);
                }
            }
            else if (pCharge <= 0)
            {
                pSpriteBatch.Draw(mVerticalBatteryLayer, mDrawRectangle, null, Color.Red, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
                pSpriteBatch.Draw(mVerticalBatteryBorder, mDrawRectangle, null, Color.Red, 0f, new Vector2(0, 0), SpriteEffects.None, 0f);
            }
        }

    }
}
