using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;


/// <summary>
/// THis is the main connector and Manager to the flow of the game 
/// </summary>
public class GameManager : Singlton<GameManager>
{
    #region Variables
    [SerializeField] private AudioManager _audioManager = default;

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
        //I get this list to make special effects on the current player 
        players = FindObjectsOfType<PlayerMovement>().ToList();    
        players.Reverse(); //reverse the list as it order by acending 


        //GEt the audio source and assign it to Audio manager so i can play sounds from any script 
        _audioManager.Audio = GetComponent<AudioSource>();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Listener to finished rolling dice to get the final number of steps also is it has number 6 or not
    /// </summary>
    /// <param name="num"></param>
    /// <param name="_isFullDice"></param>
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
                var mat = players[i].transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
                
                //Make Flasher on the material
                StartCoroutine(FlashTheCurrentPlayer(mat, i));
            }
        }
    }

    IEnumerator FlashTheCurrentPlayer(Material playerMAt , int index)
    {
        for (int i = 0; i < 3; i++)
        {
            playerMAt.SetColor("_EmissionColor", Color.white);
            
            yield return new WaitForSeconds(0.1f);
            playerMAt.SetColor("_EmissionColor", players[index].color);
            yield return new WaitForSeconds(0.1f);
        }


    }


    /// <summary>
    /// Go to Next Player turn
    /// </summary>
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


    /// <summary>
    /// Check if the current player is finished so I can skip his turn
    /// </summary>
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
