using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //Bullet Speed
    [SerializeField]
    private int speed = 3;

    //Bullet Lifetime
    [SerializeField]
    private int lifetime = 3;
    //Explosion Effect
    [SerializeField]
    private GameObject explosionEffect;



    // Start is called before the first frame update
    void Start()
    {
        //Due to bullet axis, to move bullet
        //horizontalle a force must be applied to Y axis
        GetComponent<Rigidbody>().velocity = transform.up * speed;
        Invoke("Destroy", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Destroy(){
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag.Equals("Player"))
            return;
        //First: Cancel bullet destruction
        CancelInvoke("Destroy");
        if(collision.gameObject.tag.Equals("Enemy")){
            //Enemy must die
            collision.gameObject.SendMessage("Die");
        }
        GameObject explosion = (GameObject) Instantiate (explosionEffect, transform.position, Quaternion.identity);
        //Destroy Explosion after 2 seconds
        GameObject.Destroy(explosion, 2f);
        Destroy();
    }
}
