using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : MonoBehaviour
{
    [SerializeField] protected Vector2Int startPos = Vector2Int.zero;
    [SerializeField] private int iterations = 10;
    public int walkLength = 10;
    public bool startRandomlyEachIteration = true;

    public void RunProceduralGeneration()
	{
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
		foreach (var pos in floorPositions)
		{
			Debug.Log(pos);
		}
	}

	protected HashSet<Vector2Int> RunRandomWalk()
	{
		var currentPos = startPos;
		HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
		for (int i = 0; i < iterations; i++)        // creating iteration amount of path (random walk)
		{
			var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPos, walkLength);
			floorPositions.UnionWith(path);
			if (startRandomlyEachIteration)
				currentPos = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count));		// set the start pos to a random floor pos
		}
		return floorPositions;
	}
}
