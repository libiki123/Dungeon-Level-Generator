using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
	public static void CreateWall(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
	{
		var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionList);
		var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionList);
		CreateBasicWalls(tilemapVisualizer, basicWallPositions, floorPositions);
		CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
	}

	private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
	{
		foreach (var pos in basicWallPositions)
		{
			string neighboursBinaryType = "";
			foreach (var direction in Direction2D.eightDirectionList)		// check all eight direction around the wall
			{
				var neighbourPosition = pos + direction;
				if (floorPositions.Contains(neighbourPosition))					// save the neighbour positions in binary
				{
					neighboursBinaryType += "1";
				}
				else
				{
					neighboursBinaryType += "0";
				}
			}
			Debug.Log(neighboursBinaryType);
			tilemapVisualizer.PaintSingleCornerWall(pos, neighboursBinaryType);
		}
	}

	private static void CreateBasicWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
	{
		foreach (var pos in basicWallPositions)
		{
			string neighboursBinaryType = "";
			foreach (var direction in Direction2D.cardinalDirectionList)    // check all diagnal direction around the wall
			{
				var neighnourPosition = pos + direction;
				if (floorPositions.Contains(neighnourPosition))				// save the neighbour positions in binary
				{
					neighboursBinaryType += "1";
				}
				else
				{
					neighboursBinaryType += "0";
				}
			}

			tilemapVisualizer.PaintSingleBasicWall(pos, neighboursBinaryType);
		}
	}

	private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
	{
		HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
		foreach (var pos in floorPositions)
		{
			foreach (var dicrection in directionList)							// get positions around a floor pos
			{
				var neighboorPosition = pos + dicrection;
				if (floorPositions.Contains(neighboorPosition) == false)        // if the floor pos have an empty tile around it -> its a wall
					wallPositions.Add(neighboorPosition);
			}
		}

		return wallPositions;
	}
}
