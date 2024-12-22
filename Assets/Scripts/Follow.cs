using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    //GameObject to follow. 
    //It is a SerializeField so, it is accesible in the unity editor
    [SerializeField]
    private GameObject follow;
    //Relative Position among the follower and the follow
    private Vector3 relativePosition;
    // Use this for initialization

    [SerializeField]
    private GameObject gunFollow;
    void Start()
    {
        relativePosition = follow.transform.position - transform.position;
    }

    // We have to update position follower after update the frame
    void LateUpdate()
    {
        transform.position = follow.transform.position - this.relativePosition;
        gunFollow.transform.position = follow.transform.position;

    }
}
