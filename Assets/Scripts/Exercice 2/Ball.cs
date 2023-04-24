using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum BallColor
    {
        Red,
        Yellow,
        White
    }
    
    #region Attributes

    [SerializeField] private AudioClip soundOnImpact;
    private AudioSource _audioSource;
    
    private BallColor InColor;

    public BallColor Color
    {
        get => InColor;
        set
        {
            InColor = value; 
            SetColorObject();
        } 
}

    private Vector2 _initPos;

    public Vector2 InitPos
    {
        get => _initPos;
        set => _initPos = value;
    }

    #endregion

    #region Methods

    protected virtual void Start()
    {
        SetColorObject();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource != null) _audioSource.clip = soundOnImpact;
    }

    private void SetColorObject()
    {
        Color newColor = new Color();
        switch (Color)
        {
            case BallColor.Red:
                newColor = UnityEngine.Color.red;
                break;
            case BallColor.Yellow:
                newColor = UnityEngine.Color.yellow;
                break;
            case BallColor.White:
                newColor = UnityEngine.Color.white;
                break;
        }
        
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }

    public void ResetPosition()
    {
        transform.position = _initPos;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }

    #endregion
}
