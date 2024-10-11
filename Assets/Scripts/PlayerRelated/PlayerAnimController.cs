using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GroundChecker _groundChecker;
    private Rigidbody2D _rigidbody2D;
    private bool isDimensionChangeing;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        PlayerDimensionChanger.OnDimensionChanged += PlayDimensionChangeAnim;
        AnimationFinished.OnAnimationFinished += FinishDimensionChangeAnimation;
    }

    private void Update()
    {
        AnimHandler();
    }

    private void AnimHandler()
    {
        if (isDimensionChangeing)
            return;
        if(Mathf.Abs(_rigidbody2D.velocity.x) > 0.1f && _groundChecker.IsGrounded())
            _animator.Play("playerMove");
        else if(Mathf.Abs(_rigidbody2D.velocity.x) < 0.1f && _groundChecker.IsGrounded())
            _animator.Play("playerIdle");
        else if(_rigidbody2D.velocity.y < 0.1f && !_groundChecker.IsGrounded())
            _animator.Play("playerFall");
        else if(_rigidbody2D.velocity.y > 0.1f && !_groundChecker.IsGrounded())
            _animator.Play("playerJump");
    }

    private void PlayDimensionChangeAnim()
    {
        isDimensionChangeing = true;
        _animator.Play("playerDimensionChange");
    }

    private void FinishDimensionChangeAnimation(string s)
    {
        if(s == "playerDimensionChange")
            isDimensionChangeing = false;
    }
    

    private void OnDisable()
    {
        PlayerDimensionChanger.OnDimensionChanged -= PlayDimensionChangeAnim;
        AnimationFinished.OnAnimationFinished -= FinishDimensionChangeAnimation;
    }
}
