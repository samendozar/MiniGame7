using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor.VersionControl;
//using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    private const int POINT_PER_PART = 5;
    private int score = 0;
    private int parts = 0; //number of parts to collecte
    [SerializeField]
    private Text scoreBoard = null;
    private string textScoreBoard = "";
    [SerializeField]
    private Slider healthBar = null;

    private Transform playerPos = null; //Needed to get the player position

    private enum GameSatus { Alive, Win, Lose };
    private GameSatus status = GameSatus.Alive;
    [SerializeField]
    private Text message = null;
    private const string YOU_WIN = "You Win!!!";
    private const string YOU_LOSE = "You Lose!!!";
    private int ellapsedPlayTime = 0;
    [SerializeField]
    private int MILLISECONDS_AMONG_NEW_ENEMY = 30000;
    [SerializeField]
    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //Si se ha establecido el campo de texto se obtiene el texto
        textScoreBoard = scoreBoard?.text;
        //Debug.Log(textScoreBoard);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController controller = player.GetComponent<PlayerController>();
        playerPos = player.transform;
        controller.OnPartCollected += PartCollected;
        parts = GameObject.FindGameObjectsWithTag("CollectMe").Length;
    }

    // LateUpdate is called after frame is updated
    private void LateUpdate()
    {

        if(this.ellapsedPlayTime >= MILLISECONDS_AMONG_NEW_ENEMY){
            this.ellapsedPlayTime = 0;
            //new Enemy added to Game
            Vector3 position = new Vector3(0, 0, 0);
            Quaternion rotation = Quaternion.identity;
            GameObject newObject = Instantiate(enemy, position, rotation);
        }else{
            this.ellapsedPlayTime += (int)(Time.deltaTime * 1000);
        }
        

        if (status == GameSatus.Alive && playerPos.position.y < 0)
        {
            status = GameSatus.Lose;
            EndGame();
        }
    }

    public void PartCollected()
    {
        score += GameController.POINT_PER_PART;
        //Debug.Log("Current Score: " + score);
        if (scoreBoard != null)
        {
          scoreBoard.text = textScoreBoard + score;
        }
        parts--;
        if (parts == 0 && status == GameSatus.Alive)
        {
            status = GameSatus.Win;
            EndGame();
        }
     }

    public void UpdateHealthMeter(int health)
    {
        healthBar.value = health;
        if (status == GameSatus.Alive && healthBar.value <= 0.0f)
        {
            status = GameSatus.Lose;
            EndGame();
        }
    }

    private void EndGame()
    {
        switch (status)
        { //SW
            case GameSatus.Lose:
                message.gameObject.SetActive(true);
                message.text = YOU_LOSE;
                message.color = Color.red;
                Time.timeScale = 0;
                break;
            case GameSatus.Win:
                message.gameObject.SetActive(true);
                message.text = YOU_WIN;
                message.color = Color.green;
                Time.timeScale = 0;
                break;
        }//SW
    }

}
