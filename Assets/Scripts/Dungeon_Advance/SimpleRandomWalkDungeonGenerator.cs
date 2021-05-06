using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{
	[SerializeField] protected SimpleRandomWalk_SO randomWalkParameters;

	protected override void RunProcedualGeneration()
	{
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPos);
		tilemapVisualizer.Clear();
		tilemapVisualizer.PaintFloorTile(floorPositions);
		WallGenerator.CreateWall(floorPositions, tilemapVisualizer);
	}

	protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalk_SO parameters, Vector2Int pos)
	{
		var currentPos = pos;
		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
		for (int i = 0; i < parameters.iterations; i++)															// creating iteration amount of path (random walk)
		{
			var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPos, parameters.walkLength);
			floorPositions.UnionWith(path);
			if (parameters.startRandomlyEachIteration)
				currentPos = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count));		// set the start pos to a random floor pos
		}
		return floorPositions;
	}

}
