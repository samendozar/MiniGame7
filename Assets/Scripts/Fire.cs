using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private int timeBetweenShots = 3;
    private float timeSinceLastShots = 0f;
    //Reference to prefab to be used as bullet
    [SerializeField]
    private GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeSinceLastShots >= timeBetweenShots && Input.GetButtonDown("Fire1")){
            Vector3 angle = bullet.transform.rotation.eulerAngles;
            Instantiate(bullet, this.transform.position, Quaternion.Euler(angle));
            timeSinceLastShots = 0f;
        }else{
            timeSinceLastShots += Time.deltaTime;
        }
    }
}
