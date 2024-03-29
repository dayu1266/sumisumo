﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using QimOLib;
using System.Numerics;

namespace sumisumo
{
    public class PlayScene : Scene
    {
        // プレイ画面の状態
        public enum State
        {
            Active, // 通常時
            OnAlert, //警戒中
            PlayerDied, // プレイヤーが死んだとき
        }

        // 参照
        public Map map;
        public Player player;

        // 全GameObjectを一括管理するリスト
        public List<GameObject> gameObjects = new List<GameObject>();

        public List<GameObject> enemyObjects = new List<GameObject>();

        public State state = State.Active;// PlaySceneの状態
        int timeToGameOver = 120; // ゲームオーバーになるまでの時間（フレーム）
        public bool isGoal = false; // ゴールしたかどうか
        bool isPausing = false; // ポーズ中かどうか
        int targetAmout = 1000; // 目標金額

        public PlayScene()
        {
            // インスタンス生成
            map = new Map(this, "stage1");
            Camera.LookAt(player.pos.X, player.pos.Y);
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
                if (Input.GetButtonDown(DX.PAD_INPUT_W))
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

            // 全オブジェクトの更新
            int enemyObjectsCount = enemyObjects.Count; // ループ前の個数を取得しておく
            for (int i = 0; i < enemyObjectsCount; i++)
            {
                enemyObjects[i].Update();
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

            // オブジェクト同士の衝突を判定
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject a = gameObjects[i];

                for (int j = i; j < gameObjects.Count; j++)
                {
                    // オブジェクトAが死んでたらこのループは終了
                    if (a.isDead) break;

                    GameObject b = enemyObjects[j];

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
            enemyObjects.RemoveAll(go => go.isDead);

            Camera.LookAt(player.pos.X, player.pos.Y);

            // プレイヤーがゴールしたときの処理
            if (state != State.PlayerDied && isGoal)
            {
                Game.ChangeScene(new GameClearScene());// GameClearSceneにする
            }

            // プレイヤーが死んでゲームオーバーに移る直前の状態の処理
            if (state == State.PlayerDied)
            {
                timeToGameOver--; // カウントダウン

                if (timeToGameOver <= 0) // 0になったら
                {
                    Game.ChangeScene(new GameOverScene()); // GameOverSceneにする
                }
            }

            // STARTボタン（Wキー）が押されたらポーズ
            if (Input.GetButtonDown(DX.PAD_INPUT_W))
            {
                isPausing = true;
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, Image.play_bg, 0);

            // マップの描画
            map.DrawTerrain();
            
            // プレイヤーのHPのUI
            for (int i = 0; i < player.hp; i++)
            {
                DX.DrawRotaGraph(32 + (i * 48), 20, 2, 0, Image.heart, 1);
            }

            // 全オブジェクトの描画
            foreach (GameObject go in gameObjects)
            {
                go.Draw();
            }

            // 全オブジェクトの描画
            foreach (GameObject go in enemyObjects)
            {
                go.Draw();
            }

            DX.DrawGraph(1020, 16, Image.curMoney, 1);
            // プレイヤーの所持金表示
            string money = player.curMoney.ToString("000000");
            for (int i = 0; i < money.Length; i++)
            {
                DX.DrawRotaGraph(1140 + (16 * i), 32, 1.0f, 0, Image.number[money[i] - '0'], 1);
            }
            DX.DrawGraph(1230, 16, Image.yen, 1);

            //DX.DrawString(1060, 26, "/", DX.GetColor(0, 0, 0));
            //for (int i = 0; i < targetAmout.ToString().Length; i++)
            //{
            //    DX.DrawRotaGraph(1080 + (16 * i), 32, 0.3f, 0, Image.number[targetAmout.ToString()[i] - '0'], 1);
            //}

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

            #if DEBUG // Debugのみ実行される
            // 当たり判定のデバッグ表示
            foreach (GameObject go in gameObjects)
            {
                go.DrawHitBox();
            }
            // 視野のデバッグ表示
            foreach (GameObject go in gameObjects)
            {
                go.DrawView();
            }
            // 当たり判定のデバッグ表示
            foreach (GameObject go in enemyObjects)
            {
                go.DrawHitBox();
            }
            // 視野のデバッグ表示
            foreach (GameObject go in enemyObjects)
            {
                go.DrawView();
            }
#endif
        }
    }
}