using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    private GameController gameController;
    private int index;

    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }
    public void SetSpace()
    {
        if (gameController.isTwoPlayerGameMode)
        {
            buttonText.text = gameController.getPlayerSide();
            gameController.EndTurn();
        }
        else
        {
            buttonText.text = "X";
            gameController.setMyArray();
            gameController.EndTurn();
            if (gameController.getPlayerSide() == "O")
            {
                gameController.makeAIPlay();
            }
        }
        button.interactable = false;
    }
   
}
