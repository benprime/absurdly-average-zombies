using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ZombiePath {
        public List<GameObject> nodes;
};

public class Path_Create : MonoBehaviour
{
    public ZombiePath path;
    

    // Use this for initialization
    void Awake()
    {
        //foreach (Transform node in transform)
        //{
        //    nodes.Add(node.gameObject);
        //}
        //nodes = nodes.OrderBy(x => x.name).ToList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
