using DxLibDLL;

namespace sumisumo
{
    public static class Image
    {
        public static int player;                        // プレイヤー
        public static int test_shiitake;                 // しいたけ
        public static int test_playerShot;               // プレイヤーの弾
        public static int[] test_zentaman = new int[22]; // ゼンタマン
        public static int[] test_mapchip = new int[128]; // マップチップ(地形・背景)画像
        public static int play_bg;                       // プレイ画面の背景
        public static int goMenu;                        // メインメニューに戻る
        public static int retry;                         // リトライ
        public static int nextStage;                     // 次のステージへ
        public static int cursor;                        // カーソル

        public static void Load()
        {
            player = DX.LoadGraph("res/Image/player.png");
            test_shiitake = DX.LoadGraph("res/Image/test_shiitake.png");
            test_playerShot = DX.LoadGraph("res/Image/test_player_shot.png");
            DX.LoadDivGraph("res/Image/test_zentaman.png", test_zentaman.Length, 4, 6, 60, 70, test_zentaman);
            DX.LoadDivGraph("res/Image/test_mapchip.png", test_mapchip.Length, 16, 8, 32, 32, test_mapchip);
            play_bg = DX.LoadGraph("res/Image/background.png");
            goMenu = DX.LoadGraph("res/Image/goMain.png");
            nextStage = DX.LoadGraph("res/Image/nextStage.png");
            retry = DX.LoadGraph("res/Image/retry.png");
            cursor = DX.LoadGraph("res/Image/cursor.png");
        }
    }
}
