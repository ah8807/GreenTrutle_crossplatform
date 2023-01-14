using System;
using System.Collections.Generic;
using System.IO;
using GreenTrutle_crossplatform.gameStates.gameplay;
using GreenTrutle_crossplatform.GameStates.menus;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GreenTrutle_crossplatform
{
    public class Game1 : Game
    {
        private Gameplay currGamePlay;
        private MainMenu menu;

        public Game1()
        {
            Globals.graphics = new GraphicsDeviceManager(this);
            Globals.game = this;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Globals.spriteBatch= new SpriteBatch(GraphicsDevice);
            Globals.levels.Add(new Level());
            loadLevel(Globals.levels[0]);
            base.Initialize();
        }

        private void loadLevel(Level level)
        {
            //if(currGamePlay != null)
              //  this.Components.Remove((GameComponent)currGamePlay);
            //currGamePlay = new Gameplay(level);
            //this.Components.Add(currGamePlay);
            if(menu != null)
                this.Components.Remove((GameComponent)menu);
            menu = new MainMenu();
            menu.activate();
        }
    }
}