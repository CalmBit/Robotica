using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace Robotica.Entity.Component
{
    public enum ShotType
    {
        RIFLE,
        MINIGUN,
        SHOTGUN,
        LASER
    }
    public class ShooterComponent
    {
        public int[] AmmoCounts = { 0, 0, 0, 0 };
        public int[] MaxAmmoCounts = { 32, 512, 32, 128 };

        public ShooterComponent()
        {
            
        }

        public bool Shoot(ShotType shotType)
        {
            if (AmmoCounts[(int) shotType] != 0) AmmoCounts[(int)shotType]--;
            return AmmoCounts[(int)shotType]+1 != 0;
        }
    }
}
