using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController SharedComponent;

    private Vector2 inputVector;
    private Vector2 forceMovement;
    private Vector2 simulatedInput;

    public Floating floating { get; set; }
    public GameObject fakeWing { get; set; }

    private bool resetInputVector;
    private bool forceMovementMode = false;
    private bool simulateInputMode = false;

    [SerializeField] private Vector2 inputVectorWithDeadZone;
    [SerializeField] private Vector3 movementVector;
    [SerializeField] private Vector2 inertedInput = Vector2.zero;
    [SerializeField] private float inertedTilt;
    private float inertedTiltSpeedTimed;
    [SerializeField] private float tiltFloat;
    [SerializeField] private float inertedTiltSpeed = 1f;
    [SerializeField] private float inertedTiltSpeedModifier = 1f;
    [SerializeField] private bool isTilting = false;
    [SerializeField] private bool controlsEnabled = true;

    [SerializeField] private float inputDeadZone = .25f;
    [SerializeField] private float inputSpeed = 1f;
    [SerializeField] private float objectSpeed = 2f;
    [SerializeField] private float maxTiltAngle = 10f;
    [SerializeField] private float maxTurnAngle = 10f;
    [SerializeField] private float maxAdditionalTilt = 45f;

    public bool invertYAxis = true;

    private bool tiltLeftButton = false;
    private bool tiltRightButton = false;

    private float additionalTilt = 45f;

    private void Awake()
    {
        fakeWing = transform.Find("Fake Wing").gameObject;
        floating = fakeWing.GetComponent<Floating>();
        SharedComponent = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        movementVector = Vector3.zero;
    }

    //controlsEnabled
    public bool ControlsEnabled()
    {
        return controlsEnabled;
    }

    // Set the position first then render
    void FixedUpdate()
    {
        ApplyMovement();
    }

    public void SimulateInputFor(Vector2 simInput, float duration)
    {
        simulatedInput = simInput;
        if (invertYAxis)
        {
            simulatedInput = new Vector2(
                simInput.x,
                simInput.y * -1f
            );
        }
        StartCoroutine(SimulateInputRoutine(duration));
    }

    private IEnumerator SimulateInputRoutine(float time)
    {
        simulateInputMode = true;
        yield return new WaitForSeconds(time);
        simulateInputMode = false;
    }

    public void ForceMovement(
        Vector3 v, float duration, bool keepInitialX, bool keepInitialY
    )
    {
        //only force movement if at least one coordinate should not be kept
        if (!keepInitialY || !keepInitialX)
        {
            var invertYDir = invertYAxis ? -1f : 1f;
            var vWithKeeps = new Vector2(
                keepInitialX ? inertedInput.x : v.x,
                keepInitialY ? inertedInput.y : invertYDir * v.y
            );

            inertedInput = vWithKeeps;
            forceMovement = vWithKeeps;
            StartCoroutine(ForceMovementRoutine(duration));
        }

    }

    private IEnumerator ForceMovementRoutine(float time)
    {
        forceMovementMode = true;
        yield return new WaitForSeconds(time);
        forceMovementMode = false;
    }

    public void DisableControls()
    {
        controlsEnabled = false;
    }

    //movement application
    void ApplyMovement()
    {
        //set the movement vector field
        SetMovementVector();

        //set the movement vector field
        SetTiltFloat();

        //rotate to direction
        SetRotation();

        //apply movement
        transform.position += movementVector;
    }

    void SetMovementVector()
    {
        var inputSpeedTimed = inputSpeed * Time.deltaTime;

        var inertedTiltMovement = new Vector2(
            -inertedTiltSpeedModifier * inertedTilt,
            0f
        );

        var virtualInputVector = simulateInputMode ?
            simulatedInput : inputVectorWithDeadZone;

        //let the inertedInput "wander" to the input
        var requestedMovement =
            forceMovementMode ? forceMovement : virtualInputVector;

        var deltaInputInertedInput = requestedMovement - inertedInput;
        if (deltaInputInertedInput.magnitude > inputSpeedTimed)
        {
            inertedInput += deltaInputInertedInput.normalized * inputSpeedTimed;
        } else
        {
            inertedInput = requestedMovement;
        }

        movementVector = inertedInput * objectSpeed
            + inertedTiltMovement * inertedInput.magnitude;

        if (invertYAxis)
        {
            movementVector = new Vector2(
                movementVector.x,
                -movementVector.y
            );
        }
    }

    void SetTiltFloat()
    {
        inertedTiltSpeedTimed = inertedTiltSpeed * Time.deltaTime;
        var deltaTilt = tiltFloat - inertedTilt;

        if (Mathf.Abs(deltaTilt) > inertedTiltSpeedTimed)
        {
            inertedTilt += Mathf.Sign(deltaTilt) * inertedTiltSpeedTimed;
        }
        else
        {
            inertedTilt = tiltFloat;
        }
    }

    void SetRotation()
    {
        transform.rotation = Quaternion.Euler(
            -movementVector.y * maxTurnAngle,
            movementVector.x * maxTurnAngle,
            -movementVector.x * (maxTiltAngle) + inertedTilt * maxAdditionalTilt
        );
    }

    void OnMove(InputValue inputValue)
    {
        //Set movement vector
        if (controlsEnabled)
        {
            inputVector = inputValue.Get<Vector2>();
            if (inputVector.magnitude > 1)
            {
                inputVector = inputVector.normalized;
            }

            inputVectorWithDeadZone =
                inputVector.magnitude > inputDeadZone ?
                    inputVector :
                    Vector2.zero
            ;
        } else
        {
            inputVector = Vector3.zero;
            inputVectorWithDeadZone = Vector3.zero;
        }
            
    }

    void OnLeftTilt(InputValue inputValue)
    {
        if (controlsEnabled)
        {
            tiltFloat = inputValue.Get<float>();
        } else
        {
            tiltFloat = 0f;
        }
    }

    void OnRightTilt(InputValue inputValue)
    {
        if (controlsEnabled)
        {
            tiltFloat = -inputValue.Get<float>();
        }
        else
        {
            tiltFloat = 0f;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
