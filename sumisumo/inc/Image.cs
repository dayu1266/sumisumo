using DxLibDLL;

namespace sumisumo
{
    public static class Image
    {
        public static int test_shiitake;                 // しいたけ
        public static int test_playerShot;               // プレイヤーの弾
        public static int[] test_zentaman = new int[22]; // ゼンタマン
        public static int[] test_mapchip = new int[128]; // マップチップ(地形・背景)画像

        public static void Load()
        {
            test_shiitake = DX.LoadGraph("res/Image/test_shiitake.png");
            test_playerShot = DX.LoadGraph("res/Image/test_player_shot.png");
            DX.LoadDivGraph("res/Image/test_zentaman.png", test_zentaman.Length, 4, 6, 60, 70, test_zentaman);
            DX.LoadDivGraph("res/Image/test_mapchip.png", test_mapchip.Length, 16, 8, 32, 32, test_mapchip);
        }
    }
}
