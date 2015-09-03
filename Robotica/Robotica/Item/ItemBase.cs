using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Robotica.Item
{
    /// <summary>
    /// Basis for Item definitions.
    /// NOT TO BE CONFUSED WITH ItemEntity - dropped, in game items.
    /// </summary>
    public abstract class ItemBase
    {
        public String Name;
        public Texture2D Texture;

        protected ItemBase(string name, Texture2D texture)
        {
            
        }

        public abstract bool Action();
    }
}
