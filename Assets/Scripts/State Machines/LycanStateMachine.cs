using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class LycanStateMachine : MonoBehaviour {

    private const string lycanTag = "Lycan";
    public Camera playerCamera;
    public GameObject player;
    public GameObject lycan;
    public float minTimeBetweenSpawns = 20;
    public float maxTimeBetweenSpawns = 30;
    public float minSpawnDistanceFromPlayer = 20;
    public float maxSpawnDistanceFromPlayer = 30;
    public float timeToFindLycan = 3;
    public float timeToReadjustSight = 1;
    [Range(0, 1)]
    public float chanceToDespawn = 0.05f;
    public int spawnTries = 3;

    public LayerMask lycanLayer;

    private float timerStartPoint;

    private float nextSpawnTimeInterval;

    private StateMachine<LycanStates> fsm;

    private Vector3 noYVector = new Vector3(1, 0, 1);

    private bool hiddenBehindObstacle;

    void Awake()
    {
        fsm = StateMachine<LycanStates>.Initialize(this, LycanStates.WaitingForRespawn);
    }

    void Inactive_Enter()
    {
        lycan.SetActive(false);
    }

    void Inactive_Update()
    {
        bool playerOutsideSafeZone = false;
        if (playerOutsideSafeZone)
        {
            fsm.ChangeState(LycanStates.WaitingForRespawn);
        }
    }

    void WaitingForRespawn_Enter()
    {
        hiddenBehindObstacle = false;
        timerStartPoint = Time.time;
        nextSpawnTimeInterval = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }

    void WaitingForRespawn_Update()
    {
        if (Time.time - timerStartPoint >= nextSpawnTimeInterval)
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

    Vector3 CalculateSpawnPosition()
    {
        float minAngle = playerCamera.fieldOfView;
        float maxAngle = 360 - playerCamera.fieldOfView;
        float angle = Random.Range(minAngle, maxAngle);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        Vector3 playerForward = playerCamera.transform.forward;
        playerForward.y = 0;
        playerForward = playerForward.normalized;
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
        // Play Sfx
        lycan.SetActive(true);
    }

    void WaitingForFirstContact_Update()
    {
        CheckForDespawn();
        /*Vector3 cameraPosition = playerCamera.transform.position;
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(cameraPosition, playerCamera.transform.forward, out hitInfo);

        if (hit)
        {
            if (hitInfo.collider.gameObject.CompareTag(lycanTag))
            {
                fsm.ChangeState(LycanStates.StaringAtPlayer);
            } else
            {

            }
        } else if ()
        {

        }*/
    }

    void WaitingForReContact_Update()
    {
    }

    void StaringAtPlayer_Update()
    {
    }

    private void CheckForDespawn()
    {
        bool newHiddenBehindObstacle = IshiddenBehindObstacle();
        // If lycan was visible before but its now hidden, it has a % of dissapearing
        if (!hiddenBehindObstacle && newHiddenBehindObstacle && IsCursorOnLycan())
        {
            float random = Random.Range(0, 1f);
            if (random <= chanceToDespawn)
            {
                fsm.ChangeState(LycanStates.WaitingForRespawn);
            }
        }
        hiddenBehindObstacle = newHiddenBehindObstacle;
    }

    private bool IsCursorOnLycan()
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        float distance = Vector3.Distance(lycan.transform.position, cameraPosition);
        RaycastHit hitInfo;
        return Physics.Raycast(cameraPosition, playerCamera.transform.forward, out hitInfo, distance, lycanLayer);
    }

    private bool IshiddenBehindObstacle()
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        Vector3 direction = lycan.transform.position - cameraPosition;
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(cameraPosition, direction, out hitInfo, direction.magnitude);
        return !hit || !hitInfo.collider.gameObject.CompareTag(lycanTag);
    }
}
