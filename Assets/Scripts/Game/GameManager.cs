using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : Singlton<GameManager>
{
    #region Variables

    public int numOfRows;
    public int numOfCols;
    public float tileSize;


    [HideInInspector] public int currentTurn;
    [HideInInspector] public int diceNum;
    [HideInInspector] public bool isFullDice;


    //UI
    [Space(5f)]
    [Header("UI Elements")]
    public TextMeshProUGUI statusTxt;
    public GameObject resultPanel;
    public List<TextMeshProUGUI> ranktxts = new List<TextMeshProUGUI>();
    public List<Image> rankColors = new List<Image>();

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


    public void ShowResultPanel(List<PlayerMovement> players)
    {
        resultPanel.SetActive(true);

        for (int i = 0; i < ranktxts.Count; i++)
        {
            ranktxts[i].text = "Player " + players[i].index;
            rankColors[i].color = players[i].color;
        }
    }
    #endregion
}
