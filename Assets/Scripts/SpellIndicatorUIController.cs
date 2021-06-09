using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellIndicatorUIController : MonoBehaviour
{
    private Transform _indicatorTail;
    private Transform _indicatorHead;

    private SpriteRenderer _srIndicatorTail;

    private float defaultHeightOfTail = 1f ;
    private float defaultYPositionOfHead = 1.5f ;
    
    private void Awake()
    {
        _indicatorTail = transform;
        _indicatorHead = transform.GetChild(0);
        _srIndicatorTail = _indicatorTail.GetComponent<SpriteRenderer>();
    }

    public void SetUI(float height)
    {
        _srIndicatorTail.size = new Vector2(_srIndicatorTail.size.x, defaultHeightOfTail + height - 2.5f);
        _indicatorHead.localPosition = new Vector3(_indicatorHead.localPosition.x, defaultYPositionOfHead + height - 2.5f );
    }
    
    public void ResetUI()
    {
        SetUI(0);
    }
    
}
