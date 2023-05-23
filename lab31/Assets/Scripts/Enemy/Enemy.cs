using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Moving movingScript = other.gameObject.GetComponent<Moving>();
            if (movingScript != null)
            {
                movingScript.Dead(gameObject);
            }
        }
        else if (other.gameObject.CompareTag("PlayerAdditional"))
        {
            Moving movingScriptInParent = other.GetComponentInParent<Moving>();
            if (movingScriptInParent != null)
            {
                movingScriptInParent.Dead(gameObject);
            }
        }
        
    }
}
