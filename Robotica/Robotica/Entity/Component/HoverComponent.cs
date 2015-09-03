using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Robotica.Entity.Component
{
    public class HoverComponent
    {
        private readonly EntityBase Parent;
        public static bool HoverUp = true;
        public static float Counter = 0.0f;
        public static float Bounds = 10.0f;
        public const float Step = 0.30f;
        public HoverComponent(EntityBase parent)
        {
            Parent = parent;
            if(Parent.PositionComponent == null) throw new Exception("HoverComponent needs a PositionComponent");
        }

        public static void UpdateAnimation()
        {
            Counter += Step;
            if (!(Counter >= Bounds)) return;
            Counter = 0.0f;
            HoverUp = !HoverUp;
        }

        public void Update()
        {
            if (HoverUp)
            {
                Parent.PositionComponent.CurrentPosition.Y -= Step;
                Parent.RenderComponent.ShadowOffset += Step;
            }
            else
            {
                Parent.PositionComponent.CurrentPosition.Y += Step;
                Parent.RenderComponent.ShadowOffset -= Step;
            }
        }


    }
}
