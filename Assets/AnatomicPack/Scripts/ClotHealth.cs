using UnityEngine;

public class ClotHealth : MonoBehaviour
{
    [Header("Clot Health")]
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool destroyed;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (destroyed || damage <= 0f)
            return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);

        Debug.Log($"Clot hit! Health: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
            DestroyClot();
    }

    private void DestroyClot()
    {
        destroyed = true;

        Debug.Log("Blood clot destroyed! You win!");

        gameObject.SetActive(false);
    }
}