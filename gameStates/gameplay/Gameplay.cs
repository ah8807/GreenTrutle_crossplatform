using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GreenTrutle_crossplatform.player;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.Physics;
using GreenTrutle_crossplatform.scene.Objects;
using System;
using System.Net;
using GreenTrutle_crossplatform.GameStates;
using GreenTrutle_crossplatform.GameStates.menus;
using GreenTrutle_crossplatform.player.Human;
using GreenTrutle_crossplatform.player.AI;
using Microsoft.Xna.Framework.Input;

namespace GreenTrutle_crossplatform.gameStates.gameplay
{
    public class Gameplay: GameState
    {
        public event EventHandler OnClose;

        Level level;

        HumanPlayer player;

        AIPlayer rex;

        Renderer renderer;
        PhysicsEngine physics;

        GameHud gameHud;
        HudRenderer hudRenderer;

        private Options options;

        public Gameplay(Level level)
        {
            this.DrawOrder = 3;
            Globals.currLevel = level;
            this.level = Globals.currLevel;


            renderer = new Renderer(level);

            physics = new PhysicsEngine(level);

            player = new HumanPlayer(level.turtle, level.scene);

            rex = new AIPlayer(level.trex, level.scene);

            gameHud = GameHud.GetInstance();
            hudRenderer = new HudRenderer(gameHud.scene);
            
            options = new Options(this);

            addComp(renderer);
            addComp(physics);
            addComp(gameHud);
            addComp(hudRenderer);
            
            
            base.Initialize();
            options.OnClickBack += (sender, args) => {Close();
                    options.Close();
                    OnClose?.Invoke(this,EventArgs.Empty);
            };

        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            rex.Update(gameTime);
            
            KeyboardState Kstate = Keyboard.GetState();
            if (Kstate.IsKeyDown(Keys.Escape))
            {
                physics.Enabled = false;
                this.Enabled = false;
                options.activate();
            }

            base.Update(gameTime);
        }

        public void ressume()
        {
            addComp(this);
            addComp(physics);
        }
        public void pause()
        {
            removeComp(this);
            removeComp(physics);
        }

    }
}
