using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using RTS;

public class Base : WorldObject {
    //will roll back all changes to look where errors started to appear

    public float maxBuildProgress; //production speed
    protected Queue<string> buildQueue;
    private float currentBuildProgress = 0.0f; //to take time to build
    private Vector3 spawnPoint; //spawn point for units of this building
 
    protected override void Awake()
    {
        base.Awake();
        //initialize base spawn point
        buildQueue = new Queue<string>();
        //maybe move it from awake somewhere else if it's not working?
        float spawnX = selectionBounds.center.x + transform.forward.x * selectionBounds.extents.x + transform.forward.x * 10;
        float spawnY = transform.forward.y;//how to get base's y-axis position in space
        float spawnZ = selectionBounds.center.z + transform.forward.z + selectionBounds.extents.z + transform.forward.z * 10;
        spawnPoint = new Vector3(spawnX, 0.0f, spawnZ); //make sure unit is not created inside the base
    }
    protected void CreateUnit(string unitName)
    {
        buildQueue.Enqueue(unitName);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        ProcessBuildQueue();
    }
    protected void ProcessBuildQueue()
    {
        if (buildQueue.Count > 0)
        {
            currentBuildProgress += Time.deltaTime * ResourceManager.BuildSpeed;
            if (currentBuildProgress >= maxBuildProgress)//changed > to >=
            {
                if (player) player.AddUnit(buildQueue.Dequeue(), spawnPoint, transform.rotation);
                currentBuildProgress = 0.0f;
            }
        }
    }
    public string[] getBuildQueueValues()
    {
        string[] values = new string[buildQueue.Count];
        int pos = 0;
        foreach (string unit in buildQueue) values[pos++] = unit;
        return values;
    }

    public float getBuildPercentage()
    {
        return currentBuildProgress / maxBuildProgress;
    }
    protected override void OnGUI()
    {
        base.OnGUI();
    }
}
