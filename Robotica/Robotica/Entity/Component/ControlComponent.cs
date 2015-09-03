using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Robotica.Entity.Component
{
    public class ControlComponent
    {
        private readonly EntityBase Parent;
        private const float SPEED = 4.0f;
        public ControlComponent(EntityBase parent)
        {
            Parent = parent;
            if(Parent.PositionComponent == null) throw new Exception("ControlComponent needs a PositionComponent!");
        }

        public void Update()
        {
            var keyboard = Keyboard.GetState();
            //TODO: Replace with bindings pleaaaaaase
            if (keyboard.IsKeyDown(Keys.W))
            {
                Parent.RenderComponent.Orientation = Orientation.BACK;
                Parent.PositionComponent.CurrentPosition.Y -= SPEED;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                Parent.RenderComponent.Orientation = Orientation.FRONT;
               Parent.PositionComponent.CurrentPosition.Y += SPEED;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                Parent.RenderComponent.Orientation = Orientation.RIGHT;
                Parent.PositionComponent.CurrentPosition.X += SPEED;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                Parent.RenderComponent.Orientation = Orientation.LEFT;
                Parent.PositionComponent.CurrentPosition.X -= SPEED;
            }
        }
    }
}
