using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Board))]
public class BoardManager : MonoBehaviour
{
    public const int WINNUM = 15;
    public GameObject mainMenu;
    //public GameObject boardPrefab;
    public Transform canvasTransform;

	public Player currentPlayer = Player.PLAYER1;
    public GameType gameType;

    public Block selectedBlock { get; set; }
	public Loader assetLoader;

	public Sprite cross,zero;
	public Sprite currentSprite{ get;private set;}

    private Board board;
    private int turns;
	private AI ai;
	private string winner;

    void Awake()
    {
        board = GetComponent<Board>();
		ai = new AI (board);
		winner = string.Empty;
		if (!assetLoader)
			assetLoader = FindObjectOfType (typeof(Loader)) as Loader;
        if (canvasTransform == null)
        {
            Debug.LogError("Please assign boardPrefab or canvasTransform");
            return;
        }
    }

    public void InitializeGame(GameType type)
    {
		winner = string.Empty;
        gameType = type;
		if(assetLoader.prefab != null)
			board.CreateBoard(assetLoader.prefab,canvasTransform);
        board.SetBlocks(this);
    }


    public void OnClickBlockNonAI()
    {
        turns++;

        // set block type and get its value
		switch (currentPlayer)
		{
			case Player.PLAYER1:
				selectedBlock.type = BlockType.CROSS;
				currentSprite = cross;
				break;
					
			case Player.PLAYER2:
				selectedBlock.type = BlockType.ZERO;
				currentSprite = zero;
				break;
		}
		
		selectedBlock.image.sprite = currentSprite;
		selectedBlock.button.interactable = false;
        // check if win condition is reached break if so and end game

        if (CheckWin())
            return;

        // changeplayer
		ChangePlayer();

        // no empty block left restart game or quit
		if (turns == board.boardSizeX * board.boardSizeY)
		{
			StartCoroutine (Quit ());
			winner = "Draw";
		}

    }

	public void OnClickBlockAI()
	{
		if (turns ==  board.boardSizeX * board.boardSizeY)
			return;

		ai.usedBlock.Add (selectedBlock);
		ai.playerMoves.Add (selectedBlock);

		ai.moves.Clear ();

		ai.AIMove ();
		//Debug.Log (ai.moves [0].value);
		if (ai.moves.Count > 1)
		{
			selectedBlock = ai.moves [Random.Range (0, ai.moves.Count)];
		} else
		{
			selectedBlock = ai.moves [0];
		}
		turns++;
		selectedBlock.type = BlockType.ZERO;
		currentSprite = zero;
		selectedBlock.image.sprite = currentSprite;
		selectedBlock.button.interactable = false;
		ai.usedBlock.Add (selectedBlock);
		ai.aiMoves.Add (selectedBlock);
		if (CheckWin ())
			return;
		ChangePlayer ();

		if (turns == board.boardSizeX * board.boardSizeY)
		{
			StartCoroutine (Quit ());
			winner = "Draw";
		}


	}

    private bool CheckWin()
    {
        for (int i = 0; i < board.boardSizeY; i++)
        {
            if (board.SumRow(i, selectedBlock.type) == WINNUM)
            {
				StartCoroutine (Quit ());
                return true;
            }

            if (board.SumColumn(i, selectedBlock.type) == WINNUM)
            {
				StartCoroutine (Quit ());
                return true;
            }

        }
        if (board.SumDiagonalA(selectedBlock.type) == WINNUM)
        {
			StartCoroutine (Quit ());
            return true;
        }

        if (board.SumDiagonalB(selectedBlock.type) == WINNUM)
        {
			StartCoroutine (Quit ());
            return true;
        }

        return false;
    }
		
	private IEnumerator Quit()
    {
		board.Deactivate ();
		winner = currentPlayer.ToString ();
		yield return new WaitForSeconds (5);
        board.DestroyBoard();
        OnGameOver();
		turns = 0;
		currentPlayer = Player.PLAYER1;
		ai.ClearAll ();
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
		if(winner != string.Empty)
			GUILayout.Label("Winner : " + winner);
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
