using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    [SerializeField] GameObject cursorPrefab;

    bool buildModeIsObjects = false;

    string buildModeObjectType; 
    TILE_TYPE buildModeTile = TILE_TYPE.FLOOR;

    Vector3 currentFramePos;
    Vector3 lastFramePos;
    Vector3 DragStartPos;
    GameObject indicatorParent; 
    List<GameObject> cursorIndicators = new List<GameObject>();


    //Zoom properties 
    [SerializeField] float zoomSpeed = 1;
    [SerializeField] float zoomMin = 1;
    [SerializeField] float zoomMax = 10;
    
    Camera cam; 
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        //Create Parent GameObject for cursor Indicators. 
        indicatorParent = new GameObject();
        indicatorParent.name = "Cursors"; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Tile tileUnderMouse = WorldController.Instance.GetTileAtWorldCoord(currentFramePos);

        HandleScreenDrag();
        //MoveCursor(tileUnderMouse);
        HandleDragTileSelection();
        HandleZooming(); 
    }

    void HandleScreenDrag()
    {
        currentFramePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Vector3 diff = lastFramePos - currentFramePos;
            cam.transform.Translate(diff);
        }

        lastFramePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    /// <summary>
    /// Snaps cursor to the center of a tile where mouse is. 
    /// </summary>
    void MoveCursor(Tile tileUnderMouse)
    {
        if (tileUnderMouse != null)
        {
            cursorPrefab.SetActive(true);
            cursorPrefab.transform.position = new Vector3(tileUnderMouse.X, tileUnderMouse.Y, 0);
        }
        else
        {
            cursorPrefab.SetActive(false);
        }
    }

    /// <summary>
    /// Select all tiles in x and y axis. Right now its converting empty tiles into floor. 
    /// </summary>
    void HandleDragTileSelection()
    {
        //Bail if mouse is over UI element
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //Start Drag 
        if (Input.GetMouseButtonDown(0))
        {
            DragStartPos = currentFramePos;
        }

        int startX = Mathf.RoundToInt(DragStartPos.x);
        int endX = Mathf.RoundToInt(currentFramePos.x);
        if (endX < startX)
        {
            int tmp = endX;
            endX = startX;
            startX = tmp;
        }

        int startY = Mathf.RoundToInt(DragStartPos.y);
        int endY = Mathf.RoundToInt(currentFramePos.y);
        if (endY < startY)
        {
            int tmp = endY;
            endY = startY;
            startY = tmp;
        }

        //Clear old cursor indicators
        while(cursorIndicators.Count > 0)
        {
            GameObject go = cursorIndicators[0];
            cursorIndicators.RemoveAt(0);
            SimplePool.Despawn(go);
        }


        //Handle Drag
        if (Input.GetMouseButton(0))
        {
            //Display a preview of selected area. 
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    Tile t = WorldController.Instance.world.GetTileAt(x, y);
                    if (t != null)
                    {
                        GameObject go = SimplePool.Spawn(cursorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                        go.transform.SetParent(indicatorParent.transform);
                        cursorIndicators.Add(go); 
                    }
                }
            }
        }

        //End Drags
        if (Input.GetMouseButtonUp(0))
        {
            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    Tile t = WorldController.Instance.world.GetTileAt(x, y);
                    if (t != null)
                    {
                        if (buildModeIsObjects)
                        {
                            //Create installedObject and place it. 

                            //TODO: only walls for now. 
                            //WorldController.Instance.world.PlaceInstalledObject(); s
                        }
                        else
                        {
                            //Tile mode building/buldozing. 
                            t.Type = buildModeTile;
                        }
                    }
                }
            }
        }
    }

    void HandleZooming()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != Mathf.Epsilon)
        {
            cam.orthographicSize -= (Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed);
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, zoomMin, zoomMax);
        }
    }

    /// <summary>
    /// Clamps the value to world height or width. 
    /// Currently the cursor is dissapearing so the method is not being used. 
    /// </summary>
    int ClampCursorToWorldSize(int value, bool xAxis)
    {
        int maxValue = (xAxis) ? WorldController.Instance.world.Width : (WorldController.Instance.world.Height - 1);
        return Mathf.Clamp(value, 0, maxValue); 
    }

    #region BUTTON MENU 

    public void SetModeBuildFloor()
    {
        buildModeTile = TILE_TYPE.FLOOR;
        buildModeIsObjects = false; 
    }

    public void SetModeBuldozerFloor()
    {
        buildModeTile = TILE_TYPE.EMPTY;
        buildModeIsObjects = false; 
    }

    public void SetModeBuildWall( string objectType )
    {
        buildModeIsObjects = true;
        buildModeObjectType = objectType;
    }

    #endregion
}
