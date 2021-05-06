using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneration : MonoBehaviour {

    public enum Direction { }

    public Transform[] startingPositions;       // top roll of the grid
    public GameObject[] rooms; // index 0 --> closed, index 1 --> LR, index 2 --> LRB, index 3 --> LRT, index 4 --> LRBT

    private int direction;
    private bool stopGeneration;
    private int downCounter;
    private float timeBtwSpawn;         // time between spawning a room

    public float moveIncrement;
    public float startTimeBtwSpawn;

    public float minX = 25f;
    public float maxX = 0f;
    public float minY = -25f;

    public LayerMask whatIsRoom;
    

    private void Start()
    {
       
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[1], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (timeBtwSpawn <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwSpawn = startTimeBtwSpawn;       // reset timer after spwan a room
        }
        else {
            timeBtwSpawn -= Time.deltaTime;
        }
    }

    private void Move()
    {

        if (direction == 1 || direction == 2)       // 40% it will move Right 
        { 
            if (transform.position.x < maxX)
            {
                downCounter = 0;
                Vector2 pos = new Vector2(transform.position.x + moveIncrement, transform.position.y);      // get Pos 10 points to the right
                transform.position = pos;

                int randRoom = Random.Range(1, 4);      // NOT close room or have bot open
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity);      //create room at the new Pos

                direction = Random.Range(1, 6);     // Only allow 1,2,5 so the level generator doesn't move Left (backward)
                if (direction == 3) direction = 1; 
                else if (direction == 4) direction = 5;
            }
            else 
            {
                direction = 5;      // go down if it exceed max X pos
            }
        }
        else if (direction == 3 || direction == 4)      // 40% it will move Left
        { 
            if (transform.position.x > minX)
            {
                downCounter = 0;
                Vector2 pos = new Vector2(transform.position.x - moveIncrement, transform.position.y);
                transform.position = pos;

                int randRoom = Random.Range(1, 4);      // NOT close room or have bot open
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);     // Only allow 3,4,5 so the level generator doesn't move Right (backward)
            }
            else 
            {
                direction = 5;      // go down if it exceed min X pos
            }
           
        }
        else if (direction == 5)        // 20% it will move Down
        { 
            downCounter++;
            if (transform.position.y > minY)
            {
                Room previousRoom = Physics2D.OverlapCircle(transform.position, 1, whatIsRoom).GetComponent<Room>();    // get the room before going d
                
                if (previousRoom.roomType != 4 && previousRoom.roomType != 2)
                {
                    if (downCounter >= 2)       // if the room go down twice replace the previous room with a Top and Bot open room
                    {
                        previousRoom.RoomDestruction();
                        Instantiate(rooms[4], transform.position, Quaternion.identity);
                    }
                    else       // replace the previous room with a Bot open room
                    {
                        previousRoom.RoomDestruction();
                        int randRoomDownOpening = Random.Range(2, 5);
                        if (randRoomDownOpening == 3)
                        {
                            randRoomDownOpening = 2;
                        }
                        Instantiate(rooms[randRoomDownOpening], transform.position, Quaternion.identity);
                    }

                }
  
                Vector2 pos = new Vector2(transform.position.x, transform.position.y - moveIncrement);
                transform.position = pos;

                int randRoom = Random.Range(3, 5);      // Makes sure the room we drop into has a TOP opening !
                Instantiate(rooms[randRoom], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);     // continue the cycle
            }
            else {
                stopGeneration = true;      // Stop generate room when exceeding min Y
            }
            
        }
    }
}
