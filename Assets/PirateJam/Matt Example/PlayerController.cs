using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 RawInput;
    public float MoveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        RawInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        GetComponent<CharacterController>().Move(((transform.forward * RawInput.y + transform.right * RawInput.x) * MoveSpeed) * Time.deltaTime);
    }
}
