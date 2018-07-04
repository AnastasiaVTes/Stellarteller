using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class HUD : MonoBehaviour {
    public GUISkin resource_bar, orders_bar, selectBoxSkin;
    public GUISkin mouseCursorSkin;
    private const int ORDERS_BAR_WIDTH = 150, RESOURCE_BAR_HEIGHT = 40;
    private Player player;

    private const int SELECTION_NAME_HEIGHT = 15;
    //cursors
    public Texture2D activeCursor;
    public Texture2D selectCursor; 
    public Texture2D[] attackCursors, moveCursors; 
    private CursorState activeCursorState;
    private int currentFrame = 0;

    private Dictionary<ResourceType, int> resourceValues, resourceLimits;
    private const int ICON_WIDTH = 32, ICON_HEIGHT = 32, TEXT_WIDTH = 128, TEXT_HEIGHT = 32;
    public Texture2D[] resources;
    private Dictionary<ResourceType, Texture2D> resourceImages;

    private WorldObject lastSelection;
    private float sliderValue;

    public Texture2D buttonHover, buttonClick;
    private const int BUILD_IMAGE_WIDTH = 64, BUILD_IMAGE_HEIGHT = 64;
    private int buildAreaHeight = 0;
    private const int BUTTON_SPACING = 7;
    private const int SCROLL_BAR_WIDTH = 22;

	void Start () {//check here
        player = transform.root.GetComponent<Player>(); //if remove root everything is sad
        resourceValues = new Dictionary<ResourceType, int>();
        resourceLimits = new Dictionary<ResourceType, int>();
        ResourceManager.StoreSelectBoxItems(selectBoxSkin);
        SetCursorState(CursorState.Select);
        resourceImages = new Dictionary<ResourceType, Texture2D>();
        for (int i = 0; i < resources.Length; i++)
        {
            switch (resources[i].name)
            {
                case "money":
                    resourceImages.Add(ResourceType.Money, resources[i]);
                    resourceValues.Add(ResourceType.Money, 0);
                    resourceLimits.Add(ResourceType.Money, 0);
                    break;
                case "power":
                    resourceImages.Add(ResourceType.Power, resources[i]);
                    resourceValues.Add(ResourceType.Power, 0);
                    resourceLimits.Add(ResourceType.Power, 0);
                    break;
                default: break;
            }
        }
        //buildAreaHeight = Screen.height - RESOURCE_BAR_HEIGHT - SELECTION_NAME_HEIGHT - 2 * BUTTON_SPACING;
	}
	
	
	void OnGUI () {//look this up
        if (player && player.human) //lol if I disable this check there will be lots of null reference exceptions
        {
            
            DrawResourceBar();
            DrawOrdersBar();//check here
            DrawMouseCursor();
        }
	}

    private void DrawOrdersBar() ///!!!!!!!
    {
        GUI.skin = orders_bar;
        GUI.BeginGroup(new Rect(Screen.width - ORDERS_BAR_WIDTH, RESOURCE_BAR_HEIGHT, ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT));
        GUI.Box(new Rect(0, 0, ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT), "");
        string selectionName = "";
        if (player.SelectedObject)
        {
            selectionName = player.SelectedObject.objectName;
        }
        //??? WTF AND WHY O BAR ISNT RENDERING AT THE START
        //well, now the basic bar itself (previous two ifs) is rendering, but THIS is wrecking everything
        ///AND WHY THE BRACKETS (SELECTION) ARE"T RENDERING AROUND PLANET AND SPACE STATION
        //if (player.SelectedObject.IsOwnedBy(player)) //maybe change if to elseif or wahtever ->
        //basically, it must be almost the same as the previous if clause - same player, same selected object
        //just + check if the object we selected is owned by player
        //it shouldn't give this much problems
        //as it doesn't ENTER in this if clause, the problem must be either with how this ownership is checked
        //OR it could be in why in is checked every frame unless an object is selected - why it's doing this???
        ///AND WHERE IS UPDATE??
        /////what check is called every frame???
        //{
        //    //reset slider value if the selected object has changed
        //    if (lastSelection && lastSelection != player.SelectedObject) sliderValue = 0.0f;
        //    DrawActions(player.SelectedObject.GetActions());
        //    //store the current selection
        //    lastSelection = player.SelectedObject;
            
        //}
        
        if (!selectionName.Equals(""))
        {
            //int topPos = buildAreaHeight + BUTTON_SPACING;
            //GUI.Label(new Rect(0, topPos, ORDERS_BAR_WIDTH, SELECTION_NAME_HEIGHT), selectionName);
            GUI.Label(new Rect(0, 10, ORDERS_BAR_WIDTH, SELECTION_NAME_HEIGHT), selectionName);
        }
        
        GUI.EndGroup();
    }
    private void DrawResourceBar()
    {///resource bar iskn't drawn too from the start, maybe because of orders bar? 
     ///yep, the problem is PURELY with ORDERS bar, there is nothing wrong with building a bar itself
        GUI.skin = resource_bar;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, RESOURCE_BAR_HEIGHT));
        GUI.Box(new Rect(0, 0, Screen.width - 100, RESOURCE_BAR_HEIGHT), "");

        int topPos = 4, iconLeft = 4, textLeft = 20;
        DrawResourceIcon(ResourceType.Money, iconLeft, textLeft, topPos);
        iconLeft += TEXT_WIDTH;
        textLeft += TEXT_WIDTH;
        DrawResourceIcon(ResourceType.Power, iconLeft, textLeft, topPos);

        GUI.EndGroup();
    }

    //private void DrawActions(string[] actions)
    //{
    //    GUIStyle buttons = new GUIStyle();
    //    buttons.hover.background = buttonHover;
    //    buttons.active.background = buttonClick;
    //    GUI.skin.button = buttons;
    //    int numActions = actions.Length;
    //    //define the area to draw the actions inside
    //    GUI.BeginGroup(new Rect(0, 0, ORDERS_BAR_WIDTH, buildAreaHeight));
    //    //draw scroll bar for the list of actions if need be
    //    if (numActions >= MaxNumRows(buildAreaHeight)) DrawSlider(buildAreaHeight, numActions / 2.0f);
    //    //display possible actions as buttons and handle the button click for each
    //    for (int i = 0; i < numActions; i++)
    //    {
    //        int column = i % 2;
    //        int row = i / 2;
    //        Rect pos = GetButtonPos(row, column);
    //        Texture2D action = ResourceManager.GetBuildImage(actions[i]);
    //        if (action)
    //        {
    //            //create the button and handle the click of that button
    //            if (GUI.Button(pos, action))
    //            {
    //                if (player.SelectedObject) player.SelectedObject.PerformAction(actions[i]);
    //            }
    //        }
    //    }
    //    GUI.EndGroup();
    //}
    //private int MaxNumRows(int areaHeight)
    //{
    //    return areaHeight / BUILD_IMAGE_HEIGHT;
    //}

    //private Rect GetButtonPos(int row, int column)
    //{
    //    int left = SCROLL_BAR_WIDTH + column * BUILD_IMAGE_WIDTH;
    //    float top = row * BUILD_IMAGE_HEIGHT - sliderValue * BUILD_IMAGE_HEIGHT;
    //    return new Rect(left, top, BUILD_IMAGE_WIDTH, BUILD_IMAGE_HEIGHT);
    //}
   
    //private void DrawSlider(int groupHeight, float numRows)
    //{
    //    //slider goes from 0 to the number of rows that do not fit on screen
    //    sliderValue = GUI.VerticalSlider(GetScrollPos(groupHeight), sliderValue, 0.0f, numRows - MaxNumRows(groupHeight));
    //}

    //private Rect GetScrollPos(int groupHeight)
    //{
    //    return new Rect(BUTTON_SPACING, BUTTON_SPACING, SCROLL_BAR_WIDTH, groupHeight - 2 * BUTTON_SPACING);
    //}
    public bool MouseInBounds()
    {
        //Screen coordinates start in the lower-left corner of the screen
        //not the top-left of the screen like the drawing coordinates do
        Vector3 mousePos = Input.mousePosition;
        bool insideWidth = mousePos.x >= 0 && mousePos.x <= Screen.width - ORDERS_BAR_WIDTH;
        bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - RESOURCE_BAR_HEIGHT;
        return insideWidth && insideHeight;
    }

    private void DrawResourceIcon(ResourceType type, int iconLeft, int textLeft, int topPos)
    {
        Texture2D icon = resourceImages[type];
        string text = resourceValues[type].ToString() + "/" + resourceLimits[type].ToString();
        GUI.DrawTexture(new Rect(iconLeft, topPos, ICON_WIDTH, ICON_HEIGHT), icon);
        GUI.Label(new Rect(textLeft, topPos, TEXT_WIDTH, TEXT_HEIGHT), text);
    }
    private void DrawMouseCursor()
    {
        if (!MouseInBounds())
        {
            Cursor.visible = true; //changed obsolete code
        }
        else
        {
            Cursor.visible = false;
            GUI.skin = mouseCursorSkin;
            GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
            UpdateCursorAnimation();
            Rect cursorPosition = GetCursorDrawPosition();
            GUI.Label(cursorPosition, activeCursor);
            GUI.EndGroup();
        }
    }
    private void UpdateCursorAnimation()
    {
        //sequence animation for cursor (based on more than one image for the cursor)
        //change once per second, loops through array of images
        if (activeCursorState == CursorState.Move)
        {
            currentFrame = (int)Time.time % moveCursors.Length;
            activeCursor = moveCursors[currentFrame];
        }
        else if (activeCursorState == CursorState.Attack)
        {
            currentFrame = (int)Time.time % attackCursors.Length;
            activeCursor = attackCursors[currentFrame];
        }
        
    }
    private Rect GetCursorDrawPosition()
    {
        //set base position for custom cursor image
        float leftPos = Input.mousePosition.x;
        float topPos = Screen.height - Input.mousePosition.y; //screen draw coordinates are inverted
        //adjust position base on the type of cursor being shown
        if (activeCursorState == CursorState.Move || activeCursorState == CursorState.Select) //removed what I'm not using
        {
            topPos -= activeCursor.height / 2;
            leftPos -= activeCursor.width / 2;
        }
        return new Rect(leftPos, topPos, activeCursor.width, activeCursor.height);
    }
    public void SetCursorState(CursorState newState)
    {
        activeCursorState = newState;
        switch (newState)
        {
            case CursorState.Select:
                activeCursor = selectCursor;
                break;
            case CursorState.Attack:
                currentFrame = (int)Time.time % attackCursors.Length;
                activeCursor = attackCursors[currentFrame];
                break;
            case CursorState.Move:
                currentFrame = (int)Time.time % moveCursors.Length;///ex here!!!
                activeCursor = moveCursors[currentFrame];
                break;
                default: break;
        }
    }
    public Rect GetPlayingArea()
    {
        return new Rect(0, RESOURCE_BAR_HEIGHT, Screen.width - ORDERS_BAR_WIDTH, Screen.height - RESOURCE_BAR_HEIGHT);
    }

    public void SetResourceValues(Dictionary<ResourceType, int> resourceValues, Dictionary<ResourceType, int> resourceLimits)
    {
        this.resourceValues = resourceValues;
        this.resourceLimits = resourceLimits;
    }

}
