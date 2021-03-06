﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyManager : MonoBehaviour {

    public int maxNumberOfDecoys = 30;
    public float alphaSpeed = 6f;
    public float minSpawnDistanceFromPlayer = 25;
    public float maxSpawnDistanceFromPlayer = 80;
    public float minDespawnDistanceFromPlayer = 15;
    public float maxDespawnDistanceFromPlayer = 80;
    public float minY = 0.5f;
    public float maxY = 2f;

    private GameObject player;
    private Camera playerCamera;
    private List<GameObject> activeDecoys;
    private List<GameObject> despawningDecoys;
    private bool active;

    void Start ()
    {
        activeDecoys = new List<GameObject>(maxNumberOfDecoys);
        despawningDecoys = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerCamera = player.GetComponentInChildren<Camera>();
    }
	
	void Update ()
    {
        UpdateActiveDecoys();
        UpdateDespawningDecoys();
        if (active)
        {
            CreateNewDecoys();
        }
    }

    void UpdateActiveDecoys()
    {
        for (int i = activeDecoys.Count - 1; i >= 0; i--)
        {
            GameObject decoy = activeDecoys[i];
            bool canDespawnDecoy = CanDespawnDecoy(decoy);
            if (canDespawnDecoy)
            {
                despawningDecoys.Add(decoy);
                activeDecoys.RemoveAt(i);
            }
            else
            {
                FadeIn(decoy);
            }
        }
    }

    private void FadeIn(GameObject decoy)
    {
        Renderer[] renderers = decoy.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];
            Color color = renderer.material.color;
            color.a = Mathf.Clamp(color.a + alphaSpeed * Time.deltaTime, 0, 1);
            renderer.material.color = color;
        }
    }

    private bool FadeOut(GameObject decoy)
    {
        Renderer[] renderers = decoy.GetComponentsInChildren<Renderer>();
        bool noAlpha = false;
        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];
            Color color = renderer.material.color;
            color.a = Mathf.Clamp(color.a - alphaSpeed * Time.deltaTime, 0, 1);
            renderer.material.color = color;
            if (color.a == 0)
            {
                noAlpha = true;
            }
        }
        return noAlpha;
    }

    void UpdateDespawningDecoys()
    {
        for (int i = despawningDecoys.Count - 1; i >= 0; i--)
        {
            GameObject decoy = despawningDecoys[i];
            bool noAlpha = FadeOut(decoy);
            if (noAlpha)
            {
                despawningDecoys.RemoveAt(i);
                ReleaseDecoy(decoy);
            }
        }
    }

    void CreateNewDecoys()
    {
        Vector3 cameraPosition = playerCamera.transform.position;
        int decoysToAdd = maxNumberOfDecoys - activeDecoys.Count;
        for (int i = 0; i < decoysToAdd; i++)
        {
            Vector3 spawnPosition = CalculateSpawnPosition();
            Vector3 direction = spawnPosition - cameraPosition;
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(cameraPosition, direction, out hitInfo, direction.magnitude);
            if (!hit)
            {
                GameObject decoy = GetDecoy(spawnPosition);
                activeDecoys.Add(decoy);
            }
        }
    }

    GameObject GetDecoy(Vector3 spawnPosition)
    {
        GameObject decoy = DecoyPool.instance.GetObject();
        decoy.transform.position = spawnPosition;
        Renderer renderer = decoy.GetComponentInChildren<Renderer>();
        Color color = renderer.material.color;
        color.a = 0;
        renderer.material.color = color;
        decoy.SetActive(true);
        return decoy;
    }

    private Vector3 CalculateSpawnPosition()
    {
        float angle = Random.Range(0, 360f);
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        Vector3 noY = new Vector3(1, 0, 1);
        Vector3 playerForward = Vector3.Scale(playerCamera.transform.forward, noY).normalized;
        if (playerForward.magnitude == 0)
        {
            playerForward = Vector3.forward;
        }

        Vector3 direction = rotation * playerForward;
        float distance = Random.Range(minSpawnDistanceFromPlayer, maxSpawnDistanceFromPlayer);
        Vector3 newY = new Vector3(0, Random.Range(minY, maxY), 0);
        Vector3 playerPositon = Vector3.Scale(player.transform.position, noY);
        Vector3 spawnPosition = playerPositon + (direction * distance) + newY;
        spawnPosition.y += GetTerrainHeightAtPosition(spawnPosition);
        return spawnPosition;
    }

    private float GetTerrainHeightAtPosition(Vector3 position)
    {
        Terrain[] terrains = Terrain.activeTerrains;
        float height = 0;
        for (int i = 0; i<terrains.Length; i++)
        {
            height = terrains[i].SampleHeight(position);
            if (height != 0)
            {
                break;
            }
        }
        return height;
    }

    void ReleaseDecoy(GameObject decoy)
    {
        DecoyPool.instance.ReleaseObject(decoy);
    }

    bool CanDespawnDecoy(GameObject decoy)
    {
        Vector3 noY = new Vector3(1, 0, 1);
        Vector3 playerPosition = Vector3.Scale(player.transform.position, noY);
        Vector3 decoyPosition = Vector3.Scale(decoy.transform.position, noY);
        float distance = Vector3.Distance(playerPosition, decoyPosition);
        if (distance <= minDespawnDistanceFromPlayer || distance >= maxDespawnDistanceFromPlayer)
        {
            return true;
        }
        return false;
    }

    public bool Active
    {
        get { return active; }
        set {
            active = value;
            if (!active)
            {
                DespawnDecoys();
            }
        }
    }

    void DespawnDecoys()
    {
        for (int i = activeDecoys.Count - 1; i >= 0; i--)
        {
            despawningDecoys.Add(activeDecoys[i]);
            activeDecoys.RemoveAt(i);
        }
    }
}
