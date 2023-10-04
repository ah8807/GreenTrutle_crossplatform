using System;
using System.Collections.Generic;
using System.IO;
using GreenTrutle_crossplatform.gameStates.gameplay;
using GreenTrutle_crossplatform.GameStates.menus;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.sound;
using GreenTrutle_crossplatform.tools;
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
            Globals.appDataFilePath=Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            Globals.save = new Save();
        }

        protected override void Initialize()
        {
            Globals.soundControl = new SoundControl("queen");
            Globals.spriteBatch= new SpriteBatch(GraphicsDevice);
            startGame();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.soundControl.addSound("pickUpLettuce","pickupCoin",1);
            Globals.soundControl.addSound("die","die",1);
            Globals.soundControl.addSound("win","win",1);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Globals.gameTime = gameTime;
            base.Update(gameTime);
        }
        

        private void startGame()
        {
            //if(currGamePlay != null)
              //  this.Components.Remove((GameComponent)currGamePlay);
            //currGamePlay = new Gameplay(level);
            //this.Components.Add(currGamePlay);
            if(menu != null)
                this.Components.Remove((GameComponent)menu);
            Globals.debugRenderer = new DebugRenderer();
            menu = new MainMenu(null);
            menu.activate();
            Globals.game.Components.Add(Globals.debugRenderer);
        }
    }
}