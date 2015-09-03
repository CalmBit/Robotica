using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Robotica.Entity.Component;

namespace Robotica.Entity
{
    public class EntityPlayer : EntityBase
    {
        public EntityPlayer(Texture2D playerTexture, int spriteId, Texture2D shadowTexture, int shadowOffset) : base()
        {
            PositionComponent = new PositionComponent(new Vector2(200, 200));
            RenderComponent = new RenderComponent(playerTexture, new Vector2(0, spriteId), shadowTexture, shadowOffset, Color.White);
            ShooterComponent = new ShooterComponent();
            ControlComponent = new ControlComponent(this);
        }


    }
}
