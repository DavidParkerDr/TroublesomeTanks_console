using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TroublesomeTanks.World;
using TroublesomeTanks.GUI;

namespace TroublesomeTanks
{
    public class Player
    {
        public TeamGUI GUI { get; private set; }
        public Tank Tank { get; private set; }
        public IController Controller { get; private set; }
        public Color Colour { get; private set; }
        public Avatar Avatar { get; private set; }

        public List<Bullet> Bullets { get; private set; }

        public bool ColourSet { get; private set; }
        public bool AvatarSet { get; private set; }
        public int SelectionIndex { get; set; }
        public Player(IController pController)
        {
            Controller = pController;
            ColourSet = false;
            AvatarSet = false;
            SelectionIndex = 0;
        }

        public void AddColour(Color pColour)
        {
            Colour = pColour;
        }
        public void SetColour()
        {
            ColourSet = true;
        }
        public void RemoveColour()
        {
            ColourSet = false;
            Colour = Color.White;
        }
        public void AddAvatar(Avatar pAvatar)
        {
            Avatar = pAvatar;
        }
        public void SetAvatar()
        {
            AvatarSet = true;
        }
        public void RemoveAvatar()
        {
            AvatarSet = false;
            Avatar = null;
        }        
        public Player(Color pColour, IController pController, 
            float pTankXPosition, float pTankYPosition, float pTankRotation, float pTankScale,
            Texture2D pWhitePixel,
            Texture2D pHealthBarBlackAndWhiteLayer,
            Texture2D pHealthBarColourLayer, Texture2D pAvatarBlackAndWhiteLayer,
            Texture2D pAvatarColourLayer,
            Rectangle pRectangle, bool pIsOnLeft)
        {
            Colour = pColour;
            Controller = pController;
            Controller.SetColour(pColour);
            Bullets = new List<Bullet>();
            Tank = new Tank(pTankXPosition, pTankYPosition, pTankRotation, Colour, Bullets, pTankScale);
            GUI = new TeamGUI(pWhitePixel, pHealthBarBlackAndWhiteLayer, pHealthBarColourLayer, pAvatarBlackAndWhiteLayer,
            pAvatarColourLayer, pRectangle, Tank, Controller, Colour);
        }
        public void GamePreparation(
            float pTankXPosition, float pTankYPosition, float pTankRotation, float pTankScale,
            Texture2D pHealthBarBlackAndWhiteLayer,
            Texture2D pHealthBarColourLayer, 
            Rectangle pRectangle)
        {
            TroublesomeTanks game = (TroublesomeTanks)TroublesomeTanks.Instance();
            Controller.SetColour(Colour);
            Bullets = new List<Bullet>();
            Tank = new Tank(pTankXPosition, pTankYPosition, pTankRotation, Colour, Bullets, pTankScale);
            Texture2D whitePixel = game.CM().Load<Texture2D>("white_pixel");
            GUI = new TeamGUI(whitePixel, pHealthBarBlackAndWhiteLayer, pHealthBarColourLayer, Avatar, pRectangle, Tank, Controller, Colour);
        }
        public void Reset()
        {
            Controller.ResetJacks();
            
        }
        public bool DoTankControls(float pSeconds)
        {
            bool tankMoved = false;
            Tank.StoreRotation();
            Tank.StorePosition();
            
            if (Tank.Health() > 0)
            {
                Controller.AddCharge(DGS.CHARGE_AMOUNT * pSeconds);
                Controller.AddBatteryCharge(DGS.BATTERY_CHARGE_RATE * pSeconds);
                if (Controller.GetRemainingBatteryCharge() <= 0)
                {
                    Controller.CancelBatteryAllocationMode();
                    Controller.ConnectBattery(BatteryAllocation.NOT_ALLOCATED);
                }
                if (Controller.IsPressedWithCharge(Control.TRACKS_FORWARDS))
                {
                    tankMoved = true;
                    if (Controller.IsPressedWithCharge(Control.TRACKS_RIGHT))
                    {
                        Tank.LeftTrackForward();
                        Controller.DepleteCharge(Control.TRACKS_RIGHT, DGS.TRACK_DEPLETION_RATE * pSeconds);
                    }
                    else if (Controller.IsPressedWithCharge(Control.TRACKS_LEFT))
                    {
                        Tank.RightTrackForward();
                        Controller.DepleteCharge(Control.TRACKS_LEFT, DGS.TRACK_DEPLETION_RATE * pSeconds);
                    }
                    else
                    {
                        Tank.BothTracksForward(pSeconds);
                        Controller.DepleteCharge(Control.TRACKS_FORWARDS, DGS.TRACK_DEPLETION_RATE * pSeconds);
                    }
                }
                else if (Controller.IsPressedWithCharge(Control.TRACKS_BACKWARDS))
                {
                    tankMoved = true;
                    if (Controller.IsPressedWithCharge(Control.TRACKS_RIGHT))
                    {
                        Tank.LeftTrackBackward();
                        Controller.DepleteCharge(Control.TRACKS_RIGHT, DGS.TRACK_DEPLETION_RATE * pSeconds);
                    }
                    else if (Controller.IsPressedWithCharge(Control.TRACKS_LEFT))
                    {
                        Tank.RightTrackBackward();
                        Controller.DepleteCharge(Control.TRACKS_LEFT, DGS.TRACK_DEPLETION_RATE * pSeconds);
                    }
                    else
                    {
                        Tank.BothTracksBackward(pSeconds);
                        Controller.DepleteCharge(Control.TRACKS_BACKWARDS, DGS.TRACK_DEPLETION_RATE * pSeconds);
                    }
                }
                else if (Controller.IsPressedWithCharge(Control.TRACKS_RIGHT))
                {
                    tankMoved = true;
                    Tank.LeftTrackForward();
                    Tank.RightTrackBackward();
                    Controller.DepleteCharge(Control.TRACKS_RIGHT, DGS.TRACK_DEPLETION_RATE * pSeconds);
                }
                else if (Controller.IsPressedWithCharge(Control.TRACKS_LEFT))
                {
                    tankMoved = true;
                    Tank.LeftTrackBackward();
                    Tank.RightTrackForward();

                    Controller.DepleteCharge(Control.TRACKS_LEFT, DGS.TRACK_DEPLETION_RATE * pSeconds);
                }


                if (Controller.IsPressedWithCharge(Control.TURRET_LEFT))
                {
                    Tank.CannonLeft();
                    Controller.DepleteCharge(Control.TURRET_LEFT, DGS.TURRET_DEPLETION_RATE * pSeconds);
                }
                else if (Controller.IsPressedWithCharge(Control.TURRET_RIGHT))
                {
                    Tank.CannonRight();
                    Controller.DepleteCharge(Control.TURRET_RIGHT, DGS.TURRET_DEPLETION_RATE * pSeconds);

                }

                if (Controller.IsPressedWithCharge(Control.FIRE))
                {
                    Tank.PrimingWeapon(pSeconds);
                }
                else
                {
                    if (Tank.FireIfPrimed())
                    {
                        SoundEffectInstance bulletShot = TroublesomeTanks.Instance().GetSoundManager().GetSoundEffectInstance("Sounds/Tank_Gun");
                        bulletShot.Play();
                        Controller.DepleteCharge(Control.FIRE, DGS.BULLET_CHARGE_DEPLETION); // BULLET CHARGE HERE
                        Tank.SetFired(DGS.BLAST_DELAY);
                    }
                }

                HandleBatteryAndCellControls();

            }
            else
            {
                // tank is dead
                Controller.EmptyBattery();
                Controller.EmptyAllPower();
            }
            if (Tank.Fired() > 0)
            {
                Tank.DecFired();
            }
            return tankMoved;
        }

        private void HandleBatteryAndCellControls()
        {
            if (Controller.IsPressed(Control.BATTERY))
            {
                if (!Tank.BatteryDown)
                {
                    Tank.BatteryPressed();
                    // should cancel battery allocation mode
                    Controller.CancelBatteryAllocationMode();
                    Controller.CancelControlSwapMode();
                }

            }
            else
            {
                Tank.BatteryReleased();
            }

            if (Controller.WasPressed(Control.POWER1))
            {
                if (!Controller.IsPressed(Control.POWER1))
                {
                    if (Controller.GetDuration(Control.POWER1) < DGS.LONG_PRESS_DURATION)
                    {
                        // connect the battery to power 1
                        if (!Controller.IsControlSwapMode())
                        {
                            Controller.ConnectBattery(BatteryAllocation.POWER1);
                        }
                        else
                        {
                            Controller.SwapControls(0);
                        }
                    }
                    else
                    {
                        // long press release should have triggered swap mode already           
                    }
                }
            }
            if (Controller.IsPressed(Control.POWER1) && Controller.GetDuration(Control.POWER1) >= DGS.LONG_PRESS_DURATION)
            {
                // long press so do control swap?
                // enter control swap mode with power 1
                Controller.EnterControlSwapMode(0);
            }
            if (Controller.WasPressed(Control.POWER2))
            {
                if (!Controller.IsPressed(Control.POWER2))
                {
                    if (Controller.GetDuration(Control.POWER2) < DGS.LONG_PRESS_DURATION)
                    {
                        // connect the battery to power 2
                        if (!Controller.IsControlSwapMode())
                        {
                            Controller.ConnectBattery(BatteryAllocation.POWER2);
                        }
                        else
                        {
                            Controller.SwapControls(1);
                        }
                    }
                    else
                    {
                        // long press release should have triggered swap mode already
                    }
                }
            }
            if (Controller.IsPressed(Control.POWER2) && Controller.GetDuration(Control.POWER2) >= DGS.LONG_PRESS_DURATION)
            {
                // long press so do control swap?
                // enter control swap mode with power 2
                Controller.EnterControlSwapMode(1);
            }
            if (Controller.WasPressed(Control.POWER3))
            {
                if (!Controller.IsPressed(Control.POWER3))
                {
                    if (Controller.GetDuration(Control.POWER3) < DGS.LONG_PRESS_DURATION)
                    {
                        // connect the battery to power 1
                        if (!Controller.IsControlSwapMode())
                        {
                            Controller.ConnectBattery(BatteryAllocation.POWER3);
                        }
                        else
                        {
                            Controller.SwapControls(2);
                        }
                    }
                    else
                    {
                        // long press release should have triggered swap mode already
                    }
                }
            }
            if (Controller.IsPressed(Control.POWER3) && Controller.GetDuration(Control.POWER3) >= DGS.LONG_PRESS_DURATION)
            {
                // long press so do control swap?
                // enter control swap mode with power 3
                Controller.EnterControlSwapMode(2);
            }
        }
        private void HandleBatteryAndCellControlsBatterySelection()
        {
            if (Controller.IsPressed(Control.BATTERY))
            {
                if (!Tank.BatteryDown)
                {
                    Tank.BatteryPressed();
                    if (Controller.IsBatteryAllocationMode())
                    {
                        // should cancel battery allocation mode
                        Controller.CancelBatteryAllocationMode();
                    }
                    else
                    {
                        // should be in battery allocation mode now
                        if (Controller.GetRemainingBatteryCharge() > 0)
                        {
                            Controller.CancelControlSwapMode();
                            Controller.EnterBatteryAllocationMode();
                        }
                    }
                }

            }
            else
            {
                Tank.BatteryReleased();
            }

            if (Controller.IsPressed(Control.POWER1))
            {
                if (!Tank.Power1Down)
                {
                    Tank.Power1Pressed();
                    // check to see if in battery allocation mode
                    if (Controller.IsBatteryAllocationMode())
                    {
                        // connect the battery to power 1
                        Controller.ConnectBattery(BatteryAllocation.POWER1);
                    }
                    else
                    {
                        // enter control swap mode with power 1
                        if (!Controller.IsControlSwapMode())
                        {
                            Controller.EnterControlSwapMode(0);
                        }
                        else
                        {
                            Controller.SwapControls(0);
                        }
                    }
                }
            }
            else
            {
                Tank.Power1Released();
            }
            if (Controller.IsPressed(Control.POWER2))
            {
                if (!Tank.Power2Down)
                {
                    Tank.Power2Pressed();
                    // check to see if in battery allocation mode
                    if (Controller.IsBatteryAllocationMode())
                    {
                        // connect the battery to power 2
                        Controller.ConnectBattery(BatteryAllocation.POWER2);
                    }
                    else
                    {
                        // enter control swap mode with power 2
                        if (!Controller.IsControlSwapMode())
                        {
                            Controller.EnterControlSwapMode(1);
                        }
                        else
                        {
                            Controller.SwapControls(1);
                        }
                    }
                }
            }
            else
            {
                Tank.Power2Released();
            }
            if (Controller.IsPressed(Control.POWER3))
            {
                if (!Tank.Power3Down)
                {
                    Tank.Power3Pressed();
                    // check to see if in battery allocation mode
                    if (Controller.IsBatteryAllocationMode())
                    {
                        // connect the battery to power 3
                        Controller.ConnectBattery(BatteryAllocation.POWER3);
                    }
                    else
                    {
                        // enter control swap mode with power 3
                        if (!Controller.IsControlSwapMode())
                        {
                            Controller.EnterControlSwapMode(2);
                        }
                        else
                        {
                            Controller.SwapControls(2);
                        }
                    }
                }
            }
            else
            {
                Tank.Power3Released();
            }
        }
    }
}
