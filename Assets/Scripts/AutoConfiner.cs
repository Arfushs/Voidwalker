using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AutoConfiner : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer; // Tileable Sprite Renderer
    [SerializeField] private float _yOffset = -0.5f; // Y ekseninde offset
    [SerializeField] private CinemachineVirtualCamera _virtualCamera; // Virtual Camera referansı

    private PolygonCollider2D _polygonCollider;
    private CinemachineConfiner2D _confiner;

    void Awake()
    {
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _confiner = _virtualCamera.GetComponent<CinemachineConfiner2D>();

        if (_spriteRenderer != null)
        {
            FitColliderToTiledSprite();
        }
        else
        {
            Debug.LogError("SpriteRenderer atanmadı.");
        }

        if (_confiner != null)
        {
            UpdateConfinerBounds();
        }
        else
        {
            Debug.LogError("Cinemachine Confiner bulunamadı.");
        }
    }

    private void FitColliderToTiledSprite()
    {
        // Sprite Renderer'in Bounds (ekrandaki görsel boyutu) alınır
        Bounds bounds = _spriteRenderer.bounds;

        // Bounds'tan boyutları çıkarırız (world space)
        Vector2 size = new Vector2(bounds.size.x, bounds.size.y);

        // PolygonCollider2D için 4 köşe noktası oluştur (Sprite’ın görünen alanı için)
        Vector2[] points = new Vector2[4];

        points[0] = new Vector2(-size.x / 2, -size.y / 2 + _yOffset); // Sol Alt
        points[1] = new Vector2(size.x / 2, -size.y / 2 + _yOffset);  // Sağ Alt
        points[2] = new Vector2(size.x / 2, size.y / 2 );   // Sağ Üst
        points[3] = new Vector2(-size.x / 2, size.y / 2 );  // Sol Üst

        // PolygonCollider2D'nin noktalarını güncelle
        _polygonCollider.SetPath(0, points);
    }

    private void UpdateConfinerBounds()
    {
        // Confiner'ın sınırlarını güncellemek için Collider2D'yi tekrar ayarla
        _confiner.m_BoundingShape2D = _polygonCollider;

        // Cinemachine Confiner'ın sınırlarını tekrar hesaplaması için ForceUpdate çağırın
        _confiner.InvalidateCache();
    }
}
