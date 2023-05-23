using UnityEngine;
using PathCreation;

public class JumpPoint : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private GameObject _finish;
    private float _jumpStart = 0;
    private float _jumpEnd = 0;

    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        _finish.GetComponent<MeshRenderer>().enabled = false;

        _jumpStart = _pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        _jumpEnd = _pathCreator.path.GetClosestDistanceAlongPath(_finish.GetComponent<Transform>().position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Moving>().PrepeareToJump(_jumpStart, _jumpEnd);
        }
    }
}
