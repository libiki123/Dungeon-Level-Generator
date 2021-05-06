using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField] private int dungeonWidth = 20, dungeonHeight = 20;
    [Range(0,10)]
    [SerializeField] private int offset = 1;
    [SerializeField] private bool randomWalkRooms = false;

	protected override void RunProcedualGeneration()
	{
        CreateRooms();

	}

	private void CreateRooms()
	{
		var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPos, new Vector3Int(dungeonWidth, dungeonHeight, 0)),
																				minRoomWidth, minRoomHeight);	// divide the dungeon into multiple rooms
		HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

		if (randomWalkRooms)
		{
			floor = CreateRoomsRandomly(roomsList);                                                             // create rooms with randomwalk using the divided rooms provided
		}
		else
		{
			floor = CreateSimpleRooms(roomsList);																// create simple rooms using the divided rooms provided
		}

		List<Vector2Int> roomCenters = new List<Vector2Int>();
		foreach (var room in roomsList)
		{
			roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));									// get the center of each room and add it to roomCenters
		}

		HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
		floor.UnionWith(corridors);

		tilemapVisualizer.PaintFloorTile(floor);
		WallGenerator.CreateWall(floor, tilemapVisualizer);
	}

	private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
	{
		HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
		for (int i = 0; i < roomsList.Count; i++)
		{
			var roomBounds = roomsList[i];
			var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
			var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);                    // Create a room using randomwalk start from roomcenter

			foreach (var pos in roomFloor)
			{
				if(pos.x >= (roomBounds.xMin + offset) && pos.x <= (roomBounds.xMax - offset)
					&& pos.y >= (roomBounds.yMin + offset) && pos.y <= (roomBounds.yMax - offset))
				{
					floor.Add(pos);																// Only add floor minus the offset (spreading rooms)
				}
			}
		}

		return floor;
	}


	private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
	{
		HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
		var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];    // get a random room center
		roomCenters.Remove(currentRoomCenter);

		while(roomCenters.Count > 0)	// loop through all centers
		{
			Vector2Int closest = FindClosetPointTo(currentRoomCenter, roomCenters);			// find the closet center to the current center
			HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);   // create corridor between current to the closest center

			currentRoomCenter = closest;                                                    // the new current center is the closest center
			roomCenters.Remove(closest);                                                    // remove this room center so we dont connect to it again 
			corridors.UnionWith(newCorridor);
		}

		return corridors;
	}

	private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
	{
		HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
		var pos = currentRoomCenter;
		corridor.Add(pos);							// corridor start at current center

		while (pos.y != destination.y)
		{
			if(destination.y >= pos.y)				// if the destination is higher
			{
				pos += Vector2Int.up;				// go UP
			}
			else if(destination.y < pos.y)          // if the destination is lower
			{
				pos += Vector2Int.down;				// go DOWN
			}
			corridor.Add(pos);
		}

		while (pos.x != destination.x)
		{
			if (destination.x >= pos.x)             // if the destination on the right
			{
				pos += Vector2Int.right;            // go Right
			}
			else if (destination.x < pos.x)			// if the destination on the left
			{
				pos += Vector2Int.left;             // go LEFT
			}
			corridor.Add(pos);
		}

		return corridor;
	}

	private Vector2Int FindClosetPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
	{
		Vector2Int closet = Vector2Int.zero;
		float distance = float.MaxValue;

		foreach (var pos in roomCenters)										// loop and check distance betweenn current center to all center
		{
			float currentDistance = Vector2.Distance(pos, currentRoomCenter);
			if(currentDistance < distance)
			{
				distance = currentDistance;										// if it shorter then save it to compare to the next
				closet = pos;
			}
		}

		return closet;
	}

	private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
	{
		HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
		foreach (var room in roomsList)
		{
			for (int col = offset; col < room.size.x - offset; col++)				// spread the room out base on offset 
			{
				for (int row = offset; row < room.size.y - offset; row++)           // spread the room out base on offset 
				{
					Vector2Int pos = (Vector2Int)room.min + new Vector2Int(col, row);
					floor.Add(pos);													
				}
			}
		}

		return floor;																// return the new spread out rooms
	}
}
