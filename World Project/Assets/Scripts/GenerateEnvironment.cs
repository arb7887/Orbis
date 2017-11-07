using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnvironment : MonoBehaviour {
    public GameObject Tree;
    public GameObject World;
    public int amountOfTrees;
	// Use this for initialization
	void Start () {
        //place trees
        PlaceTrees();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    //instantiate and place trees around the world based on height.
    //This will only place trees where there is a grassy area
    void PlaceTrees()
    {
        Vector3[] vertlist = World.GetComponent<MeshFilter>().mesh.vertices;
        float minDistance;
        Vector3 nearestVertex;
        for (int i = 0; i < amountOfTrees; i++)
        {
            Vector3 treePos = World.transform.position + Random.onUnitSphere * 20;
            
            //find the nearest vertex on the world to the random position:
            minDistance = Mathf.Infinity;
            nearestVertex = Vector3.zero;
            foreach(Vector3 v in vertlist)
            {
                Vector3 difference = treePos - v;//difference between this vertex and the pos
                float diffmag = difference.magnitude;
                if(diffmag < minDistance)
                {
                    minDistance = diffmag;
                    nearestVertex = v;
                }
            }
            Vector3 nearestNormal = nearestVertex - World.transform.position;
            treePos = World.transform.position + treePos.normalized * nearestNormal.magnitude;
            Debug.Log(nearestNormal.magnitude);
            
            Instantiate(Tree, treePos, Quaternion.identity);
        }
    }
}
