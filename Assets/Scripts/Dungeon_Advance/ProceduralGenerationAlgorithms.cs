using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int walkLength)		// Haset have no duplicate
	{
		HashSet<Vector2Int> path = new HashSet<Vector2Int>();
		path.Add(startPos);

		var previousPos = startPos;

		for (int i = 0; i < walkLength; i++)
		{
			var newPos = previousPos + Direction2D.GetRandomCardinalDirection();				// get a new random pos from the previous pos
			path.Add(newPos);
			previousPos = newPos;
		}

		return path;
	}

	public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int corridorLenght)		// List to keep access to the last element
	{
		List<Vector2Int> corridor = new List<Vector2Int>();
		var direction = Direction2D.GetRandomCardinalDirection();
		var currentPos = startPos;
		corridor.Add(currentPos);																	// new start pos AKA the end pos of last corridor
		for (int i = 0; i < corridorLenght; i++)
		{
			currentPos += direction;
			corridor.Add(currentPos);
		}

		return corridor;
	}

	public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
	{
		Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();						// Boundint is a box
		List<BoundsInt> roomsList = new List<BoundsInt>();
		roomsQueue.Enqueue(spaceToSplit);
		while(roomsQueue.Count > 0)													// count will increase as more room can be split added
		{
			var room = roomsQueue.Dequeue();										// remove the room that being split (it gonna beceome 2 different room)
			if(room.size.y >= minHeight && room.size.x >= minWidth)
			{
				if(Random.value < 0.5)												// random value return random 0-1
				{
					if(room.size.y >= minHeight * 2)								// if the room can split in 2 and still have space (Horizontal)
					{
						SplitHorizontally(minHeight, roomsQueue, room);
					}
					else if(room.size.x >= minWidth * 2)							// if the room can split in 2 and still have space (Vertical)
					{
						SplitVertically(minWidth, roomsQueue, room);
					}
					else if(room.size.x >= minWidth && room.size.y >= minHeight)	// cant no longer be split but contain a room
					{
						roomsList.Add(room);										// save the spar space as a room
					}
				}
				else																// Same step but check Vertical first create more randomize result
				{
					if (room.size.x >= minWidth * 2)        
					{
						SplitVertically(minWidth, roomsQueue, room);
					}
					else if (room.size.y >= minHeight * 2)       
					{
						SplitHorizontally(minHeight, roomsQueue, room);
					}
					else if (room.size.x >= minWidth && room.size.y >= minHeight)   
					{
						roomsList.Add(room);
					}
				}
			}
		}

		return roomsList;
	}

	private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
	{
		var xSplit = Random.Range(1, room.size.x);															// slipt verically random in the middle but not at borders
		BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
		BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),		// new min corner is where the xsplit is
			new Vector3Int(room.size.x - xSplit, room.size.y, room.size.z));                                // new room size is the whole room minus the xsplit

		roomsQueue.Enqueue(room1);
		roomsQueue.Enqueue(room2);

	}

	private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
	{
		var ySplit = Random.Range(1, room.size.y);															// slipt horizontally random in the middle but not at borders
		BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
		BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),		// new min corner is where the ysplit is
			new Vector3Int(room.size.x, room.size.y - ySplit, room.size.z));                                // new room size is the whole room minus the ysplit

		roomsQueue.Enqueue(room1);
		roomsQueue.Enqueue(room2);
	}
}

public static class Direction2D
{
	public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>
	{
		new Vector2Int(0,1),	// UP
		new Vector2Int(1,0),	// RIGHT
		new Vector2Int(0,-1),	// DOWN
		new Vector2Int(-1,0)	// LEFT
	};

	public static List<Vector2Int> diagonalDirectionList = new List<Vector2Int>
	{
		new Vector2Int(1,1),	// UP - RIGHT
		new Vector2Int(1,-1),	// RIGHT - DOWN
		new Vector2Int(-1,-1),	// DOWN - LEFT
		new Vector2Int(-1,1)	// LEFT - UP
	};

	public static List<Vector2Int> eightDirectionList = new List<Vector2Int>
	{
		new Vector2Int(0,1),	// UP
		new Vector2Int(1,1),	// UP - RIGHT
		new Vector2Int(1,0),	// RIGHT
		new Vector2Int(1,-1),	// RIGHT - DOWN
		new Vector2Int(0,-1),	// DOWN
		new Vector2Int(-1,-1),	// DOWN - LEFT
		new Vector2Int(-1,0),	// LEFT
		new Vector2Int(-1, 1)	// LEFT - UP
	};

	public static Vector2Int GetRandomCardinalDirection()
	{
		return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
	}
}
