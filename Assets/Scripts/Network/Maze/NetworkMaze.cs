﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetworkMaze : NetworkBehaviour {

	public IntVector2 size;

	public MazeCell cellPrefab;
	public Material blue,red;
	public float generationStepDelay;
	private int generationStepDelay_multiplier;

	public MazePassage passagePrefab;
	public MazeWall wallPrefab;

	private MazeCell start, finish;
	public MazeDirection start_direction;

	private MazeCell[,] cells;

	private int maxStackSize = 0;//currentStackSize=0;
	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public MazeCell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

	public IEnumerator Generate () {
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		int cooldown = 0;

		cells = new MazeCell[size.x, size.z];
		List<MazeCell> activeCells = new List<MazeCell>();

		DoFirstGenerationStep(activeCells);

		while (activeCells.Count > 0) {
			if (cooldown > generationStepDelay_multiplier && generationStepDelay_multiplier!=-1) {
				cooldown = 0;
				yield return delay;
			}
			cooldown++;
			DoNextGenerationStep(activeCells);
		}

		for (int i = 0; i < 4; i++)
			if (!start.GetEdge ((MazeDirection)i).haswall)
				start_direction = (MazeDirection)i;

		finish.gameObject.transform.FindChild ("Quad").GetComponent<MeshRenderer> ().material = red;
		finish.gameObject.name="FinishCell";
	}

	private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		activeCells.Add(CreateCell(new IntVector2(0,0)));
		start = activeCells [0];
		start.gameObject.name="StartCell";
		activeCells [0].gameObject.transform.FindChild ("Quad").GetComponent<MeshRenderer> ().material = blue;
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) {
			if (maxStackSize < activeCells.Count) {
				maxStackSize = activeCells.Count;
				finish = activeCells [currentIndex];
			}
			else
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MazeDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();

		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null) {
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else {
				CreateWall(currentCell, neighbor, direction);
			}
		}
		else {
			CreateWall(currentCell, null, direction);
		}
	}

	private MazeCell CreateCell (IntVector2 coordinates) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
		NetworkServer.Spawn (newCell.gameObject);
		return newCell;
	}

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction,false);
		//NetworkServer.Spawn (passage.gameObject);

		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite(),false);
		//NetworkServer.Spawn (passage.gameObject);
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction,true);
		NetworkServer.Spawn (wall.gameObject);

		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite(),true);
			NetworkServer.Spawn (wall.gameObject);
		}
	}
}