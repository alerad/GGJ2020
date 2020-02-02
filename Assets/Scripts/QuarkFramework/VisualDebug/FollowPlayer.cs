using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO DELETE class if none using
public class FollowPlayer : MonoBehaviour {

    public Transform transformToFollow;


    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(transformToFollow.position.x, transform.position.y, transformToFollow.position.z + 0.05f);
    }
}
