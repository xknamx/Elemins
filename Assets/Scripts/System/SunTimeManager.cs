using Cinemachine;
using Sydewa;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//エンディング処理用
//太陽を上らせ、花を咲かせる命令をするクラス
public class SunTimeManager : MonoBehaviour
{
    public static SunTimeManager Instance { get; private set; }
    // public float CurrentTime { get; private set; } //現在の時刻

    public LightingManager lightingManager; //夜明け用の時刻管理用アセット
     FootPrintsAllController footPrintsAllController;　//シーンのすべての足跡を管理するクラス

    [SerializeField] float startTimeOfDay = 2;　//ゲーム開始時点(暗闇時)の時刻設定
    [SerializeField] float targetTimeOfDay = 12f;　//太陽が昇ったときの時刻設定
    [SerializeField] float sunRiseSpeed = 1f;　//太陽の上るスピード
    [SerializeField] float startBloomSunTime = 6f; //花が咲き始める時刻

    CinemachineVirtualCamera endcam;



    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも破棄されないようにする
        }
        else
        {
            Destroy(gameObject); // 二重に存在する場合は破棄
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lightingManager = GetComponent<LightingManager>();
        //ゲーム開始時の時刻設定
        lightingManager.TimeOfDay = startTimeOfDay;
        lightingManager.SunDirectionalLight = GameObject.FindWithTag("DirectionalLight").GetComponent<Light> ();
         footPrintsAllController = GameObject.FindAnyObjectByType<FootPrintsAllController>();
    }

    //エンディングイベント時のイベント
    public void Ending()
    {
        StartCoroutine(SunRise());
    }

    //夜明けコルーチン
    IEnumerator SunRise()
    {
        footPrintsAllController = GameObject.FindAnyObjectByType<FootPrintsAllController>();
        //目標時刻になるまで繰り返す
        while (lightingManager.TimeOfDay < targetTimeOfDay)
        {
            // 時刻を徐々に増加
            lightingManager.TimeOfDay += sunRiseSpeed * Time.deltaTime;

            //設定時刻になったらfootPrintsAllControllerの花を咲かせるメソッドを呼び出す
            if (lightingManager.TimeOfDay >= startBloomSunTime)
            {
                footPrintsAllController.GetFootPrintsFlowers();
            }

            // 次のフレームまで待機
            yield return null;
        }

        // 最終的に目標時刻にそろえる
        lightingManager.TimeOfDay = targetTimeOfDay;

        endcam = GameObject.FindWithTag("EndCam").GetComponent<CinemachineVirtualCamera>();
        if (endcam != null)
        {
            endcam.Priority = 50;
        }

        StartCoroutine(EndGame());
    }

    //クリア後にタイトルに戻る
    IEnumerator  EndGame()
    {
        GameManager.Instance.SaveGame();
        yield return new WaitForSeconds(15f);
        GameManager.Instance.BackTitleScene();
    }
}
