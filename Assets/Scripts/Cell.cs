using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _bottomWall;
    [SerializeField] private GameObject _deadZone;

    public void RemoveLeftWall()
    {
        _leftWall.SetActive(false);
    }

    public void RemoveBottomWall()
    {
        _bottomWall.SetActive(false);
    }

    public void ActivateDeadZone()
    {
        _deadZone.SetActive(true);
    }
}
