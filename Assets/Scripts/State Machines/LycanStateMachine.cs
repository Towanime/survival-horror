using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class LycanStateMachine : MonoBehaviour {

    private const string lycanTag = "Lycan";
    private const string lycanEyesColliderTag = "LycanEyesCollider";

    public Camera playerCamera;
    public GameObject player;
    public GameObject lycan;
    public float minTimeBetweenSpawns = 20;
    public float maxTimeBetweenSpawns = 30;
    public float minSpawnDistanceFromPlayer = 25;
    public float maxSpawnDistanceFromPlayer = 30;
    public float maxDistanceFromPlayer = 30;
    public float distanceFromPlayerForGameOver = 10;
    public float timeToFindLycan = 3;
    public float timeToReadjustSight = 1;
    [Range(0, 1)]
    public float chanceToDespawn = 0.05f;
    public int spawnTries = 3;
    public float runningSpeed = 30;
    public bool gameOverEnabled = false;
    public bool followPlayer = true;

    public LayerMask obstacleIgnoreLayer;
    public LayerMask lycanContactAreaLayer;

    public Transform topLeftEyeTransform;
    public Transform topRightEyeTransform;
    public Transform bottomLeftEyeTransform;
    public Transform bottomRightEyeTransform;

    private StateMachine<LycanStates> fsm;
    private float nextSpawnTimeInterval;
    private bool visibleByCamera;
    private bool loopingSfx;

    private float timer;

    void Awake()
    {
        fsm = StateMachine<LycanStates>.Initialize(this, LycanStates.WaitingForRespawn);
    }

    void Update()
    {
        Debug.Log("Current Lycan state: " + fsm.State + ", Timer: " + timer);
    }

    void Inactive_Enter()
    {
        StopLoopSfx();
        lycan.SetActive(false);
    }

    void WaitingForRespawn_Enter()
    {
        if (SoundManager.Instance != null)
        {
            StopLoopSfx();
            SoundManager.Instance.FadeIn(SoundId.FOREST_AMBIENT);
        }
        timer = 0;
        lycan.SetActive(false);
        visibleByCamera = false;
        nextSpawnTimeInterval = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }

    void WaitingForRespawn_Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawnTimeInterval)
        {
            fsm.ChangeState(LycanStates.CalculatingSpawnPosition);
        }
    }

    void CalculatingSpawnPosition_Update()
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        for (int i = 0; i < spawnTries; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            Vector3 direction = spawnPosition - cameraPosition;
            bool hit = Physics.Raycast(cameraPosition, direction, direction.magnitude);
            if (!hit)
            {
                lycan.transform.position = spawnPosition;
                fsm.ChangeState(LycanStates.WaitingForFirstContact);
                break;
            }
        }
    }

    private Vector3 CalculateSpawnPosition()
    {
        float minAngle = playerCamera.fieldOfView;
        float maxAngle = 360 - playerCamera.fieldOfView;
        float angle = Random.Range(minAngle, maxAngle);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        Vector3 noY = new Vector3(1, 0, 1);
        Vector3 playerForward = Vector3.Scale(playerCamera.transform.forward, noY).normalized;
        if (playerForward.magnitude == 0)
        {
            playerForward = Vector3.forward;
        }

        Vector3 direction = rotation * playerForward;
        float distance = Random.Range(minSpawnDistanceFromPlayer, maxSpawnDistanceFromPlayer);
        return player.transform.position + (direction * distance);
    }

    void WaitingForFirstContact_Enter()
    {
        SoundManager.Instance.FadeOut(SoundId.FOREST_AMBIENT);
        SoundManager.Instance.Play(SoundId.HOWL);
        lycan.SetActive(true);
        timer = 0;
        loopingSfx = false;
    }

    private void PlayLoopSfx()
    {
        if (!loopingSfx && !SoundManager.Instance.isPlaying(SoundId.HOWL))
        {
            PlayingAudioClipContext heartbeatSfxContext = SoundManager.Instance.StopAndPlay(SoundId.HEART_BEAT);
            heartbeatSfxContext.audioSource.loop = true;
            PlayingAudioClipContext wolfSfxContext = SoundManager.Instance.StopAndPlay(SoundId.WOLF_AMBIENT);
            wolfSfxContext.audioSource.loop = true;
            PlayingAudioClipContext growlSfxContext = SoundManager.Instance.StopAndPlay(SoundId.GROWL);
            growlSfxContext.audioSource.loop = true;
            loopingSfx = true;
        }
    }
    
    private void StopLoopSfx()
    {
        SoundManager.Instance.FadeOut(SoundId.HEART_BEAT);
        SoundManager.Instance.FadeOut(SoundId.WOLF_AMBIENT);
        SoundManager.Instance.FadeOut(SoundId.GROWL);
    }

    private void UpdateLycanPosition()
    {
        if (followPlayer)
        {
            Vector3 noY = new Vector3(1, 0, 1);
            Vector3 playerPosition = Vector3.Scale(player.transform.position, noY);
            Vector3 lycanPosition = Vector3.Scale(lycan.transform.position, noY);
            float distance = Vector3.Distance(playerPosition, lycanPosition);
            if (distance > maxDistanceFromPlayer)
            {
                Vector3 translation = lycanPosition - playerPosition;
                translation = Vector3.ClampMagnitude(translation, distance - maxDistanceFromPlayer);
                lycan.transform.Translate(translation);
            }
        }
    }

    void WaitingForFirstContact_Update()
    {
        bool visibleByCamera = IsVisibleByCamera();
        bool cursorOnLycan = IsCursorOnLycan();
        bool playerStaringAtLycan = visibleByCamera && cursorOnLycan;
        bool despawned = CheckForDespawn(visibleByCamera, cursorOnLycan);
        if (despawned)
        {
            fsm.ChangeState(LycanStates.WaitingForRespawn);
        }
        else if (playerStaringAtLycan)
        {
            fsm.ChangeState(LycanStates.StaringContestWithPlayer);
        }
        else
        {
            timer += (visibleByCamera) ? Time.deltaTime : 0;
            CheckIfTimerHasRunOut(timeToFindLycan);
        }
        CheckIfPlayerIsTooClose(visibleByCamera);
        PlayLoopSfx();
        UpdateLycanPosition();
    }

    void StaringContestWithPlayer_Enter()
    {
        timer = 0;
    }

    void StaringContestWithPlayer_Update()
    {
        bool visibleByCamera = IsVisibleByCamera();
        bool cursorOnLycan = IsCursorOnLycan();
        bool playerStaringAtLycan = visibleByCamera && cursorOnLycan;
        bool despawned = CheckForDespawn(visibleByCamera, cursorOnLycan);
        if (despawned)
        {
            fsm.ChangeState(LycanStates.WaitingForRespawn);
        }
        else
        {
            timer = (!visibleByCamera || playerStaringAtLycan) ? 0 : timer + Time.deltaTime;
            CheckIfTimerHasRunOut(timeToReadjustSight);
        }
        CheckIfPlayerIsTooClose(visibleByCamera);
        PlayLoopSfx();
        UpdateLycanPosition();
    }

    void GameOverSequenceStarted_Enter()
    {
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnGameOverSequenceStarted");
    }

    void GameOverSequenceStarted_Update()
    {
        Vector3 destination = player.transform.position;
        Vector3 newPosition = Vector3.MoveTowards(lycan.transform.position, destination, runningSpeed * Time.deltaTime);
        lycan.transform.position = newPosition;
        if (Vector3.Distance(newPosition, destination) == 0)
        {
            fsm.ChangeState(LycanStates.GameOverSequenceEnded);
        }
    }

    void GameOverSequenceEnded_Enter()
    {
        GameObject.FindGameObjectWithTag("GameStateMachine").SendMessage("OnGameOverSequenceEnded");
    }

    private void CheckIfTimerHasRunOut(float maxTime)
    {
        if (timer >= maxTime && gameOverEnabled)
        {
            fsm.ChangeState(LycanStates.GameOverSequenceStarted);
        }
    }

    private void CheckIfPlayerIsTooClose(bool visibleByCamera)
    {
        Vector3 noY = new Vector3(1, 0, 1);
        Vector3 playerPosition = Vector3.Scale(player.transform.position, noY);
        Vector3 lycanPosition = Vector3.Scale(lycan.transform.position, noY);
        float distance = Vector3.Distance(playerPosition, lycanPosition);
        if (visibleByCamera && distance <= distanceFromPlayerForGameOver && gameOverEnabled)
        {
            fsm.ChangeState(LycanStates.GameOverSequenceStarted);
        }
    }

    private bool CheckForDespawn(bool visibleByCamera, bool cursorOnLycan)
    {
        bool oldVisibleByCamera = this.visibleByCamera;
        this.visibleByCamera = visibleByCamera;
        // If lycan was visible before but its now hidden, it has a % of dissapearing
        if (oldVisibleByCamera && !visibleByCamera && cursorOnLycan)
        {
            float random = Random.Range(0, 1f);
            if (random <= chanceToDespawn)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsCursorOnLycan()
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        float distance = Vector3.Distance(lycan.transform.position, cameraPosition);
        RaycastHit hitInfo;
        Debug.DrawRay(cameraPosition, playerCamera.transform.forward * distance, Color.red);
        return Physics.Raycast(cameraPosition, playerCamera.transform.forward, out hitInfo, distance, lycanContactAreaLayer);
    }

    private bool IsVisibleByCamera()
    {
        Vector3 bottomLeftCameraPosition = playerCamera.ViewportToWorldPoint(new Vector3(0, 0, playerCamera.nearClipPlane));
        bool visible = IsVisibleByCamera(bottomLeftCameraPosition, bottomLeftEyeTransform);
        if (!visible)
        {
            Vector3 bottomRightCameraPosition = playerCamera.ViewportToWorldPoint(new Vector3(1, 0, playerCamera.nearClipPlane));
            visible = IsVisibleByCamera(bottomRightCameraPosition, bottomRightEyeTransform);
        }
        if (!visible)
        {
            Vector3 topLeftCameraPosition = playerCamera.ViewportToWorldPoint(new Vector3(0, 1, playerCamera.nearClipPlane));
            visible = IsVisibleByCamera(topLeftCameraPosition, topLeftEyeTransform);
        }
        if (!visible)
        {
            Vector3 topRightCameraPosition = playerCamera.ViewportToWorldPoint(new Vector3(1, 1, playerCamera.nearClipPlane));
            visible = IsVisibleByCamera(topRightCameraPosition, topRightEyeTransform);
        }

        return visible;
    }

    private bool IsVisibleByCamera(Vector3 cameraPosition, Transform transform)
    {
        Vector3 position = transform.position;
        Vector3 direction = position - cameraPosition;
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(cameraPosition, direction, out hitInfo, direction.magnitude, ~obstacleIgnoreLayer);
        if (!hit)
        {
            Debug.DrawRay(cameraPosition, direction, Color.blue);
        }
        return !hit;
    }

    public StateMachine<LycanStates> FSM
    {
        get { return fsm; }
    }
}
