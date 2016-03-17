using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour,IPointerClickHandler
{
    public int value;
	public Button button;
    public BlockType type = BlockType.EMPTY;

	public Image image;
    public BoardManager boardManager;


	void Start()
	{
		image = GetComponent<Image> ();
		button = GetComponent<Button>();
	}

    public void OnPointerClick(PointerEventData eventData)
    {
		if (!button.IsInteractable ())
			return;

		if (boardManager.gameType == GameType.TWO)
		{
			boardManager.selectedBlock = this;
			boardManager.OnClickBlockNonAI ();

		} else
		{
			boardManager.selectedBlock = this;
			boardManager.OnClickBlockNonAI ();

			boardManager.OnClickBlockAI ();
		}
    }
		
}

public enum BlockType
{
    EMPTY,
    CROSS,
    ZERO
}
