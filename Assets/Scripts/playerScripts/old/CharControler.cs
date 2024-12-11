using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControler : MonoBehaviour {
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 360;
    private Vector3 input;

    private void Update() {
        GatherInput();
        Look();
    }

    private void FixedUpdate() {
        Move();
    }

    private void GatherInput() {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    private void Look() {
        if (input == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, rot, turnSpeed * Time.deltaTime);
    }

    private void Move() {
        Vector3 moveDirection = input.ToIso().normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    public Vector3 GetIsoInputDirection() {
        return input.ToIso().normalized;
    }
}

public static class Helpers {
    private static Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static Vector3 ToIso(this Vector3 input) => isoMatrix.MultiplyPoint3x4(input);
}
