using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class WorldObject : MonoBehaviour {

    public string objectName;
    public Texture2D buildImage; //changed 3d to 2d
    public int hitPoints, maxhitPoints;

    public Player player;
    protected string[] actions = { };
    protected bool currentlySelected = false;

    protected Bounds selectionBounds;
    protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

    protected virtual void Awake()
    {
        selectionBounds = ResourceManager.InvalidBounds;
        CalculateBounds();
    }

    protected virtual void Start()
    {
        player = transform.root.GetComponentInChildren<Player>();
    }

    protected virtual void Update()
    {
            if (currentlySelected) CalculateBounds(); //haha I did it though it looks a bit weird lol 
            
    }
    //public bool IsOwnedBy(Player owner) //the problem is with the local player? - BIG TROUBLE HERE
    //{
    //    if (player && player.Equals(owner))//player.name == owner.name? - doesn't pass this check if nothing was selected at the beginning
    //    {
    //        Debug.Log(owner.name + "+" + player.name);//goes here for spaceship
    //        return true;
    //    }
    //    else
    //    {//actually gives nullreference (hidden, as always) for  actually owner(???) here, owner is null
    //      //but this is because the planet itself doesn't have an owner yet
    //      //I hate null reference exceptions
    //        Debug.Log(owner.name + "-" + player.name); //goes here for planet & spacestation
    //        return false;
    //    }
    //}
    protected virtual void OnGUI()
    {
        if (currentlySelected) DrawSelection();
    }
    private void DrawSelection()
    {   //draw selection only when player camera is active
        if (GameObject.FindWithTag("MapCamera").GetComponent<Camera>().depth != 1) ///yess it works
        {
            GUI.skin = ResourceManager.SelectBoxSkin;
            Rect selectBox = WorkManager.CalculateSelectionBox(selectionBounds, playingArea);
            //Draw the selection box around the currently selected object, within the bounds of the playing area
            GUI.BeginGroup(playingArea);
            DrawSelectionBox(selectBox);
            GUI.EndGroup();
        }
    }
    protected virtual void DrawSelectionBox(Rect selectBox)
    {
        GUI.Box(selectBox, "");
    }

    public void CalculateBounds()
    {
        selectionBounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            selectionBounds.Encapsulate(r.bounds);
        }
    }

    public void SetSelection(bool selected, Rect playingArea)
    {
    currentlySelected = selected;
    if(selected) this.playingArea = playingArea;

    }

    public string[] GetActions()
    {
        return actions;
    }

    public virtual void PerformAction(string actionToPerform)
    {
        //it is up to children with specific actions to determine what to do with each of those actions
        //at least for now
    }

    public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller)
    {
        //only handle input if currently selected
        if (currentlySelected && hitObject && hitObject.tag != "Sun" && hitObject.tag != "Skybox")
        {
            WorldObject worldObject = hitObject.GetComponentInParent<WorldObject>();
            //clicked on another selectable object
            if (worldObject) ChangeSelection(worldObject, controller);
        }
    }
    private void ChangeSelection(WorldObject worldObject, Player controller)
    {
        //this should be called by the following line, but there is an outside chance it will not
        SetSelection(false, playingArea);
        if (controller.SelectedObject) controller.SelectedObject.SetSelection(false, playingArea);
        controller.SelectedObject = worldObject;
        worldObject.SetSelection(true, controller.hud.GetPlayingArea());
    }

    public virtual void SetHoverState(GameObject hoverObject)
    {
        //only handle input if owned by a human player and currently selected
        if (player && player.human && currentlySelected)
        {
            if (hoverObject.tag != "Sun") player.hud.SetCursorState(CursorState.Select);//|| hoverObject.tag != "Skybox"
        }
    }
   
}
