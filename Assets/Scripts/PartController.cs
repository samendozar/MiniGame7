using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //Si ponemos esto hace que se añada el rigidbody al asignar el script a un objeto
public class PartController : MonoBehaviour
{
    // Start is called before the first frame update
    private float xDegree = 5f;
    private float yDegree = 7.2f;

    private float force = 100;

    private bool incX = true;
    private bool incZ = false;
    private Rigidbody rigid;
    void Start()
    {
        //Random.InitState(77);
        rigid = this.GetComponent<Rigidbody>();
        incX = (int)(Random.value * 100) % 2 == 0;
        incZ = !incX;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = force * Time.deltaTime * ((incX) ? 1 : -1);
        float z = force * Time.deltaTime * ((incZ) ? 1 : -1);
        rigid.AddForce(new Vector3(x, 0.0f, z));
        //Rotating gameObect
        rigid.AddTorque(new Vector3(xDegree, yDegree, 0));
    }

    //We have to change direction when there are a collision with a wall
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.name) { //SW
            case "Front":
                incZ = false;
                break;
            case "Back":
                incZ = true;
                break;
            case "Left":
                incX = true;
                break;
            case "Right":
                incX = false;
                break;
        }//SW
    }
}
