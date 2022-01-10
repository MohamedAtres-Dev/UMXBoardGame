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
    [Header("Audio Manager")]
    public AudioManager audioManager;
    public AudioClip moveSound;
    
    [Space]
    [Tooltip("THis is the current state of the player")]
    public PlayerState currentState;

    [Space]
    [Tooltip("THe current index of the player it's very useful for detecting the players")]
    public int index;

    [Tooltip("THe current color of the player it's very useful for detecting the players")]
    public Color color;

    //The IEnumerator of moving the player
    [HideInInspector] public Coroutine movePlayer;

    [Tooltip("the time between moving steps")]
    [SerializeField] private float stepTime = 0.3f;
    
    
    //Get the player collision component so i can get the current tile position
    private PlayerCollision _playerCollision;

    public static UnityAction onPlayerFinishedMove = delegate { };

    #endregion


    #region Monobehaviour
    
    void Start()
    {
        _playerCollision = GetComponent<PlayerCollision>();
        

        //Set the initial state of the player
        currentState = PlayerState.STOPPING;
    }


    /// <summary>
    /// Moving the player when the user press on his pawn by mouse
    /// </summary>
    private void OnMouseDown()
    {
        //Check if it is his turn
        if (index == GameManager.Instance.currentTurn)
        {
            //Make sure that he has already the dice rolled
            if (GameManager.Instance.diceNum > 0)
                movePlayer = StartCoroutine(MovePlayer(GameManager.Instance.diceNum));
        }

    }

    #endregion


    #region Methods

    /// <summary>
    /// THis is called to Move the player on the board depends on its current position and number of the rolled dice 
    /// </summary>
    /// <param name="numOfSteps"></param>
    /// <returns></returns>
    IEnumerator MovePlayer(int numOfSteps)
    {
        //Change the state to moving state so he can't interact with special tiles until he stopped
        currentState = PlayerState.ISMOVING;
        
        var tempPos = numOfSteps;

        //Moving player depends on number of rolled dice
        for (int i = 0; i < numOfSteps; i++)
        {
            //Here Check for the last 2 rows so To move the player to the finished tile if he has the correct number of steps
            if (_playerCollision.currentTilePos.y == GameManager.Instance.numOfRows - 1 || _playerCollision.currentTilePos.y == GameManager.Instance.numOfRows - 2)
            {
                if (((_playerCollision.currentTilePos.x + (GameManager.Instance.numOfRows - (_playerCollision.currentTilePos.y + 1)) * tempPos) - tempPos) >= 0)
                {
                    tempPos--;
                }
                else
                {
                    if (!GameManager.Instance.isFullDice)
                    {
                        //Go to next player turn
                        GameManager.Instance.IncreaseCurrentTurn();
                    }
                    
                    //Tell the dice manager that i had finished moving
                    GameManager.Instance.diceNum = 0;
                    onPlayerFinishedMove.Invoke();

                    currentState = PlayerState.STOPPING;

                    yield break;
                }

            }

            //This is for even rows steps
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
                //This is for odd rows steps
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
            //Go to next player turn
            GameManager.Instance.IncreaseCurrentTurn();
        }

        //Tell the dice manager that i had finished moving
        GameManager.Instance.diceNum = 0;
        onPlayerFinishedMove.Invoke();


        //change the player state to stopping when finished moving
        currentState = PlayerState.STOPPING;
    }


    /// <summary>
    /// Moving physics using Translate function also change the parent of current tile 
    /// </summary>
    /// <param name="moveDir"></param>
    public void MoveMechanic(Vector2 moveDir)
    {

        transform.Translate(new Vector3(moveDir.x * GameManager.Instance.tileSize, 0, moveDir.y * GameManager.Instance.tileSize));
        gameObject.transform.parent = _playerCollision.currentTile.transform;

        //Add Sound and Simple Particle effect
        audioManager.PlaySound(moveSound);
        
    }

    #endregion

}
