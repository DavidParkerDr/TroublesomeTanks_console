using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TroublesomeTanks.GUI
{
    class JackIcon
    {
        private static Texture2D mTracksIcon;
        private static Texture2D mGunIcon;
        private static Texture2D mFireIcon;
        private static Texture2D mChargeIcon;
        private static Texture2D mBatteryIcon;
        private Vector2 mIconPosition;
        private int mIconWidth, mIconHeight;

        public static void SetupStaticTextures(Texture2D pTracksIcon, Texture2D pGunIcon, Texture2D pFireIcon, Texture2D pChargeIcon, Texture2D pBatteryIcon)
        {
            mTracksIcon = pTracksIcon;
            mGunIcon = pGunIcon;
            mFireIcon = pFireIcon;
            mChargeIcon = pChargeIcon;
            mBatteryIcon = pBatteryIcon;
        }

        public JackIcon(Vector2 pPos, int pWidth, int pHeight)
        {
            mIconPosition = pPos;
            mIconWidth = pWidth;
            mIconHeight = pHeight;
        }

        public void Draw(SpriteBatch pSpriteBatch, ControlGroup pControl, Color pIconColour)
        {

            Texture2D icon = mTracksIcon;

            if (pControl == ControlGroup.TRACKS)
            {
                icon = mTracksIcon;
            }
            else if (pControl == ControlGroup.GUN)
            {
                icon = mGunIcon;
            }
            else if (pControl == ControlGroup.FIRE)
            {
                icon = mFireIcon;
            }
            else if (pControl == ControlGroup.CHARGE)
            {
                icon = mChargeIcon;
            }
            else if (pControl == ControlGroup.BATTERY)
            {
                icon = mBatteryIcon;
            }
            int iconWidth = (int)(((float)icon.Width / icon.Height) * mIconHeight);
            Vector2 iconPosition = mIconPosition - new Vector2(iconWidth * 0.5f, mIconHeight * 0.5f);

            pSpriteBatch.Draw(icon, new Rectangle((int)iconPosition.X, (int)iconPosition.Y, iconWidth, mIconHeight), pIconColour);
        }
    }
}
