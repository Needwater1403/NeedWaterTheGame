using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
//using Input = InputWrapper.Input;
public class BottleController : MonoBehaviour
{
    public Color[] liquidColor;
    private float _beerHight;
    private float _foamHight;
    private float _temp;
    private bool _isStop;
    private bool _isEnd;
    public SpriteRenderer[] liquidSpriteRenderer;
    public SpriteRenderer sRenderer;
    [FormerlySerializedAs("Max")] public Transform max;
    [FormerlySerializedAs("Min")] public Transform min;
    public TextMeshProUGUI status;
    private static float _spriteSize;
    private void Start()
    {
        _isEnd = false;
        _spriteSize = sRenderer.bounds.size.y;
        _beerHight = - _spriteSize / 2;
        _foamHight = - _spriteSize / 2;
        max.position = new Vector2(max.position.x,UnityEngine.Random.Range(0.0f, _spriteSize/ 2));
        min.position = new Vector2(min.position.x,UnityEngine.Random.Range(-_spriteSize/ 4, max.position.y - _spriteSize/ 10));
        Debug.Log(_spriteSize + " Max: " + max.position.y + " Min: " + min.position.y);
        liquidSpriteRenderer[0].material.SetFloat("_FillAmount1", _beerHight);
        liquidSpriteRenderer[1].material.SetFloat("_FillAmount1", _foamHight);
    }

    private void FixedUpdate()
    {
        // if(!_isStop && _beerHight < 2.58f && _foamHight < 2.58f)
        // {
        //     _beerHight += 0.015f;
        //     _foamHight += 0.025f;
        //     liquidSpriteRenderer[0].material.SetFloat("_FillAmount1", _beerHight);
        //     liquidSpriteRenderer[1].material.SetFloat("_FillAmount1", _foamHight);
        // }
        // else
        // {
        //     if(!_isEnd)
        //     {
        //         var c = _foamHight - _beerHight;
        //         _temp = _beerHight + c / 2.5f;
        //         _isEnd = true;
        //     }
        //     _foamHight -= 0.015f;
        //     liquidSpriteRenderer[1].material.SetFloat("_FillAmount1", _foamHight);
        //     if (!(_beerHight < _temp))
        //     {
        //         CheckResult();
        //         return;
        //     }
        //     _beerHight += 0.01f;
        //     liquidSpriteRenderer[0].material.SetFloat("_FillAmount1", _beerHight);
        // }
        Test();
    }

    public void OnClick()
    {
        //_isStop = true;
    }
    
    private void CheckResult()
    {
        //CHECK WIN CONDITION (Min <= a <= Max)
        if(status.gameObject.activeInHierarchy) return;
        var minValue = min.localPosition.y;
        var maxValue = max.localPosition.y;
        if (_beerHight < maxValue && _beerHight > minValue)
        {
            status.SetText("YOU WIN!");
            status.gameObject.SetActive(true);
            Debug.Log("a = " + _beerHight + " maxValue = " + maxValue + " minValue = " + minValue);
        }
        else
        {
            status.SetText("YOU LOSE!");
            status.gameObject.SetActive(true);
            Debug.Log("a = " + _beerHight + " maxValue = " + maxValue + " minValue = " + minValue);
        }
    }

    private void Test()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (!_isEnd && touch.phase == TouchPhase.Stationary)
            {
                // GAMEPLAY
                if(_beerHight < 2.58f && _foamHight < 2.58f)
                {
                    _beerHight += 0.015f;
                    _foamHight += 0.025f;
                    liquidSpriteRenderer[0].material.SetFloat("_FillAmount1", _beerHight);
                    liquidSpriteRenderer[1].material.SetFloat("_FillAmount1", _foamHight);
                }
            }

            if (!_isEnd && touch.phase is TouchPhase.Ended or TouchPhase.Canceled)
            {
                _isEnd = true;
                var c = _foamHight - _beerHight;
                _temp = _beerHight + c / 2.5f;
            }
            
        }
        if (_isEnd)
        {
            _foamHight -= 0.015f;
            liquidSpriteRenderer[1].material.SetFloat("_FillAmount1", _foamHight);
            if (!(_beerHight < _temp))
            {
                CheckResult();
                return;
            }
            _beerHight += 0.01f;
            liquidSpriteRenderer[0].material.SetFloat("_FillAmount1", _beerHight);
        }
    }
}
