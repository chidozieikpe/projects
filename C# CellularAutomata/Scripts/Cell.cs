using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    //list of all current neighbors
    public List<Cell> Neighbors { get; set; }
    //horizontal coordinate value
    public int XPos { get; private set; }
    //vertical coordinate value
    public int YPos { get; private set; }
    //materials to visually indicate cell state
    public Material activeMat, inactiveMat;
    //number of neighbors
    public int neighborCount = 0;
    //variable to set cell state
    public enum State {Active, Inactive}

    public State cellState;

    Renderer myRenderer;

    Transform myTransform;
    //maximum height a cell can get to
    float maxObjectScale = 5.0f;
    //minumum height a cell can get to
    float minObjectScale = 1.0f;
    //current height of cell
    float currentScale = 1.0f;

    private void Start()
    {
        myTransform = GetComponent<Transform>();
        myRenderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        Morph();
        neighborCount = Neighbors.Count;
    }
    //change the color of the cell
    public void SetMaterial()
    {
        if(myRenderer != null)
        {
            if (cellState == State.Active)
            {
                myRenderer.material = activeMat;
            }
            else
            {
                myRenderer.material = inactiveMat;
            }
        }
        
    }
    //check whether a cell is active
    public bool Active()
    {
        if(cellState == State.Active)
        {
            return true;
        }
        return false;
    }
    //switch state of cell
    public void SwitchState()
    {
        if (cellState == State.Active)
        {
            cellState = State.Inactive;
        }
        else
        {
            cellState = State.Active;
        }
        SetMaterial();
    }
    //set the active state of a cell
    public void SetActive(bool active)
    {
        if (active)
        {
            cellState = State.Active;
        }
        else
        {
            cellState = State.Inactive;
        }
        SetMaterial();
    }
    //initialize cell properties
    public void InitCell(int x, int y, bool active)
    {
        XPos = x;
        YPos = y;
        Neighbors = new List<Cell>();
        SetActive(active);
    }
    //change the state of neighboring cells
    public void TriggerNeighborCells()
    {
        if(Neighbors.Count > 0)
        {
            for(int i = 0; i < Neighbors.Count; i++)
            {
                Neighbors[i].SwitchState();
            }
        }
    }
    //get the number of active neighboring cells
    public int GetActiveNeighbors()
    {
        int count = 0;
        if(Neighbors.Count > 0)
        {
            for(int i = 0; i < Neighbors.Count; i++)
            {
                if (Neighbors[i].Active())
                {
                    count++;
                }
            }
        }
        return count;
    }
    //make a cell grow or shrink
    public void Morph()
    {
        if (cellState == State.Active && currentScale < maxObjectScale)
        {
            currentScale = Approach(currentScale, maxObjectScale, 0.025f);
            myTransform.localScale = new Vector3(1.0f, currentScale, 1.0f);
            
        }
        else if(cellState == State.Inactive && currentScale > minObjectScale)
        {
            currentScale = Approach(currentScale, minObjectScale, 0.05f);
            myTransform.localScale = new Vector3(1.0f, currentScale, 1.0f);
        }
        myTransform.position = new Vector3(myTransform.position.x, currentScale / 2.0f, myTransform.position.z);
    }
    //a function to make the value of a float turn to that of another
    public float Approach(float start, float end, float step)
    {
        float result;
        if(start < end)
        {
            result = Mathf.Min(start + step, end);
        }
        else
        {
            result = Mathf.Max(start - step, end);
        }
        return result;
    }
}
