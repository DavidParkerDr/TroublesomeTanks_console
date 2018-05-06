using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TroublesomeTanks.World.Particles;


namespace TroublesomeTanks.World
{
    public class Tank
    {
        private float mRotation;
        private float mOldRotation;
        private Vector3 mPosition;
        private Vector3 mOldPosition;
        private float mCannonRotation;
        private float mFireTimePrimed;
        private int mHealth;
        private int mFired;
        private Color mColour;
        private List<Bullet> mBullets;
        private int mLeftTrackFrame;
        private int mRightTrackFrame;
        private float mScale;
        

        public Tank(float pXPosition, float pYPosition, float pRotation, Color pColour, List<Bullet> pBullets, float pScale)
        {
            mScale = pScale;
            mBullets = pBullets;
            mRotation = pRotation;
            mOldRotation = mRotation;
            int screenWidth = TroublesomeTanks.Instance().GDM().GraphicsDevice.Viewport.Width;
            int screenHeight = TroublesomeTanks.Instance().GDM().GraphicsDevice.Viewport.Height;
            mPosition = new Vector3(pXPosition, pYPosition, 0);
            mOldPosition = mPosition;
            mCannonRotation = pRotation;
            mHealth = 5;
            mFired = 0;
            mColour = pColour;
            BatteryDown = false;
            mLeftTrackFrame = 1;
            mRightTrackFrame = 1;
            
        }

        public List<Bullet> GetBulletList()
        {
            return mBullets;
        }

        private void LeftTrackFrameForwards()
        {
            mLeftTrackFrame++;
            if(mLeftTrackFrame > 14)
            {
                mLeftTrackFrame = 1;
            }
            DustInitialisationPolicy dust = new DustInitialisationPolicy(GetLeftFrontCorner(), GetLeftBackCorner());
            ParticleManager.Instance().InitialiseParticles(dust, 4);
        }

        private void LeftTrackFrameBackwards()
        {
            mLeftTrackFrame--;
            if (mLeftTrackFrame < 1)
            {
                mLeftTrackFrame = 14;
            }
            DustInitialisationPolicy dust = new DustInitialisationPolicy(GetLeftFrontCorner(), GetLeftBackCorner());
            ParticleManager.Instance().InitialiseParticles(dust, 4);
        }

        private void RightTrackFrameForwards()
        {
            mRightTrackFrame++;
            if (mRightTrackFrame > 14)
            {
                mRightTrackFrame = 1;
            }
            DustInitialisationPolicy dust = new DustInitialisationPolicy(GetRightFrontCorner(), GetRightBackCorner());
            ParticleManager.Instance().InitialiseParticles(dust, 4);
        }

        private void RightTrackFrameBackwards()
        {
            mRightTrackFrame--;
            if (mRightTrackFrame < 1)
            {
                mRightTrackFrame = 14;
            }
            DustInitialisationPolicy dust = new DustInitialisationPolicy(GetRightFrontCorner(), GetRightBackCorner());
            ParticleManager.Instance().InitialiseParticles(dust, 4);
        }

        public bool BatteryDown { get; private set; }
        public bool Power1Down { get; private set; }
        public bool Power2Down { get; private set; }
        public bool Power3Down { get; private set; }


        public int Health()
        {
            return mHealth;
        }

        public int Fired()
        {
            return mFired;
        }
        public void DecFired()
        {
            mFired--;
        }
        public Color Colour()
        {
            return mColour;
        }
        public void SetFired(int pDelay)
        {
            mFired = pDelay;
        }

        

        public void Rotate(float pRotate)
        {
           // StoreRotation();
            mRotation += pRotate;
        }
        public void StorePosition()
        {
            mOldPosition = mPosition;
        }

        public void StoreRotation()
        {
            mOldRotation = mRotation;
        }

        public float GetRotation()
        {
            return mRotation;
        }

        public Vector2 GetWorldPosition()
        {
            Vector3 v = LocalTransform.Translation;
            return new Vector2(v.X, v.Y);
        }
        public void Translate(float distance)
        {
            Vector3 translationVector = new Vector3(distance, 0, 0);
            translationVector = Vector3.Transform(translationVector, Matrix.CreateRotationZ(mRotation));
            //StorePosition();
            mPosition += translationVector;
        }

        public Vector2 GetIndexedCorner(int pIndex)
        {
            Vector3 temp = Vector3.Zero;
            temp.X = DGS.TANK_CORNERS[pIndex].X;
            temp.Y = DGS.TANK_CORNERS[pIndex].Y;
            temp = Vector3.Transform(temp, Matrix.CreateRotationZ(mRotation));
            temp = temp + mPosition;
            return new Vector2(temp.X, temp.Y);
        }

        public Vector2 GetRightFrontCorner()
        {
            return GetIndexedCorner(3);
        }

        public Vector2 GetLeftTrackMidpoint()
        {
            return GetLeftBackCorner();
        }

        public Vector2 GetRightTrackMidpoint()
        {
            return GetRightBackCorner();
        }

        public Vector2 GetRightBackCorner()
        {
            return GetIndexedCorner(2);
        }

        public Vector2 GetLeftFrontCorner()
        {
            return GetIndexedCorner(0);
        }

        public Vector2 GetLeftBackCorner()
        {
            return GetIndexedCorner(1);
        }

        public void GetCorners(Vector2[] pCorners)
        {
            if(pCorners.Length== 4)
            {
                Vector3 temp = Vector3.Zero;
                for(int i = 0; i < 4; i++)
                {
                    temp.X = DGS.TANK_CORNERS[i].X * mScale;
                    temp.Y = DGS.TANK_CORNERS[i].Y * mScale;
                    temp = Vector3.Transform(temp, Matrix.CreateRotationZ(mRotation));
                    temp = temp + mPosition;
                    pCorners[i].X = temp.X;
                    pCorners[i].Y = temp.Y;
                }
            }
        }

        public void BatteryPressed()
        {
            BatteryDown = true;
        }

        public void BatteryReleased()
        {
            BatteryDown = false;
        }

        public void Power1Pressed()
        {
            Power1Down = true;
        }
        public void Power1Released()
        {
            Power1Down = false;
        }
        public void Power2Released()
        {
            Power2Down = false;
        }

        public void Power2Pressed()
        {
            Power2Down = true;
        }
        public void Power3Released()
        {
            Power3Down = false;
        }

        public void Power3Pressed()
        {
            Power3Down = true;
        }


        public float GetCannonWorldRotation()
        {
            float cannonRotation = mCannonRotation;
            if (!DGS.CANNON_INDEPENDENT_FROM_TRACKS)
            {
                cannonRotation += mRotation;
            }
            return cannonRotation;
        }
        public Vector2 GetCannonWorldPosition()
        {
                return GetWorldPosition();
        }
        public void BothTracksForward(float pSeconds)
        {
            Translate(DGS.TANK_SPEED * pSeconds);
            RightTrackFrameForwards();
            LeftTrackFrameForwards();
        }
        public void BothTracksBackward(float pSeconds)
        {
            Translate(-DGS.TANK_BACKWARDS_SPEED * pSeconds);
            RightTrackFrameBackwards();
            LeftTrackFrameBackwards();
        }
        public void LeftTrackForward() {
            Rotate(DGS.BASE_TANK_ROTATION_ANGLE);
            LeftTrackFrameForwards();
            AdvancedTrackRotation(DGS.BASE_TANK_ROTATION_ANGLE, true);
        }

        private void AdvancedTrackRotation(float pAngle, bool pForwards)
        {
                float arcLength = (float)Math.Sqrt(2 * DGS.TRACK_OFFSET_SQRD - 2 * DGS.TRACK_OFFSET_SQRD * Math.Cos(pAngle));
                arcLength = pForwards ? arcLength : arcLength * -1 ;
                Vector3 translationVector = new Vector3(arcLength, 0, 0);
                translationVector = Vector3.Transform(translationVector, Matrix.CreateRotationZ(mRotation));
                //StorePosition();
            
                mPosition += translationVector;
        }
        public void RightTrackForward() { 
            Rotate(-DGS.BASE_TANK_ROTATION_ANGLE); 
            AdvancedTrackRotation(-DGS.BASE_TANK_ROTATION_ANGLE, true);
            RightTrackFrameForwards(); }

        public bool PointIsInTank(Vector2 pPoint)
        {
            Vector2[] corners = new Vector2[4];
            GetCorners(corners);
            int i;
            int j;
            bool result = false;
            for (i = 0, j = corners.Length - 1; i < corners.Length; j = i++)
            {
                if ((corners[i].Y > pPoint.Y) != (corners[j].Y > pPoint.Y) &&
                    (pPoint.X < (corners[j].X - corners[i].X) * (pPoint.Y - corners[i].Y) / (corners[j].Y - corners[i].Y) + corners[i].X))
                {
                    result = !result;
                }
            }
            return result;
        }

        public void LeftTrackBackward() {
            Rotate(-DGS.BASE_TANK_ROTATION_ANGLE);
            LeftTrackFrameBackwards();
            AdvancedTrackRotation(-DGS.BASE_TANK_ROTATION_ANGLE, false);
        }
        public void RightTrackBackward() { 
            Rotate(DGS.BASE_TANK_ROTATION_ANGLE); 
            RightTrackFrameBackwards();
            AdvancedTrackRotation(DGS.BASE_TANK_ROTATION_ANGLE, false);
        }
        public void CannonLeft() { mCannonRotation -= DGS.BASE_TURRET_ROTATION_ANGLE; }
        public void CannonRight() { mCannonRotation += DGS.BASE_TURRET_ROTATION_ANGLE; }

        public Matrix LocalTransform
        {
            get
            {
                return Matrix.CreateRotationZ(mRotation) * Matrix.CreateTranslation(mPosition);
            }
        }

        public int LeftTrackFrame()
        {
            return mLeftTrackFrame;
        }

        public int RightTrackFrame()
        {
            return mRightTrackFrame;
        }

        public void PrimingWeapon(float pSeconds)
        {
            if(mFireTimePrimed > 0)
            {
                mFireTimePrimed += pSeconds;
            }
            else
            {
                mFireTimePrimed = pSeconds;
            }
        }
        
        public bool FireIfPrimed()
        {
            if(mFireTimePrimed > 0)
            {
                float cannonRotation = GetCannonWorldRotation();
                Vector2 cannonDirection = new Vector2((float)Math.Cos(cannonRotation), (float)Math.Sin(cannonRotation));
                cannonDirection.Normalize();
                Vector2 endOfCannon = GetCannonWorldPosition() + cannonDirection * 30;
                mBullets.Add(new Bullet(endOfCannon, cannonDirection * DGS.BULLET_SPEED, Colour()));
                mFireTimePrimed = -1;
                return true;
            }
            return false;
       /*     if(m_TimePrimed > 0)
            {
                float cannonRotation = GetCannonWorldRotation();

                float bulletSpeed = m_TimePrimed * DGS.BULLET_SPEED_SCALER;

                Vector2 cannonDirection = new Vector2((float)Math.Cos(cannonRotation), (float)Math.Sin(cannonRotation));
                Vector2 endOfCannon = GetCannonWorldPosition() + cannonDirection * 30;
                m_Bullets.Add(new Bullet(endOfCannon, cannonDirection * bulletSpeed, m_TimePrimed));
                m_TimePrimed = -1;
                return true;
            }
            return false;
        */}

        public void PutBack()
        {
            mPosition = mOldPosition;
            mRotation = mOldRotation;
        }

        public bool Collide(Tank pTank)
        {
            /*
            Vector2 tankPos = pTank.GetWorldPosition();
            float tankRot = pTank.GetRotation();
            Vector2 tankPos2 = GetWorldPosition();

            if ((tankPos - tankPos2).Length() < 2 * DGS.TANK_RADIUS)
            {
                return true;
            }

            return false;
             * */
            Vector2[] thisTankCorners = new Vector2[4];
            Vector2[] otherTankCorners = new Vector2[4];
            GetCorners(thisTankCorners);
            pTank.GetCorners(otherTankCorners);

            for(int i = 0; i < 4; i++)
            {
                if(PointIsInTank(otherTankCorners[i])||pTank.PointIsInTank(thisTankCorners[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public void CollideWithPlayArea(Rectangle pRectangle)
        {
            Vector2 tankPos = GetWorldPosition();
            if ((tankPos.X <= pRectangle.Left + DGS.TANK_RADIUS) ||
                (tankPos.X >= pRectangle.Right - DGS.TANK_RADIUS) ||
                (tankPos.Y >= pRectangle.Bottom - DGS.TANK_RADIUS) ||
                (tankPos.Y <= pRectangle.Top + DGS.TANK_RADIUS))
            {
                PutBack();
            }
        }

        public void TakeDamage()
        {
            mHealth--;
            if(mHealth < 0)
            {
                mHealth = 0;
            }
        }
    }
}
