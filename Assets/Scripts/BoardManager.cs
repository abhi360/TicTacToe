using UnityEngine;

[RequireComponent(typeof(Board))]
public class BoardManager : MonoBehaviour
{
    public const int WINNUM = 15;
    public GameObject mainMenu;
    public GameObject boardPrefab;
    public Transform canvasTransform;

    public Player currentPlayer;
    public GameType gameType;

    public Block selectedBlock { get; set; }


    private Board board;
    private int turns;

    void Awake()
    {
        board = GetComponent<Board>();
        if (canvasTransform == null || boardPrefab == null)
        {
            Debug.LogError("Please assign boardPrefab or canvasTransform");
            return;
        }
    }

    public void InitializeGame(GameType type)
    {
        gameType = type;
        board.CreateBoard(boardPrefab,canvasTransform);
        board.SetBlocks(this);
    }


    public void OnClickBlock()
    {
        turns++;

        // set block type and get its value
        switch (currentPlayer)
        {
            case Player.PLAYER1:
                selectedBlock.type = BlockType.CROSS;
                break;

            case Player.PLAYER2:
                selectedBlock.type = BlockType.ZERO;
                break;
        }

        // check if win condition is reached break if so and end game

        if (CheckWin())
            return;

        // changeplayer
        ChangePlayer();

        // no empty block left restart game or quit
        if (turns == board.boardSizeX * board.boardSizeY)
            Quit();

    }

    bool CheckWin()
    {
        for (int i = 0; i < board.boardSizeY; i++)
        {
            if (board.SumRow(i, selectedBlock.type) == WINNUM)
            {
                Quit();
                return true;
            }

            if (board.SumColumn(i, selectedBlock.type) == WINNUM)
            {
                Quit();
                return true;
            }

        }
        if (board.SumDiagonalA(selectedBlock.type) == WINNUM)
        {
            Quit();
            return true;
        }

        if (board.SumDiagonalB(selectedBlock.type) == WINNUM)
        {
            Quit();
            return true;
        }

        return false;
    }

    void Quit()
    {
        board.DestroyBoard();
        OnGameOver();
    }

    private void ChangePlayer()
    {
        if (gameType == GameType.TWO)
        {
            if (currentPlayer == Player.PLAYER1)
                currentPlayer = Player.PLAYER2;
            else
                currentPlayer = Player.PLAYER1;
        }
        else
        {
            if (currentPlayer == Player.PLAYER1)
                currentPlayer = Player.AI;
            else
                currentPlayer = Player.PLAYER1;
        }
    }


    public void OnClickSinglePlayer()
    {
        mainMenu.SetActive(false);
        InitializeGame(GameType.SINGLE);
    }

    public void OnClickTwoPlayer()
    {
        mainMenu.SetActive(false);
        InitializeGame(GameType.TWO);
    }

    private void OnGameOver()
    {
        mainMenu.SetActive(true);
    }

    void OnGUI()
    {
        GUILayout.Label(currentPlayer.ToString());
    }

}

public enum Player
{
    PLAYER1,
    PLAYER2,
    AI
}

public enum GameType
{
    SINGLE,
    TWO
}
