using UnityEngine;
using PathCreation;

public class JumpPoint : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private Transform _finish;
    private float _jumpStart = 0;
    private float _jumpEnd = 0;

    void Start()
    {
        _jumpStart = _pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        _jumpEnd = _pathCreator.path.GetClosestDistanceAlongPath(_finish.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Moving>().PrepeareToJump(_jumpStart, _jumpEnd);
        }
    }
}
