using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Path_Create : MonoBehaviour
{

    public List<GameObject> pathNodes;

    // Use this for initialization
    void Awake()
    {
        foreach (Transform node in transform)
        {
            pathNodes.Add(node.gameObject);
        }
        pathNodes = pathNodes.OrderBy(x => x.name).ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
