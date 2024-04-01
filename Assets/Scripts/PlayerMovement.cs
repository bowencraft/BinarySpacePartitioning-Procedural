using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public KeyCode moveUpKey = KeyCode.UpArrow;
    public KeyCode moveRightKey = KeyCode.RightArrow;
    public KeyCode moveDownKey = KeyCode.DownArrow;
    public KeyCode moveLeftKey = KeyCode.LeftArrow;

    public float moveSpeed = 10f;
    public float moveAcceleration = 100f;
    public float dampingK = 10f;

    Rigidbody2D _body;


    // Start is called before the first frame update
    void Start() {
        _body = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void FixedUpdate() {

        Vector2 moveForce = Vector2.zero;

        if (Input.GetKey(moveUpKey)) {
            moveForce += Vector2.up * moveAcceleration;
        }
        if (Input.GetKey(moveRightKey)) {
            moveForce += Vector2.right * moveAcceleration;
        }
        if (Input.GetKey(moveDownKey)) {
            moveForce -= Vector2.up * moveAcceleration;
        }
        if (Input.GetKey(moveLeftKey)) {
            moveForce -= Vector2.right * moveAcceleration;
        }

        _body.AddForce(moveForce);
        if (_body.velocity.magnitude > moveSpeed || moveForce == Vector2.zero) {
            _body.AddForce(-dampingK * _body.velocity);
        }

    }
}
