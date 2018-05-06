﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TroublesomeTanks.Scenes
{
    class ButtonList
    {
        List<Button> mButtons = null;
        int currentSelectedButtonIndex = 0;
        
        public ButtonList()
        {
            mButtons = new List<Button>();
        }
        public void Add(Button pButton)
        {
            mButtons.Add(pButton);
        }

        public void SelectNextButton()
        {
            Console.WriteLine("NextButton start: " + currentSelectedButtonIndex);
            int nextSelectedButtonIndex = currentSelectedButtonIndex + 1;
            if (nextSelectedButtonIndex >= mButtons.Count)
            {
                nextSelectedButtonIndex = 0;
            }
            mButtons[nextSelectedButtonIndex].Selected = true;
            mButtons[currentSelectedButtonIndex].Selected = false;
            currentSelectedButtonIndex = nextSelectedButtonIndex;
            Console.WriteLine("NextButton finish: " + currentSelectedButtonIndex);

        }
        public void SelectPreviousButton()
        {
            Console.WriteLine("PreviousButton start: " + currentSelectedButtonIndex);
            int previousSelectedButtonIndex = currentSelectedButtonIndex - 1;
            if (previousSelectedButtonIndex < 0)
            {
                previousSelectedButtonIndex = mButtons.Count - 1;
            }
            mButtons[previousSelectedButtonIndex].Selected = true;
            mButtons[currentSelectedButtonIndex].Selected = false;
            currentSelectedButtonIndex = previousSelectedButtonIndex;
            Console.WriteLine("PreviousButton finish: " + currentSelectedButtonIndex);

        }

        public void PressSelectedButton()
        {
            mButtons[currentSelectedButtonIndex].PressButton();
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            foreach (Button button in mButtons)
            {
                Color buttonColour = Color.White;
                if (button.Selected)
                {
                    pSpriteBatch.Draw(button.TexturePressed, button.Rect, buttonColour);
                }
                pSpriteBatch.Draw(button.Texture, button.Rect, buttonColour);
            }
        }
    }
}
