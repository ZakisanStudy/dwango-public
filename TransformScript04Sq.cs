using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TransformScript04Sq : MonoBehaviour {

    float posx;//比較対象
    int tag, Rtag;//一番近い物体のtag 右手の情報表示をするために保存するRtag
   
    float speed = 0.02f; //透明化の速さ
    float alfa = 0; //透明度を操作するための変数
    GameObject cam;
    PlayerSpeed psscript;//プレイヤーの速さ

    /*float posbackX;//positionバックアップ
    float posbackY;
    float posbackZ;*/

    bool one, one1, one2, page1;

    //Pinを所得
    public GameObject[] pinUp;//赤い部分
    public GameObject[] pinDown;//針の部分

    //右手の説明画像
    public GameObject[] fish;//魚の説明
    public GameObject[] place;//場所の説明
　　//右手のボタン所得
    private SteamVR_Action_Boolean actionToHapticL = SteamVR_Actions._default.Left;
    private SteamVR_Action_Boolean actionToHapticR = SteamVR_Actions._default.Right;

    // Use this for initialization
    void Start() {
        tag = 1;
        Rtag = 0;
        one = true;
        one1 = true;
        one2 = false;
        page1 = true;
        cam = GameObject.Find("Camera");
        psscript = cam.GetComponent<PlayerSpeed>();
    }

    // Update is called once per frame
    void Update() {
        //それぞれの球体の座標所得
        Vector3[] pos = new Vector3[5];
        /*for (int i = 0; i < 9; i++) {
            pos[i] = new Vector3();
        }*/
        pos[0] = GameObject.Find("Camera").transform.position;
        pos[1] = GameObject.Find("Sphere01").transform.position;
        pos[2] = GameObject.Find("Sphere02").transform.position;
        pos[3] = GameObject.Find("Sphere03").transform.position;
        pos[4] = GameObject.Find("Sphere04").transform.position;
     



        //それぞれの球体のオブジェクト情報所得
        GameObject[] dome = new GameObject[5];
        /*for (int i = 0; i < 9; i++) {
            dome[i] = new GameObject();
        }*/
        dome[1] = GameObject.Find("Sphere01");
        dome[2] = GameObject.Find("Sphere02");
        dome[3] = GameObject.Find("Sphere03");
        dome[4] = GameObject.Find("Sphere04");



        if (one) {
            posx = Vector3.Distance(pos[1], pos[0]);//比較対象の初期化
            one = false;
        }

        //Debug.Log("1個目比較" + Vector3.Distance(pos[1], pos[0]));
        //Debug.Log("2個目比較" + Vector3.Distance(pos[2], pos[0]));

        //右手処理
        if (actionToHapticL.GetState(SteamVR_Input_Sources.RightHand)) {
            page1 = true;
        }
        if (actionToHapticR.GetState(SteamVR_Input_Sources.RightHand)) {
            page1 = false;
        }

        


        for (int i = 1; i < 5; i++) {
            //一番近い天球を検出
            if (posx >= Vector3.Distance(pos[i], pos[0])) {
                dome[i].GetComponent<FadeinScriptSq>().enabled = true;
                dome[i].GetComponent<FadeScriptSq>().enabled = false;
                pinUp[i].GetComponent<MeshControllerL>().enabled = true;
                pinDown[i].GetComponent<MeshControllerL>().enabled = true;
                fish[i].GetComponent<MeshControllerR>().enabled = true;
                place[i].GetComponent<MeshControllerR>().enabled = true;
                Rtag = i;//二回目以降の右手切り替えの際に必要なTag

                for (int j = 1; j < 5; j++) {
                    //それ以外の天球を検出
                    if (i != j) {
                        dome[j].GetComponent<FadeinScriptSq>().enabled = false;
                        dome[j].GetComponent<FadeScriptSq>().enabled = true;
                        pinUp[j].GetComponent<MeshRenderer>().enabled = false;
                        pinDown[j].GetComponent<MeshRenderer>().enabled = false;
                        pinUp[j].GetComponent<MeshControllerL>().enabled = false;
                        pinDown[j].GetComponent<MeshControllerL>().enabled = false;
                        fish[j].GetComponent<MeshRenderer>().enabled = false;
                        place[j].GetComponent<MeshRenderer>().enabled = false;
                        fish[j].GetComponent<MeshControllerR>().enabled = false;
                        place[j].GetComponent<MeshControllerR>().enabled = false;
                        dome[j].GetComponent<Chase>().enabled = false;
                    }
                }
                tag = i;
                //ここが切り替わるタイミング
                this.GetComponent<CamShiftScript>().enabled = true; 
            }

            //右手処理
            if (page1) {
                fish[Rtag].GetComponent<MeshControllerR>().enabled = true;
                place[i].GetComponent<MeshRenderer>().enabled = false;
                place[i].GetComponent<MeshControllerR>().enabled = false;
            }
            else {
                place[Rtag].GetComponent<MeshControllerR>().enabled = true;
                fish[i].GetComponent<MeshRenderer>().enabled = false;
                fish[i].GetComponent<MeshControllerR>().enabled = false;
            }
        }
        posx = Vector3.Distance(pos[tag], pos[0]);//比較対象を変更
        posx = posx - 0.1f;//範囲の縮小
        //Debug.Log("比較対象" + posx);
    }

    /// <summary>
    /// ここから関数リスト
    /// </summary>
    public void CamFadeSystem() {
        while (this.GetComponent<RadialBlur>().strength > 1) {
            this.GetComponent<RadialBlur>().strength = alfa;
            alfa += speed;
        }
        alfa = 1;//フェードアウトのための初期値
        one1 = false;
        one2 = true;
        this.GetComponent<RadialBlur>().strength = 1;
    }

    public void CamFadeinSystem() {
        while (this.GetComponent<RadialBlur>().strength <= 0) {
            this.GetComponent<RadialBlur>().strength = alfa;
            alfa -= speed;
        }
        alfa = 0;//二回目ループのための初期値
        one2 = false;
        this.GetComponent<RadialBlur>().strength = 0;
    }

}
