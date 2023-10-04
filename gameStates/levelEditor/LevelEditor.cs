using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using GreenTrutle_crossplatform.GameStates.menus;
using GreenTrutle_crossplatform.Graphics;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.Physics;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using GreenTrutle_crossplatform.tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Collision;

namespace GreenTrutle_crossplatform.GameStates.levelEditor;

public class LevelEditor:GameState
{
    private QuadTree quadTree;
    private EmptyLevel level;
    private Vector2 world_to_game_scale =  Globals.gameSize/ new Vector2(Globals.ScreenWidth, Globals.ScreenHeight);
    private Vector2 game_to_world_scale = new Vector2(Globals.ScreenWidth, Globals.ScreenHeight) / Globals.gameSize;
    private object currObject = new Brick();
    private Vector2 tileSize = new Vector2(2,2);
    private String tool = "Add";
    private Renderer renderer;
    private PhysicsEngine phisicsEngine;
    private PauseMenu pauseMenu;
    private string path;
    public LevelEditor(GameState prevState,string path) : base(prevState)
    {
        this.path = path;
        level = new EmptyLevel();
        loadScene(level.scene, path);
        renderer = new Renderer(level);
        pauseMenu = new PauseMenu(this);
        
        addComp(level.scene);
        addComp(renderer);

        pauseMenu.OnClickBack += goToMainMenu;
        quadTree = new QuadTree(new Rectangle(0,0,(int)Globals.gameSize.X,(int)Globals.gameSize.Y),10);
    }

    protected override void loadScene(Scene scene, string scenePath)
    {
        XDocument xml = Globals.save.GetFile(scenePath);
        if (xml == null)
        {
            scene = null;
            return;
        }

        var bricks = getXElemetsOfType(xml,"Brick");
        foreach (Brick brick in bricks)
        {
            scene.addItem(brick);
        }
            
        var lettuces = getXElemetsOfType(xml,"Lettuce");
        foreach (Lettuce lettuce in lettuces)
        {
            scene.addItem(lettuce);
        }
            
        var trexs = getXElemetsOfType(xml,"Trex");
        foreach (Trex trex in trexs)
        {
            scene.addItem(trex);
        }
            
        var turtles = getXElemetsOfType(xml,"Turtle");
        foreach (Turtle turtle in turtles)
        {
            scene.addItem(turtle);
        }
        var walls = getXElemetsOfType(xml,"Wall");
        // foreach (Wall wall in walls)
        // {
        //     level.scene.addItem(wall);
        // }
        scene.Update(Globals.gameTime);
        return;
    }

    public void goToMainMenu(Object o, EventArgs args)
    {
        pauseMenu.Close();
        // OnClose?.Invoke(this,EventArgs.Empty);
        goBack(o,args);
    }
    

    public override void Update(GameTime gameTime)
    {
        KeyboardState Kstate = Keyboard.GetState();
        if (Kstate.IsKeyDown(Keys.Escape))
        {
            this.Enabled = false;
            pauseMenu.activate();
        }
        if (Kstate.IsKeyDown(Keys.D))
        {
            tool = "Delete";
        }
        if (Kstate.IsKeyDown(Keys.A))
        {
            tool = "Add";
        }
        if (Kstate.IsKeyDown(Keys.D1))
        {
            currObject = new Brick();
        }
        if (Kstate.IsKeyDown(Keys.D2))
        {
            currObject = new Lettuce();
        }
        if (Kstate.IsKeyDown(Keys.D3))
        {
            currObject = new Turtle();
        }
        if (Kstate.IsKeyDown(Keys.D4))
        {
            currObject = new Trex();
        }
        
        if (Kstate.IsKeyDown(Keys.S))
        {
            Globals.save.saveFile(level.scene.getXML(), path);
        }
        
        MouseState Mstate = Mouse.GetState();
        // Handle user input (e.g. mouse clicks, keyboard shortcuts)
        if (Mstate.LeftButton == ButtonState.Pressed) {
        quadTree = new QuadTree(new Rectangle(0,0,(int)Globals.gameSize.X,(int)Globals.gameSize.Y),10);
        foreach (IParticle obj in level.scene)
        {
            quadTree.addPoint(obj);
        }
            Vector2 mPos = new Vector2(Mstate.X,Mstate.Y)* world_to_game_scale;
            // Get the tile position under the mouse cursor
            Vector2 tilePos = World.worldScordsToTile(mPos);
            // Use the current tool to modify the level
            bool found = false;
            foreach (object obj in level.scene.items)
            {
                IParticle p = (obj as IParticle);
                Rectangle rect = p.getRect();
                if (rect.Contains(mPos)==true)
                {
                    found = true;
                    if (tool == "Delete")
                    {
                        level.scene.removeItem((DrawableGameObject)obj);
                    }
                }
            }

            if (found == false)
            {
                Vector2 position = (tilePos * tileSize) - (tileSize / 2);
                if (tool == "Add")
                {
                    DrawableGameObject clone = (DrawableGameObject)(currObject as DrawableGameObject)?.Clone();
                    clone.position = position;
                    Rectangle rect = ((IParticle)clone).getRect();
                    if (quadTree.searchWithRect((IParticle)clone,new Rectangle(rect.X-10,rect.Y-10,rect.Width+10,rect.Height+10)).Any()||clone.position.X<0||clone.position.Y<0||clone.position.X>Globals.gameSize.X||clone.position.Y>Globals.gameSize.Y)
                        return;
                    level.scene.addItem(clone);
                }
            }
        }
        // Update the level and render it to the screen
        base.Update(gameTime);
    }

    public override void Close()
    {
        Globals.debugRenderer.removeScene(level);
        base.Close();
    }
}
