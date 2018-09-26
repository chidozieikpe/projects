using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Generator : MonoBehaviour {
    //grid dimensions
    public int mapWidth;
    public int mapHeight;
    //prefab object that represents a cell
    public Transform tilePrefab;
    //seed string to store generated patterns
    public string seed;
    public bool useRandomSeed;
    //percentage of grid that is randomly activated
    float randomFillPercent = 0;
    //run the simulation every quarter second if auto generate is on
    float timer = 0.25f;
    
    //cells in grid
    Cell[,] cells;
    //bool to set neighbor definition
    public bool useMooreNeighbor = true;
    //automatically generate pattern
    public bool autoGenerate = true;
    //variable that contains ids for defined rules
    public enum Rule {Dungeon, Life, Maze}

    public Rule ruleToApply;

    private void Start()
    {
        ruleToApply = Rule.Dungeon;
        Debug.Log(ruleToApply.ToString());
        GenerateMap();
    }

    private void FixedUpdate()
    {
        if(timer > 0f)
        {
            timer -= Time.fixedDeltaTime;
        }
        else
        {
            if (autoGenerate)
            {
                CellAuto();
            }
            timer = 0.25f;
        }
    }

    //change the current rule using the given string
    public void ChangeRule(string s)
    {
        if(Enum.IsDefined(typeof(Rule), s))
        {
            ruleToApply = (Rule)Enum.Parse(typeof(Rule), s);
        }
    }
    //change neighbor definition
    public void ChangeNeighborRule(bool b)
    {
        useMooreNeighbor = b;
    }
    //switch between auto and manual generation 
    public void ChangeGenerationType(bool b)
    {
        autoGenerate = b;
    }
    //update the fillpercent value
    public void ChangeFillPercent(float i)
    {
        randomFillPercent = i;
    }
    //create the grid
    void GenerateMap()
    {
        cells = new Cell[mapWidth, mapHeight];
        InitCells();
        RandomlyFillMap();
    }
    //create and set the cells
    void InitCells()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject go = Instantiate(tilePrefab.gameObject, new Vector3(x, 0.5f, y), Quaternion.identity);
                cells[x, y] = go.GetComponent<Cell>();
                cells[x, y].InitCell(x, y, true);
            }
        }
    }

    //randomly fills a given percentage of the grid
    public void RandomlyFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random rng = new System.Random(seed.GetHashCode());

        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                if (useMooreNeighbor)
                {
                    GetMooreNeighbors(ref cells, x, y);
                }
                else
                {
                    GetNeumannNeighbors(ref cells, x, y);
                }
                if (rng.Next(0, 100) < Mathf.RoundToInt(randomFillPercent))
                {
                    cells[x, y].SetActive(true);
                }
                else
                {
                    cells[x, y].SetActive(false);
                }
            }
        }

    }

    //Get all 8 cells suurounding current cell
    public void GetMooreNeighbors(ref Cell[,] cellGrid, int xPos, int yPos)
    {
        //clear neighbors first
        cellGrid[xPos, yPos].Neighbors.Clear();

        for (int x = xPos - 1; x < xPos + 2; x++)
        {
            for (int y = yPos - 1; y < yPos + 2; y++)
            {
                
                //check if coordinates is out of bounds
                if(x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                {
                    //count everything but self
                    if (x != xPos || y != yPos)
                    {
                        cellGrid[xPos, yPos].Neighbors.Add(cellGrid[x, y]);
                    }
                }
                else//wrap around
                {
                    int tx = 0;
                    int ty = 0;
                    if(x < 0 || x >= mapWidth)
                    {
                        tx = (x + mapWidth) % mapWidth;
                    }
                    else
                    {
                        tx = x;
                    }
                    
                    if (y < 0 || y >= mapHeight)
                    {
                        ty = (y + mapHeight) % mapHeight;
                    }
                    else
                    {
                        ty = y;
                    }
                    cellGrid[xPos, yPos].Neighbors.Add(cellGrid[tx, ty]);
                }
            }
        }

    }

    //get cells to the north, south, east and west of current cell
    public void GetNeumannNeighbors(ref Cell[,] cellGrid, int xPos, int yPos)
    {
        //clear neighbors first
        cellGrid[xPos, yPos].Neighbors.Clear();

        int tx = 0;
        int ty = 0;

        for (int x = xPos - 1; x < xPos + 2; x++)
        {
            if (x < 0 || x >= mapWidth)
            {
                tx = (x + mapWidth) % mapWidth;
            }
            else
            {
                tx = x;
            }

            if (tx != xPos)
            {
                cellGrid[xPos, yPos].Neighbors.Add(cellGrid[tx, yPos]);
            }
        }


        for (int y = yPos - 1; y < yPos + 2; y++)
        {
            if (y < 0 || y >= mapHeight)
            {
                ty = (y + mapHeight) % mapHeight;
            }
            else
            {
                ty = y;
            }

            if (ty != yPos)
            {
                cellGrid[xPos, yPos].Neighbors.Add(cellGrid[xPos, ty]);
            }
        }
    }

    //runs cellular automation on the grid using defined rules
    public void CellAuto()
    {
        Cell[,] nextGenCells = new Cell[mapWidth, mapHeight];
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                //this number is used to set rule parameters depending on neighbor search type
                int numNeighbors = 0;
                //uses the moore or neumann definition of neighbors
                if (useMooreNeighbor)
                {
                    GetMooreNeighbors(ref cells, x, y);
                    numNeighbors = 4;
                }
                else
                {
                    GetNeumannNeighbors(ref cells, x, y);
                    numNeighbors = 2;
                }
                
                Cell tempCell = cells[x, y];
                int neighbors = cells[x, y].GetActiveNeighbors();

                if(ruleToApply == Rule.Dungeon)
                {
                    ApplyRuleOfDungeon(tempCell, ref nextGenCells[x, y], neighbors, numNeighbors);
                }
                else if(ruleToApply == Rule.Life)
                {
                    ApplyRuleOfLife(tempCell, ref nextGenCells[x, y], neighbors, numNeighbors);
                }
                else
                {
                    ApplyRuleOfMaze(tempCell, ref nextGenCells[x, y], neighbors, numNeighbors);
                }

            }
        }
        
        
        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                if (useMooreNeighbor)
                {
                    GetMooreNeighbors(ref nextGenCells, x, y);
                }
                else
                {
                    GetNeumannNeighbors(ref nextGenCells, x, y);
                }
            }
        }
        cells = nextGenCells;
    }

    //generates bacteria like pattern and never stabilizes
    void ApplyRuleOfLife(Cell currCell, ref Cell newCell, int activeNeighbors, int numNeighbors)
    {
        if(numNeighbors < 3)
        {
            numNeighbors = 3;
        }
        newCell = currCell;
        if (currCell.Active() && (activeNeighbors > numNeighbors - 1 || activeNeighbors < numNeighbors - 2))
        {
            newCell.InitCell(currCell.XPos, currCell.YPos, false);
        }
        else if (!currCell.Active() && activeNeighbors == numNeighbors -1)
        {
            newCell.InitCell(currCell.XPos, currCell.YPos, true);
        }
    }

    //generates maze like pattern and sometimes stabilizes when using moore neighbors
    void ApplyRuleOfMaze(Cell currCell, ref Cell newCell, int activeNeighbors, int numNeighbors)
    {
        newCell = currCell;
        if (activeNeighbors > numNeighbors)
        {
            newCell.InitCell(currCell.XPos, currCell.YPos, false);
        }
        else if (activeNeighbors < numNeighbors)
        {
            newCell.InitCell(currCell.XPos, currCell.YPos, true);
        }
    }

    //generates dungeon like pattern and stabilizes
    void ApplyRuleOfDungeon(Cell currCell, ref Cell newCell, int activeNeighbors, int numNeighbors)
    {
        newCell = currCell;
        if (activeNeighbors > numNeighbors)
        {
            newCell.InitCell(currCell.XPos, currCell.YPos, true);
        }
        else if (activeNeighbors < numNeighbors)
        {
            newCell.InitCell(currCell.XPos, currCell.YPos, false);
        }
    }

    //get the percentage of randomly activated cells
    public float GetFillPercent()
    {
        return Mathf.Round(randomFillPercent);
    }

    //returns the rules as a list of strings
    public List<string> GetRuleNames(){
        string[] ruleNames = Enum.GetNames(typeof(Rule));
        List<string> names = new List<string>(ruleNames);
        return names;
    }
}
