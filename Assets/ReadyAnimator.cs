using UnityEngine;

public class ReadyAnimator : MonoBehaviour
{
    StageSceneManager stageManager;

    void Start()
    {
        stageManager = GetComponentInParent<StageSceneManager>();
    }
    void FE_PlayReadyVoice()
    {
        stageManager.SoundEffects[0].Play();
    }
    void FE_SpawnPlayer()
    {
        stageManager.player.transform.position
            = stageManager.playerSpawnPos.position;
        stageManager.player.RequestSpawn();
    }
}