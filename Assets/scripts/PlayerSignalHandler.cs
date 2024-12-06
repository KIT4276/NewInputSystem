using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSignalHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _doubleClickTime = 0.2f;

    public event Action OnMouceClick;
    public event Action OnButtoneClick;

    public bool IsRunningMouce { get; private set; }
    public bool IsRunningKeyboard { get; private set; }
    public Vector2 MoveVector { get; private set; }
    public ControlType ControlType { get; private set; }
    public Vector3 TargetPosition { get; private set; }

    private Vector2 _moucePosition;
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
                switch (action.phase)
                {
                    case InputActionPhase.Started:
                        HandleTargetCommand();
                        break;
                }
                break;
        }
    }

    private void HandleTargetCommand()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_moucePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            IsHeRunning();

            ControlType = ControlType.AI;

            var worldPosition = Camera.main.ScreenToWorldPoint(_moucePosition);
            TargetPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);

            OnMouceClick?.Invoke();
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
        _moucePosition = lookCommand;
    }

    private void HandleMoveCommand(Vector2 moveCommand)
    {
        MoveVector = moveCommand;

        if (MoveVector != Vector2.zero)
        {
            ControlType = ControlType.WASD;
            OnButtoneClick?.Invoke();
        }
    }
}
