using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Robotica.Entity.Component;

namespace Robotica.Entity
{
    public abstract class EntityBase
    {
        public RenderComponent RenderComponent;
        public ShooterComponent ShooterComponent;
        public ControlComponent ControlComponent;
        public PositionComponent PositionComponent;
        public HoverComponent HoverComponent;
        protected EntityBase()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if(ControlComponent != null) ControlComponent.Update();
            if(HoverComponent != null) HoverComponent.Update();
        }

        public virtual void Render(SpriteBatch spriteBatch, Vector2? OverridePosition)
        {
            if(RenderComponent != null) RenderComponent.Render(spriteBatch, PositionComponent == null ? OverridePosition ?? new Vector2(300, 300) : PositionComponent.CurrentPosition);
        }
    }
}
