using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using QimOLib;
using System.Numerics;

namespace sumisumo
{
    public class Guardman : GameObject
    {
        const float WalkSpeed = 6f;   // 歩きの速度
        const float MaxFallSpeed = 12f;  // 最大落下速度

        const int initialHp = 1;

        Vector2 velocity = Vector2.Zero; // 移動速度

        public int hp = 1;

        public Guardman(PlayScene playScene, Vector2 pos) : base(playScene)
        {
            this.pos.X = pos.X;
            this.pos.Y = pos.Y;

            imageWidth = 60;
            imageHeight = 140;
            hitboxOffsetLeft = 17;
            hitboxOffsetRight = 17;
            hitboxOffsetTop = 9;
            hitboxOffsetBottom = 10;

            hp = initialHp;
        }

        public override void Update()
        {
            MoveX();
            MoveY();
        }

        void MoveX()
        {
            // 横に移動する
            pos.X += velocity.X;

            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float middle = top + 32;
            float bottom = GetBottom() - 0.01f;

            // 左端が壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) ||    // 左上が壁か？
                playScene.map.IsWall(left, middle) || // 左中が壁か？
                playScene.map.IsWall(left, bottom))   // 左下が壁か？
            {
                float wallRight = left - left % Map.CellSize + Map.CellSize; // 壁の右端
                SetLeft(wallRight); // 一般人の左端を壁の右端に沿わす
            }
            // 右端が壁にめりこんでいるか？
            else if (
                playScene.map.IsWall(right, top) ||   // 右上が壁か？
                playScene.map.IsWall(right, middle) || // 右中が壁か？
                playScene.map.IsWall(right, bottom))  // 右下が壁か？
            {
                float wallLeft = right - right % Map.CellSize; // 壁の左端
                SetRight(wallLeft); // 一般人の右端を壁の左端に沿わす
            }
        }

        // 縦の移動処理
        void MoveY()
        {
            // 縦に移動する
            pos.Y += velocity.Y;

            // 着地したかどうか
            bool grounded = false;

            // 当たり判定の四隅の座標を取得
            float left = GetLeft();
            float right = GetRight() - 0.01f;
            float top = GetTop();
            float bottom = GetBottom() - 0.01f;

            // 上端が壁にめりこんでいるか？
            if (playScene.map.IsWall(left, top) || // 左上が壁か？
                playScene.map.IsWall(right, top))   // 右上が壁か？
            {
                float wallBottom = top - top % Map.CellSize + Map.CellSize; // 天井のy座標
                SetTop(wallBottom); // 一般人の頭を天井に沿わす
                velocity.Y = 0; // 縦の移動速度を0に
            }
            // 下端が壁にめりこんでいるか？
            else if (
                playScene.map.IsWall(left, bottom) || // 左下が壁か？
                playScene.map.IsWall(right, bottom))   // 右下が壁か？
            {
                grounded = true; // 着地した
            }

            if (grounded) // もし着地してたら
            {
                float wallTop = bottom - bottom % Map.CellSize; // 床のy座標
                SetBottom(wallTop); // 一般人の足元を床の高さに沿わす
                velocity.Y = 0; // 縦の移動速度を0に
            }
        }

        public override void OnCollision(GameObject other)
        {
        }

        public void Die()
        {
            isDead = true;
        }

        public override void Draw()
        {
            Camera.DrawGraph(pos.X, pos.Y, Image.guardman);
        }
    }
}
