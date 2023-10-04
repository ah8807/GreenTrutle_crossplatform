using GreenTrutle_crossplatform.scene;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GreenTrutle_crossplatform.player;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.Physics;
using GreenTrutle_crossplatform.scene.Objects;
using System;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Xml.Serialization;
using GreenTrutle_crossplatform.GameStates;
using GreenTrutle_crossplatform.GameStates.levelEditor;
using GreenTrutle_crossplatform.GameStates.menus;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.player.Human;
using GreenTrutle_crossplatform.player.AI;
using GreenTrutle_crossplatform.tools;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace GreenTrutle_crossplatform.gameStates.gameplay
{
    public class Gameplay: GameState
    {
        public event EventHandler OnClose;

        EmptyLevel level;

        private List<HumanPlayer> HumanPlayers;
        private List<AIPlayer> AIPlayers;
         
        HumanPlayer player;

        AIPlayer rex;

        Renderer renderer;
        PhysicsEngine physics;

        public GameHud gameHud;
        HudRenderer hudRenderer;

        private PauseMenu _pauseMenu;
        

        private bool happened = false;

        private string scenePath;

        public Gameplay(GameState prevState, Level level) : base(prevState)
        {
            this.DrawOrder = 3;
            this.level = level;

            renderer = new Renderer(level);

            physics = new PhysicsEngine(level);
            
            giveControlersToPlayers();

            gameHud = new GameHud();
            hudRenderer = new HudRenderer(gameHud.scene);
            hudRenderer.transparent=true;

            _pauseMenu = new PauseMenu(this);

            addComp(level.scene);
            addComp(renderer);
            addComp(physics);
            addComp(gameHud);
            addComp(hudRenderer);


            base.Initialize();
            _pauseMenu.OnClickBack += goToMainMenu;
            Globals.eventManager.Subscribe("respawnTurtle", respawnTurtle);
            physics.updateGrid(this,null);
            
        }
        public Gameplay(GameState prevState, String scenePath) : base(prevState)
        {
            this.DrawOrder = 3;
            this.level = new EmptyLevel();
            loadScene(level.scene,scenePath);
            
            renderer = new Renderer(level);

            physics = new PhysicsEngine(level);
            
            giveControlersToPlayers();



            gameHud = new GameHud();
            hudRenderer = new HudRenderer(gameHud.scene);
            hudRenderer.transparent=true;
            
            
            _pauseMenu = new PauseMenu(this);

            addComp(level.scene);
            addComp(renderer);
            addComp(physics);
            addComp(gameHud);
            addComp(hudRenderer);


            base.Initialize();
            _pauseMenu.OnClickBack += goToMainMenu;
            Globals.eventManager.Subscribe("respawnTurtle", respawnTurtle);
            
        }

        private void giveControlersToPlayers()
        {
            if (AIPlayers == null)
                AIPlayers = new List<AIPlayer>();
            if (HumanPlayers == null)
                HumanPlayers = new List<HumanPlayer>();
            foreach (Turtle turtle in level.scene.items.OfType<Turtle>())
            {
                HumanPlayers.Add(new HumanPlayer(turtle,level.scene));
            }
            foreach (Trex rex in level.scene.items.OfType<Trex>())
            {
                AIPlayers.Add(new AIPlayer(rex,level.scene,HumanPlayers[0].body));
            }
        }

        public void checkIfAllLetuceWasPickedUp(Object o, Dictionary<string, object> args)
        {
            if (level.scene.items.OfType<Lettuce>().Any())
            {
                return;
            }

            ((Turtle)HumanPlayers[0].body).unlocked_edibles.Add(new Trex());
            if (level.scene.items.OfType<Trex>().Any())
            {
                return;
            }
            Globals.soundControl.playSound("win");
            goToMainMenu(this,null);
        }
        public void goToMainMenu(Object o, EventArgs args)
        {
            gameHud.close();
            _pauseMenu.Close();
            // OnClose?.Invoke(this,EventArgs.Empty);
            goBack(o,args);
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

        public override void Close()
        {
            foreach (AIPlayer ai in AIPlayers)
            {
                ai.Close();
            }
            Globals.eventManager.ClearListeners("respawnTurtle");
            base.Close();
            Globals.debugRenderer.removeScene(level);
        }

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            processKeyInputs();
            updateGameplayObjects(gameTime);
            applyGameRules();
            
            base.Update(gameTime);
        }

        private void updateGameplayObjects(GameTime gameTime)
        {
            foreach (var VARIABLE in AIPlayers)
            {
               VARIABLE.Update(gameTime); 
            }
            foreach (var VARIABLE in HumanPlayers)
            {
                VARIABLE.Update(gameTime); 
            }
            // player.Update(gameTime);
            // rex.Update(gameTime);
        }

        private void applyGameRules()
        {
            checkIfAllLetuceWasPickedUp(this, new Dictionary<string, object>());
            setTurtleSize();
        }

        public int? n_of_edible_objects { get; set; }
        private void setTurtleSize()
        {
            if (n_of_edible_objects == null)
            {
                n_of_edible_objects = level.scene.items.FindAll((obj) => { return obj is IEdible; }).Count;
            }

            int curr_n_of_edible_objects = level.scene.items.FindAll((obj) => { return obj is IEdible; }).Count;
            ((Turtle)HumanPlayers[0].body).scale = Vector2.One* (float)((curr_n_of_edible_objects - 0) / (n_of_edible_objects - 0));
            ((Turtle)HumanPlayers[0].body).scale = Vector2.One * (float)(0.5 + 0.5*(1 - ((float)curr_n_of_edible_objects /(float)n_of_edible_objects)));
         }


        private void processKeyInputs()
        {
            KeyboardState Kstate = Keyboard.GetState();
            if (Kstate.IsKeyDown(Keys.Escape))
            {
                physics.Enabled = false;
                this.Enabled = false;
                _pauseMenu.activate();
            }

            if (Kstate.IsKeyDown(Keys.S))
            {
                Globals.save.saveFile(level.scene.getXML(), "scene.xml");
            }

            if (Kstate.IsKeyDown(Keys.L) && !happened)
            {
                loadScene(level.scene,"scene.xml");
                happened = true;
            }
        }

        // private bool loadScene(String scenePath)
        // {
        //     XDocument xml = Globals.save.GetFile(scenePath);
        //     if (xml == null)
        //         return true;
        //     
        //     var bricks = getXElemetsOfType(xml,"Brick");
        //     while (bricks.OfType<Brick>().Any())
        //     {
        //         Wall wall = new Wall();
        //         level.scene.addItem((DrawableGameObject)wall.CreateWall(bricks));
        //     }
        //     
        //     var lettuces = getXElemetsOfType(xml,"Lettuce");
        //     foreach (Lettuce lettuce in lettuces)
        //     {
        //         level.scene.addItem(lettuce);
        //     }
        //     
        //     var trexs = getXElemetsOfType(xml,"Trex");
        //     foreach (Trex trex in trexs)
        //     {
        //         level.scene.addItem(trex);
        //     }
        //     
        //     var turtles = getXElemetsOfType(xml,"Turtle");
        //     foreach (Turtle turtle in turtles)
        //     {
        //         level.scene.addItem(turtle);
        //     }
        //     var walls = getXElemetsOfType(xml,"Wall");
        //     // foreach (Wall wall in walls)
        //     // {
        //     //     level.scene.addItem(wall);
        //     // }
        //     level.scene.Update(Globals.gameTime);
        //     return false;
        // }

        private static List<object> getXElemetsOfType(XDocument xml, String type)
        {
            List<XElement> objList = (from t in xml.Element("Root").Element("GameObjects").Descendants("GameObject")
                where t.Element("Type").Value == type
                select t).ToList<XElement>();
            List<object> gameObjects = new List<object>();
            foreach (XElement obj in objList)
            {
                Vector2 position = new Vector2(Convert.ToSingle(obj.Element("Position").Element("X").Value),
                    Convert.ToSingle(obj.Element("Position").Element("Y").Value));
                object gameObject = new DrawableGameObject();
                switch (type)
                {
                    case "Brick":
                        gameObject = new Brick();
                        break;
                    case "Lettuce":
                        gameObject = new Lettuce();
                        break;
                    case "Trex":
                        gameObject = new Trex();
                        break;
                    case "Turtle":
                        gameObject = new Turtle();
                        break;
                    case "Wall":
                        gameObject = new Wall();
                        break;
                }
                
                if(gameObject is IPosition)
                    ((IPosition)gameObject).position = position;
                gameObjects.Add(gameObject);
            }

            return gameObjects;
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
