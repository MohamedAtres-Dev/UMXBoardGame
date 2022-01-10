using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DiceManager : MonoBehaviour
{

    #region Variables
    [SerializeField] Image image;
    [SerializeField] GameObject skipButton;
    [SerializeField] DiceItem[] diceItems;

    private int lastDice;

    private int currentNumOfSteps = 0;
    private int previousNumOfSteps = 0; //use this if the player press skip so 
    private int numOfFullDice = 0;
    private int numOfSkips = 0;
    bool shuffleInProgress = false;
    bool canPlayDice = true;

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
    public void PlayDice()
    {
        if (!canPlayDice)
            return;

        if (shuffleInProgress)
            return;

        currentNumOfSteps = 0;
        StartCoroutine(iplaydice());
    }

    IEnumerator iplaydice()
    {
        shuffleInProgress = true;
        yield return new WaitForSeconds(0);
        //image.transform.localScale = Vector3.one;
        image.gameObject.SetActive(true);
        for (int i = 0; i < 30; i++)
        {
            lastDice = Random.Range(0, 6);
            image.sprite = diceItems[lastDice].image;
          

            yield return new WaitForSeconds(.01f);
        }

        currentNumOfSteps = diceItems[lastDice].step + previousNumOfSteps;

        if(diceItems[lastDice].step == 6 && numOfFullDice < 1)
        {
            Debug.Log("Player First Six " );
            onSendDiceNum.Invoke(diceItems[lastDice].step , true);
            numOfFullDice++;
            canPlayDice = false;
        }
        else
        {
            
            if (diceItems[lastDice].step == 6)  //The Player will lose his turn 
            {
                Debug.Log("Player Second Six ");
                yield return new WaitForSeconds(0.4f);
                image.sprite = diceItems[0].image;
                Debug.Log("Player lose this turn " );
                GameManager.Instance.IncreaseCurrentTurn();
                onSendDiceNum.Invoke(0, false);
                numOfFullDice = 0;
                NextTurn();
            }
            else
            {
                if (numOfSkips < 1)
                {
                    
                    skipButton.SetActive(true);           
                    previousNumOfSteps = currentNumOfSteps;
                    numOfSkips++;
                }
                Debug.Log("Player Can Skip this turn " + currentNumOfSteps);
                numOfFullDice = 0;
                onSendDiceNum.Invoke(currentNumOfSteps, false);
                canPlayDice = false;
            }

        }

           
        shuffleInProgress = false;

    }

    public void OnSkipPressed()
    {
        canPlayDice = true;
        PlayDice();
    }

    private void NextTurn()
    {
        Debug.Log("Next Turn ");
        skipButton.SetActive(false);
        canPlayDice = true; 
        numOfSkips = 0;
        previousNumOfSteps = 0;
        image.sprite = diceItems[0].image;
    }

    #endregion

}

[System.Serializable]
public class DiceItem
{
    public Sprite image;
    public int step;
}
