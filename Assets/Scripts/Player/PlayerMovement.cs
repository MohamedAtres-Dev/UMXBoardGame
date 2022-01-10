using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerMovement : MonoBehaviour
{

    public enum PlayerState
    {
        ISMOVING,
        STOPPING,
        FINISHED
    }

    #region Variables
    public int index;
    public Color color;
    [HideInInspector] public Coroutine movePlayer;

    [SerializeField] private float stepTime = 0.2f;
    [SerializeField] private float movingSpeed = 5f;

    private int diceNum;
    private PlayerCollision _playerCollision;
    public static UnityAction onPlayerFinishedMove = delegate { };

    public PlayerState currentState;

    #endregion


    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        _playerCollision = GetComponent<PlayerCollision>();
        currentState = PlayerState.STOPPING;
    }



    private void OnMouseDown()
    {
        if (index == GameManager.Instance.currentTurn)
        {

            if (GameManager.Instance.diceNum > 0)
                movePlayer = StartCoroutine(MovePlayer(GameManager.Instance.diceNum));
        }

    }

    #endregion


    #region Methods
    IEnumerator MovePlayer(int numOfSteps)
    {
        currentState = PlayerState.ISMOVING;

        //Here the logic of moving player 
        var tempPos = numOfSteps;


        for (int i = 0; i < numOfSteps; i++)
        {
            if (_playerCollision.currentTilePos.y == GameManager.Instance.numOfRows - 1 || _playerCollision.currentTilePos.y == GameManager.Instance.numOfRows - 2)
            {
                if (((_playerCollision.currentTilePos.x + (GameManager.Instance.numOfRows - (_playerCollision.currentTilePos.y + 1)) * 8) - tempPos) >= 0)
                {

                    tempPos--;
                }
                else
                {
                    if (!GameManager.Instance.isFullDice)
                    {
                        GameManager.Instance.IncreaseCurrentTurn();
                    }
                    //Increase Current turn

                    GameManager.Instance.diceNum = 0;
                    onPlayerFinishedMove.Invoke();
                    yield break;
                }

            }

            if (_playerCollision.currentTilePos.y % 2 == 0)
            {
                if (_playerCollision.currentTilePos.x == GameManager.Instance.numOfCols - 1)
                {
                    //Increase the y value here by 1 step
                    MoveMechanic(new Vector2(0, 1));
                }
                else
                {
                    //Increase the x value here 
                    MoveMechanic(new Vector2(1, 0));
                }


            }
            else
            {
                if (_playerCollision.currentTilePos.x == 0)
                {
                    //Increase the y value here by 1 step
                    MoveMechanic(new Vector2(0, 1));
                }
                else
                {
                    //Decrease the x value here 
                    MoveMechanic(new Vector2(-1, 0));
                }

            }


            yield return new WaitForSeconds(stepTime);
        }


        if (!GameManager.Instance.isFullDice)
        {
            GameManager.Instance.IncreaseCurrentTurn();
        }
        

        GameManager.Instance.diceNum = 0;
        onPlayerFinishedMove.Invoke();

        currentState = PlayerState.STOPPING;
    }

    public void MoveMechanic(Vector2 moveDir)
    {

        transform.Translate(new Vector3(moveDir.x * GameManager.Instance.tileSize, 0, moveDir.y * GameManager.Instance.tileSize));
        gameObject.transform.parent = _playerCollision.currentTile.transform;

        //Add Sound and Simple Particle effect
    }

    #endregion

}
