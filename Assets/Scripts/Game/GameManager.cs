using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI infoTxt;
    public GameObject resultPanel;
    public List<TextMeshProUGUI> ranktxts = new List<TextMeshProUGUI>();
    public List<Image> rankColors = new List<Image>();

    [SerializeField] List<PlayerMovement> players = new List<PlayerMovement>(); //get the material to make special effect on it
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


    private void Start()
    {
        //I have changed the exceution order in Project Settings of scripts so this is called after instantiating the map 
        players = FindObjectsOfType<PlayerMovement>().ToList();   //Flip for List  
        players.Reverse();

    }

    #endregion

    #region Methods
    private void SetDiceNum(int num, bool _isFullDice)
    {
        diceNum = num;
        isFullDice = _isFullDice;

        ModifyCurrentTurnPlayer();
    }

    /// <summary>
    /// Use this method to modify or add Effects to the current player pawn
    /// </summary>
    private void ModifyCurrentTurnPlayer()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (currentTurn == players[i].index)
            {
                var mat = players[i].transform.GetChild(0).gameObject.GetComponent<Material>();
                //Make Flasher on the material

                //mat.SetColor("_EmissionColor", Color.red);
            }
        }
    }

    public void IncreaseCurrentTurn()
    {
        currentTurn++;
        if (currentTurn > 3)
        {
            currentTurn = 0;
        }

        CheckTheStateOFPlayer();

        statusTxt.text = "Player " + (currentTurn + 1) + " Turn";
    }

    private void CheckTheStateOFPlayer()
    {
        if (players[currentTurn].currentState == PlayerMovement.PlayerState.FINISHED)
        {
            IncreaseCurrentTurn();
        }

    }
    #endregion

    #region UIMethods

    public void OnRestartPressed(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnHomePressed(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowResultPanel(List<PlayerMovement> players)
    {
        resultPanel.SetActive(true);

        for (int i = 0; i < ranktxts.Count; i++)
        {
            ranktxts[i].text = "Player " + (players[i].index + 1);
            rankColors[i].color = players[i].color;
        }
    }
    #endregion
}
