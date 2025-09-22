using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Refs")]
    [SerializeField] private Camera playerCamera;

    [Header("Movimiento")]
    public float moveSpeed = 4.5f;
    public float gravity = -9.81f;
    public float jumpForce = 3.5f;

    [Header("Mirada")]
    public float mouseSensitivity = 0.1f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    [Header("Input")]
    public InputActionAsset actionsAsset;
    [SerializeField] private string actionMapName = "Player";
    [SerializeField] private string moveAction = "Move";
    [SerializeField] private string lookAction = "Look";
    [SerializeField] private string jumpAction = "Jump";

    private CharacterController controller;
    private InputAction move, look, jump;

    private Vector2 moveInput, lookInput;
    private float pitch;
    private Vector3 velocity;
    private bool jumpPressed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!playerCamera) playerCamera = Camera.main;

        var map = actionsAsset.FindActionMap(actionMapName, true);
        move = map.FindAction(moveAction, true);
        look = map.FindAction(lookAction, true);
        jump = map.FindAction(jumpAction, true);

        move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        move.canceled  += _   => moveInput = Vector2.zero;

        look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        look.canceled  += _   => lookInput = Vector2.zero;

        jump.performed += _ => jumpPressed = true;
    }

    private void OnEnable()
    {
        move.Enable(); look.Enable(); jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable(); look.Disable(); jump.Disable();
    }

    private void Update()
    {
        //Mirada
        float yaw = lookInput.x * mouseSensitivity;
        float pitchDelta = -lookInput.y * mouseSensitivity;

        transform.Rotate(0f, yaw, 0f);
        pitch = Mathf.Clamp(pitch + pitchDelta, minPitch, maxPitch);
        if (playerCamera)
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0f, 0f);

        //Movimiento plano
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 worldDir = transform.TransformDirection(inputDir);
        Vector3 horizontal = worldDir * moveSpeed;

        //Gravedad y salto
        if (controller.isGrounded)
        {
            velocity.y = -1f; //pegado al piso
            if (jumpPressed)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                jumpPressed = false;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        Vector3 moveVec = (horizontal + new Vector3(0, velocity.y, 0)) * Time.deltaTime;
        controller.Move(moveVec);
    }
    }
}