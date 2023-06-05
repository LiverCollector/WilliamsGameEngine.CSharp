﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using SFML.Graphics;
using SFML.System;

namespace MyGame.GameEngine.TileEntites
{
    internal class TileEntity : GameObject
    {
        internal Sprite sprite;
        public Vector2i tileDimensions; //how many tiles it takes up
        public Vector2f position;
        internal float lastLoaded;      //how long since it was last loaded (works up to like 1,000,000 hours)
        internal float topMargin;
        public virtual void Clicked() { }
        public override void Draw() { sprite.Position = position - new Vector2f(0,topMargin * sprite.Scale.Y); sprite.Position = Game._Camera.ToLocalPos(sprite.Position); Game.RenderWindow.Draw(sprite); }
        public override void Update(Time elapsed) { }
        public virtual void LongUpdate(float time) { }//what happens to it after a long time has passed since it was loaded
        public void Initialize(Scene scene)
        {
            //snaps position to tilemmap
            AssignTag("impass");
            position = scene.tileMap.SnapToTile(position);
            lastLoaded = Game.time;
            stopsTiles = true;
        }
        public override Vector2f GetPosition()
        {
            return position;
        }
        public override FloatRect GetCollisionRect()
        {
            FloatRect box = sprite.GetGlobalBounds();
            box.Left = position.X;
            box.Top = position.Y;
            box.Height -= topMargin * sprite.Scale.Y;
            return box;
        }
        public override void SetPosition(Vector2f position)
        {
            this.position = position;
        }
    }
}
