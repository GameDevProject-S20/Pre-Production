using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using SIEvents;

public class InfoPanel : MonoBehaviour
{

    protected enum States { Closed, Open, Selected }
    protected States state;

    [SerializeField]
    protected RectTransform rt;

    [SerializeField]
    protected RectTransform pointerRt;
    protected Tween t;

    [SerializeField]
    int a;
    [SerializeField]
    int b;

    [SerializeField]
    protected float openTime = 0.2f;


    protected virtual void Awake()
    {
        Close();
    }

    private void Update() {
        rt.ForceUpdateRectTransforms();
    }

    protected void Open()
    {
        state = States.Open;
        Sequence s = DOTween.Sequence();
        s.Append(pointerRt.DOScale(1, openTime));
        s.Join(pointerRt.DOLocalMoveY(-13, openTime));
        s.Join(rt.DOLocalMoveY(a, openTime));
        s.Insert(openTime / 2, rt.DOScaleX(1, openTime));
        s.Play();
    }

    protected virtual void Close()
    {
        state = States.Closed;
        Sequence s = DOTween.Sequence();
        s.Append(rt.DOScaleX(0, openTime));
        s.Insert(openTime / 2, pointerRt.DOScale(0, openTime));
        s.Join(pointerRt.DOLocalMoveY(-40, openTime));
        s.Join(rt.DOLocalMoveY(b, openTime));
        s.AppendCallback(OnClosed);
        s.Play();

    }

    protected virtual void OnClosed(){

    }
}
