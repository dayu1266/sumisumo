using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace sumisumo
{
    class guardman : GameObject
    {
        public guardman(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 60;
            imageHeight = 140;
            hitboxOffsetLeft = 17;
            hitboxOffsetRight = 17;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 10;
        }

        public override void Update()
        {

        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y, Image.guardman);
        }

        public override void OnCollision(GameObject other)
        {

        }

    }
}
