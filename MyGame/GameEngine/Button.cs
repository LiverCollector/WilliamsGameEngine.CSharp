﻿using GameEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace MyGame.GameEngine
{
    internal class Button : GameObject, MouseInterface
    {
        Sprite _sprite = new Sprite();
        public override void Draw()
        {
            Game.RenderWindow.Draw(_sprite);
        }
        public override void Update(Time elapsed)
        {
        }
        public void Hover()
        {

        }
        public void Clicked()
        {

        }
    }
}