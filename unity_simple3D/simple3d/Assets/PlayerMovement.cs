using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody cubeRigidBody;

    public float forwardForce = 1500f;

    public float sidewayForce = 500f;

    private float _normalizedForwardForce;

    private float _normalizedSidewayForce;

    void Start()
    {
        _normalizedForwardForce = forwardForce * Time.deltaTime;
        _normalizedSidewayForce = sidewayForce * Time.deltaTime;
    }

    void Update()
    {
        // check for input here and store them in variable and update in fixed update

    }

    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            cubeRigidBody.AddForce(0, 0, _normalizedForwardForce);
        }

        if (Input.GetKey("a"))
        {
            cubeRigidBody.AddForce(-_normalizedSidewayForce, 0, 0);
        }

        if (Input.GetKey("s"))
        {
            cubeRigidBody.AddForce(0, 0, -_normalizedForwardForce);
        }

        if (Input.GetKey("d"))
        {
            cubeRigidBody.AddForce(_normalizedSidewayForce, 0, 0);
        }
    }
}
