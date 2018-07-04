using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using RTS;

public class Player : MonoBehaviour {
    public string username;
    public bool human;
    public HUD hud;

    public int startPower, startPowerLimit, startMoney, startMoneyLimit;
    private Dictionary<ResourceType, int> resources, resourceLimits;

    public WorldObject SelectedObject 
    { 
        get ; 
        set ; 
    }

	
	void Start () {
        hud = this.GetComponentInChildren<HUD>();
        AddStartResourceLimits();
        AddStartResources();
	}

    void Awake()
    {
        resources = InitResourceList();
        resourceLimits = InitResourceList();
    }
    private Dictionary<ResourceType, int> InitResourceList()
    {
        Dictionary<ResourceType, int> list = new Dictionary<ResourceType, int>();
        list.Add(ResourceType.Money, 0);
        list.Add(ResourceType.Power, 0);
        return list;
    }
    private void AddStartResourceLimits()
    {
        IncrementResourceLimit(ResourceType.Money, startMoneyLimit);
        IncrementResourceLimit(ResourceType.Power, startPowerLimit);
    }

    private void AddStartResources()
    {
        AddResource(ResourceType.Money, startMoney);
        AddResource(ResourceType.Power, startPower);
    }
    public void AddResource(ResourceType type, int amount)
    {
        resources[type] += amount;
    }

    public void IncrementResourceLimit(ResourceType type, int amount)
    {
        resourceLimits[type] += amount;
    }

    //rolled back any build unit functionality to see where's the problem

    public void AddUnit(string unitName, Vector3 spawnPoint, Quaternion rotation)
    {
        Debug.Log("add " + unitName + " to player");
    }
    //public void AddUnit(string unitName, Vector3 spawnPoint, Quaternion rotation) {
    //    Unit units = GetComponentInChildren<Unit>();
    //    GameObject newUnit = (GameObject)Instantiate(ResourceManager.GetUnit(unitName),spawnPoint, rotation); ///spawn object
    //    newUnit.transform.parent = units.transform;
    //}
	void Update () {
        if (human)
        {
            hud.SetResourceValues(resources, resourceLimits);
        }
	}
}
