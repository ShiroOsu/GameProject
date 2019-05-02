using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    // Fields

    // Movements variables.
    private float forwardSpeed;
    private float turnSpeed;
    private float backwardSpeed;
    private float jumpPower;

    private float horizontalSpeed;
    private float verticalSpeed;

    // Oof.
    private Vector3 velocity;
    private Vector3 orgVectColCenter;
    private float orgCelHeight;

    // Components.
    private Rigidbody body;
    private new CapsuleCollider collider;


    // Jumping booleans, air & ground.
    private bool isJumping;
    private bool onGround = true;
    private bool isInAir;

    private float gravitationForce = 9.82f;

    // Const string variables for inputs
    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string jump = "Jump";

    // Zero variables
    private const int zero = 0;

    // Layer mask
    private LayerMask layerMask;

    // Call SetupComponents before calling SetupVariables to avoid object reference not set
    // to an instance of an object errors.
    private void SetupVariables()
    {
        forwardSpeed = 5f;
        turnSpeed = 2.5f;
        backwardSpeed = forwardSpeed * 0.3f;
        jumpPower = 10f;

        layerMask = 1 << 8;
        layerMask = ~layerMask;


        orgCelHeight = collider.height;
        orgVectColCenter = collider.center;
    }

    private void SetupComponents()
    {
        body = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        // This will freeze rotation of every axis.
        body.freezeRotation = true;
        body.useGravity = true;
    }

    private void SetInputs()
    {
        horizontalSpeed = Input.GetAxis(horizontal);
        verticalSpeed = Input.GetAxis(vertical);
        isJumping = Input.GetButtonDown(jump);
    }

    private void canWeJump()
    {
        float rayLength = 2f; // OOF ??!

        if (isInAir)
        {
            onGround = false;
        }
        else if (onGround)
        {
            isInAir = false;
        }


        // Use some raycast to know if we are on ground or not.
        if (Physics.Raycast(transform.localPosition, Vector3.down, out RaycastHit hit, rayLength, layerMask))
        {
            onGround = true;
        } else
        {
            isInAir = true;
        }
    }

    private void UpdateMovements()
    {
        // Are we able to jump?
        canWeJump();

        velocity = new Vector3(0, 0, verticalSpeed);
        velocity = transform.TransformDirection(velocity);

        if (verticalSpeed > zero)
        {
            velocity *= forwardSpeed;
        }
        else if (verticalSpeed < zero)
        {
            velocity *= backwardSpeed;
        }


        // Jump
        if (isJumping && onGround)
        {
            body.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
        }

        transform.localPosition += velocity * Time.fixedDeltaTime;
        transform.Rotate(zero, horizontalSpeed * turnSpeed, zero);
    }

    private void Start()
    {
        // Important that well call SetupComponents before Variables.
        SetupComponents();
        SetupVariables();


    }

    private void FixedUpdate()
    {
        SetInputs();
        UpdateMovements();

    }

}
