using System.Collections;
using PathCreation;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class Moving : MonoBehaviour
{
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private GameObject _mesh;
    public CinemachineVirtualCamera _camera;
    [SerializeField] private Animator _animator;
    [Space(10)]
    public float jumpTime = 0.2f;
    public float maxspeed = 0;
    public bool hasControl = false;
    private bool isCanChange = true;

    private float jumpStart = 0f;
    private float jumpFinish = 0f;
    private bool isReadyToJump = false;


    private float distanceTravelled = 0f;
    // Start is called before the first frame update
    void Start()
    {
        distanceTravelled = 0f;
        transform.position = _pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(distanceTravelled);
    }

    // Update is called once per frame
    void Update()
    {
        var speed = 0f;
        if (hasControl)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                speed = maxspeed;
                if (_animator != null)
                    _animator.SetBool("isRunning", true);
            }
            else
            {
                if (_animator != null)
                    _animator.SetBool("isRunning", false);
            }
        }

        if(isReadyToJump)
        {
            if(distanceTravelled >= jumpStart)
            {
                isReadyToJump = false;
                StartJump();
            }
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
    
    public void PrepeareToJump(float start, float finish)
    {
        jumpStart = start;
        jumpFinish = finish;
        isReadyToJump = true;
    }

    private void StartJump()
    {
        hasControl = false;
        // »спользуем DOTween дл€ анимации изменени€ значени€ dist
        DOTween.To(() => distanceTravelled, x => distanceTravelled = x, jumpFinish, jumpTime)
            .SetEase(Ease.Linear)
            .OnComplete(OnJumpComplete);
    }

    private void OnJumpComplete()
    {
        hasControl = true;
    }

}
