using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TroublesomeTanks.World;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;
using TroublesomeTanks.World.Particles;
using TroublesomeTanks.GUI;

namespace TroublesomeTanks.Scenes
{
    public class GameScene : IScene
    {
        private IController mController0;
        private IController mController1;
        private IController mController2;
        private IController mController3;
        private TheWorld m_World;
        private Texture2D m_TankBaseTexture;
        private Texture2D m_TankBrokenTexture;
        private Texture2D m_TankRightTrackTexture;
        private Texture2D m_TankLeftTrackTexture;
        private Texture2D mPlayAreaTexture;
        private Texture2D m_CircleTexture;
        private Texture2D m_CannonTexture;
        private Texture2D m_CannonFireTexture;
        private Texture2D m_BulletTexture;
        private SpriteBatch m_SpriteBatch;
        private SoundEffectInstance introMusicInstance = null;
        private SoundEffectInstance loopMusicInstance = null;
        private SoundEffectInstance tankMoveSound = null;
        private List<Vector2> mTankPositions = new List<Vector2>();
        private List<float> mTankRotations = new List<float>();

        private const float SECONDS_BETWEEN_TRACKS_ADDED = 0.2f;
        private float m_SecondsTillTracksAdded = SECONDS_BETWEEN_TRACKS_ADDED;

        private List<Player> mTeams = new List<Player>();

        Texture2D mBackgroundTexture = null;
        Texture2D mPixelTexture = null;
        Rectangle mBackgroundRectangle;
        Rectangle mPlayAreaRectangle;
        Rectangle mPlayAreaOutlineRectangle;
        public Rectangle PlayArea { get { return mPlayAreaRectangle; } }

        // private Effect m_Shader;
        // private RenderTarget2D m_ShaderRenderTarget; // might not need this
        // private Texture2D m_ShaderTexture; // might not need this

        public GameScene(List<Player> pPlayers)
        {
            TroublesomeTanks game = (TroublesomeTanks)TroublesomeTanks.Instance();
            m_TankBaseTexture = game.CM().Load<Texture2D>("Tank-B-05");
            m_TankBrokenTexture = game.CM().Load<Texture2D>("BrokenTank");
            m_TankRightTrackTexture = game.CM().Load<Texture2D>("Tank track B-R");
            m_TankLeftTrackTexture = game.CM().Load<Texture2D>("Tank track B-L");
            m_CannonTexture = game.CM().Load<Texture2D>("cannon");
            m_CannonFireTexture = game.CM().Load<Texture2D>("cannonFire");
            m_BulletTexture = game.CM().Load<Texture2D>("circle");
            mPlayAreaTexture = game.CM().Load<Texture2D>("playArea");
            mPixelTexture = game.CM().Load<Texture2D>("block");
            TrackSystem.SetupStaticMembers(game.CM().Load<Texture2D>("track"));
            
            JackIcon.SetupStaticTextures(
                game.CM().Load<Texture2D>("tracks_icon"),
                game.CM().Load<Texture2D>("gun_icon"),
                game.CM().Load<Texture2D>("fire_icon"),
                game.CM().Load<Texture2D>("charge_icon"),
                game.CM().Load<Texture2D>("battery_icon"));

            PowerBar.SetupStaticTextures(game.CM().Load<Texture2D>("horizontal_power_border"),
                game.CM().Load<Texture2D>("horizontal_power_bar"), game.CM().Load<Texture2D>("horizontal_power_layer"));

            Battery.SetupStaticTextures(game.CM().Load<Texture2D>("vertical_battery_border"),
                game.CM().Load<Texture2D>("vertical_battery_bar"), game.CM().Load<Texture2D>("vertical_battery_layer"));

            m_CircleTexture = game.CM().Load<Texture2D>("circle");

            m_SpriteBatch = new SpriteBatch(game.GDM().GraphicsDevice);

            /*m_Shader = game.CM().Load<Effect>("shader");
            m_ShaderRenderTarget = new RenderTarget2D(game.GDM().GraphicsDevice,
                game.GDM().GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GDM().GraphicsDevice.PresentationParameters.BackBufferHeight);
            m_ShaderTexture = new Texture2D(game.GDM().GraphicsDevice,
                game.GDM().GraphicsDevice.PresentationParameters.BackBufferWidth,
                game.GDM().GraphicsDevice.PresentationParameters.BackBufferHeight, false, m_ShaderRenderTarget.Format);
                */
            mBackgroundTexture = game.CM().Load<Texture2D>("background_01");
            mBackgroundRectangle = new Rectangle(0, 0, game.GDM().GraphicsDevice.Viewport.Width, game.GDM().GraphicsDevice.Viewport.Height);
            mPlayAreaRectangle = new Rectangle(game.GDM().GraphicsDevice.Viewport.Width * 2 / 100, game.GDM().GraphicsDevice.Viewport.Height * 25 / 100, game.GDM().GraphicsDevice.Viewport.Width * 96 / 100, game.GDM().GraphicsDevice.Viewport.Height * 73 / 100);
            mPlayAreaOutlineRectangle = new Rectangle(mPlayAreaRectangle.X - 5, mPlayAreaRectangle.Y - 5, mPlayAreaRectangle.Width + 10, mPlayAreaRectangle.Height + 10);
            introMusicInstance = game.ReplaceCurrentMusicInstance("Music/Music_intro", false);

            mController0 = TroublesomeTanks.Instance().Controller0();
            mController1 = TroublesomeTanks.Instance().Controller1();
            mController2 = TroublesomeTanks.Instance().Controller2();
            mController3 =  TroublesomeTanks.Instance().Controller3();
            TroublesomeTanks.Instance().ResetControllers();

            //loopMusicInstance = game.GetSoundManager().GetLoopableSoundEffectInstance("Music/Music_loopable");  // Put the name of your song here instead of "song_title"
            // game.ReplaceCurrentMusicInstance("Music/Music_loopable", true);
            tankMoveSound = game.GetSoundManager().GetLoopableSoundEffectInstance("Sounds/Tank_Tracks");  // Put the name of your song here instead of "song_title"

            mTeams = pPlayers;
            if (mTeams.Count < 4)
            {
                setupNot4Player(mPlayAreaRectangle);
            }
            else
            {
                setup4Player(mPlayAreaRectangle);
            }
            foreach (Player p in mTeams)
            {
                m_World.AddTank(p.Tank);
            }
        }

        private void setupMap1(Rectangle pPlayArea)
        {
            mTankPositions = new List<Vector2>();
            mTankRotations = new List<float>();

            int middleBlockHeight = pPlayArea.Height / 3;
            int outerBlockHeight = pPlayArea.Height / 5;
            int blockThickness = pPlayArea.Width / 50;
            int outerBlockXOffset = pPlayArea.Width / 8;

            List<RectWall> Walls = new List<RectWall>();

            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + 3 * outerBlockXOffset - 2 * blockThickness, pPlayArea.Y + middleBlockHeight, blockThickness, middleBlockHeight)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + pPlayArea.Width - 3 * outerBlockXOffset + blockThickness, pPlayArea.Y + middleBlockHeight, blockThickness, middleBlockHeight)));

            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + pPlayArea.Width - 3 * outerBlockXOffset + 2 * blockThickness, pPlayArea.Y + (pPlayArea.Height - blockThickness) / 2, (pPlayArea.X + pPlayArea.Width) - (pPlayArea.X + pPlayArea.Width - 3 * outerBlockXOffset + 2 * blockThickness), blockThickness)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X, pPlayArea.Y + (pPlayArea.Height - blockThickness) / 2, (pPlayArea.X + pPlayArea.Width) - (pPlayArea.X + pPlayArea.Width - 3 * outerBlockXOffset + 2 * blockThickness), blockThickness)));

            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + (pPlayArea.Width - blockThickness) / 2, pPlayArea.Y, blockThickness, middleBlockHeight)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + (pPlayArea.Width - blockThickness) / 2, pPlayArea.Y + pPlayArea.Height - middleBlockHeight, blockThickness, middleBlockHeight)));
            m_World = new TheWorld(pPlayArea, Walls);

            if (mTeams.Count == 2)
            {
                float xPosition = pPlayArea.X + outerBlockXOffset;
                float yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                Vector2 tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add(0);

                xPosition = pPlayArea.X + pPlayArea.Width - outerBlockXOffset;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI);
            }
            else if (mTeams.Count == 3)
            {
                float xPosition = pPlayArea.X + outerBlockXOffset;
                float yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                Vector2 tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add(0);

                xPosition = pPlayArea.X + pPlayArea.Width - outerBlockXOffset;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI);

                xPosition = pPlayArea.Width / 2 + 35;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI / 2);
            }
            else if (mTeams.Count == 4)
            {
                float xPosition = pPlayArea.X + outerBlockXOffset / 2;
                float yPosition = pPlayArea.Y + outerBlockXOffset / 2;
                Vector2 tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)(-Math.PI + Math.PI / 4));

                xPosition = pPlayArea.X + pPlayArea.Width - outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)-Math.PI / 4);

                xPosition = pPlayArea.X + outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height - outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)(Math.PI / 2 + Math.PI / 4));

                xPosition = pPlayArea.X + pPlayArea.Width - outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height - outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI / 4);
            }
            else if (mTeams.Count == 5)
            {
                float xPosition = pPlayArea.X + pPlayArea.Width / 2 + 0 - outerBlockXOffset / 2;
                float yPosition = pPlayArea.Y + pPlayArea.Height / 2 - outerBlockXOffset / 2;
                Vector2 tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)(-Math.PI + Math.PI / 4));

                xPosition = pPlayArea.X + pPlayArea.Width / 2 + 0 + outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2 - outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)-Math.PI / 4);

                xPosition = pPlayArea.X + pPlayArea.Width / 2 + 0 - outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2 + outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)(Math.PI / 2 + Math.PI / 4));

                xPosition = pPlayArea.X + pPlayArea.Width / 2 + 0 + outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2 + outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI / 4);
            }
        }

        private void setupMap2(Rectangle pPlayArea)
        {
            mTankPositions = new List<Vector2>();
            mTankRotations = new List<float>();

            int middleBlockHeight = pPlayArea.Height / 3;
            int outerBlockHeight = pPlayArea.Height / 5;
            int blockThickness = pPlayArea.Width / 50;
            int outerBlockXOffset = pPlayArea.Width / 8;

            List<RectWall> Walls = new List<RectWall>();

            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + outerBlockXOffset, pPlayArea.Y + outerBlockHeight, blockThickness, outerBlockHeight)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + outerBlockXOffset, pPlayArea.Y + outerBlockHeight, outerBlockHeight, blockThickness)));

            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + outerBlockXOffset, pPlayArea.Y + 3 * outerBlockHeight, blockThickness, outerBlockHeight)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + outerBlockXOffset, pPlayArea.Y + 4 * outerBlockHeight - blockThickness, outerBlockHeight, blockThickness)));

            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + pPlayArea.Width - outerBlockXOffset - blockThickness, pPlayArea.Y + outerBlockHeight, blockThickness, outerBlockHeight)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + pPlayArea.Width - outerBlockXOffset - outerBlockHeight, pPlayArea.Y + outerBlockHeight, outerBlockHeight, blockThickness)));


            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + pPlayArea.Width - outerBlockXOffset - blockThickness, pPlayArea.Y + 3 * outerBlockHeight, blockThickness, outerBlockHeight)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + pPlayArea.Width - outerBlockXOffset - outerBlockHeight, pPlayArea.Y + 4 * outerBlockHeight - blockThickness, outerBlockHeight, blockThickness)));


            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + 3 * outerBlockXOffset - 2 * blockThickness, pPlayArea.Y + middleBlockHeight, blockThickness, middleBlockHeight)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + pPlayArea.Width - 3 * outerBlockXOffset + blockThickness, pPlayArea.Y + middleBlockHeight, blockThickness, middleBlockHeight)));

            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + pPlayArea.Width - 3 * outerBlockXOffset + 2 * blockThickness, pPlayArea.Y + (pPlayArea.Height - blockThickness) / 2, outerBlockHeight, blockThickness)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + 3 * outerBlockXOffset - outerBlockHeight - 2 * blockThickness, pPlayArea.Y + (pPlayArea.Height - blockThickness) / 2, outerBlockHeight, blockThickness)));

            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + (pPlayArea.Width - blockThickness) / 2, pPlayArea.Y, blockThickness, middleBlockHeight)));
            Walls.Add(new RectWall(TroublesomeTanks.Instance().CM().Load<Texture2D>("block"),
                new Rectangle(pPlayArea.X + (pPlayArea.Width - blockThickness) / 2, pPlayArea.Y + pPlayArea.Height - middleBlockHeight, blockThickness, middleBlockHeight)));
            m_World = new TheWorld(pPlayArea, Walls);

            if (mTeams.Count == 2)
            {
                float xPosition = pPlayArea.X + outerBlockXOffset;
                float yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                Vector2 tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add(0);

                xPosition = pPlayArea.X + pPlayArea.Width - outerBlockXOffset;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI);
            }
            else if (mTeams.Count == 3)
            {
                float xPosition = pPlayArea.X + outerBlockXOffset;
                float yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                Vector2 tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add(0);

                xPosition = pPlayArea.X + pPlayArea.Width - outerBlockXOffset;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI);

                xPosition = pPlayArea.Width / 2 + 35;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI / 2);
            }
            else if (mTeams.Count == 4)
            {
                float xPosition = pPlayArea.X + outerBlockXOffset / 2;
                float yPosition = pPlayArea.Y + outerBlockXOffset / 2;
                Vector2 tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)(-Math.PI + Math.PI / 4));

                xPosition = pPlayArea.X + pPlayArea.Width - outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)-Math.PI / 4);

                xPosition = pPlayArea.X + outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height - outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)(Math.PI / 2 + Math.PI / 4));

                xPosition = pPlayArea.X + pPlayArea.Width - outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height - outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI / 4);
            }
            else if (mTeams.Count == 5)
            {
                float xPosition = pPlayArea.X + pPlayArea.Width / 2 + 0 - outerBlockXOffset / 2;
                float yPosition = pPlayArea.Y + pPlayArea.Height / 2 - outerBlockXOffset / 2;
                Vector2 tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)(-Math.PI + Math.PI / 4));

                xPosition = pPlayArea.X + pPlayArea.Width / 2 + 0 + outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2 - outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)-Math.PI / 4);

                xPosition = pPlayArea.X + pPlayArea.Width / 2 + 0 - outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2 + outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)(Math.PI / 2 + Math.PI / 4));

                xPosition = pPlayArea.X + pPlayArea.Width / 2 + 0 + outerBlockXOffset / 2;
                yPosition = pPlayArea.Y + pPlayArea.Height / 2 + outerBlockXOffset / 2;
                tankPosition = new Vector2(xPosition, yPosition);
                mTankPositions.Add(tankPosition);
                mTankRotations.Add((float)Math.PI / 4);
            }
        }

        private void setupPlayers(Rectangle pPlayArea)
        {
            TroublesomeTanks game = (TroublesomeTanks)TroublesomeTanks.Instance();

            float tankScale = (float)pPlayArea.Width / (50 * 40);
            int textureWidth = game.GDM().GraphicsDevice.Viewport.Width / 4;
            int spacePerPlayer = game.GDM().GraphicsDevice.Viewport.Width / mTeams.Count;
            int textureHeight = game.GDM().GraphicsDevice.Viewport.Height * 24 / 100;
            for(int i = 0; i < mTeams.Count; i++)
            {
                mTeams[i].GamePreparation(
                mTankPositions[i].X, mTankPositions[i].Y, mTankRotations[i], tankScale,
                game.CM().Load<Texture2D>("healthbars/heart_bw"),
                game.CM().Load<Texture2D>("healthbars/heart_colour"),
                new Rectangle((int)(i * spacePerPlayer + (spacePerPlayer - textureWidth) * 0.5f), 0, textureWidth, textureHeight));
            }
            
        }

        private void setup4Player(Rectangle pPlayArea)
        {
            setupMap1(pPlayArea);
            setupPlayers(pPlayArea);
        }

        private void setupNot4Player(Rectangle pPlayArea)
        {
            setupMap2(pPlayArea);
            setupPlayers(pPlayArea);
        }

        private void IntroFinished()
        {
            if (introMusicInstance.State == SoundState.Stopped)
            {
                TroublesomeTanks game = (TroublesomeTanks)TroublesomeTanks.Instance();
                game.ReplaceCurrentMusicInstance("Music/Music_loopable", true);
            }
        }


        public void Draw(float pSeconds)
        {
            TroublesomeTanks.Instance().GDM().GraphicsDevice.Clear(Color.CornflowerBlue);

            m_SpriteBatch.Begin();

            m_SpriteBatch.Draw(mBackgroundTexture, mBackgroundRectangle, Color.White);

            foreach (Player player in mTeams)
            {
                if (player.GUI != null)
                {
                    player.GUI.Draw(m_SpriteBatch);
                }
            }

            m_SpriteBatch.Draw(mPixelTexture, mPlayAreaOutlineRectangle, Color.Black);
            m_SpriteBatch.Draw(mPixelTexture, mPlayAreaRectangle, DGS.COLOUR_GROUND);

            TrackSystem.GetInstance().Draw(m_SpriteBatch);

            // bullet background
            int bulletRadius = 10;
            int radius = bulletRadius + 2 * DGS.PARTICLE_EDGE_THICKNESS;
            Rectangle bulletRect = new Rectangle(0, 0, radius, radius);
            foreach (Player p in mTeams)
            {
                List<Bullet> bullets = p.Tank.GetBulletList();
                foreach (Bullet b in bullets)
                {
                    bulletRect.X = (int)b.Position.X - radius / 2;
                    bulletRect.Y = (int)b.Position.Y - radius / 2;
                    m_SpriteBatch.Draw(m_BulletTexture, bulletRect, Color.Black);
                }
            }

            World.Particles.ParticleManager.Instance().Draw(m_SpriteBatch);

            // bullet colour
            bulletRect.Width = bulletRadius;
            bulletRect.Height = bulletRadius;
            foreach (Player p in mTeams)
            {
                List<Bullet> bullets = p.Tank.GetBulletList();
                foreach (Bullet b in bullets)
                {
                    bulletRect.X = (int)b.Position.X - bulletRadius / 2;
                    bulletRect.Y = (int)b.Position.Y - bulletRadius / 2;
                    m_SpriteBatch.Draw(m_BulletTexture, bulletRect, b.Colour);
                }
            }

            Rectangle trackRect = new Rectangle(0, 0, m_TankLeftTrackTexture.Width, m_TankLeftTrackTexture.Height / 15);

            float tankScale = (float)mPlayAreaRectangle.Width / (50 * 40);

            for (int i = 0; true; i++)
            {
                Tank t = m_World.GetTank(i);
                if (t == null)
                {
                    break;
                }
                if (t.Health() > 0)
                {
                    trackRect.Y = t.LeftTrackFrame() * m_TankLeftTrackTexture.Height / 15;
                    m_SpriteBatch.Draw(m_TankLeftTrackTexture, t.GetWorldPosition(), trackRect, t.Colour(), t.GetRotation(), new Vector2(m_TankBaseTexture.Width / 2, m_TankBaseTexture.Height / 2), tankScale, SpriteEffects.None, 0.0f);
                    trackRect.Y = t.RightTrackFrame() * m_TankLeftTrackTexture.Height / 15;
                    m_SpriteBatch.Draw(m_TankRightTrackTexture, t.GetWorldPosition(), trackRect, t.Colour(), t.GetRotation(), new Vector2(m_TankBaseTexture.Width / 2, m_TankBaseTexture.Height / 2), tankScale, SpriteEffects.None, 0.0f);
                    m_SpriteBatch.Draw(m_TankBaseTexture, t.GetWorldPosition(), null, t.Colour(), t.GetRotation(), new Vector2(m_TankBaseTexture.Width / 2, m_TankBaseTexture.Height / 2), tankScale, SpriteEffects.None, 0.0f);
                    if (t.Fired() == 0)
                    {
                        m_SpriteBatch.Draw(m_CannonTexture, t.GetCannonWorldPosition(), null, t.Colour(), t.GetCannonWorldRotation(), new Vector2(m_CannonTexture.Width / 2, m_CannonTexture.Height / 2), tankScale, SpriteEffects.None, 0.0f);
                    }
                    else
                    {
                        m_SpriteBatch.Draw(m_CannonFireTexture, t.GetCannonWorldPosition(), null, t.Colour(), t.GetCannonWorldRotation(), new Vector2(m_CannonTexture.Width / 2, m_CannonTexture.Height / 2), tankScale, SpriteEffects.None, 0.0f);
                    }
                }
                else
                {
                    m_SpriteBatch.Draw(m_TankBrokenTexture, t.GetWorldPosition(), null, t.Colour(), t.GetRotation(), new Vector2(m_TankBrokenTexture.Width / 2, m_TankBrokenTexture.Height / 2), tankScale, SpriteEffects.None, 0.0f);
                }
            }

     /*       Vector2 bulletOffset = new Vector2(m_BulletTexture.Width / 2, m_BulletTexture.Height / 2);
            foreach (Player p in m_Teams)
            {
                List<Bullet> bullets = p.Tank.GetBulletList();
                foreach (Bullet b in bullets)
                {
                    m_SpriteBatch.Draw(m_BulletTexture, b.Position, null, p.Colour, 0, bulletOffset, 1f, SpriteEffects.None, 0.0f);
                }
            }
            */
            foreach (RectWall w in m_World.Walls)
            {
                w.DrawOutlines(m_SpriteBatch);
            }

            foreach (RectWall w in m_World.Walls)
            {
                w.Draw(m_SpriteBatch);
            }

            m_SpriteBatch.End();
        }

        public void Escape()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                TroublesomeTanks.Instance().SM().Transition(null);
            }
        }

        public void Update(float pSeconds)
        {
            Escape();

            IntroFinished();
            bool tankMoved = false;
            List<int> remainingTeamsList = remainingTeams();
            if (remainingTeamsList.Count <= 1 && m_World.LiveBulletCount() <= 0)
            {
                int winner = -1;
                if (remainingTeamsList.Count == 1)
                {
                    winner = remainingTeamsList[0];
                }
                Reset();
                TroublesomeTanks.Instance().SM().Transition(new GameOverScene(mBackgroundTexture, mTeams, winner));

            }
            if (remainingTeamsList.Count > 1)
            {
                TroublesomeTanks.Instance().UpdateControllers(pSeconds);
                

                foreach (Player p in mTeams)
                {
                    tankMoved = tankMoved | p.DoTankControls(pSeconds);
                }
            }

            m_World.Update(pSeconds);

            m_World.CollideAllTheThingsWithPlayArea(mPlayAreaRectangle);

            if (tankMoved)
            {
                tankMoveSound.Play();
            }
            else
            {
                tankMoveSound.Pause();
            }
            

            m_SecondsTillTracksAdded -= pSeconds;
            if(m_SecondsTillTracksAdded <= 0)
            {
                m_SecondsTillTracksAdded += SECONDS_BETWEEN_TRACKS_ADDED;
                TrackSystem trackSystem = TrackSystem.GetInstance();
                foreach (Player p in mTeams)
                {
                    trackSystem.AddTrack(p.Tank.GetWorldPosition(), p.Tank.GetRotation(), p.Tank.Colour());
                }
            }
        }

        private void Reset()
        {
            foreach (Player p in mTeams)
            {
                p.Reset();
            }
        }

        private List<int> remainingTeams()
        {
            List<int> remaining = new List<int>();
            int index = 0;
            foreach (Player player in mTeams)
            {
                if (player.Tank.Health() > 0)
                {
                    remaining.Add(index);
                }
                index++;
            }
            return remaining;
        }
    }
}

