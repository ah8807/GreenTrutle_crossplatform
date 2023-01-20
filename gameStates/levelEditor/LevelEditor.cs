using System;
using System.Runtime.CompilerServices;
using GreenTrutle_crossplatform.interfaces;
using GreenTrutle_crossplatform.scene;
using GreenTrutle_crossplatform.scene.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GreenTrutle_crossplatform.GameStates.levelEditor;

public class LevelEditor:GameState
{
    private Scene scene;
    private Vector2 world_to_game_scale =  new Vector2(240, 135)/ new Vector2(Globals.ScreenWidth, Globals.ScreenHeight);
    private Vector2 game_to_world_scale =  new Vector2(Globals.ScreenWidth, Globals.ScreenHeight)/new Vector2(240, 135);
    private object currObject;
    public LevelEditor(Scene scene)
    {
        this.scene = scene;
    }
    public override void Update(GameTime gameTime)
    {
        
        MouseState Mstate = Mouse.GetState();
        // Handle user input (e.g. mouse clicks, keyboard shortcuts)
        if (Mstate.LeftButton == ButtonState.Pressed) {
            Vector2 mPos = new Vector2(Mstate.X,Mstate.Y)* world_to_game_scale;
            // Get the tile position under the mouse cursor
            Vector2 tilePos = ScreenToTile(mPos);
            // Use the current tool to modify the level
            bool found = false;
            foreach (object obj in scene.items)
            {
                if ((obj as IParticle)?.getRect().Contains(mPos)==true)
                {
                    currObject = obj;
                    found = true;
                }
            }

            if (found == false)
            {
                
                DrawableGameObject clone = (DrawableGameObject)(currObject as DrawableGameObject)?.Clone();
                clone.position = (tilePos * 16);
                scene.addItem(clone);
                
                // Wall wall = new Wall();
                // wall.position = (tilePos * 16);
                // scene.addItem(wall);
            }
        }
        // Update the level and render it to the screen
        base.Update(gameTime);
    }

    private Vector2 ScreenToTile(Vector2 position)
    {
        float scaleX = 240 / 15;
        float scaleY = 135 / 8.5f;
        // Divide the world position by the size of a tile to get the tile coordinates
        Vector2 tilePos = position / new Vector2(scaleX,scaleY);
        // Round the tile position to the nearest integer
        tilePos.X = (int)Math.Round(tilePos.X);
        tilePos.Y = (int)Math.Round(tilePos.Y);
        return tilePos;
    }
}