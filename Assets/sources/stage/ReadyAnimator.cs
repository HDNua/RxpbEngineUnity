using UnityEngine;

public class ReadyAnimator : MonoBehaviour
{
    StageManager stageManager;

    void Start()
    {
        stageManager = GetComponentInParent<StageManager>();
    }
    void FE_PlayReadyVoice()
    {
        stageManager.AudioSources[0].Play();
    }
    void FE_SpawnPlayer()
    {
        // HUD를 활성화 합니다.
        stageManager.hud.Health.SetActive(true);
        stageManager.hud.HealthBar.SetActive(true);

        // 플레이어 소환을 요청합니다.
        stageManager.player.transform.position
            = stageManager.playerSpawnPos.position;
        stageManager.player.RequestSpawn();
    }
}