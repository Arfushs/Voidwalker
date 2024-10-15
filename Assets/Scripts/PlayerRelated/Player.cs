using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Enums;
using UnityEngine;

public class Player : MonoBehaviour
{
    public DimensionType CurrentDimension { get; private set; }
    private Level _playerLevel;
    private float _score;

    #region PlayerComponents

    private PlayerAnimController _playerAnimController;
    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
    private PlayerDimensionChanger _dimensionChanger;
    private Rigidbody2D _rigidbody2D;

    #endregion
    
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerMove = GetComponent<PlayerMove>();
        _playerAnimController = GetComponent<PlayerAnimController>();
        _playerJump = GetComponent<PlayerJump>();
        _dimensionChanger = GetComponent<PlayerDimensionChanger>();
    }

    private void OnEnable()
    {
        AnimationFinished.OnAnimationFinished += ResetLevel;
    }

    private void OnDisable()
    {
        AnimationFinished.OnAnimationFinished -= ResetLevel;
    }
    

    public void SetupPlayer(Level level)
    {
        CurrentDimension = level.InitialDimension;
        _playerLevel = level;
    }

    public void SetDimension(DimensionType dimension)
    {
        CurrentDimension = dimension;
    }

    public void Death()
    {
        _playerMove.enabled = false;
        _playerJump.enabled = false;
        _dimensionChanger.enabled = false;
        _playerAnimController.enabled = false;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
        _animator.Play("playerDeath");
    }

    public void ResetLevel(string s)
    {
        if (s.Equals("playerDeath"))
        {
            _playerLevel.ResetLevel();
            _playerMove.enabled = true;
            _playerJump.enabled = true;
            _dimensionChanger.enabled = true;
            _playerAnimController.enabled = true;
            _rigidbody2D.isKinematic = false;
        }
    }
    
    
}
