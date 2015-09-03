using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Robotica.Item;

namespace Robotica.Weapon
{
    public enum WeaponPrefix
    {
        NONE,
        ELECTRIC, // Electricity, shocks enemies. Other effects?
        INCENDIARY, // Fire, burns enemies and sets them on fire.
        EXPLOSIVE, // Explosive. Self-explanatory.
        SEPTIC, // Poison. Ditto.
        GOLDEN, // Golden, higher damage.
        CURSED, // Cursed, damage multiplier (up to 8) with varying levels of recoil damage.
        RANDOM, // Random, random effect each room.
        BOUNTIFUL, // Bountiful, gets 2x ammo from each pickup.
        STARVING, // Starving, gets 0.5x ammo from each pickup.
        MULTIPLYING, // Multiplying, splinter shots.
        RICOCHET, // Richochet, shots ricochet
    }

    public enum WeaponSuffix
    {
        NONE, // Suffix effects are 1:1 with Prefix effects (i.e. Shocking = Electric, Burning = Incendiary). THIS MAY BE OPTIMIZED LATER
        SHOCKING,
        BURNING,
        EXPLODING,
        POISONING,
        MIDAS,
        CONSQUENCES,
        RANDOMNESS,
        BOUNTY,
        FAMINE,
        MULTIPLICITY,
        RICOCHETING,
    }

    public enum WeaponType
    {
        PISTOL,
        MACHINE_GUN,
        SHOTGUN,
        LAZWEAPON
    }

    public class WeaponDefinition
    {
        public WeaponPrefix Prefix;
        public WeaponSuffix Suffix;
        public WeaponType Type;

        public List<Color> Colors = new List<Color>
        {
            Color.Black,
            Color.Yellow,
            Color.Red,
            Color.Orange,
            Color.Green,
            Color.Gold,
            Color.DarkRed,
            Color.Pink,
            Color.Wheat,
            Color.Brown,
            Color.Teal,
            Color.Salmon
        };

        public string FormattedName
        {
            get { return ( Prefix != WeaponPrefix.NONE ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Prefix.ToString().ToLower()) + " " : "")  + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Type.ToString().ToLower()).Replace('_',' ') + (Suffix != WeaponSuffix.NONE ? " of " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Suffix.ToString().ToLower()) : ""); }
        }

        public Color FormattedColor
        {
            get
            {
                var one = Colors[(int)Prefix];
                var two = Colors[(int) Suffix];
                return new Color(((one.R + two.R) / 2), ((one.G + two.G) / 2), ((one.B + two.B) / 2));
            }
        }

        public WeaponDefinition()
        {
            if (Game1.Random.Next(100) >= 35)
                Prefix =
                   Prefix =
                    (WeaponPrefix)
                        Game1.Random.Next((int) Enum.GetValues(typeof (WeaponPrefix)).Cast<WeaponPrefix>().Last() + 1);
            if (Game1.Random.Next(100) >= (Prefix == WeaponPrefix.NONE ? 35 : 75))
            {
                Suffix =
                    (WeaponSuffix)
                        Game1.Random.Next((int) Enum.GetValues(typeof (WeaponSuffix)).Cast<WeaponSuffix>().Last() + 1);
                if ((int) Prefix == (int) Suffix) Suffix = WeaponSuffix.NONE;
            }
            Type = (WeaponType)Game1.Random.Next((int)Enum.GetValues(typeof(WeaponType)).Cast<WeaponType>().Last() + 1);
        }

        public void Update()
        {
            
        }

        public void Render(SpriteBatch spriteBatch, Vector2 Position)
        {
            spriteBatch.Draw(Game1.Weapon,Position, new Rectangle((int)Type * 64, 0, 64, 64), Color.White, 0.0f, new Vector2(32, 32), 0.5f, SpriteEffects.None, 0.0f);
        }
    }
}
