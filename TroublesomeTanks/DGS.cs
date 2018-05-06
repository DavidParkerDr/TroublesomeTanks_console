﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TroublesomeTanks
{
    static class DGS
    {
        public const float SECONDS_TO_DISPLAY_FLASH_SCREEN = 1.5f;
        public const float SECONDS_TO_DISPLAY_GAMEOVER_SCREEN = 5.0f;
        public const float TRACK_DEPLETION_RATE = 10f;//20f;
        public const float TURRET_DEPLETION_RATE = 10f;//10f;
        public const float BULLET_RADIUS_POWER_SCALE = 5f;
        public const float TANK_SPEED = 100f;
        public const float TANK_BACKWARDS_SPEED = TANK_SPEED * 0.5f; // tank is half as fast going backwards
        public const float BASE_TANK_ROTATION_ANGLE = 0.02f;
        public const float BASE_TURRET_ROTATION_ANGLE = 0.02f;
        public const float MAX_CHARGE = 200f;
        public const float MAX_BATTERY_CHARGE = MAX_CHARGE * 2;
        public const float BATTERY_STARTING_CHARGE = MAX_BATTERY_CHARGE;
        public const float CHARGE_AMOUNT = 0.1f * MAX_CHARGE ; // 10% of maximum charge per second
        public const float BATTERY_CHARGE_RATE = 0.05f * MAX_CHARGE; // 5% of maximum charge per second
        public const float STARTING_CHARGE = MAX_CHARGE;//* 0.2f; // twenty percent charge on every port to begin with     
        public const float BULLET_CHARGE_DEPLETION = MAX_CHARGE / 2;
        public const int TANK_RADIUS = 25;
        public const int MAX_TANK_HEALTH = 5;
        public const int BLAST_DELAY = 15;
        public const float BULLET_SPEED = 200;
        public const float LONG_PRESS_DURATION = 1f;
        public const bool CANNON_INDEPENDENT_FROM_TRACKS = true;
        public const bool HAVE_CONTROLLER = false;
        public const int NUM_PLAYERS = 3;
        public const bool FINITE_POWER = true;
        public const bool BATTERY_CAN_RECHARGE = true;
        public const int NUMBER_OF_AVATARS = 12;
        public const float GAME_START_COUNTDOWN = 3.0f;

        public const int SCREEN_RESIZE_HACK = 1;

        public const int SCREENWIDTH = 1920/ SCREEN_RESIZE_HACK; // screen smaller hack
        public const int SCREENHEIGHT = 1000/ SCREEN_RESIZE_HACK; // screen smaller hack

        public const bool IS_FULL_SCREEN = false;

        public const float TRACK_OFFSET = 17;
        public const float TRACK_OFFSET_SQRD = TRACK_OFFSET * TRACK_OFFSET;
        private const int TANK_WIDTH = 44 / SCREEN_RESIZE_HACK;// screen smaller hack
        private const int TANK_HEIGHT = 59 / SCREEN_RESIZE_HACK;// screen smaller hack
        private const int TANK_FRONT_BUFFER = 5;
        public static Vector2[] TANK_CORNERS = { new Vector2(TANK_HEIGHT / 2 - TANK_FRONT_BUFFER, -TANK_WIDTH / 2), new Vector2(-TANK_HEIGHT / 2, -TANK_WIDTH / 2), new Vector2(-TANK_HEIGHT / 2, TANK_WIDTH / 2), new Vector2(TANK_HEIGHT / 2 - TANK_FRONT_BUFFER, TANK_WIDTH / 2) };
        #region Tank Render Constants
        // These are to help with places where rendering is tightly coupled to physics

        #endregion

        public const int PARTICLE_EDGE_THICKNESS = 2;

        #region Colours

        public static Color COLOUR_GROUND = Color.Khaki;// new Color(220, 205, 50);
        public static Color COLOUR_WALLS = Color.SlateGray;//new Color(127, 93, 26);
        public static Color COLOUR_TANK1 = Color.SteelBlue; //new Color(50, 160, 255);
        public static Color COLOUR_TANK2 = Color.Goldenrod; //new Color(255, 105, 30);
        public static Color COLOUR_TANK3 = Color.SeaGreen;
        public static Color COLOUR_TANK4 = Color.LightPink;
        public static Color COLOUR_DUST = Color.Gainsboro;
      //  public static Color COLOUR_FIRE = Color.DarkOrange;
      //  public static Color COLOUR_TRACK_PRINT = Color.DarkKhaki;

        #endregion
    }
}
