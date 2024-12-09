using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sydewa;

public class UI_Loading : MonoBehaviour
{
    Animator animator;
    Slider slider;
    GraphicRaycaster raycaster;
    //string targetScene; //遷移先のシーン名

    [SerializeField] LightingManager lightingManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        slider = GetComponentInChildren<Slider>();
        raycaster = GetComponent<GraphicRaycaster>();
    }

    private void Start()
    {
        ResetLoadingUI();
        Debug.Log(slider != null ? slider.gameObject.name : "スライダーが見つかりません");
    }


    

    void ResetLoadingUI()
    {
        animator.SetBool("IsLoading", false);
        raycaster.enabled = false;
        slider.value = 0;

    }
    public void LoadingScene(string targetScene)
    {


        raycaster.enabled = true;
        StartCoroutine(LoadScene(targetScene));

    }

    IEnumerator LoadScene(string nextScene)
    {

        Debug.Log($"{nextScene}をローディング");
        animator.SetBool("IsLoading", true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextScene); //読み込み状況を取得
                                                                                // asyncOperation.allowSceneActivation = false; //読み込み完了後自動で遷移しない
       

        while (!asyncOperation.isDone)
        {
            slider.value = asyncOperation.progress; // 読み込み進行状況をスライダーに反映

            Debug.Log($"Loading Progress: {asyncOperation.progress}");


            if (asyncOperation.progress >= 0.95f) // 
            {
                // スライダーを最大値に設定
                slider.value = 1f;
                yield return new WaitForSeconds(0.2f); // アニメーション完了待機

            }

            yield return null;
        }
        if (lightingManager.SunDirectionalLight == null)
        {
            //LightingManagerのSunDirectionalLightがnullならシーンのDirectionalLightをセットする
            lightingManager.SunDirectionalLight = GameObject.FindWithTag("DirectionalLight").GetComponent<Light>();
        }
        ResetLoadingUI();
    }
}
