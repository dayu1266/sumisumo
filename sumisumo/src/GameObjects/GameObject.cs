using DxLibDLL;
using QimOLib;
using System.Numerics;

namespace sumisumo
{
    // ゲーム上に表示される物体の基底クラス。
    // プレイヤーや敵、アイテムなどはこのクラスを継承して作る。
    public abstract class GameObject
    {   
        public Vector2 pos = new Vector2(); // プレイヤーのポジション
        public bool isDead = false;         // 死んだ（削除対象）フラグ

        protected PlayScene playScene;  // PlaySceneの参照
        protected int imageWidth;       // 画像の横ピクセル数
        protected int imageHeight;      // 画像の縦ピクセル数
        protected int hitboxOffsetLeft   = 0; // 当たり判定の左端のオフセット
        protected int hitboxOffsetRight  = 0; // 当たり判定の右端のオフセット
        protected int hitboxOffsetTop    = 0; // 当たり判定の上端のオフセット
        protected int hitboxOffsetBottom = 0; // 当たり判定の下端のオフセット

        protected int viewTop    = 0; // 視野の上端
        protected int viewBottom = 0; // 視野の下端
        protected int viewLeft   = 0; // 視野の左端
        protected int viewRight  = 0; // 視野の右端

        // コンストラクタ
        public GameObject(PlayScene playScene)
        {
            this.playScene = playScene;
        }

        // 当たり判定の左端を取得
        public virtual float GetLeft()
        {
            return pos.X + hitboxOffsetLeft;
        }

        // 左端を指定することにより位置を設定する
        public virtual void SetLeft(float left)
        {
            pos.X = left - hitboxOffsetLeft;
        }

        // 右端を取得
        public virtual float GetRight()
        {
            return pos.X + imageWidth - hitboxOffsetRight;
        }

        // 右端を指定することにより位置を設定する
        public virtual void SetRight(float right)
        {
            pos.X = right + hitboxOffsetRight - imageWidth;
        }

        // 上端を取得
        public virtual float GetTop()
        {
            return pos.Y + hitboxOffsetTop;
        }

        // 上端を指定することにより位置を設定する
        public virtual void SetTop(float top)
        {
            pos.Y = top - hitboxOffsetTop;
        }

        // 下端を取得する
        public virtual float GetBottom()
        {
            return pos.Y + imageHeight - hitboxOffsetBottom;
        }

        // 下端を指定することにより位置を設定する
        public virtual void SetBottom(float bottom)
        {
            pos.Y = bottom + hitboxOffsetBottom - imageHeight;
        }

        public virtual int GetViewTop()
        {
            return (int)pos.Y + viewTop;
        }

        public virtual int GetViewBottom()
        {
            return (int)pos.Y + imageHeight - viewBottom;
        }

        public virtual int GetViewRight()
        {
            return (int)pos.X + imageWidth - viewRight;
        }

        public virtual int GetViewLeft()
        {
            return (int)pos.X + viewLeft;
        }

        public virtual void SetViewRight(int value)
        {
            viewRight = value;
        }

        public virtual void SetViewLeft(int value)
        {
            viewLeft = value;
        }

        public virtual void ViewDirectionChange()
        {
            int tmpL = viewLeft;
            int tmpR = viewRight;
            SetViewLeft(tmpR);
            SetViewRight(tmpL);
        }

        // 更新処理
        public abstract void Update();

        // 描画処理
        public abstract void Draw();

        // 当たり判定を描画（デバッグ用）
        public void DrawHitBox()
        {
            // 四角形を描画
            Camera.DrawLineBox(GetLeft(), GetTop(), GetRight(), GetBottom(), DX.GetColor(255, 0, 0));
        }

        // 当たり判定を描画（デバッグ用）
        public void DrawView()
        {
            // 四角形を描画
            Camera.DrawLineBox(GetViewLeft(), GetViewTop(), GetViewRight(), GetViewBottom(), DX.GetColor(0, 0, 255));
        }

        // 他のオブジェクトと衝突したときに呼ばれる
        public abstract void OnCollision(GameObject other);

        // ほかのオブジェクトが視界に入った時呼ばれる
        public abstract void OnView(GameObject other);

        // 画面内に映っているか？
        public virtual bool IsVisible()
        {
            return Math2D.RectRectIntersect(
                pos.X, pos.Y, pos.X + imageWidth, pos.Y + imageHeight,
                Camera.cameraPos.X, Camera.cameraPos.Y, Camera.cameraPos.X + Screen.Size.X, Camera.cameraPos.Y + Screen.Size.Y);
        }
    }
}
