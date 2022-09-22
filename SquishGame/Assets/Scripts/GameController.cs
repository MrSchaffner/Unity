using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Spawner spawnerScript;
    public int totalEnemies = 6;
    public Enemy[] Enemies;
    private int index = 0;
    private int enemiesThisWave = 0;
    bool GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        Enemies = new Enemy[totalEnemies];
        spawnerScript = GameObject.Find("spawner").GetComponent<Spawner>();

        StartNextLevel();

        InvokeRepeating("CheckIfLevelIsWon", 2f, 2f);  //1s delay, repeat every 1s

        //shoot out
        //enemyInstance.AddForce(spawner.up * 1);
    }

    void StartNextLevel()
    {

        if (enemiesThisWave < 3) //game is still going
        {
            enemiesThisWave++;
            for (int i = 1; i <= enemiesThisWave; i++)
            {
                //spawn using spawner and set value in array
                Enemy newEnemy = spawnerScript.Spawn();
                Enemies[index] = newEnemy;

                index++;
            }
        }
        else //game is over
        {
            GameWon();
            Debug.Log("GAME OVER-------------------------------");
        }


        //increase counter
        //populate enemies
        //spawn
    }

    void CheckIfLevelIsWon()
    {
        if (GameOver) return; //leave Early
        foreach (Enemy enemy in Enemies)
        {
            if (enemy == null) // a null enemy means they havent been spawned into array yet
            {
                StartNextLevel();
                return;
            }
            if (enemy.killedActionDone)
            {
                continue; //dead enemy found
            }
            else
            {
                return; //a living enemy found
            }
        }
        //made it here
        GameWon();
    }

    void GameWon()
    {
        GameOver = true;
        this.GetComponent<LoadScene>().LoadNextScene();
        Debug.Log("Game is over");
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
