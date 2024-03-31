using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    private TextMeshProUGUI txtTouchToStart;
    private bool _isTransition;

    private void Start()
    {
        _isTransition = false;
        txtTouchToStart = GetComponent<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        if (_isTransition) return;
        _isTransition = true;
        StartCoroutine(FadeTransition());
    }

    private IEnumerator FadeTransition()
    {
        txtTouchToStart.DOFade(1,2f);
        yield return new WaitForSeconds(2f);
        txtTouchToStart.DOFade(0,2f);
        yield return new WaitForSeconds(2f);
        _isTransition = false;
        yield return null;
    }
}
