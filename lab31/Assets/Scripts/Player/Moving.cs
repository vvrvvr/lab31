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
    [SerializeField] private Transform _meshTransform;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] Transform _anchor;
    [Space(10)]
    public float jumpTime = 0.2f;
    public float jumpHeight = 2f;
    public float maxspeed = 0;
    public bool hasControl = false;
    private bool isCanChange = true;

    private float jumpStart = 0f;
    private float jumpFinish = 0f;
    private bool isReadyToJump = false;
    private float currentHeight;

    private float distanceTravelled = 0f;

    //respawn and dead
    private float nearestRespawnPoint = 0f;
    private GameObject explosionObject;
    private GameObject currentEnemy = null;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHeight = transform.position.y;
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

        if (isReadyToJump)
        {
            if (distanceTravelled >= jumpStart)
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
        if (_animator != null)
            _animator.SetBool("isJumping", true);
        hasControl = false;
        // Используем DOTween для анимации изменения значения dist
        DOTween.To(() => distanceTravelled, x => distanceTravelled = x, jumpFinish, jumpTime)
            .SetEase(Ease.InOutQuad)
            .OnComplete(OnJumpComplete);

        // Анимация от 0 до heightMax
        Tween upTween = DOTween.To(() => currentHeight, x => currentHeight = x, jumpHeight, jumpTime / 2);
        upTween.OnUpdate(() =>
        {
            // Применить текущую высоту к объекту
            Vector3 newPosition = _meshTransform.position;
            newPosition.y = currentHeight;
            _meshTransform.position = newPosition;
        });

        // Анимация от heightMax до 0
        Tween downTween = DOTween.To(() => currentHeight, x => currentHeight = x, 0f, jumpTime / 2);
        downTween.OnUpdate(() =>
        {
            // Применить текущую высоту к объекту
            Vector3 newPosition = _meshTransform.position;
            newPosition.y = currentHeight;
            _meshTransform.position = newPosition;
        });

        // Создание последовательности анимации
        Sequence sequence = DOTween.Sequence();
        sequence.Append(upTween);
        sequence.Append(downTween);
        //sequence.SetLoops(-1, LoopType.Yoyo);

        // Настройка ease
        sequence.SetEase(Ease.Linear);
    }

    private void OnJumpComplete()
    {
        hasControl = true;
        if (_animator != null)
            _animator.SetBool("isJumping", false);
    }

    public void SetNearestRespawnPoint(float dist)
    {
        nearestRespawnPoint = dist;
    }

    public void Dead(GameObject killer)
    {
        if (isDead)
            return;
        isDead = true;
        hasControl = false;
        _mesh.SetActive(false);

        currentEnemy = killer;
        explosionObject = Instantiate(explosionPrefab);
        explosionObject.transform.position = _anchor.position;
        explosionObject.transform.rotation = transform.rotation;
    }

    public void Restart()
    {
        if (explosionObject != null)
        {
            explosionObject.GetComponent<Explosion>().ReturnBlocks(transform.position);
        }
    }
}
