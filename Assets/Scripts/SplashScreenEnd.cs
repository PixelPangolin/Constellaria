﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenEnd : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SceneManager.LoadScene("Opening Cutscene", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {

    }
}