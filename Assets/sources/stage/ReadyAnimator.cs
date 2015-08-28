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
        stageManager.player.transform.position
            = stageManager.playerSpawnPos.position;
        stageManager.player.RequestSpawn();
    }
}