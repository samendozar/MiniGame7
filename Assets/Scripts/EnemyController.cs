using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Random;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]  //Adding this sentence this class will be used only if the GameObject has a NavMeshAgent
[RequireComponent(typeof(UnityEngine.Animator))]
public class EnemyController : MonoBehaviour
{
    private const int DAMAGE_POINTS = 10;
    private const int MILLISECONDS_AMONG_ATTACK = 3000;

    [SerializeField]
    private int raisePlayer = 5;
    private int millisencondsSinceLastAttack = 0;

    private GameObject player;
    private Rigidbody playerRigid;

    private NavMeshAgent pathFinder;
    private Animator anim;
    //Delegated method to attack the player
    public delegate void Attack(int damage);
    //Event to attack the player
    public event Attack OnAttack = null;

    private bool isActivated = false;

    private bool reachedWaitPoint = false;

    [SerializeField]
    private int waitPointsNo;
    private int targetWaitPoint = 0;
    private int[,] waitPoints;

    private static bool seen;

    // Start is called before the first frame update
    void Start()
    {
        //Reassing waitPoints to a minimum of 2 if needed
        if(this.waitPointsNo < 2)
            this.waitPointsNo = 2;

        generateRandomWaitPoints(this.waitPointsNo);

        player = GameObject.FindGameObjectWithTag("Player");
        playerRigid = player.GetComponent<Rigidbody>();
        OnAttack += player.GetComponent<PlayerController>().Injure;
        pathFinder = GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isActivated || seen)
        {
            goAgainstPlayer();
        }else{
            patrol();
        }

    }

    private void goAgainstPlayer()
    {
        pathFinder.SetDestination(player.transform.position);
        bool isWalking = pathFinder.remainingDistance > pathFinder.stoppingDistance;
        anim.SetBool("isWalking", isWalking);
        this.millisencondsSinceLastAttack += (int)(Time.deltaTime * 1000);
        /* if (!isWalking) //Force to look at the player when the enemy is not walking
         {
             gameObject.transform.LookAt(player.transform);
         }*/
    }

    private void OnTriggerEnter(Collider other)
    {//OnTriggerEnter
        if (other.gameObject.tag.Equals("Player"))
        {
            this.isActivated = true;
            seen = true;
        }
    }//OnTriggerEnter

    private void OnCollisionEnter(Collision collision)
    {//OnCollisionEnter
        if (this.millisencondsSinceLastAttack >= MILLISECONDS_AMONG_ATTACK && 
            collision.gameObject.tag.Equals("Player"))
        {
            OnAttack(DAMAGE_POINTS);
            playerRigid.AddForce(Vector3.up * raisePlayer, ForceMode.Impulse);
            this.millisencondsSinceLastAttack = 0;
        }
    }//OnCollisionEnter

    //When the enemy has been killed this service will be used
    void Die(){
        //The enemy is dead so it must stop
        isActivated = false;
        anim.SetTrigger("isDead");
        pathFinder.isStopped = true;
    }

    void Destroy(){
        Destroy(gameObject);
    }


    void patrol(){

        //default to walking
        anim.SetBool("isWalking", true);
        if(!this.reachedWaitPoint){
            //Reassing direction towards waitPoint
            Vector3 newPosition = new Vector3(this.waitPoints[this.targetWaitPoint, 0], this.waitPoints[this.targetWaitPoint, 1], this.waitPoints[this.targetWaitPoint, 2]);
            pathFinder.SetDestination(newPosition);

            Debug.Log("Destination is: " + this.waitPoints[this.targetWaitPoint, 0] + "," + this.waitPoints[this.targetWaitPoint, 1] + "," + this.waitPoints[this.targetWaitPoint, 2]);
            Debug.Log("Current Destination: " + pathFinder.transform.position);
            //validates if arrived 
            if(pathFinder.remainingDistance <= pathFinder.stoppingDistance)
                this.reachedWaitPoint = true;

        }else{

            Debug.Log("Reached Destination");
            if(this.targetWaitPoint + 1 >= this.waitPointsNo){
                Debug.Log("Reached Last Destination");
                this.targetWaitPoint = 0; //back to initial waitpoint
            }else{
                Debug.Log("Reached Destination: " + this.waitPointsNo);
                this.targetWaitPoint += 1; //on to the next waitpoint
            } 
                
            Vector3 newPosition = new Vector3(this.waitPoints[this.targetWaitPoint, 0], this.waitPoints[this.targetWaitPoint, 1], this.waitPoints[this.targetWaitPoint, 2]);
            pathFinder.SetDestination(newPosition);
            this.reachedWaitPoint = false;
        }
    }

    //Generates Random Wait Points for Enemy. 
    //TODO: Use GameObjects instead of Arrays
    void generateRandomWaitPoints(int waitPointsNo){
        
        this.waitPoints = new int[waitPointsNo, 3];

        //Generate n tuples with coordinates for waitPoints
        for (int i = 0; i < waitPointsNo; i++)
        {
            //perimeter delimited to square 15x15 (arbitrary)
            int x = Random.Range(-15, 16);
            int z = Random.Range(-15, 16);
            this.waitPoints[i, 0] = x;
            this.waitPoints[i, 1] = 0; //y is fixed to 0
            this.waitPoints[i, 2] = z;
        }

        for (int i = 0; i < waitPointsNo; i++)
        {
            Debug.Log($"({waitPoints[i, 0]}, {waitPoints[i, 1]}, {waitPoints[i, 2]})");
        }

    }
}