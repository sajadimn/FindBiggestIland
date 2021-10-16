using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;


[Serializable]
public class PieceOfIsland
{
    [SerializeField] public int row;
    [SerializeField] public int column;
    [SerializeField] public int value;

    public PieceOfIsland(int row, int column, int value)
    {
        this.row = row;
        this.column = column;
        this.value = value;
    }
}

[Serializable]
public class WorldColumns
{
    [SerializeField] public int[] worldColumns;
}

public class LargestIslandFinder : MonoBehaviour
{
    [SerializeField] public WorldColumns[] worldRows;
    public Queue<PieceOfIsland> piecesOfIsland = new Queue<PieceOfIsland>();
    public Queue<PieceOfIsland> island = new Queue<PieceOfIsland>();
    public Queue<Queue<PieceOfIsland>> islands = new Queue<Queue<PieceOfIsland>>();
    public int largestIsland = 0;
    public int largestIslandIndex = 0;
    [HideInInspector]public WorldColumns[] worldMap;

    public void Awake()
    {
        GamData.largestIslandFinder = this;
        // worldMap = JsonConvert.DeserializeObject<WorldColumns[]>(JsonConvert.SerializeObject(worldRows));
        worldMap = worldRows.ToArray();
    }

    public void Init()
    {
        IslandNavigate(worldRows);
    }

    public void IslandNavigate(WorldColumns[] worldRows)
    {
        for (int i = 0; i < worldRows.Length; i++)
        {
            for (int j = 0; j < worldRows[i].worldColumns.Length; j++)
            {
                if (worldRows[i].worldColumns[j] > 0)
                {
                    var pieceOfIsland = new PieceOfIsland(i, j, worldRows[i].worldColumns[j]);
                    FindIsland(pieceOfIsland);
                }
            }
        }

        Debug.Log("Islands found: " + islands.Count);
        FindTheLargestIsland();
    }

    public void FindIsland(PieceOfIsland pieceOfIsland)
    {
        Debug.Log("item.row: " + pieceOfIsland.row + "  pieceOfIsland.column: " + pieceOfIsland.column + "  pieceOfIsland.value: " + pieceOfIsland.value);
        island.Enqueue(pieceOfIsland);
        worldRows[pieceOfIsland.row].worldColumns[pieceOfIsland.column] = -1;
        
        //check left
        if (pieceOfIsland.column - 1 >= 0 && worldRows[pieceOfIsland.row].worldColumns[pieceOfIsland.column - 1] > 0)
        {
            var newPieceOfIsland = new PieceOfIsland(pieceOfIsland.row , pieceOfIsland.column - 1, worldRows[pieceOfIsland.row].worldColumns[pieceOfIsland.column - 1]);
            piecesOfIsland.Enqueue(newPieceOfIsland);
        }

        //check top
        if (pieceOfIsland.row - 1 >= 0 && worldRows[pieceOfIsland.row - 1].worldColumns[pieceOfIsland.column] > 0)
        {
            var newPieceOfIsland = new PieceOfIsland(pieceOfIsland.row - 1 ,pieceOfIsland.column,worldRows[pieceOfIsland.row - 1].worldColumns[pieceOfIsland.column]);
            piecesOfIsland.Enqueue(newPieceOfIsland);
        }

        //check right
        if (pieceOfIsland.column + 1 < worldRows[pieceOfIsland.row].worldColumns.Length &&
            worldRows[pieceOfIsland.row].worldColumns[pieceOfIsland.column + 1] > 0)
        {
            var newPieceOfIsland = new PieceOfIsland(pieceOfIsland.row, pieceOfIsland.column + 1, worldRows[pieceOfIsland.row].worldColumns[pieceOfIsland.column + 1]);
            piecesOfIsland.Enqueue(newPieceOfIsland);
        }

        //check bottom
        if (pieceOfIsland.row + 1 < worldRows.Length && worldRows[pieceOfIsland.row + 1].worldColumns[pieceOfIsland.column] > 0)
        {
            var newPieceOfIsland = new PieceOfIsland(pieceOfIsland.row + 1, pieceOfIsland.column, worldRows[pieceOfIsland.row + 1].worldColumns[pieceOfIsland.column]);
            piecesOfIsland.Enqueue(newPieceOfIsland);
        }

        while (piecesOfIsland.Count > 0)
        {
            var newPieceOfIsland = piecesOfIsland.Dequeue();
            if (worldRows[newPieceOfIsland.row].worldColumns[newPieceOfIsland.column] > 0)
            {
                FindIsland(newPieceOfIsland);
            }
        }

        if (piecesOfIsland.Count == 0 && island.Count > 0)
        {
            islands.Enqueue(new Queue<PieceOfIsland>(island));
            island.Clear();
        }
    }
    
    public void FindTheLargestIsland()
    {
        var islandsFound = JsonConvert.DeserializeObject<Queue<Queue<PieceOfIsland>>>(JsonConvert.SerializeObject(islands));
        while (islandsFound.Count > 0)
        {
            var island = islandsFound.Dequeue();
            var islandSize = 0;
            while (island.Count > 0)
            {
                islandSize += island.Dequeue().value;
            }

            Debug.Log("islandSize: " + islandSize);
            if (islandSize > largestIsland)
            {
                largestIsland = islandSize;
                largestIslandIndex = islands.Count - (islandsFound.Count + 1);
            }
        }
        
        Debug.Log("largest island: " + largestIsland);
        
        GamData.projectManager.FoundLargestIsland();
    }
}