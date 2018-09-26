using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    //the map generator
    Generator myGenerator;
    private void Start()
    {
        myGenerator = GetComponent<Generator>();
    }
    //Input evaluation
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeTile();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ChangeNeighborTiles();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            myGenerator.RandomlyFillMap();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            myGenerator.CellAuto();
        }
    }
    //change the state of a cell
    public void ChangeTile()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Cell c = hit.transform.GetComponent<Cell>();
            if (c != null)
            {
                c.SwitchState();
            }
        }
    }
    //change the state of neighboring cells
    public void ChangeNeighborTiles()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Cell c = hit.transform.GetComponent<Cell>();
            if (c != null)
            {
                c.TriggerNeighborCells();
            }
        }
    }
}
