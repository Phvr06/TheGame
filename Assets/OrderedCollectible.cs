using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class OrderedCollectible : MonoBehaviour
{
    [Header("Collection Order")]
    [Min(1)]
    [SerializeField] private int orderIndex = 1;

    [Header("Visual")]
    [SerializeField] private Sprite collectibleSprite;

    private SpriteRenderer spriteRenderer;

    public int OrderIndex => Mathf.Max(1, orderIndex);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().isTrigger = true;
        ApplySprite();
    }

    private void OnValidate()
    {
        orderIndex = Mathf.Max(1, orderIndex);
        ApplySprite();
    }

    public bool TryCollect(float timeBonus)
    {
        if (!GameController.CanCollectOrder(OrderIndex))
        {
            return false;
        }

        GameController.Collect(1, timeBonus);
        GameAudioManager.PlayCollect();
        Destroy(gameObject);
        return true;
    }

    private void ApplySprite()
    {
        if (collectibleSprite == null)
        {
            return;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = collectibleSprite;
        }
    }
}
