using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 8f;
    public float rotationAngle = 90f;
    public float hitDistance = 40f;

    private Movement movement;

    void Start()
    {
        movement = new Movement(transform, movementSpeed);
    }

    void Update()
    {
        movement.HandleMovement();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCameraAroundDynamicPivot(rotationAngle);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCameraAroundDynamicPivot(-rotationAngle);
        }


        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            movement.StopMovement();
        }
    }

    void RotateCameraAroundDynamicPivot(float angle)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hitDistance))
        {
            Debug.DrawRay(ray.origin, ray.direction * hitDistance, Color.green, 2f);
            Vector3 pivotPoint = hit.point;

            Vector3 pivotToCamera = transform.position - pivotPoint;

            transform.RotateAround(pivotPoint, Vector3.up, angle);

            transform.position = pivotPoint + Quaternion.Euler(0, angle, 0) * pivotToCamera;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * hitDistance, Color.red, 2f);
            Debug.Log("Raycast didn't hit anything.");
        }
    }


    public class Movement
    {
        private Transform transform;
        private float speed;

        private Vector3 movementDirection = Vector3.zero;

        public Movement(Transform transform, float speed)
        {
            this.transform = transform;
            this.speed = speed;
        }

        public void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 moveVector = new Vector3(horizontal, 0f, vertical).normalized * speed;

            moveVector = transform.TransformDirection(moveVector);

            moveVector.y = 0f;

            transform.Translate(moveVector * Time.deltaTime, Space.World);
        }

        public void StopMovement()
        {
            movementDirection = Vector3.zero;
        }
    }
}