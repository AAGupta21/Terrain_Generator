using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private float TurnSpeed = 5f;

    private float XRotation = 0f;
    private float YRotation = 0f;

    private Vector3 Curr_Rot = Vector3.zero;
    
    void Update()
    {
        Keyboard_Movement();
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            Mouse_Movement();
        }
    }

    private void Keyboard_Movement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * MoveSpeed * Time.deltaTime;
        }
    }

    private void Mouse_Movement()
    {
        XRotation = Input.GetAxis("Mouse Y");
        YRotation = Input.GetAxis("Mouse X");

        Curr_Rot += new Vector3(XRotation * -1 * TurnSpeed, YRotation * TurnSpeed, 0f);

        if (Curr_Rot.x > 40f)
            Curr_Rot.x = 40f;

        if (Curr_Rot.x < -60f)
            Curr_Rot.x = -60f;

        transform.rotation = Quaternion.Euler(Curr_Rot);
    }
}