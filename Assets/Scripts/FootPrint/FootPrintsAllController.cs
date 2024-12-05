using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FootPrintsAllController : MonoBehaviour
{
    FootPrintController[] footPrints;
    [SerializeField] float bloomInterval = 0.1f; //次の花が咲くまでの待ち時間

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void GetFootPrintsFlowers()
    {
        footPrints = FindObjectsByType<FootPrintController>(FindObjectsSortMode.None);

        // ヒエラルキー順に並べ替える
        footPrints = footPrints.OrderBy(fp => fp.transform.GetSiblingIndex()).ToArray();

        StartCoroutine(BloomFlowersInSequence());

    }

    IEnumerator BloomFlowersInSequence()
    {
        // 逆順でfootPrintsを処理
        for (int i = footPrints.Length - 1; i >= 0; i--)
        {
            // FootPrintControllerを取得して花を咲かせる
            FootPrintController footPrintController = footPrints[i];
            footPrintController.Bloomflowers();

            // 次のプレハブの花を生成するまで遅延を入れる
            yield return new WaitForSeconds(bloomInterval);  //インターバルをとる
        }
    }

    //保存用のデータをリスト化して渡す
    public List<FootPrintData> GetAllFootPrintData()
    {
        FootPrintController[] footPrints = FindObjectsByType<FootPrintController>(FindObjectsSortMode.None);

        List<FootPrintData> footPrintDataList = new List<FootPrintData>();
        foreach (var footPrint in footPrints)
        {
            footPrintDataList.Add(footPrint.GetFootPrintData());
        }

        return footPrintDataList;
    }

}
