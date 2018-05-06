using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TroublesomeTanks.World;

namespace TroublesomeTanks.GUI
{
    public class TeamGUI
    {
        private Texture2D mWhitePixel;
        private Rectangle mBoundsRectangle;
        private HealthBar mHealthBar;
        private Avatar mAvatar;
        private Battery mBattery;
        private PowerBar[] mPowerBars = new PowerBar[3];
        private JackIcon[] mJackIcons = new JackIcon[3];
        private JackIcon[] mChargeIcons = new JackIcon[3];
        private JackIcon mBatteryIcon;
        private IController mController;
        private Tank mTank;
        private Vector2 Frame { get; set; }
        private Color mColour { get; set; }

        public TeamGUI(
            Texture2D pWhitePixel,
            Texture2D pHealthBarBlackAndWhiteLayer,
            Texture2D pHealthBarColourLayer,
            Texture2D pAvatarBlackAndWhiteLayer,
            Texture2D pAvatarColourLayer,
            Rectangle pBoundsRectangle,
            Tank pTank,
            IController pController,
            Color pColour)
        {
            mWhitePixel = pWhitePixel;
            mBoundsRectangle = pBoundsRectangle;
            mColour = pColour;
            mController = pController;
            float healthBarRatio = (float)pHealthBarBlackAndWhiteLayer.Height / pHealthBarBlackAndWhiteLayer.Width;
            int padding = 5;
            
            
            int powerBarHeight = (pBoundsRectangle.Height) / 4;
            int powerBarWidth = 195;// (DGS.SCREENWIDTH / 4) /* 25% of screen width */ * 9 / 19;/* Just Under Three quarters */
            int jackIconHeight = 28;
            int healthBarOffset = 65;
            int yStart = padding;

            int ySpacing = (pBoundsRectangle.Height - healthBarOffset - yStart - (powerBarHeight * 3)) / 3;
            

            mTank = pTank;

            
            PreparePowerBars();
            PrepareBattery();            
            PrepareAvatar(pAvatarBlackAndWhiteLayer, pAvatarColourLayer);
            PrepareHealthBar(pHealthBarBlackAndWhiteLayer, pHealthBarColourLayer);

        }
        public TeamGUI(
            Texture2D pWhitePixel,
            Texture2D pHealthBarBlackAndWhiteLayer,
            Texture2D pHealthBarColourLayer,
            Avatar pAvatar,
            Rectangle pBoundsRectangle,
            Tank pTank,
            IController pController,
            Color pColour)
        {
            mWhitePixel = pWhitePixel;
            mBoundsRectangle = pBoundsRectangle;
            mColour = pColour;
            mController = pController;
            float healthBarRatio = (float)pHealthBarBlackAndWhiteLayer.Height / pHealthBarBlackAndWhiteLayer.Width;
            int padding = 5;


            int powerBarHeight = (pBoundsRectangle.Height) / 4;
            int powerBarWidth = 195;// (DGS.SCREENWIDTH / 4) /* 25% of screen width */ * 9 / 19;/* Just Under Three quarters */
            int jackIconHeight = 28;
            int healthBarOffset = 65;
            int yStart = padding;

            int ySpacing = (pBoundsRectangle.Height - healthBarOffset - yStart - (powerBarHeight * 3)) / 3;


            mTank = pTank;


            PreparePowerBars();
            PrepareBattery();
            PrepareAvatar(pAvatar);
            PrepareHealthBar(pHealthBarBlackAndWhiteLayer, pHealthBarColourLayer);

        }

        public HealthBar GetHealthBar()
        {
            return mHealthBar;
        }

        private void PreparePowerBars()
        {
            int padding = 5;
            int powerBarHeight = (mBoundsRectangle.Height) / 4;
            int powerBarWidth = (int)(mBoundsRectangle.Width * 0.6f - powerBarHeight);
            int powerBarLeft = (int)(mBoundsRectangle.X + mBoundsRectangle.Width * 0.4f + powerBarHeight + padding);
            int powerBarTop = 0;
            for (int j = 0; j < 3; j++)
            {
                Rectangle powerBarRectangle = new Rectangle(powerBarLeft, powerBarTop, powerBarWidth, powerBarHeight);
                mPowerBars[j] = new PowerBar(powerBarRectangle);
                powerBarTop += powerBarHeight;
            }
        }
        
        private void PrepareBattery()
        {
            int padding = 5;
            int batteryWidth = (mBoundsRectangle.Height) / 4;
            int batteryHeight = mBoundsRectangle.Height - batteryWidth;
            int batteryLeft = mBoundsRectangle.X + (int)(mBoundsRectangle.Width * 0.4);
            Rectangle batteryRectangle = new Rectangle(batteryLeft, mBoundsRectangle.Y, batteryWidth, batteryHeight);
            mBattery = new Battery(batteryRectangle);
        }

        private void PrepareAvatar(Texture2D pAvatarBlackAndWhiteLayer, Texture2D pAvatarColourLayer)
        {
            int avatarWidth = (int)(mBoundsRectangle.Width * 0.4);
            int avatarHeight = mBoundsRectangle.Height;
            Rectangle avatarRectangle = new Rectangle(mBoundsRectangle.X, mBoundsRectangle.Y, avatarWidth, avatarHeight);
            mAvatar = new Avatar(mWhitePixel, pAvatarBlackAndWhiteLayer, pAvatarColourLayer, avatarRectangle, mColour);
        }
        private void PrepareAvatar(Avatar pAvatar)
        {
            int avatarWidth = (int)(mBoundsRectangle.Width * 0.4);
            int avatarHeight = mBoundsRectangle.Height;
            Rectangle avatarRectangle = new Rectangle(mBoundsRectangle.X, mBoundsRectangle.Y, avatarWidth, avatarHeight);
            mAvatar = pAvatar;
            mAvatar.Reposition(avatarRectangle);
        }

        private void PrepareHealthBar(Texture2D pHealthBarBlackAndWhiteLayer, Texture2D pHealthBarColourLayer)
        {
            int healthBarWidth = (int)(mBoundsRectangle.Width * 0.6);
            int healthBarHeight = (int)(mBoundsRectangle.Height * 0.25);
            int healthBarLeft = mBoundsRectangle.Left + mBoundsRectangle.Width - healthBarWidth;
            int healthBarTop = mBoundsRectangle.Top + mBoundsRectangle.Height - healthBarHeight; 
            Rectangle healthBarRectangle = new Rectangle(healthBarLeft, healthBarTop, healthBarWidth, healthBarHeight);
            mHealthBar = new HealthBar(mWhitePixel, pHealthBarBlackAndWhiteLayer, pHealthBarColourLayer, healthBarRectangle, mTank);
        }

        public Tank GetTank()
        {
            return mTank;
        }

        public void DrawHealthBar(SpriteBatch pSpriteBatch)
        {
            mHealthBar.Draw(pSpriteBatch);
        }

        public void DrawAvatar(SpriteBatch pSpriteBatch)
        {
            int avatarIndex = 4;
            int health = mTank.Health();
            if (health == 5)
            {
                avatarIndex = 0;
            }
            else if (health == 4)
            {
                avatarIndex = 1;
            }
            else if (health == 3)
            {
                avatarIndex = 2;
            }
            else if (health == 2)
            {
                avatarIndex = 3;
            }
            else if (health == 1)
            {
                avatarIndex = 4;
            }
            mAvatar.Draw(pSpriteBatch, mTank.Health() > 0, avatarIndex);
        }

        public void DrawBattery(SpriteBatch pSpriteBatch)
        {
            Color barColour = Color.White;
            if(mController.IsBatteryAllocated())
            {
                barColour = Color.Yellow;
            }
            mBattery.Draw(pSpriteBatch, mController.GetRemainingBatteryCharge(), barColour, mController.IsBatteryAllocationMode());
            
        }

        public void DrawPowerBars(SpriteBatch pSpriteBatch)
        {
            int batteryIndex = mController.GetBatteryIndex();

            for (int j = 0; j < 3; j++)
            {
                Color barColour = Color.LightBlue;
                bool drawChargeIcon = false;
                if (batteryIndex == j)
                {
                    // this power bar is being charged
                    barColour = Color.Yellow;
                    drawChargeIcon = true;

                }
                if (mController.GetJackControl(j) == ControlGroup.FIRE) // if this is the power bar with the FIRE control attached
                {
                    if (mController.GetJackCharge(j) >= DGS.BULLET_CHARGE_DEPLETION) // check to see if there is sufficient power to fire
                    {
                        mPowerBars[j].Draw(pSpriteBatch, mController.GetJackCharge(j), barColour, true, drawChargeIcon, mController.GetJackControl(j), j == mController.GetSwapIndex());
                    }
                    else
                    {
                        mPowerBars[j].Draw(pSpriteBatch, mController.GetJackCharge(j), barColour, false, drawChargeIcon, mController.GetJackControl(j), j == mController.GetSwapIndex());
                    }
                }
                else // for all the other controller types
                {
                    mPowerBars[j].Draw(pSpriteBatch, mController.GetJackCharge(j), barColour, mController.GetJackCharge(j) > 0, drawChargeIcon, mController.GetJackControl(j), j == mController.GetSwapIndex()); // if there is more than zero power
                }               
                
            }

        }

        public void Reposition(Rectangle pRectangle)
        {
            mHealthBar.Reposition(pRectangle);
        }

        public void DrawBounds(SpriteBatch pSpriteBatch)
        {
            Color boundColour = mColour;
            boundColour.A = (byte)0.5f;
            pSpriteBatch.Draw(mWhitePixel, mBoundsRectangle, boundColour);


        }
        public void Draw(SpriteBatch pSpriteBatch)
        {
            //DrawBounds(pSpriteBatch);        
            DrawHealthBar(pSpriteBatch);
            DrawAvatar(pSpriteBatch);
            DrawBattery(pSpriteBatch);
            DrawPowerBars(pSpriteBatch);
        }
    }
}
