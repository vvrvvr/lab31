using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class Finish : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera finishCamera;
    public PlayableDirector finishTimeline;
    private bool isFinish = false;

    private void FinishGame()
    {
        isFinish = true;
        finishCamera.Priority = 10;
        finishTimeline.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Moving movingScript = other.gameObject.GetComponent<Moving>();
            if (movingScript != null && !isFinish)
            {
                movingScript.Finish();
                FinishGame();
            }
        }
        if (other.gameObject.CompareTag("PlayerAdditional"))
        {
            Moving movingScriptInParent = other.GetComponentInParent<Moving>();
            if (movingScriptInParent != null && !isFinish)
            {
                movingScriptInParent.Finish();
                FinishGame();
                
            }
        }

    }
}
