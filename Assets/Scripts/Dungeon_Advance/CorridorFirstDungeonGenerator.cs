using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
	[SerializeField] private int corridorLength = 14, corridorCount = 5;
	[Range(0.1f, 1)]
	[SerializeField] private float roomPercent = 0.8f;

	protected override void RunProcedualGeneration()
	{
		CorridorFirstGeneration();

	}

	private void CorridorFirstGeneration()
	{
		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
		HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

		CreateCorridoor(floorPositions, potentialRoomPositions);

		HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);
		List<Vector2Int> deadEnds = FindAllDeadEnd(floorPositions);

		CreateRoomAtDeadEnd(deadEnds, roomPositions);
		floorPositions.UnionWith(roomPositions);		// save both corridor and room floor

		tilemapVisualizer.PaintFloorTile(floorPositions);
		WallGenerator.CreateWall(floorPositions, tilemapVisualizer);
	}

	private void CreateRoomAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
	{
		foreach (var pos in deadEnds)
		{
			if(roomFloors.Contains(pos) == false)		// if there is no room there already
			{
				var room = RunRandomWalk(randomWalkParameters, pos);
				roomFloors.UnionWith(room);
			}
		}
	}

	private List<Vector2Int> FindAllDeadEnd(HashSet<Vector2Int> floorPositions)
	{
		List<Vector2Int> deadEnds = new List<Vector2Int>();
		foreach (var pos in floorPositions)
		{
			int neighboursCount = 0;
			foreach (var direction in Direction2D.cardinalDirectionList)
			{
				if (floorPositions.Contains(pos + direction))
					neighboursCount++;
			}

			if (neighboursCount == 1)
				deadEnds.Add(pos);
		}

		return deadEnds;
	}

	private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
	{
		HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
		int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

		List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();       // order by X (unique ID of each room - generated randomly) 
		foreach (var roomPos in roomsToCreate)
		{
			var roomFloor = RunRandomWalk(randomWalkParameters, roomPos);
			roomPositions.UnionWith(roomFloor);
		}

		return roomPositions;
	}

	private void CreateCorridoor(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
	{
		var currentPos = startPos;
		potentialRoomPositions.Add(currentPos);

		for (int i = 0; i < corridorCount; i++)
		{
			var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPos, corridorLength);
			currentPos = corridor[corridor.Count - 1];      // make sure the start pos of new corridor link to the end pos of the previous corridor
			potentialRoomPositions.Add(currentPos);			// adding end/start pos of each corridor as potential room pos
			floorPositions.UnionWith(corridor);
		}
	}
}
