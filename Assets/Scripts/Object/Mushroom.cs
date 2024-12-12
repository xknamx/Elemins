using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    Animator animator;
    ParticleSystem particle;
    public bool isBorn; //キノコが生えたかのフラグ
   // public bool isExit; //踏まれたあとにプレイヤーが一度離れたかの判定
    public bool isClush; //キノコがこわれたかのフラグ

    void Start()
    {
        animator = GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        isBorn = false;
     //   isExit = false;
        isClush = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    animator.SetTrigger("MushClush");
        //}
    }

    public void ParticlePlay()
    {
        particle.Play();
    }

    public void FinishBornAnim()
    {
        isBorn = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "StepCollider" && isBorn && !isClush)
        {

                animator.SetTrigger("MushClush");
                isClush = true;

                StartCoroutine(EnableMush());
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "StepCollider" && !isBorn)
        {
                animator.SetTrigger("Stepped");
               // isBorn = true;
           
        }

    }
    IEnumerator EnableMush()
    {
        yield return new WaitForSeconds(0.3f);
        //アニメーションの再生待ちのため3秒待って非アクティブにする
        gameObject.SetActive(false);
    }
}
