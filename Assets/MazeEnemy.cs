using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class MazeEnemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector2 moveDirection = Vector2.right;
    [SerializeField] private float patrolDistance = 2.5f;
    [SerializeField] private float patrolSpeed = 2f;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;

        Collider2D enemyCollider = GetComponent<Collider2D>();
        enemyCollider.isTrigger = true;
    }

    private void Update()
    {
        Vector2 direction = moveDirection.sqrMagnitude > 0f ? moveDirection.normalized : Vector2.right;
        float offset = Mathf.Sin(Time.time * patrolSpeed) * patrolDistance;
        transform.position = startPosition + (Vector3)(direction * offset);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerMovement player))
        {
            player.HandleEnemyHit();
        }
    }
}
