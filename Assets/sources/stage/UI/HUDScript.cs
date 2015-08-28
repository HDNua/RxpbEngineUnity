using UnityEngine;

/// <summary>
/// HUD(Head Up Display) 스크립트입니다.
/// </summary>
public class HUDScript : MonoBehaviour
{
    public GameObject Health;
    public GameObject HealthBar;

    void Start()
    {
        Health.SetActive(false);
        HealthBar.SetActive(false);
    }
    void Update()
    {
        StageManager stageManager = GetComponentInParent<StageManager>();
        PlayerController player = stageManager.player;
        if (player != null)
        {
            Vector3 healthScale = Health.transform.localScale;
            healthScale.y = (float)player.Health / player.MaxHealth;
            Health.transform.localScale = healthScale;
        }
    }
}
