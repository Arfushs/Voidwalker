using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ParallaxLayer : MonoBehaviour
{
    private float _length, _startpos;
    private Camera _mainCamera;
    [SerializeField] private float _parallaxEffect;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _startpos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (_mainCamera.transform.position.x * (1-_parallaxEffect));
        float dist = (_mainCamera.transform.position.x * _parallaxEffect);
        
        transform.position = new Vector3(_startpos + dist, transform.position.y, transform.position.z);
        
        if(temp>_startpos+_length) _startpos += _length;
        else if (temp<_startpos-_length) _startpos -= _length;
    }
}