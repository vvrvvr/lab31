using UnityEngine;
using PathCreation;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    private float respawnPoint = 0f;

    
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        if(_pathCreator != null)
            respawnPoint = _pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        else
            Debug.Log("path not assigned to "+ gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Moving>().SetNearestRespawnPoint(respawnPoint);
        }
    }
}
