using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using TroublesomeTanks.World;
using Microsoft.Xna.Framework;

namespace TroublesomeTanks
{
    public enum Control { TRACKS_FORWARDS = 0, TRACKS_BACKWARDS = 1, TRACKS_LEFT = 2, TRACKS_RIGHT = 3, FIRE = 4, BATTERY = 5, TURRET_LEFT = 6, TURRET_RIGHT = 8, POWER1 = 9, POWER2 = 10, POWER3 = 11 };


    public enum ControlGroup { TRACKS = 0, GUN = 1, FIRE = 2, CHARGE = 3, BATTERY = 4, NONE = 5 };

    public enum BatteryAllocation { NOT_ALLOCATED = 0, PENDING_ALLOCATION = 1, POWER1 = 2, POWER2 = 3, POWER3 = 4 };

    public interface IController
    {

        bool IsPressedWithCharge(Control pControl);
        void DepleteCharge(Control pControl, float amount);
        void DepleteBatteryCharge(float amount);
        void AddBatteryCharge(float amount);
        void EmptyBattery();
        void EmptyAllPower();
        void AddCharge(float amount);
        float GetJackCharge(int pJackIndex);
        ControlGroup GetJackControl(int pJackIndex);
        bool IsPressed(Control pControl);
        float GetDuration(Control pControl);
        bool WasPressed(Control pControl);
        void ResetJacks();
        void ResetControls();

        void SetColour(Color pColour);

        void UpdateController(float pSeconds);

        void EnterControlSwapMode(int pIndex);
        void CancelControlSwapMode();
        void SwapControls(int pIndex);
        bool IsControlSwapMode();
        void EnterBatteryAllocationMode();
        void CancelBatteryAllocationMode();
        bool IsBatteryAllocationMode();
        void ConnectBattery(BatteryAllocation pAllocation);
        int GetBatteryIndex();
        int GetSwapIndex();
        bool IsBatteryAllocated();
        float GetRemainingBatteryCharge();
        void ResetPresses();
    }

    public abstract class Controller : IController
    {
        protected class Jack {
            public ControlGroup mControl;
            public bool IsDown;
            public int[] LED_IDS = new int[4];
            public float charge;   
               
     
            public Jack()
            {
                ResetCharge();
            }
            public void ResetCharge()
            {
                charge = DGS.STARTING_CHARGE;
            }

            
        };
        private BatteryAllocation mBatteryAllocation;
        protected int mSwapIndex;
        protected Color mColour;
        protected Jack[] mJacks;

        protected Dictionary<Control, bool> mControlsDown;
        protected Dictionary<Control, bool> mControlsWereDown;
        protected Dictionary<Control, float> mControlsDownDuration;
        protected float mRemainingBatteryCharge;

        protected Controller()
        {
            mJacks = new Jack[3] { new Jack(), new Jack(), new Jack() };
            mControlsDown = new Dictionary<Control, bool>();
            mControlsDown[Control.TRACKS_FORWARDS] = false;
            mControlsDown[Control.TRACKS_BACKWARDS] = false;
            mControlsDown[Control.TRACKS_LEFT] = false;
            mControlsDown[Control.TRACKS_RIGHT] = false;
            mControlsDown[Control.TURRET_LEFT] = false;
            mControlsDown[Control.TURRET_RIGHT] = false;
            mControlsDown[Control.FIRE] = false;
            mControlsDown[Control.BATTERY] = false;
            mControlsDown[Control.POWER1] = false;
            mControlsDown[Control.POWER2] = false;
            mControlsDown[Control.POWER3] = false;
            mControlsWereDown = new Dictionary<Control, bool>();
            mControlsWereDown[Control.TRACKS_FORWARDS] = false;
            mControlsWereDown[Control.TRACKS_BACKWARDS] = false;
            mControlsWereDown[Control.TRACKS_LEFT] = false;
            mControlsWereDown[Control.TRACKS_RIGHT] = false;
            mControlsWereDown[Control.TURRET_LEFT] = false;
            mControlsWereDown[Control.TURRET_RIGHT] = false;
            mControlsWereDown[Control.FIRE] = false;
            mControlsWereDown[Control.BATTERY] = false;
            mControlsWereDown[Control.POWER1] = false;
            mControlsWereDown[Control.POWER2] = false;
            mControlsWereDown[Control.POWER3] = false;
            mControlsDownDuration = new Dictionary<Control, float>();
            mControlsDownDuration[Control.TRACKS_FORWARDS] = 0f;
            mControlsDownDuration[Control.TRACKS_BACKWARDS] = 0f;
            mControlsDownDuration[Control.TRACKS_LEFT] = 0f;
            mControlsDownDuration[Control.TRACKS_RIGHT] = 0f;
            mControlsDownDuration[Control.TURRET_LEFT] = 0f;
            mControlsDownDuration[Control.TURRET_RIGHT] = 0f;
            mControlsDownDuration[Control.FIRE] = 0f;
            mControlsDownDuration[Control.BATTERY] = 0f;
            mControlsDownDuration[Control.POWER1] = 0f;
            mControlsDownDuration[Control.POWER2] = 0f;
            mControlsDownDuration[Control.POWER3] = 0f;
            mSwapIndex = -1;
            mBatteryAllocation = BatteryAllocation.NOT_ALLOCATED;
            mRemainingBatteryCharge = DGS.BATTERY_STARTING_CHARGE;
        }

        public void ResetControls()
        {
            mJacks[0].mControl = ControlGroup.TRACKS;
            mJacks[1].mControl = ControlGroup.GUN;
            mJacks[2].mControl = ControlGroup.FIRE;
        }

        public void ResetPresses()
        {
            for(int i = 0; i < mControlsWereDown.Count; i++)
            {
                KeyValuePair<Control, bool> kvp = mControlsWereDown.ElementAt(i);
                mControlsWereDown[kvp.Key] = false;
                mControlsDown[kvp.Key] = false;
                mControlsDownDuration[kvp.Key] = 0f;
            }
        }

        public void EnterBatteryAllocationMode()
        {
            mBatteryAllocation = BatteryAllocation.PENDING_ALLOCATION;
        }

        public void CancelBatteryAllocationMode()
        {
            mBatteryAllocation = BatteryAllocation.NOT_ALLOCATED;
        }

        public bool IsBatteryAllocationMode()
        {
            return mBatteryAllocation == BatteryAllocation.NOT_ALLOCATED;
        }
        public bool IsBatteryAllocated()
        {
            if(mBatteryAllocation == BatteryAllocation.POWER1 || mBatteryAllocation == BatteryAllocation.POWER2|| mBatteryAllocation == BatteryAllocation.POWER3)
            {
                return true;
            }
            return false;
        }
        public void EmptyAllPower()
        {
            foreach(Jack jack in mJacks)
            {
                jack.charge = 0;
            }
        }
        public void EmptyBattery()
        {
            mRemainingBatteryCharge = 0;
        }

        public void SetControlsWereDown(float pSeconds)
        {
            foreach(KeyValuePair<Control, bool> kvp in mControlsDown)
            {
                bool previousState = mControlsWereDown[kvp.Key];
                bool currentState = mControlsDown[kvp.Key];
                if(currentState)
                {
                    //button is still down or just pressed so add time elapsed
                    float previousDuration = mControlsDownDuration[kvp.Key];
                    mControlsDownDuration[kvp.Key] = previousDuration + pSeconds;
                }
                else if(previousState)
                {
                    //button has been released so reset the time to zero
                    mControlsDownDuration[kvp.Key] = 0f;
                }
                mControlsWereDown[kvp.Key] = currentState;
            }
        }
        public float GetDuration(Control pControl)
        {
            return mControlsDownDuration[pControl];
        }
        public ControlGroup GetControlGroup(Control pControl)
        {
            switch (pControl)
            {
                case Control.TRACKS_FORWARDS:
                case Control.TRACKS_BACKWARDS:
                case Control.TRACKS_LEFT:
                case Control.TRACKS_RIGHT:
                    return ControlGroup.TRACKS;
                case Control.TURRET_LEFT:
                case Control.TURRET_RIGHT:
                    return ControlGroup.GUN;
                case Control.FIRE:
                    return ControlGroup.FIRE;
                default: return ControlGroup.NONE;
            }
        }

        public void SetColour(Color pColour)
        {
            mColour = pColour;
        }
        public void ResetJacks()
        {
            foreach(Jack j in mJacks)
            {
                j.ResetCharge();
            }
        }
        public float GetRemainingBatteryCharge()
        {
            return mRemainingBatteryCharge;
        }

        public ControlGroup GetJackControl(int pJackIndex)
        {
            return mJacks[pJackIndex].mControl;
        }

        public float GetJackCharge(int pJackIndex)
        {
            return mJacks[pJackIndex].charge;
        }

        public abstract void UpdateController(float pSeconds);

        public bool IsPressedWithCharge(Control pControl)
        {
            ControlGroup controlGroup = GetControlGroup(pControl);
            for ( int i = 0; i < mJacks.Length; ++i)
            {
                if (mJacks[i].mControl == controlGroup)
                {
                    if(i == GetBatteryIndex())
                    {
                        return false;
                    }
                    return mControlsDown[pControl] && ((controlGroup == ControlGroup.FIRE && mJacks[i].charge >= DGS.BULLET_CHARGE_DEPLETION) || (controlGroup != ControlGroup.FIRE && mJacks[i].charge > 0));
                }
            }
            return false;
        }
        public void DepleteCharge(Control pControl, float amount)
        {
            ControlGroup controlGroup = GetControlGroup(pControl);
            for (int i = 0; i < mJacks.Length; ++i)
            {
                if (mJacks[i].mControl == controlGroup)
                {
                    if (mJacks[i].charge > amount)
                    {

                        mJacks[i].charge -= amount;
                    }
                    else
                        mJacks[i].charge = 0;
                }
            }
        }

        public void DepleteBatteryCharge(float pAmount)
        {
            mRemainingBatteryCharge -= pAmount;
        }

        public void AddBatteryCharge(float pAmount)
        {
            if (DGS.FINITE_POWER && DGS.BATTERY_CAN_RECHARGE)
            {
                if (mBatteryAllocation == BatteryAllocation.NOT_ALLOCATED)
                {
                    mRemainingBatteryCharge += pAmount;
                    if (mRemainingBatteryCharge > DGS.MAX_BATTERY_CHARGE)
                    {
                        mRemainingBatteryCharge = DGS.MAX_BATTERY_CHARGE;
                    }
                }
            }
        }

        public int GetBatteryIndex()
        {
            if(mBatteryAllocation == BatteryAllocation.POWER1)
            {
                return 0;
            }
            else if (mBatteryAllocation == BatteryAllocation.POWER2)
            {
                return 1;
            }
            else if (mBatteryAllocation == BatteryAllocation.POWER3)
            {
                return 2;
            }
            return -1;
        }

        public int GetSwapIndex()
        {
            return mSwapIndex;
        }

        public void AddCharge(float pAmount)
        {
            int batteryIndex = GetBatteryIndex();
            
            if (batteryIndex != -1)
            {
                
                float chargeRemaining = DGS.MAX_CHARGE - mJacks[batteryIndex].charge;
                if (chargeRemaining < pAmount)
                {
                    pAmount = chargeRemaining;
                }
                if (GetRemainingBatteryCharge() < pAmount)
                {
                    pAmount = GetRemainingBatteryCharge();
                }
                    mJacks[batteryIndex].charge += pAmount;
                if(DGS.FINITE_POWER)
                {
                    // decrease the amount of energy in the battery
                    DepleteBatteryCharge(pAmount);
                }
                if(mJacks[batteryIndex].charge >= DGS.MAX_CHARGE || GetRemainingBatteryCharge() <= 0)
                {
                    // disconnect the battery
                    CancelBatteryAllocationMode();
                }
            }
            
        }

        public bool IsPressed(Control pControl)
        {
            return mControlsDown[pControl];
        }

        public bool WasPressed(Control pControl)
        {
            return mControlsWereDown[pControl];
        }

        public void EnterControlSwapMode(int pIndex)
        {
            mSwapIndex = pIndex;
        }

        public bool IsControlSwapMode()
        {
            return mSwapIndex != -1;
        }

        public void CancelControlSwapMode()
        {
            mSwapIndex = -1;
        }

        public void SwapControls(int pIndex)
        {
            ControlGroup tempControlGroup = GetJackControl(mSwapIndex);
            mJacks[mSwapIndex].mControl = mJacks[pIndex].mControl;
            mJacks[pIndex].mControl = tempControlGroup;
            CancelControlSwapMode();
        }

        public void ConnectBattery(BatteryAllocation pAllocation)
        {
            mBatteryAllocation = pAllocation;
        }
    }

    public class KeyboardController : Controller
    {
        private Dictionary<Keys, Control> m_KeyMap;

        public KeyboardController(Dictionary<Keys, Control> pKeyMap) : base()
        {
            m_KeyMap = pKeyMap;

            ResetControls();
        }

        public override void UpdateController(float pSeconds)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            SetControlsWereDown(pSeconds);
            foreach (KeyValuePair<Keys, Control> kvp in m_KeyMap)
            {                
                mControlsDown[kvp.Value] = keyboardState.IsKeyDown(kvp.Key);                
            }
        }
    }

    public class XBoxController : Controller
    {
        private Dictionary<Buttons, Control> mButtonMap;
        private PlayerIndex mPlayerId;
        public XBoxController(Dictionary<Buttons, Control> pButtonMap, PlayerIndex pPlayerId) : base()
        {
            mButtonMap = pButtonMap;

            ResetControls();
            mPlayerId = pPlayerId;
        }

        public override void UpdateController(float pSeconds)
        {
            // Check the device for Player One
            GamePadCapabilities capabilities = GamePad.GetCapabilities(
                                               mPlayerId);
            SetControlsWereDown(pSeconds);
            // If there a controller attached, handle it
            if (capabilities.IsConnected)
            {
                // Get the current state of Controller1
                GamePadState state = GamePad.GetState(mPlayerId);

                // You can check explicitly if a gamepad has support for a certain feature
                
                    // You can also check the controllers "type"
                if (capabilities.GamePadType == GamePadType.GamePad)
                {
                    bool firePressed = false;
                    foreach (KeyValuePair<Buttons, Control> kvp in mButtonMap)
                    {
                        if(kvp.Value == Control.FIRE)
                        {
                            firePressed = firePressed | state.IsButtonDown(kvp.Key);
                        }
                        else
                        {
                            mControlsDown[kvp.Value] = state.IsButtonDown(kvp.Key);
                        }
                        
                    }
                    mControlsDown[Control.FIRE] = firePressed;
                    Vector2 thumbstickVector = state.ThumbSticks.Left;
                    if(thumbstickVector.Length() > 0.5f)
                    {
                        // the thumbstick is not in the centre position
                        double angleWithXAxis = Math.Atan2(thumbstickVector.Y, thumbstickVector.X);
                        if(angleWithXAxis < 0)
                        {
                            angleWithXAxis += Math.PI * 2;
                        }
                        else if(angleWithXAxis > Math.PI * 2)
                        {
                            angleWithXAxis -= Math.PI * 2;
                        }
                        double halfBoundarySize = Math.PI / 4;
                        double downAngle = Math.PI / 2;
                        double lowerDownBoundary = downAngle - halfBoundarySize;
                        double upperDownBoundary = downAngle + halfBoundarySize;
                        double leftAngle = Math.PI;
                        double lowerLeftBoundary = leftAngle - halfBoundarySize;
                        double upperLeftBoundary = leftAngle + halfBoundarySize;
                        double upAngle = Math.PI * 3 / 2;
                        double lowerUpBoundary = upAngle - halfBoundarySize;
                        double upperUpBoundary = upAngle + halfBoundarySize;
                        double rightAngle = 0;
                        double lowerRightBoundary = upperUpBoundary;
                        double upperRightBoundary = rightAngle + halfBoundarySize;
                        if (angleWithXAxis >= lowerDownBoundary && angleWithXAxis < upperDownBoundary)
                        {
                            mControlsDown[Control.TRACKS_BACKWARDS] = false;
                            mControlsDown[Control.TRACKS_FORWARDS] = true;
                            mControlsDown[Control.TRACKS_LEFT] = false;
                            mControlsDown[Control.TRACKS_RIGHT] = false;
                        }
                        else if (angleWithXAxis >= lowerLeftBoundary && angleWithXAxis < upperLeftBoundary)
                        {
                            mControlsDown[Control.TRACKS_BACKWARDS] = false;
                            mControlsDown[Control.TRACKS_FORWARDS] = false;
                            mControlsDown[Control.TRACKS_LEFT] = true;
                            mControlsDown[Control.TRACKS_RIGHT] = false;
                        }
                        else if (angleWithXAxis >= lowerUpBoundary && angleWithXAxis < upperUpBoundary)
                        {
                            mControlsDown[Control.TRACKS_BACKWARDS] = true;
                            mControlsDown[Control.TRACKS_FORWARDS] = false;
                            mControlsDown[Control.TRACKS_LEFT] = false;
                            mControlsDown[Control.TRACKS_RIGHT] = false;
                        }
                        else if ((angleWithXAxis >= lowerRightBoundary && angleWithXAxis <= Math.PI * 2)|| (angleWithXAxis < upperRightBoundary && angleWithXAxis >= 0))
                        {
                            mControlsDown[Control.TRACKS_BACKWARDS] = false;
                            mControlsDown[Control.TRACKS_FORWARDS] = false;
                            mControlsDown[Control.TRACKS_LEFT] = false;
                            mControlsDown[Control.TRACKS_RIGHT] = true;
                        }
                    }

                }
            }
        }
    }
}
