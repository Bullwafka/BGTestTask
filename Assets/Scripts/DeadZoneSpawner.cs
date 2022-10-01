using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZoneSpawner : MonoBehaviour
{
    private List<Cell> _cellsOnPathList = new List<Cell>();
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cell>(out Cell cell))
        {
            if (!_cellsOnPathList.Contains(cell))
                _cellsOnPathList.Add(cell);
        }
    }

    public void GenerateDeadZones()
    {
        if (_cellsOnPathList.Count > 0)
        {
            int i = Random.Range(0, _cellsOnPathList.Count);
            _cellsOnPathList[i].ActivateDeadZone();
            _cellsOnPathList.RemoveAt(i);
            int j = Random.Range(0, _cellsOnPathList.Count);
            _cellsOnPathList[j].ActivateDeadZone();
        }
    }
}
