using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerMovement : MonoBehaviour
{

    #region Variables
    public int index;
    public Color color;
    [HideInInspector] public Coroutine movePlayer;

    [SerializeField] private float stepTime = 0.2f;
    [SerializeField] private float movingSpeed = 5f;

    private int diceNum;
    private PlayerCollision _playerCollision;
    public static UnityAction onPlayerFinishedMove = delegate { };


    #endregion


    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        _playerCollision = GetComponent<PlayerCollision>();
    }

    private void FixedUpdate()
    {
        
    }



    private void OnMouseDown()
    {
        if (index == GameManager.Instance.currentTurn)
        {
            Debug.Log("Player " + index + " Turn Player Manager");

            if (GameManager.Instance.diceNum > 0)
                movePlayer = StartCoroutine(MovePlayer(GameManager.Instance.diceNum));
        }

    }

    #endregion


    #region Methods
    IEnumerator MovePlayer(int numOfSteps)
    {

        //Here the logic of moving player 
        Debug.Log("Player " + numOfSteps + " Number Of Steps");
        var tempPos = numOfSteps;


        for (int i = 0; i < numOfSteps; i++)
        {
            if(_playerCollision.currentTilePos.y == GameManager.Instance.numOfRows - 1 || _playerCollision.currentTilePos.y == GameManager.Instance.numOfRows - 2)
            {
                if(((_playerCollision.currentTilePos.x + (GameManager.Instance.numOfRows - (_playerCollision.currentTilePos.y +1) ) * 8) - tempPos) >= 0)
                {

                    tempPos--;
                }
                else
                {
                    if (!GameManager.Instance.isFullDice)
                    {
                        GameManager.Instance.currentTurn++;

                        if (GameManager.Instance.currentTurn > 3)
                        {
                            GameManager.Instance.currentTurn = 0;
                        }

                        GameManager.Instance.statusTxt.text = "Player " + (GameManager.Instance.currentTurn + 1) + " Turn";
                    }
                    //Increase Current turn

                    GameManager.Instance.diceNum = 0;
                    onPlayerFinishedMove.Invoke();
                    yield break;
                }

            }

            if(_playerCollision.currentTilePos.y % 2 == 0)
            {
                if(_playerCollision.currentTilePos.x == GameManager.Instance.numOfCols - 1)
                {
                   // Debug.Log("Current Tile Position: X " + _playerCollision.currentTilePos.x + " Y " + _playerCollision.currentTilePos.y);
                    //Increase the y value here by 1 step
                    MoveMechanic(new Vector2(0,  1));
                }
                else
                {
                    //Debug.Log("Current Tile Position: X " + _playerCollision.currentTilePos.x + " Y " + _playerCollision.currentTilePos.y);
                    //Increase the x value here 
                    MoveMechanic(new Vector2( 1  , 0));
                }


            }
            else
            {
                if (_playerCollision.currentTilePos.x == 0)
                {
                    //Debug.Log("Current Tile Position: X " + _playerCollision.currentTilePos.x + " Y " + _playerCollision.currentTilePos.y);
                    //Increase the y value here by 1 step
                    MoveMechanic(new Vector2(0,  1));
                }
                else
                {
                    ///Debug.Log("Current Tile Position: X " + _playerCollision.currentTilePos.x + " Y " + _playerCollision.currentTilePos.y);
                    //Decrease the x value here 
                    MoveMechanic(new Vector2( - 1 , 0));
                }

            }


            yield return new WaitForSeconds(stepTime);
        }


        if (!GameManager.Instance.isFullDice)
        {
            GameManager.Instance.currentTurn++;

            if (GameManager.Instance.currentTurn > 3)
            {
                GameManager.Instance.currentTurn = 0;
            }

            GameManager.Instance.statusTxt.text = "Player " + (GameManager.Instance.currentTurn + 1) + " Turn";
        }
        //Increase Current turn

        GameManager.Instance.diceNum = 0;
        onPlayerFinishedMove.Invoke();
    }

    public void MoveMechanic(Vector2 moveDir)
    {
        Debug.Log("Moving Tile Position: X " + moveDir.x + " Y " + moveDir.y);
        transform.Translate ( new Vector3(moveDir.x * GameManager.Instance.tileSize , 0, moveDir.y * GameManager.Instance.tileSize)) ;
        gameObject.transform.parent = _playerCollision.currentTile.transform;
    }

    #endregion

}
