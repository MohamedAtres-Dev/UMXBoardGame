using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


/// <summary>
/// This Script is responsible for rolling the dice and send the number to the game manager
/// </summary>
public class DiceManager : MonoBehaviour
{

    #region Variables
    [Tooltip("Place holder of Dice image")]
    [SerializeField] Image image;

    [Tooltip("skip Button Game object")]
    [SerializeField] GameObject skipButton;

    [Tooltip("Dice items which each item contain the sprite and its value")]
    [SerializeField] DiceItem[] diceItems;
    

    [Space(10f)]
    [Header("Audio Effects")]
    [SerializeField] private AudioManager _audioManager = default;
    [SerializeField] AudioClip rollDiceSound;



    private int lastDice; //The final index of stoping dice

    private int currentNumOfSteps = 0;  //use this to save the numbers of dice 
    private int previousNumOfSteps = 0; //use this if the player press skip so i can add them to the current one 
    private int numOfFullDice = 0;  //use this to check if the player got 2 ( 6 ) in a row
    private int numOfSkips = 0;     //use this to make skip once 
    bool shuffleInProgress = false;  //use this to not interuppet the process of rolling the dice 
    bool canPlayDice = true;  //use this variable to not play dice while the player moving 

    public static UnityAction<int , bool> onSendDiceNum = delegate { };
    #endregion

    #region Monobehaviour
    private void OnEnable()
    {
        PlayerMovement.onPlayerFinishedMove += NextTurn;
    }

    private void OnDisable()
    {
        PlayerMovement.onPlayerFinishedMove -= NextTurn;
    }

    #endregion

    #region Methods
    /// <summary>
    /// Call this function from the Image of the dice to start rolling
    /// </summary>
    public void PlayDice()
    {
        if (!canPlayDice)
            return;

        if (shuffleInProgress)
            return;

        //restart the value of current dice number 
        currentNumOfSteps = 0;

        StartCoroutine(IPlayDice());

        _audioManager.PlaySound(rollDiceSound);
    }

    /// <summary>
    /// IEnumerator of rolling dice 
    /// </summary>
    /// <returns></returns>
    IEnumerator IPlayDice()
    {
        shuffleInProgress = true;
        yield return new WaitForSeconds(0);
        
        //Rolling the dice 
        for (int i = 0; i < 20; i++)
        {
            lastDice = Random.Range(0, 6);
            image.sprite = diceItems[lastDice].image;
          

            yield return new WaitForSeconds(.04f);
        }

        currentNumOfSteps = diceItems[lastDice].step + previousNumOfSteps;


        //Check if this is first 6 
        if(diceItems[lastDice].step == 6 && numOfFullDice < 1)
        {
            
            onSendDiceNum.Invoke(diceItems[lastDice].step , true);
            numOfFullDice++;
            canPlayDice = false;
        }
        else
        {
            
            if (diceItems[lastDice].step == 6)  //The Player will lose his turn as this is the second 6 in a row 
            {
                yield return new WaitForSeconds(0.4f);

                //reset the default sprite of the dice 
                image.sprite = diceItems[0].image;
                
                //Go to next player turn
                GameManager.Instance.IncreaseCurrentTurn();
                
                onSendDiceNum.Invoke(0, false);
                numOfFullDice = 0;
                
                //Call this function to reset some values for the next turn
                NextTurn();
            }
            else
            {

                //here is the condition of the skipping state
                if (numOfSkips < 1)
                {
                    
                    skipButton.SetActive(true);           
                    previousNumOfSteps = currentNumOfSteps;
                    numOfSkips++;
                }
                
                numOfFullDice = 0;
                onSendDiceNum.Invoke(currentNumOfSteps, false);
                canPlayDice = false;
            }

        }

           
        shuffleInProgress = false;

    }

    /// <summary>
    /// Skip button pressed so i can roll the dice again
    /// </summary>
    public void OnSkipPressed()
    {
        canPlayDice = true;
        PlayDice();
    }


    /// <summary>
    /// Reset some values to prepare for next turn
    /// </summary>
    private void NextTurn()
    {
        
        skipButton.SetActive(false);
        canPlayDice = true; 
        numOfSkips = 0;
        previousNumOfSteps = 0;
        image.sprite = diceItems[0].image;
    }

    #endregion

}

/// <summary>
/// Make a dice item to hold the sprite and its value
/// </summary>
[System.Serializable]
public class DiceItem
{
    public Sprite image;
    public int step;
}
