using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Interfaces;
using UnityEngine;

public class BasicPlatform : MonoBehaviour, IDimensional
{
    [field: Header("Platform Infos")]
    [field: Space]
    [field:SerializeField] public DimensionType DimensionType { get; private set;}
    [SerializeField] protected Transform _visual;
    [SerializeField] protected Transform _visualHolo;
    
    [Header("Player Bounce Tween")]
    [Space]
    [SerializeField] protected float _moveDownDistance = 0.1f;  // Ne kadar aşağı hareket edecek
    [SerializeField] protected float _moveDuration = 0.2f;
    [SerializeField] protected float _bounceAmplitude = 0.05f;  // Aşağı ve yukarı sürekli hareket miktarı
    [SerializeField] protected float _bounceDuration = 0.5f;
    
    private Tween _bounceTween;
    
    private Vector3 _initialPosition;

    private BoxCollider2D _boxCollider2D;
    private void Awake()
    {
        _initialPosition = transform.position;
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Show()
    {
        if(DimensionType == DimensionType.Both)
            return;
        _boxCollider2D.enabled = true;
        _visual.gameObject.SetActive(true);
        _visualHolo.gameObject.SetActive(false);
    }

    public void Hide()
    {
        if(DimensionType == DimensionType.Both)
            return;
        _boxCollider2D.enabled = false;
        _visual.gameObject.SetActive(false);
        _visualHolo.gameObject.SetActive(true);
    }

    public DimensionType GetDimensionType()
    {
        return DimensionType;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform); 

            // İlk başta normal aşağı hareket
            transform.DOMoveY(_initialPosition.y - _moveDownDistance, _moveDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => StartBounce());  // Aşağı inme tamamlandıktan sonra sürekli bounce başlat
        }
    }

    private void StartBounce()
    {
        // Sürekli aşağı yukarı hareket etmesi için loop tween başlatılır
        _bounceTween = transform.DOMoveY(_initialPosition.y - _bounceAmplitude, _bounceDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);  // Sonsuz ileri geri hareket
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null); 

            // Oyuncu platformdan ayrıldığında sürekli bounce hareketini durdur
            if (_bounceTween != null)
            {
                _bounceTween.Kill();
                _bounceTween = null;
            }

            // Platformu eski pozisyonuna geri döndür
            transform.DOMoveY(_initialPosition.y, _moveDuration)
                .SetEase(Ease.OutQuad);
        }
    }
    
    
}
