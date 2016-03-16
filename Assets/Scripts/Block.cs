using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour,IPointerClickHandler
{
    public int value;

    public BlockType type = BlockType.EMPTY;

    public BoardManager boardManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        boardManager.selectedBlock = this;
        boardManager.OnClickBlock();
        if (type == BlockType.CROSS)
            GetComponent<Image>().color = Color.green;
        else
            GetComponent<Image>().color = Color.blue;
        GetComponent<Button>().interactable = false;
    }
}

public enum BlockType
{
    EMPTY,
    CROSS,
    ZERO
}
