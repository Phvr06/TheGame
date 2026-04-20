using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector3 spawnPoint;
    private bool invulnerable;

    [Header("Gameplay")]
    [SerializeField] private float speed = 5f;
    [Tooltip("Tempo ganho ao coletar o item correto.")]
    [SerializeField] private float collectibleTimeBonus = 8f;
    [Tooltip("Tempo perdido ao encostar em um inimigo.")]
    [SerializeField] private float enemyTimePenalty = 6f;
    [SerializeField] private float invulnerabilityDuration = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPoint = transform.position;
    }

    private void FixedUpdate()
    {
        if (!GameController.CanPlayerMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector2 movement = new(moveHorizontal, moveVertical);
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameController.CanPlayerMove)
        {
            return;
        }

        if (other.CompareTag("Coletavel"))
        {
            if (other.TryGetComponent(out OrderedCollectible orderedCollectible))
            {
                orderedCollectible.TryCollect(collectibleTimeBonus);
                return;
            }

            Destroy(other.gameObject);
            GameController.Collect(1, collectibleTimeBonus);
            GameAudioManager.PlayCollect();
        }
    }

    public void HandleEnemyHit()
    {
        if (invulnerable || GameController.GameOver)
        {
            return;
        }

        StartCoroutine(DamageRoutine());
    }

    private System.Collections.IEnumerator DamageRoutine()
    {
        invulnerable = true;
        GameController.DamagePlayer(1, enemyTimePenalty);
        GameAudioManager.PlayHit();
        rb.position = spawnPoint;
        rb.linearVelocity = Vector2.zero;

        float elapsed = 0f;
        while (elapsed < invulnerabilityDuration)
        {
            elapsed += 0.1f;
            spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(0.1f);
        }

        spriteRenderer.enabled = true;
        invulnerable = false;
    }
}
