using DxLibDLL;
using QimOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sumisumo
{
    class TitleScene : Scene
    {
        public override void Init()
        {
            Sound.BgmPlay(Sound.bgm_nomalBGM);
        }

        public override void Update()
        {
            Sound.BgmPlay(Sound.bgm_nomalBGM);
            if (Input.GetButtonDown(DX.PAD_INPUT_1))
            {
                Game.ChangeScene(new PlayScene());
            }

        }

        public override void Draw()
        {
            DX.DrawString(0, 0, "TitleSceneです。ボタン押下でPlaySceneへ。", DX.GetColor(255, 255, 255));
        }
    }
}
