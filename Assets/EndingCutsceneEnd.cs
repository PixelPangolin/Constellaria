﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCutsceneEnd : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //Load the main menu
        SceneManager.LoadScene("Opening Cutscene");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
