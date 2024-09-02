using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RecoilFeedback : Feedback
{
    [SerializeField] private Transform _targetTrm;
    [SerializeField] private float _recoilAmmount = .15f, _recoilTime = .05f;

    private Vector3 _initPos;
    private Tween _recoilTween;

    private void Awake()
    {
        _initPos = _targetTrm.localPosition;
    }

    public override void PlayFeedback()
    {

    }

    public override void StopFeedback()
    {

    }
}
