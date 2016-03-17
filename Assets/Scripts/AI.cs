using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AI 
{
	public const int MINVALUE = 0;
	public const int MAXVALUE = 9;

	public List<Block> playerMoves = new List<Block>();
	public List<Block> aiMoves = new List<Block>();
	public List<Block> usedBlock = new List<Block>();

	public List<Block> moves = new List<Block>();

	public Board board;

	public AI(Board board)
	{
		this.board = board;
	}

	public void ClearAll()
	{
		moves.Clear ();
		usedBlock.Clear ();
		aiMoves.Clear ();
		playerMoves.Clear ();
	}

	public void AIMove()
	{
		if (usedBlock.Count > 2)
		{
			// check if can win
			for (int i = 0; i < aiMoves.Count; i++)
			{
				foreach (Block bl in aiMoves)
				{
					if (bl != aiMoves [i])
					{
						int d = BoardManager.WINNUM - (bl.value + aiMoves [i].value);
						if(d > MINVALUE && d <= MAXVALUE)
						{
							if(!usedBlock.Contains(board.FindBlock(d)))
								moves.Add (board.FindBlock (d));
						}
					}
				}
			}

			// check if can block
			if (moves.Count == 0)
			{
				for (int i = 0; i < playerMoves.Count; i++)
				{
					foreach (Block bl in playerMoves)
					{
						if (bl != playerMoves [i])
						{
							int d = BoardManager.WINNUM - (bl.value + playerMoves [i].value);
							if(d > MINVALUE && d <= MAXVALUE)
							{
								if(!usedBlock.Contains(board.FindBlock(d)))
									moves.Add (board.FindBlock (d));
							}
						}
					}
				}
			}

			if (moves.Count == 0)
				PickRandomMove ();
			
		} else
		{
			// select random block
			PickRandomMove();
		}


	}

	void PickRandomMove()
	{
		if (!usedBlock.Contains (board.GetBlock (1, 1)))
		{
			moves.Add (board.GetBlock (1, 1));
			return;
		}

		int z = Random.Range (0, 4);

		if (!usedBlock.Contains (board.GetBlock (0, 2)))
		{
			moves.Add (board.GetBlock (0, 2));
			return;
		}

		if (!usedBlock.Contains (board.GetBlock (0, 0)))
		{
			moves.Add (board.GetBlock (0, 0));
			return;
		}

		if (!usedBlock.Contains (board.GetBlock (2, 2)))
		{
			moves.Add (board.GetBlock (2, 2));
			return;
		}

		if (!usedBlock.Contains (board.GetBlock (2, 0)))
		{
			moves.Add (board.GetBlock (2, 0));
			return;
		}

		if (!usedBlock.Contains (board.GetBlock (0, 1)))
		{
			moves.Add (board.GetBlock (0, 1));
			return;
		}

		if (!usedBlock.Contains (board.GetBlock (1, 0)))
		{
			moves.Add (board.GetBlock (1, 0));
			return;
		}

		if (!usedBlock.Contains (board.GetBlock (1, 2)))
		{
			moves.Add (board.GetBlock (1, 2));
			return;
		}

		if (!usedBlock.Contains (board.GetBlock (2, 1)))
		{
			moves.Add (board.GetBlock (2, 1));
			return;
		}
	}
		
}
