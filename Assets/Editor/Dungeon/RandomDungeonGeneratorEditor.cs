using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator), true)]      // true to access child class since the parent is abstract
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator;

	private void Awake()
	{
		generator = (AbstractDungeonGenerator)target;		// target of the customEditor (cast since its an object)
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("Create Dungeon"))
		{
			generator.GenerateDungeon();
		}
	}
}
