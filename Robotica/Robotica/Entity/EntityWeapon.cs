using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Robotica.Entity.Component;
using Robotica.Weapon;

namespace Robotica.Entity
{
    public class EntityWeapon : EntityBase
    {
        public WeaponDefinition Weapon;
        public EntityWeapon(Vector2 position)
        {
            Weapon = new WeaponDefinition();
            PositionComponent = new PositionComponent(position);
            RenderComponent = new RenderComponent(Game1.Weapon, new Vector2((int)Weapon.Type,0), Game1.Shadow, 57, Weapon.FormattedColor, false);
            HoverComponent = new HoverComponent(this);
            Console.WriteLine("Generated " + Weapon.FormattedName);
        }

    }
}
