﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TroublesomeTanks.Scenes
{
    public class SceneManager
    {
        private List<IScene> mScenes;

        public SceneManager()
        {
            mScenes = new List<IScene>();
        }

        public void Push(IScene p_Scene)
        {
            mScenes.Add(p_Scene);
        }

        public void Transition(IScene pNextScene, bool replaceCurrent = true)
        {
            IScene currentScene = Top;
            // gain access to the scene before the current scene
            if (pNextScene == null)
            {
                pNextScene = Previous;
            }
            if (replaceCurrent)
            {
                Pop();
            }
            if (pNextScene != null)
            {
                IScene transitionScene = new TransitionScene(currentScene, pNextScene);
                mScenes.Add(transitionScene);
            }
            else
            {
                // we should exit the game here...
                  TroublesomeTanks game = (TroublesomeTanks)TroublesomeTanks.Instance();
                  game.Exit();
            }
        }

        public void Pop()
        {
            if (mScenes.Count > 0)
            {
                mScenes.RemoveAt(mScenes.Count - 1);
            }
        }

        public IScene Top
        {
            get
            {
                if (mScenes.Count > 0)
                {
                    return mScenes.Last();
                }
                return null;
            }
        }
        public IScene Previous
        {
            get
            {
                if (mScenes.Count > 1)
                {
                    return mScenes[mScenes.Count - 2];
                }
                return null;
            }
        }

        public void Update(float pSeconds)
        {
            if (mScenes.Count > 0)
            {
                Top.Update(pSeconds);
            }
        }

        public void Draw(float pSeconds)
        {
            if (mScenes.Count > 0)
            {
                Top.Draw(pSeconds);
            }
        }
    }
}
