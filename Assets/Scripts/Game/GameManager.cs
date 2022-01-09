using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singlton<GameManager>
{
    #region Variables
    [HideInInspector] public int currentTurn;
    [HideInInspector] public int diceNum;
    [HideInInspector] public bool isFullDice;

    public TextMeshProUGUI statusTxt;

    [SerializeField] List<GameObject> players = new List<GameObject>(); //get the material to make special effect on it
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        DiceManager.onSendDiceNum += SetDiceNum;
    }

    private void OnDisable()
    {
        DiceManager.onSendDiceNum -= SetDiceNum;
    }

    #endregion

    #region Methods
    private void SetDiceNum(int num , bool _isFullDice)
    {

        diceNum = num;
        isFullDice = _isFullDice;
    }
    #endregion
}
