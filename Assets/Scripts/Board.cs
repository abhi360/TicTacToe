using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject gameBoard { get; private set; }
    public Block[,] blocks { get; private set; }

    private RectTransform gameBoardRect;
    public int boardSizeX, boardSizeY;

    private int[] magicSquareArr = new int[] { 4, 9, 2, 3, 5, 7, 8, 1, 6 };
    
    /// <summary>
    /// This function thus can create any ui based board that is set as a prefab
    /// giving added flexibilty to create further different types of boards
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="canvas"></param>
    public void CreateBoard(GameObject obj, Transform canvas)
    {
        gameBoard = Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
        gameBoard.transform.SetParent(canvas);
        gameBoardRect = gameBoard.GetComponent<RectTransform>();
        gameBoardRect.sizeDelta = Vector2.zero;
        gameBoardRect.localPosition = Vector3.zero;
        gameBoardRect.localScale = Vector3.one;
        boardSizeX = (int)Mathf.Sqrt(gameBoardRect.childCount);
        boardSizeY = boardSizeX;
    }

    public void SetBlocks(BoardManager manager)
    {
        blocks = new Block[boardSizeY, boardSizeX];

        for (int i = 0, k = 0; i < boardSizeY; i++)
        {
            for (int j = 0; j < boardSizeX; j++,k++)
            {
                GameObject obj = gameBoardRect.GetChild(k).gameObject;
                blocks[i, j] = obj.AddComponent<Block>();
                blocks[i, j].value = magicSquareArr[k];
                blocks[i, j].boardManager = manager;
            }
        }
    }

    public int SumRow(int rowNum,BlockType type)
    {
        int sum = 0;
        for (int i = 0; i < boardSizeX; i++)
        {
            if(blocks[rowNum,i].type == type)
                sum += blocks[rowNum, i].value;
        }

        return sum;
    }

    public int SumColumn(int colNum,BlockType type)
    {
        int sum = 0;
        for (int i = 0; i < boardSizeY; i++)
        {
            if (blocks[i, colNum].type == type)
                sum += blocks[i, colNum].value;
        }

        return sum;
    }

    public int SumDiagonalA(BlockType type)
    {
        int sum = 0;
        for (int i = 0, j = 0; i < boardSizeY; i++,j++)
        {
            if (blocks[i, j].type == type)
                sum += blocks[i, j].value;
        }

        return sum;
    }

    public int SumDiagonalB(BlockType type)
    {
        int sum = 0;
        for (int i = 0, j = boardSizeX - 1; i < boardSizeY; i++, j--)
        {
            if (blocks[i, j].type == type)
                sum += blocks[i, j].value;
        }
        return sum;
    }

    public void DestroyBoard()
    {
        Destroy(gameBoard);
    }
    
}
