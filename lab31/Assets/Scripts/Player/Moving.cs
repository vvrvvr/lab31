using System.Collections;
using PathCreation;
using UnityEngine;
using Cinemachine;

public class Moving : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    public float maxspeed = 0;
    public bool hasControl = false;
    public CinemachineVirtualCamera _camera;
    [SerializeField] private GameObject _mesh;
    private bool isCanChange = true;

    private float distanceTravelled = 0f;
    // Start is called before the first frame update
    void Start()
    {
        distanceTravelled = 0.1f;
        transform.position = _pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasControl)
            return;
        var speed = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            speed = maxspeed;
        }
        distanceTravelled += speed * Time.deltaTime;
        transform.position = _pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }

    public void ChangePlayer()
    {
        hasControl = true;
        isCanChange = false;
        StartCoroutine(WaitToCanChangePlayer());
        _camera.Priority = 1;
        _mesh.SetActive(true);
        Debug.Log("change player " + gameObject.name);
    }

    public void DeactivatePlayer()
    {
        hasControl = false;
        isCanChange = false;
        gameObject.SetActive(false);
        _camera.Priority = 0;
        Debug.Log("deactivate " + gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasControl || !isCanChange)
            return;
        if (other.gameObject.CompareTag("Player"))
        {

            Debug.Log("collision in " + gameObject.name);
            var anotherPlayer = other.gameObject.GetComponent<Moving>();
            anotherPlayer.ChangePlayer();
            DeactivatePlayer();
        }
    }
    private IEnumerator WaitToCanChangePlayer()
    {
        yield return new WaitForSeconds(0.1f);
        isCanChange = true;

    }
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (!hasControl)
    //         return;
    //     if (collision.transform.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("collision");
    //         var anotherPlayer = collision.transform.gameObject.GetComponent<Moving>();
    //         anotherPlayer.ChangePlayer();
    //         DeactivatePlayer();
    //     }
    // }
}
