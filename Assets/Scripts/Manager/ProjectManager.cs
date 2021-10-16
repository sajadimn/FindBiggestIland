using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectManager : MonoBehaviour
{
    void Awake()
    {
        GamData.projectManager = this;
    }
    
    void Start()
    {
        GamData.worldController.Init();
        GamData.largestIslandFinder.Init();
    }

    public void FoundLargestIsland()
    {
        GamData.worldController.FoundLargestIsland();
    }
}
