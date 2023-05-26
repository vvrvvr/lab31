using UnityEngine;

public class ToggleAnimator : MonoBehaviour
{
    public float time = 0f; // Время задержки перед включением аниматора

    private Animator animator;

    private void Start()
    {
        // Получаем компонент Animator
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            // Отключаем аниматор
            animator.enabled = false;

            // Запускаем корутину для включения аниматора через время time
            StartCoroutine(EnableAnimatorAfterDelay());
        }
    }

    private System.Collections.IEnumerator EnableAnimatorAfterDelay()
    {
        yield return new WaitForSeconds(time);

        // Включаем аниматор
        animator.enabled = true;
    }
}