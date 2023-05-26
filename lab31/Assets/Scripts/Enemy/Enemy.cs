using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject parentObj;
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

        if (other.gameObject.CompareTag("PlayerAdditional"))
        {
            Moving movingScriptInParent = other.GetComponentInParent<Moving>();
            if (movingScriptInParent != null)
            {
                movingScriptInParent.Dead(gameObject);
            }
        }
    }

    public void DestroyMe()
    {
        parentObj = transform.parent.gameObject;
        while (parentObj.transform.parent != null)
        {
            parentObj = parentObj.transform.parent.gameObject;
        }
        parentObj.SetActive(false);
    }
}