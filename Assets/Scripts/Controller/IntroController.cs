using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{

    public Animator IntroAnimator;
    // Use this for initialization
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (!IntroAnimator.GetCurrentAnimatorStateInfo(0).IsName("IntroStart") && Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene("main");
        }
    }
}
