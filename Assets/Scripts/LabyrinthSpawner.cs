using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LabyrinthSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Vector3 _startPositiont;
    private void OnEnable()
    {
        CellGenerator generator = new CellGenerator();
        CellInfo[,] cellsArray = generator.GenerateGrid();

        Vector3 position = _startPositiont;

        for (int i = 0; i < cellsArray.GetLength(0); i++)
        {
            position.z += 2;
            position.x = _startPositiont.x;

            for (int j = 0; j < cellsArray.GetLength(1); j++)
            {
                position.x += 2;
                GameObject cell = Instantiate(_cellPrefab, position, Quaternion.identity);

                Cell wall = cell.GetComponent<Cell>();
                if (cellsArray[i, j].BottomWall == false)
                    wall.RemoveBottomWall();
                if (cellsArray[i, j].LeftWall == false)
                    wall.RemoveLeftWall();

            }
        }

        GetComponent<NavMeshSurface>().BuildNavMesh();

    }

    //private void SpawnLabyrinth(CellInfo[,] cellsArray)
    //{
    //    CellInfo currentCell = cellsArray[0, 0];
        
        
    //}
}
