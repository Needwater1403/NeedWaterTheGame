using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;
using DG.Tweening;

//using Input = InputWrapper.Input;
public class BottleController : MonoBehaviour
{
    private enum GameState  {_start,_isPlaying, _end, _wait};
    private float _beerHight;
    private float _foamHight;
    private float _temp;
    private bool _isStop;
    private float endPos;
    private float startPos;
    private GameState _state;
    public SpriteRenderer[] liquidSpriteRenderer;
    public SpriteRenderer sRenderer;
    [FormerlySerializedAs("Max")] public Transform max;
    [FormerlySerializedAs("Min")] public Transform min;
    public TextMeshProUGUI status;
    private static float _spriteSize;
    private int _score = 0;
    private void Start()
    {
        _state = GameState._wait;
        var bound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        endPos = bound.x + sRenderer.bounds.size.x / 2f + 1;
        startPos =  - endPos;
        transform.position = new Vector3(startPos, transform.position.y, 0);
        ResetParam();
    }

    private void FixedUpdate()
    {
        Test();
    }
    

    private void Test()
    {
        if (Input.touchCount > 0 && _state == GameState._start)
        {
            
            _isStop = true;
            var touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Stationary:
                {
                    // GAMEPLAY
                    if(_beerHight < 2.58f && _foamHight < 2.58f)
                    {
                        _beerHight += 0.015f;
                        _foamHight += 0.025f;
                        liquidSpriteRenderer[0].material.SetFloat("_FillAmount1", _beerHight);
                        liquidSpriteRenderer[1].material.SetFloat("_FillAmount1", _foamHight);
                    }

                    break;
                }
                case TouchPhase.Ended or TouchPhase.Canceled:
                {
                    _isStop = false;
                    _state = GameState._isPlaying;
                    var c = _foamHight - _beerHight;
                    _temp = _beerHight + c / 2.5f;
                    break;
                }
            }
        }
        else if(_isStop)
        {
            _isStop = false;
            _state = GameState._isPlaying;
            var c = _foamHight - _beerHight;
            _temp = _beerHight + c / 2.5f;
        }
        
        if (_state == GameState._isPlaying)
        {
            _foamHight -= 0.015f;
            liquidSpriteRenderer[1].material.SetFloat("_FillAmount1", _foamHight);
            if (!(_beerHight < _temp))
            {
                _state = GameState._end;
                CheckResult();
                return;
            }
            _beerHight += 0.01f;
            liquidSpriteRenderer[0].material.SetFloat("_FillAmount1", _beerHight);
        }
    }
    IEnumerator SlideOutTransition()
    {
        min.GetComponent<SpriteRenderer>().material.DOFade(0,.6f);
        max.GetComponent<SpriteRenderer>().material.DOFade(0,.6f);
        yield return new WaitForSeconds(.6f);
        var position = transform.position;
        gameObject.transform.DOMove(new Vector3(endPos, position.y, 0),2,false);
        yield return new WaitForSeconds(2f);
        gameObject.transform.position = new Vector3(startPos, position.y, 0);
        ResetParam();
        yield return null;
    }

    IEnumerator SlideInTransition()
    {
        gameObject.transform.DOMove(new Vector3(0, transform.position.y, 0),2,false);
        yield return new WaitForSeconds(2f);
        min.GetComponent<SpriteRenderer>().material.DOFade(1,.6f);
        max.GetComponent<SpriteRenderer>().material.DOFade(1,.6f);   
        yield return new WaitForSeconds(.6f);
        _state = GameState._start;
        yield return null;
    }
    private void ResetParam()
    {
        _spriteSize = sRenderer.bounds.size.y;
        _beerHight = - _spriteSize / 2;
        _foamHight = - _spriteSize / 2;
        max.position = new Vector2(max.position.x,UnityEngine.Random.Range(0.0f, _spriteSize/ 2));
        min.position = new Vector2(min.position.x,UnityEngine.Random.Range(-_spriteSize/ 4, max.position.y - _spriteSize/ 10));
        liquidSpriteRenderer[0].material.SetFloat("_FillAmount1", _beerHight);
        liquidSpriteRenderer[1].material.SetFloat("_FillAmount1", _foamHight);
        //status.SetText("");
        StartCoroutine(SlideInTransition());
    }
    private void CheckResult()
    {
        //CHECK WIN CONDITION (Min <= a <= Max)
        var minValue = min.localPosition.y;
        var maxValue = max.localPosition.y;
        if (_beerHight < maxValue && _beerHight > minValue)
        {
            _score++;
            status.SetText(_score.ToString());
            StartCoroutine(SlideOutTransition());
            status.gameObject.SetActive(true);
            //Debug.Log("a = " + _beerHight + " maxValue = " + maxValue + " minValue = " + minValue + " state = " + _state.ToString());
        }
        else
        {
            Debug.Log("a = " + _beerHight + " maxValue = " + maxValue + " minValue = " + minValue + " state = " + _state.ToString());
            status.SetText("YOU LOSE!");
            status.gameObject.SetActive(true);
        }
    }
}
