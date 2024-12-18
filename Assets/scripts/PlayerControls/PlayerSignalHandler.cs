using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSignalHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _doubleClickTime = 0.2f;

    public event Action MouseClickOnGround;
    public event Action ButtoneClick;

    public bool IsRunningMouce { get; private set; }
    public bool IsRunningKeyboard { get; private set; }
    public Vector2 MoveVector { get; private set; }
    public ControlType ControlType { get; private set; }
    public Vector3 TargetPosition { get; private set; }
    public Vector2 MoucePosition { get; private set; }

    private float _lastClickTime;

    private void Awake()
    {
        _playerInput.onActionTriggered += OnPlayerInputActionTriggered;
    }

    private void OnPlayerInputActionTriggered(InputAction.CallbackContext context)
    {
        InputAction action = context.action;

        switch (action.name)
        {
            case "Move":
                Vector2 moveCommand = action.ReadValue<Vector2>();
                HandleMoveCommand(moveCommand);
                break;

            case "Look":
                Vector2 lookCommand = action.ReadValue<Vector2>();
                HandleLookCommand(lookCommand);
                break;

            case "Run":
                switch (action.phase)
                {
                    case InputActionPhase.Started:
                        IsRunningKeyboard = true;
                        break;
                    case InputActionPhase.Canceled:
                        IsRunningKeyboard = false;
                        break;
                }

                break;

            case "Target":
                if (action.phase == InputActionPhase.Started)
                    HandleClikCommand();
                break;
        }
    }

    private void HandleClikCommand()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(MoucePosition), Vector2.zero);
        
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            IsHeRunning();

            ControlType = ControlType.AI;

            var worldPosition = Camera.main.ScreenToWorldPoint(MoucePosition);
            TargetPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);

            MouseClickOnGround?.Invoke();
        }
    }

    private void IsHeRunning()
    {
        float timeSinceLastClick = Time.time - _lastClickTime;

        if (timeSinceLastClick <= _doubleClickTime)
            IsRunningMouce = true;
        else
            IsRunningMouce = false;

        _lastClickTime = Time.time;
    }

    private void HandleLookCommand(Vector2 lookCommand)
    {
        MoucePosition = lookCommand;
    }

    private void HandleMoveCommand(Vector2 moveCommand)
    {
        MoveVector = moveCommand;

        if (MoveVector != Vector2.zero)
        {
            ControlType = ControlType.WASD;
            ButtoneClick?.Invoke();
        }
    }
}
