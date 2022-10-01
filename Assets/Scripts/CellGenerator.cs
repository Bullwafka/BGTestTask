using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellInfo
{
    public int X;
    public int Y;

    public bool LeftWall = true;
    public bool BottomWall = true;
    public bool Visited = false;

    public CellInfo(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class CellGenerator
{
    private int _quadLevelSize = 10;

    public CellInfo[,] GenerateGrid()
    {
        CellInfo[,] cellsArray = new CellInfo[_quadLevelSize, _quadLevelSize];

        for (int i = 0; i < cellsArray.GetLength(0); i++)
        {
            for (int j = 0; j < cellsArray.GetLength(1); j++)
            {
                cellsArray[i, j] = new CellInfo(i, j);
            }
        }

        CheckAdjacentCells(cellsArray);
        
        return cellsArray;
    }

    private void CheckAdjacentCells(CellInfo[,] cellsArray)
    {
        CellInfo currentCell = cellsArray[0, 0];
        currentCell.Visited = true;

        Stack<CellInfo> cellsStack = new Stack<CellInfo>();
        List<CellInfo> unvisitedAdjacentCells = new List<CellInfo>();

        do
        {
            int x = currentCell.X;
            int y = currentCell.Y;

            if (y < _quadLevelSize - 1 && cellsArray[x, y + 1].Visited == false)
                unvisitedAdjacentCells.Add(cellsArray[x, y + 1]);
            if (y > 0 && cellsArray[x, y - 1].Visited == false)
                unvisitedAdjacentCells.Add(cellsArray[x, y - 1]);
            if (x > 0 && cellsArray[x - 1, y].Visited == false)
                unvisitedAdjacentCells.Add(cellsArray[x - 1, y]);
            if (x < _quadLevelSize - 1 && cellsArray[x + 1, y].Visited == false)
                unvisitedAdjacentCells.Add(cellsArray[x + 1, y]);

            if (unvisitedAdjacentCells.Count > 0)
            {
                CellInfo nextCell = unvisitedAdjacentCells[Random.Range(0, unvisitedAdjacentCells.Count)];
                RemoveWall(currentCell, nextCell);

                nextCell.Visited = true;
                cellsStack.Push(nextCell);
                currentCell = nextCell;
            }
            else
            {
                currentCell = cellsStack.Pop();
            }

            unvisitedAdjacentCells.Clear();

        } while (cellsStack.Count > 0);
    }

    private void RemoveWall(CellInfo current, CellInfo next)
    {
        if(current.Y == next.Y)
        {
            if(current.X < next.X)
            {
                next.LeftWall = false;
            }
            else
            {
                current.LeftWall = false;
            }
        }
        else 
        {
            if(current.Y < next.Y)
            {
                next.BottomWall = false;
            }
            else
            {
                current.BottomWall = false;
            }
        }
    }
}
