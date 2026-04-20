using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ExitPortal : MonoBehaviour
{
    [SerializeField] private Color lockedColor = new(0.18f, 0.38f, 0.46f, 1f);
    [SerializeField] private Color unlockedColor = new(0.26f, 0.84f, 0.74f, 1f);

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().isTrigger = true;
        UpdateVisual();
    }

    private void OnEnable()
    {
        GameController.StateChanged += UpdateVisual;
    }

    private void OnDisable()
    {
        GameController.StateChanged -= UpdateVisual;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerMovement _))
        {
            return;
        }

        GameController.TryFinishLevel();
    }

    private void UpdateVisual()
    {
        spriteRenderer.color = GameController.ExitUnlocked ? unlockedColor : lockedColor;
    }
}
