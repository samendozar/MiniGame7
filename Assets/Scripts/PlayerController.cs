using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//With this stament we force Unity to add a rigidbody to a game object
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public delegate void PartCollected();
    public event PartCollected OnPartCollected;

    private int health = 100;
    private GameController gameController = null;

    [SerializeField]
    private float force = 100f;
    /*
	 * In order to illustrate how Collision detection works, set speed to 15000
	 * and then ensure that collision detection in the rigdbody is set to Discrete. 
	 * Run the game and move the player straight to a wall.
	 * After that, change to continuous and try again
	 */
    private Rigidbody rigid;

    private const string TAG_COLLECT = "CollectMe";
    /*
     * In order to get RigidBody reference we can use Awake or Start. 
     *  -Awake is called in the very beginnig, when PlayerController is instantiated
     *  -Start is called in the first frame where PlayerController is activated
     */
    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
        gameController = gameObject.GetComponent<GameController>();
        /*
         * Verifying that rigid is not null, 
         * it is not required due to 
         * [RequireComponent(typeof(Rigidbody))] sentence
         * 
         */
    }

    // Update is called once per frame
    /*void Update()
    {
        //We will use axis horizontal for x, 
        //and axis Vertical for z
        //Using keyboard axis we will get v
        //alues from -1 to 1
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //Get object position 
        Vector3 position = gameObject.transform.position;
        //Velecity is given in points per frame 
        //so we have to use 
        //Time.deltaTime (secnods among frames)
        position.x += x * speed * Time.deltaTime;
        position.z += z * speed * Time.deltaTime;
        //Update gameObject position
        gameObject.transform.position = position;
    }*/

    //When a rigidBody is attached to gameObject and working with physic engine is required,
    //the method FixedUpdate must be implemented
    void FixedUpdate()
    {
        //We will use axes horizontal for x, and axes Vertical for z
        //Using keyboard axes we will get values from -1 to 1
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        /*
         * Due to force is in units per seconds we will adjusted the force
         * vector using Time.deltaTime
         * 
         */
        rigid.AddForce(new Vector3(x, 0, z) * 5 * force * Time.deltaTime);
    }

    public void Injure(int damage)
    {
        if (health <= 0)
        {
          return;
        }
        Debug.Log("invocado");
        health -= damage;
        gameController.UpdateHealthMeter(health);
        Debug.Log("Player health: " + health);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(TAG_COLLECT))
        {
            Object.Destroy(other.gameObject);
            OnPartCollected?.Invoke();
        }
    }
}
