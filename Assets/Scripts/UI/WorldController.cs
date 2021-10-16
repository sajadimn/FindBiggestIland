using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WorldColumnsUi
{
    [SerializeField] public List<PieceOfWorldUi> worldColumnsUi = new List<PieceOfWorldUi>();
}

public class WorldController : MonoBehaviour
{
    public Color oceansColor = new Color();
    public Color islandsColor = new Color();
    public Color largestIslandColor = new Color();
    public Text largestIslandSize = null;
    public Text largestIslandNumber = null;
    public GameObject world = null;
    public GameObject worldRow = null;
    public GameObject pieceOfWorld = null;
    
    [SerializeField] public List<WorldColumnsUi> worldRowsUi = new List<WorldColumnsUi>();

    void Awake()
    {
        GamData.worldController = this;
    }
    
    public void Init()
    {
        for (int i = 0; i < GamData.largestIslandFinder.worldMap.Length; i++)
        {
            worldRowsUi.Add(new WorldColumnsUi());
            var newWorldRow = Instantiate(worldRow , world.transform);
            for (int j = 0; j < GamData.largestIslandFinder.worldMap[i].worldColumns.Length; j++)
            {
                var newPieceOfWorld = Instantiate(pieceOfWorld, newWorldRow.transform);
                var newPieceOfWorldScript = newPieceOfWorld.GetComponent<PieceOfWorldUi>();
                newPieceOfWorldScript.Init(i , j , GamData.largestIslandFinder.worldMap[i].worldColumns[j]);
                var pieceImage = newPieceOfWorldScript.pieceBack;
                var pieceValue = newPieceOfWorldScript.pieceValue;
                if (newPieceOfWorldScript.value > 0)
                {
                    pieceImage.color = islandsColor;
                    pieceValue.text = newPieceOfWorldScript.value.ToString();
                }
                else
                {
                    pieceImage.color = oceansColor;
                    pieceValue.text = newPieceOfWorldScript.value.ToString();
                }
                worldRowsUi[i].worldColumnsUi.Add(newPieceOfWorldScript);
            }
        }
    }
    
    public void FoundLargestIsland()
    {
        largestIslandSize.text = GamData.largestIslandFinder.largestIsland.ToString();
        largestIslandNumber.text = (GamData.largestIslandFinder.largestIslandIndex + 1).ToString();
        var islandCounter = 0;
        var islands = JsonConvert.DeserializeObject<Queue<Queue<PieceOfIsland>>>(JsonConvert.SerializeObject(GamData.largestIslandFinder.islands));
        while (islands.Count > 0)
        {
            var island = islands.Dequeue();
            if (islandCounter == GamData.largestIslandFinder.largestIslandIndex)
            {
                while (island.Count > 0)
                {
                    var pieceOfLargestIsland = island.Dequeue();
                    var pieceOfIslandUi = worldRowsUi[pieceOfLargestIsland.row].worldColumnsUi[pieceOfLargestIsland.column];
                    var largestPieceBack = pieceOfIslandUi.pieceBack;
                    largestPieceBack.color = largestIslandColor;
                    pieceOfIslandUi.pieceValue.text = "N: " + (islandCounter+1) +" / V: " + pieceOfLargestIsland.value;
                }
            }
            else
            {
                while (island.Count > 0)
                {
                    var pieceOfIsland = island.Dequeue();
                    var pieceOfIslandUi = worldRowsUi[pieceOfIsland.row].worldColumnsUi[pieceOfIsland.column];
                    pieceOfIslandUi.pieceValue.text = "N: " + (islandCounter+1) +" / V: " + pieceOfIsland.value;
                }
            }

            islandCounter++;
        }
    }
}
