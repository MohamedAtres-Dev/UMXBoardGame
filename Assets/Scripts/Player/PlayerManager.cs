using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerManager : MonoBehaviour
{

    #region Variables
    public int index;

    Coroutine movePlayer;

    [SerializeField] private float stepTime = 0.2f;

    private int diceNum;
    public static UnityAction onPlayerFinishedMove = delegate { };


    #endregion


    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
        Debug.Log("Player " + numOfSteps + " Number Of Steps");
        yield return new WaitForSeconds(numOfSteps * stepTime);

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

    #endregion

}
