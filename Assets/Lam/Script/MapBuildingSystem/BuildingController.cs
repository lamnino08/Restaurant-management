using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class BuildingController : MonoBehaviour
{
    private Camera _camera;
    public Vector3 _offset = Vector3.zero;
    public  int level;
    [SerializeField] private bool _isDragging;
    [SerializeField] private Renderer _renderer;
    public ObjectData dataObject; 

    private Vector3 _firstPostion;
    private Vector3 _previous;

    private void Start()
    {
        _camera = Camera.main;
        _previous = transform.position;
    }

    public void SetData(ObjectData dataa)
    {
        this.dataObject = dataa;
        if ((dataObject.width & 1) == 0) _offset.x += 0.5f;
        if ((dataObject.height & 1) == 0) _offset.z += 0.5f;
        transform.position += _offset;
        Debug.Log(_offset);
        Debug.Log(transform.position);

        if (CheckData())
        {
            _renderer.material.color = Color.green;
        } else 
        {
            _renderer.material.color = Color.red;
        }
    }

    private void OnMouseDown()
    {
        _isDragging = true;
        _renderer.material.color = Color.green;
        List<Vector2> rs = GridSystem.AreaByPosition(transform.position, dataObject.width, dataObject.height);
        Debug.Log("#######"+transform.position);    
        // foreach(var d in rs)
        // {
        //     Debug.Log(d);
        // }
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosInGrid = PlacementSystem.instance.GetPositionGrid();
        if (_previous != mousePosInGrid)
        {
            Movement(mousePosInGrid);
            if (CheckData())
            {
                _renderer.material.color = Color.green;
            } else 
            {
                _renderer.material.color = Color.red;
            }
            _previous = mousePosInGrid;
        }
    }

    public bool CheckData()
    {
        List<Vector2> area = GridSystem.AreaByPosition(transform.position, dataObject.width, dataObject.height);

        foreach(Vector2 cell in area)
        {
            if (!GridSystem.instance.IsAllowedPlace(cell))
            {
                // Debug.Log("Herer"+cell);
                return false;
            }
        }

        return true;
       
    }

    private void Movement(Vector3 mousePosInGrid)
    {
        {
            Vector3 nepose = new Vector3(mousePosInGrid.x, 0.03f, mousePosInGrid.z);
            // Debug.Log(_offset+"    "+nepose);
            transform.position = nepose + _offset;
            _previous = transform.position;
        }
    }

    public void Rotate()
    {
        transform.Rotate(0,90,0);
    }

    private void OnMouseUp()
    {
        Vector3 mousePosInGrid = PlacementSystem.instance.GetPositionGrid();
        Vector3 nepose = new Vector3(mousePosInGrid.x, 0, mousePosInGrid.z);
        
        ActBuildingUI.instance.StartPlaceBuilding(this);
    }

    public void Place()
    {
        if (CheckData())
        {
            float x = (dataObject.width & 1) == 1 ? transform.position.x : transform.position.x + 0.5f;
            float y = (dataObject.height & 1) == 1 ? transform.position.z : transform.position.z + 0.5f;
            Vector2 cellMain = new Vector2(y,x);

            List<Vector2> area = GridSystem.AreaByPosition(transform.position, dataObject.width, dataObject.height);
            GridSystem.instance.PlaceBuilding(area, dataObject.id, cellMain);

            ActBuildingUI.instance._currentBuildingAction = null;
            PlacementSystem.instance._currentBuil = null;
        }
    }
}
