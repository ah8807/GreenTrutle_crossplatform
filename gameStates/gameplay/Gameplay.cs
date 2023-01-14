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
            Globals.currLevel = level;
            this.level = Globals.currLevel;


            renderer = new Renderer(level);

            physics = new PhysicsEngine(level);

            player = new HumanPlayer(level.turtle, level.scene);

            rex = new AIPlayer(level.trex, level.scene);

            gameHud = GameHud.GetInstance();
            hudRenderer = new HudRenderer(gameHud.scene);
            
            options = new Options(this);


        }

        public override void Initialize()
        {
            base.Initialize();
            options.OnClickBack += (sender, args) => {Close();
                 gameHud.close();
                 gameHud.Dispose();
                 OnClose?.Invoke(this,EventArgs.Empty);
                };
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            rex.Update(gameTime);
            
            KeyboardState Kstate = Keyboard.GetState();
            if (Kstate.IsKeyDown(Keys.Escape))
            {
                Globals.game.Components.Remove(physics);
                Globals.game.Components.Remove(this);
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
        public override void activate()
        {
            addComp(this);
            addComp(physics);
            addComp(renderer);
            addComp(level);
            addComp(gameHud);
            addComp(hudRenderer);
        }

        public override void deactivate()
        {
            removeComp(this);
            removeComp(physics);
            removeComp(renderer);
            removeComp(level);
            removeComp(gameHud);
            removeComp(hudRenderer);
        }
    }
}
