using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GreenTrutle_crossplatform.player;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.Physics;
using GreenTrutle_crossplatform.scene.Objects;
using System;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Net;
using GreenTrutle_crossplatform.GameStates;
using GreenTrutle_crossplatform.GameStates.levelEditor;
using GreenTrutle_crossplatform.GameStates.menus;
using GreenTrutle_crossplatform.player.Human;
using GreenTrutle_crossplatform.player.AI;
using GreenTrutle_crossplatform.tools;
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

        public GameHud gameHud;
        HudRenderer hudRenderer;

        private Options options;
        
        private LevelEditor levelEditor;

        public Gameplay(Level level)
        {
            this.DrawOrder = 3;
            Globals.currLevel = level;
            this.level = Globals.currLevel;


            renderer = new Renderer(level);

            physics = new PhysicsEngine(level);

            player = new HumanPlayer(level.turtle, level.scene);

            rex = new AIPlayer(level.trex, level.scene,level.turtle);

            gameHud = new GameHud();
            hudRenderer = new HudRenderer(gameHud.scene);

            levelEditor = new LevelEditor(level.scene);
            
            options = new Options(this);

            addComp(renderer);
            addComp(physics);
            addComp(gameHud);
            addComp(hudRenderer);
            addComp(levelEditor);
            levelEditor.activate();
            
            
            base.Initialize();
            options.OnClickBack += goToMainMenu;
            Globals.eventManager.Subscribe("respawnTurtle", respawnTurtle);
        }

        public void checkIfAllLetuceWasPickedUp(Object o, Dictionary<string, object> args)
        {
            if (level.scene.items.OfType<Lettuce>().Any())
            {
                return;
            }    
            goToMainMenu(this,EventArgs.Empty);
        }
        public void goToMainMenu(Object o, EventArgs args)
        {
            gameHud.close();
            options.Close();
            OnClose?.Invoke(this,EventArgs.Empty);
        } 
        public void respawnTurtle(Object o, Dictionary<string, object> args)
        {
            if ((int)args["lives"] == 0)
            {
                goToMainMenu(this,EventArgs.Empty);
                return;
            }
            level.scene.removeItem((DrawableGameObject)args["Turtle"]);
            Timer timer = new Timer(1500);
            timer.oneTime += (sender, argsds) => { level.scene.addItem((DrawableGameObject)args["Turtle"]);
                Timer t = (Timer)sender;t.Close();
            };
        }

        public void Close()
        {
            rex.Close();
            Globals.eventManager.ClearListeners("respawnTurtle");
            base.Close();
            Globals.debugRenderer.removeScene(level);
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

            checkIfAllLetuceWasPickedUp(this,new Dictionary<string, object>());
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
