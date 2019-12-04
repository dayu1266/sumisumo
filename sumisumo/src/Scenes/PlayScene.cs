using System.Numerics;
using System.Collections.Generic;
using DxLibDLL;
using QimOLib;

namespace sumisumo
{
    public class PlayScene : Scene
    {
        // プレイ画面の状態
        public enum State
        {
            Active, // 通常時
            PlayerDied, // プレイヤーが死んだとき
        }

        // 参照
        public Map map;
        public Player player;

        // 全GameObjectを一括管理するリスト
        public List<GameObject> gameObjects = new List<GameObject>();

        public State state = State.Active;// PlaySceneの状態
        int timeToGameOver = 120; // ゲームオーバーになるまでの時間（フレーム）
        bool isPausing = false; // ポーズ中かどうか

        public PlayScene()
        {
            // インスタンス生成
            map = new Map(this, "stage1");
            Camera.LookAt(player.pos.X,player.pos.Y);
        }

        public override void Init()
        {
            
        }

        public override void Update()
        {
            // ポーズ中の場合
            if (isPausing)
            {
                // STARTボタン（Wキー）が押されたら再開
                if (Input.GetButtonDown(DX.PAD_INPUT_UP))
                {
                    isPausing = false;
                }
                return; // Update()を抜ける
            }

            // 全オブジェクトの更新
            int gameObjectsCount = gameObjects.Count; // ループ前の個数を取得しておく
            for (int i = 0; i < gameObjectsCount; i++)
            {
                gameObjects[i].Update();
            }

            // オブジェクト同士の衝突を判定
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject a = gameObjects[i];

                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    // オブジェクトAが死んでたらこのループは終了
                    if (a.isDead) break;

                    GameObject b = gameObjects[j];

                    // オブジェクトBが死んでたらスキップ
                    if (b.isDead) continue;

                    // オブジェクトAとBが重なっているか？
                    if (Math2D.RectRectIntersect(
                        a.GetLeft(), a.GetTop(), a.GetRight(), a.GetBottom(),
                        b.GetLeft(), b.GetTop(), b.GetRight(), b.GetBottom()))
                    {
                        a.OnCollision(b);
                        b.OnCollision(a);
                    }
                }
            }

            // 不要となったオブジェクトを除去する
            gameObjects.RemoveAll(go => go.isDead);

            Camera.LookAt(player.pos.X,player.pos.Y);

            // プレイヤーが死んでゲームオーバーに移る直前の状態の処理
            if (state == State.PlayerDied)
            {
                timeToGameOver--; // カウントダウン

                if (timeToGameOver <= 0) // 0になったら
                {
                    Game.ChangeScene(new ResultScene()); // GameOverSceneにする
                }
            }

            // STARTボタン（Wキー）が押されたらポーズ
            if (Input.GetButtonDown(DX.PAD_INPUT_UP))
            {
                isPausing = true;
            }
        }

        public override void Draw()
        {
            // マップの描画
            map.DrawTerrain();

            // 全オブジェクトの描画
            foreach (GameObject go in gameObjects)
            {
                go.Draw();
            }

            // ポーズ中の半透明のスクリーンの描画
            if (isPausing)
            {
                // 半透明の指定。第2引数で0～255でアルファ値（不透明度）を指定する。
                // 不透明度を変えたら、明示的に元に戻すまでは継続されるので注意
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 80);
                // 画面全体を黒で塗りつぶす
                DX.DrawBox(0, 0, (int)Screen.Size.X, (int)Screen.Size.Y, DX.GetColor(0, 0, 0), DX.TRUE);
                // 不透明度を元に戻す
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, 255);
            }

            // Debugのみ実行される
            #if DEBUG
            // 当たり判定のデバッグ表示
            foreach (GameObject go in gameObjects)
            {
                go.DrawHitBox();
            }
            #endif
        }
    }
}