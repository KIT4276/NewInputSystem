using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private PlayerSignalHandler _playerSignalHandler;
    [Space]
    [SerializeField] private float _walkSpeed = 1;
    [SerializeField] private float _runningSpeed = 2;

    [SerializeField] private float _moveSpeed;

    private void Update()
    {
        UpdateWASD(_playerSignalHandler.MoveVector, _playerSignalHandler.IsRunning);
        UpdateAI(_playerSignalHandler.TargetPosition, _playerSignalHandler.IsRunning);
    }

    public void UpdateWASD(Vector2 moveVector, bool isRunning)
    {
        _navMeshAgent.enabled = false;

        if (isRunning)
            _moveSpeed = _runningSpeed;
        else
            _moveSpeed = _walkSpeed;

        _characterController.Move((Vector3)(_moveSpeed * Time.deltaTime * moveVector));
    }

    public void UpdateAI(Vector3 targetPosition, bool isRunning)
    {
        _navMeshAgent.enabled = true;

        if (isRunning)
            _navMeshAgent.speed = _runningSpeed;
        else
            _navMeshAgent.speed = _walkSpeed;

        _navMeshAgent.SetDestination(targetPosition);
    }
}