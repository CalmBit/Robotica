using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Robotica.Entity.Component
{
    public class PositionComponent
    {
        public Vector2 CurrentPosition;

        public PositionComponent(Vector2 initalPosition)
        {
            this.CurrentPosition = initalPosition;
        }
    }
}
