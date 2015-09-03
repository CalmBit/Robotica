using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Robotica.Entity.Component
{
    public enum Orientation
    {
        FRONT,
        BACK,
        RIGHT,
        LEFT
    }
    public class RenderComponent
    {
        private readonly Texture2D Texture;
        private readonly Vector2 SpriteOffset;
        private readonly Texture2D Shadow;
        public float ShadowOffset;
        public Orientation Orientation;
        public bool OrientationBased;
        public Color Color;

        public RenderComponent(Texture2D texture, Texture2D shadow, float shadowOffset, Color color, bool orientationBased = true)
        {
            Texture = texture;
            SpriteOffset = new Vector2(0,0);
            Shadow = shadow;
            ShadowOffset = shadowOffset;
            OrientationBased = orientationBased;
            Color = color;
        }
        public RenderComponent(Texture2D texture, Vector2 spriteOffset, Texture2D shadow, float shadowOffset, Color color, bool orientationBased = true)
        {
            Texture = texture;
            SpriteOffset = spriteOffset;
            Shadow = shadow;
            ShadowOffset = shadowOffset;
            OrientationBased = orientationBased;
            Color = color;
        }


        public void Render(SpriteBatch spriteBatch, Vector2 Position)
        {
            spriteBatch.Draw(Shadow, new Vector2(Position.X, Position.Y+ShadowOffset), null, Color.White, 0.0f, new Vector2(32,32),1.0f,SpriteEffects.None,0);
            spriteBatch.Draw(Texture, Position,
                OrientationBased
                    ? new Rectangle((int) Orientation*64 + 11, (int) SpriteOffset.X*64, 64, 64)
                    : new Rectangle((int) SpriteOffset.X*64, (int) SpriteOffset.Y*64, 64, 64), Color, 0.0f,
                new Vector2(32, 32), 1.0f, SpriteEffects.None, 0);
        }
    }
}
