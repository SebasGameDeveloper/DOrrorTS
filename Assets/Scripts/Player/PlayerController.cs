using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float mouseSensitivity = 2f;

        private PlayerInputActions inputActions;
        private CharacterController characterController;
        private Vector2 mouseInput;
        private float xRotation = 0f;

        private void Awake()
        {
            inputActions = new PlayerInputActions();
            characterController = GetComponent<CharacterController>();

            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void OnEnable() => inputActions.Enable();
        private void OnDisable() => inputActions.Disable();
        
        private void Update()
        {
            HandleMovement();
            HandleMouseLook();
        }
        
        private void HandleMovement()
        {
            Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();
            Vector3 moveDirection = (transform.right * input.x + transform.forward * input.y) * moveSpeed;
            moveDirection.y = Physics.gravity.y;
            characterController.Move(moveDirection * Time.deltaTime);
        }
        
        private void HandleMouseLook()
        {
            mouseInput = inputActions.Player.Look.ReadValue<Vector2>();
            transform.Rotate(Vector3.up * mouseInput.x * mouseSensitivity);
            xRotation -= mouseInput.y * mouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}