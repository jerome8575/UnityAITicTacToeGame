using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    public Text[] buttonList;
    public GameObject gameOverPanel;
    public Text gameOverText;
    public GameObject restartButton;
    public GameObject introPanel;
    public GameObject AIplayButton;
    public GameObject twoPlayerButton;
    public bool isTwoPlayerGameMode;

    private string playerSide;
    private int moveCount;
    private string[,] board = new string[3, 3];
    private bool isTie;

    struct Move
    {
        public int row;
        public int col;
    }
    public void RemoveIntro(bool isTwoPlayer)
    {
        introPanel.SetActive(false);
        isTwoPlayerGameMode = isTwoPlayer;
    }

    public void restartGame()
    {
        playerSide = "X";
        gameOverPanel.SetActive(false);
        moveCount = 0;
        setBoardToInteractable(true);
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }
        restartButton.SetActive(false);
    }
    public void setMyArray()
    {
        int index = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = buttonList[index].text;
                index++;
            }
        }
    }
    public void makeAIPlay()
    {
        Move Aimove = findBestMove();
        int buttonlistindex = Aimove.col + Aimove.row * 3;
        buttonList[buttonlistindex].GetComponentInParent<Button>().interactable = false;
        buttonList[buttonlistindex].text = "O";
        EndTurn();
    }
    public string getPlayerSide()
    {
        return playerSide;
    }
    public void EndTurn()
    {
        moveCount++;
        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver();
        }

        if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver();
        }

        if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver();
        }

        if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver();
        }

        if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver();
        }

        if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver();
        }

        if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver();
        }

        if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver();
        }
        if (moveCount == 9)
        {
            isTie = true;
            GameOver();
        }
        changeSides();
    }
    int checkWin()
    {
        for (int row = 0; row < 3; row++)
        {
            if (board[row, 0] == board[row, 1] && board[row, 1] == board[row, 2])
            {
                if (board[row, 0] == "O")
                    return 10;
                else if (board[row, 0] == "X")
                    return -10;
            }
        }
        for (int col = 0; col < 3; col++)
        {
            if (board[0, col] == board[1, col] && board[1, col] == board[2, col])
            {
                if (board[0, col] == "O")
                    return 10;
                else if (board[0, col] == "X")
                    return -10;
            }
        }
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == "O")
                return 10;
            else if (board[0, 0] == "X")
                return -10;
        }
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == "O")
                return 10;
            else if (board[0, 2] == "X")
                return -10;
        }
        return 0;
    }
    bool isFull()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (board[i, j] == "")
                    return false;
        return true;
    }
    int minimax(int depth, bool isMax)
    {

        int score = checkWin();

        if (score == 10)
            return score;
        if (score == -10)
            return score;
        if (isFull())
            return 0;

        if (isMax)
        {
            int best = -1000;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                    {
                        board[i, j] = "O";
                        best = Math.Max(best, minimax(depth + 1, false));
                        board[i, j] = "";
                    }
                }
            }
            return best;
        }
        else
        {
            int best = 1000;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                    {
                        board[i, j] = "X";
                        best = Math.Min(best, minimax(depth + 1, true));
                        board[i, j] = "";
                    }
                }
            }
            return best;
        }
    }
    Move findBestMove()
    {
        int bestVal = -1000;
        Move bestMove;
        bestMove.row = -1;
        bestMove.col = -1;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == "")
                {
                    board[i, j] = "O";

                    int moveValue = minimax(0, false);
                    board[i, j] = "";
                    if (moveValue > bestVal)
                    {
                        bestMove.row = i;
                        bestMove.col = j;
                        bestVal = moveValue;
                    }

                }
            }
        }
        return bestMove;
    }
    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }
    void Awake()
    {
        SetGameControllerReferenceOnButtons();
        playerSide = "X";
        gameOverPanel.SetActive(false);
        moveCount = 0;
        restartButton.SetActive(false);
    }
    void GameOver()
    {
        setBoardToInteractable(false);
        gameOverPanel.SetActive(true);
        if (isTie)
        {
            gameOverText.text = "Tie Game!";
        }
        else
        {
            gameOverText.text = playerSide + " Wins!";
        }
        restartButton.SetActive(true);
    }
    void changeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
    }
    void setBoardToInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }
}
