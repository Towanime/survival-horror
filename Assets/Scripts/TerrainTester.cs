using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTester : MonoBehaviour {
    public TerrainData data;
    public GameObject treePrefab;
    public GameObject treeContainer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [ContextMenu("Read Trees")]
    public void readTrees()
    {
        foreach (TreeInstance tree in data.treeInstances)
        {
            // Find its local position scaled by the terrain size (to find the real world position)
            Vector3 worldTreePos = Vector3.Scale(tree.position, data.size) + Terrain.activeTerrain.transform.position;
            GameObject t = Instantiate(treePrefab, worldTreePos, Quaternion.identity); // Create a prefab tree on its pos
            t.transform.parent = treeContainer.transform;
        }

    }
}
