using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    Animator _anim;
    Rigidbody2D _body;
    SpriteRenderer _spriteRenderer;

    public float walkThreshold = 0.5f;

    void Start() {
        _anim = GetComponent<Animator>();
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {

        if (_body.velocity.y > 0
            && Mathf.Abs(_body.velocity.y) > Mathf.Abs(_body.velocity.x)) {
            _anim.SetInteger("Direction", 0);
        }
        else if (_body.velocity.y < 0
                 && Mathf.Abs(_body.velocity.y) > Mathf.Abs(_body.velocity.x)) {
            _anim.SetInteger("Direction", 2);
        }
        else if (Mathf.Abs(_body.velocity.x) > Mathf.Abs(_body.velocity.y)) {
            _anim.SetInteger("Direction", 1);
            if (_body.velocity.x >= 0) {
                _spriteRenderer.flipX = false;
            }
            else {
                _spriteRenderer.flipX = true;
            }
        }
        _anim.SetBool("Walking", _body.velocity.magnitude >= walkThreshold);
    }

}
