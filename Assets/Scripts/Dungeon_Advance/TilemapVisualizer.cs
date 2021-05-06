using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap floorTilemap, wallTilemap;
	[SerializeField] private TileBase floorTile, wallTop, wallSideRight, wallSideLeft, wallBottom, wallFull, 
		wallInnerCornerDownLeft, wallInnerCornerDownRight,
		wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    public void PaintFloorTile(IEnumerable<Vector2Int> floorPositions)
	{
        PaintTiles(floorPositions, floorTilemap, floorTile);
	}

	private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
	{
		foreach(var pos in positions)
		{
			PaintSingleTile(tilemap, tile, pos);
		}
	}

	internal void PaintSingleBasicWall(Vector2Int pos, string binaryType)
	{
		int typeAsInt = Convert.ToInt32(binaryType, 2);
		TileBase tile = null;

		if (WallTypesHelper.wallTop.Contains(typeAsInt))				// compare the binary to the helper to know which tile it should use
		{
			tile = wallTop;
		}
		else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
		{
			tile = wallSideRight;
		}
		else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
		{
			tile = wallSideLeft;
		}
		else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
		{
			tile = wallBottom;
		}
		else if (WallTypesHelper.wallFull.Contains(typeAsInt))
		{
			tile = wallFull;
		}

		if (tile != null)
		{
			PaintSingleTile(wallTilemap, tile, pos);
		}
	}

	internal void PaintSingleCornerWall(Vector2Int pos, string binaryType)
	{
		int typeAsInt = Convert.ToInt32(binaryType, 2);
		TileBase tile = null;

		if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
		{
			tile = wallInnerCornerDownLeft;
		}
		else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
		{
			tile = wallInnerCornerDownRight;
		}
		else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
		{
			tile = wallDiagonalCornerDownLeft;
		}
		else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
		{
			tile = wallDiagonalCornerDownRight;
		}
		else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
		{
			tile = wallDiagonalCornerUpRight;
		}
		else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
		{
			tile = wallDiagonalCornerUpLeft;
		}
		else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
		{
			tile = wallFull;
		}
		else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
		{
			tile = wallBottom;
		}


		if (tile != null)
		{
			PaintSingleTile(wallTilemap, tile, pos);
		}
	}

	private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int pos)
	{
		var tilePos = tilemap.WorldToCell((Vector3Int)pos);			// get the position in the tilemap
		tilemap.SetTile(tilePos, tile);
	}

	public void Clear()
	{
		floorTilemap.ClearAllTiles();
		wallTilemap.ClearAllTiles();
	}


}
