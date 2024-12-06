using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterController), typeof(PlayerSignalHandler))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerSignalHandler _playerSignalHandler;
    [Space]
    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _runningSpeed = 2;

    [SerializeField] private float _moveSpeed;

    private void Start()
    {
        _playerSignalHandler.OnMouceClick += UpdateAI;
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        if (_playerSignalHandler.ControlType == ControlType.WASD)
            UpdateWASD();
    }

    public void UpdateWASD()
    {
        _navMeshAgent.enabled = false;

        if (_playerSignalHandler.IsRunningKeyboard)
            _moveSpeed = _runningSpeed;
        else
            _moveSpeed = _walkSpeed;

        _characterController.Move((Vector3)(_moveSpeed * Time.deltaTime * _playerSignalHandler.MoveVector));
    }

    public void UpdateAI()
    {
        _navMeshAgent.enabled = true;

        if (_playerSignalHandler.IsRunningMouce)
            _navMeshAgent.speed = _runningSpeed;
        else
            _navMeshAgent.speed = _walkSpeed;

        _navMeshAgent.SetDestination(_playerSignalHandler.TargetPosition);
    }
}