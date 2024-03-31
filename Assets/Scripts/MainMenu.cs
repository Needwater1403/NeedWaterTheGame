using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool _isMenu;
    // Update is called once per frame
    private void Start()
    {
        _isMenu = true;
    }

    private void Update()
    {
        if (Input.touchCount <= 0 || !_isMenu) return;
        _isMenu = false;
        LoadScene();
    }

    private void LoadScene()
    {
        //Load Animation
        //Load game scene
        SceneManager.LoadScene("SampleScene");
    }
}
