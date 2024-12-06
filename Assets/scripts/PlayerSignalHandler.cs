using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSignalHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    public event Action OnMouceClick;
    public event Action OnButtoneClick;

    public bool IsRunning { get; private set; }
    public Vector2 MoveVector { get; private set; }
    public ControlType ControlType { get; private set; }
    public Vector3 TargetPosition { get; private set; }

    private Vector2 _moucePosition;

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

            //case "Rotate":
            //    Vector2 rotateCommand = action.ReadValue<Vector2>();
            //    HandleRotateCommand(rotateCommand);
            //    break;

            case "Look":
                Vector2 lookCommand = action.ReadValue<Vector2>();
                HandleLookCommand(lookCommand);
                break;

            case "Run":
                switch (action.phase)
                {
                    case InputActionPhase.Started:
                        IsRunning = true;
                        break;
                    case InputActionPhase.Canceled:
                        IsRunning = false;
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
            OnMouceClick?.Invoke();

            ControlType = ControlType.AI;
            var worldPosition = Camera.main.ScreenToWorldPoint(_moucePosition);
            TargetPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);
            //Debug.Log(TargetPosition);
        }
    }

    private void HandleLookCommand(Vector2 lookCommand)
    {
        //Debug.Log("HandleLookCommand " + lookCommand);

        _moucePosition = lookCommand;
    }

    private void HandleMoveCommand(Vector2 moveCommand)
    {
        MoveVector = moveCommand;

        if (MoveVector != Vector2.zero)
        {
            OnButtoneClick?.Invoke();
            ControlType = ControlType.WASD;
        }
    }
}
