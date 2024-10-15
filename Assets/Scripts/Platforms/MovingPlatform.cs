using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Enums;
using Interfaces;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IDimensional
{
    [field: Header("Platform Infos")]
    [field: Space]
    [field:SerializeField] public DimensionType DimensionType { get; private set;}
    [SerializeField] protected Transform _visual;
    [SerializeField] protected Transform _visualHolo;
    
    [Header("Moving Platform Settings")]
    [SerializeField] private Vector2 _moveDistance;  
    [SerializeField] private float _moveDuration = 2f;  
    private Vector3 _startPosition; 
    private Tween _moveTween;  

    private BoxCollider2D _boxCollider2D;
    private void Awake()
    {
        
        _startPosition = transform.position;
        _boxCollider2D = GetComponent<BoxCollider2D>();
        
        StartMovement();
    }
    
    private void FixedUpdate()
    {
        // Tween işlemleri fizik motoruyla uyumlu çalışacak şekilde zamanlanır
        DOTween.ManualUpdate(Time.fixedDeltaTime, Time.fixedDeltaTime);
    }

    private void StartMovement()
    {
        _moveTween = transform.DOMove(_startPosition + (Vector3)_moveDistance, _moveDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);  
    }

    private void OnDisable()
    {
        if (_moveTween != null)
        {
            _moveTween.Kill();
        }
    }
    
    private void OnDrawGizmos()
    {
        // Başlangıç pozisyonunu al
        Vector3 startPosition = Application.isPlaying ? _startPosition : transform.position;

        // Hedef pozisyonu hesapla
        Vector3 endPosition = startPosition + (Vector3)_moveDistance;

        // Gizmos çizimi için renk ayarı
        Gizmos.color = Color.yellow;

        // Platformun gideceği yolu çiz
        Gizmos.DrawLine(startPosition, endPosition);

        // Başlangıç ve bitiş noktalarını birer küçük küre ile göster
        Gizmos.DrawSphere(startPosition, 0.1f);
        Gizmos.DrawSphere(endPosition, 0.1f);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform); 
           
            
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null); 
            
        }
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
}