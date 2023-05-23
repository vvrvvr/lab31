using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// эффект разлетания куба на куски при столкновении с препятствием
/// </summary>
public class Explosion : MonoBehaviour
{
    [SerializeField] Rigidbody[] blocks = new Rigidbody[0];
    [SerializeField] MeshRenderer[] blocksMesh = new MeshRenderer[0];
    [SerializeField] private float power = 0f;
    public float time = 1f;
    private Sequence sequence;

    //private Vector3[] originalPositions;  // Исходные позиции блоков

    private void OnEnable()
    {
        foreach(var b in blocks)
        {
            Vector3 direction = b.transform.position - transform.position;
            b.AddForce(direction.normalized * power, ForceMode.Impulse);
        }
        //originalPositions = new Vector3[blocks.Length];
        //for (int i = 0; i < blocks.Length; i++)
        //{
        //    originalPositions[i] = blocks[i].transform.position;
        //}
    }

    public void ReturnBlocks(Vector3 position)
    {
        sequence = DOTween.Sequence();

        for (int i = 0; i < blocks.Length; i++)
        {
            sequence.Join(blocks[i].transform.DOMove(position, time));
        }

        sequence.OnComplete(CompleteTween);
    }
    private void CompleteTween()
    {
        sequence.Kill();
        Destroy(gameObject);
    }
}
