using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScaleOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleFactor = 1.1f;

    private Vector3 _startScale;
    private Tween _tween;

    private void Start()
    {
        _startScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tween.Kill();
        _tween = transform.DOScale(_startScale * scaleFactor, 0.25f).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tween.Kill();
        _tween = transform.DOScale(_startScale, 0.25f).SetEase(Ease.OutBack);
    }

    private void OnDisable()
    {
        _tween.Kill();
        transform.localScale = _startScale;
    }
}
