using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeExplosion : MonoBehaviour
{
    [SerializeField] private Color _playerColor;

    private float _cubeSize = 0.2f;
    private int _cubesInRow = 5;

    private float _cubesPivotDistance;
    private Vector3 _cubesPivot;
    private MeshRenderer _meshRenderer;

    private float _explosionForce = 50f;
    private float _explosionRadius = 4f;
    private float _explosionUpward = 0.4f;

    private List<GameObject> _pieces;

    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _cubesPivotDistance = _cubeSize * _cubesInRow / 2;
        _cubesPivot = new Vector3(_cubesPivotDistance, _cubesPivotDistance, _cubesPivotDistance);
    }

    public void Explode()
    {
        _meshRenderer.enabled = false;
        _pieces = new List<GameObject>();

        for (int x = 0; x < _cubesInRow; x++)
        {
            for (int y = 0; y < _cubesInRow; y++)
            {
                for (int z = 0; z < _cubesInRow; z++)
                {
                    _pieces.Add(CreatePiece(x, y, z));
                }
            }
        }

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, _explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, _explosionUpward);
            }
        }

        StartCoroutine(DeletePieces());
    }

    private GameObject CreatePiece(int x, int y, int z)
    {

        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        piece.transform.position = transform.position + new Vector3(_cubeSize * x, _cubeSize * y, _cubeSize * z) - _cubesPivot;
        piece.transform.localScale = new Vector3(_cubeSize, _cubeSize, _cubeSize);

        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = _cubeSize;
        piece.GetComponent<MeshRenderer>().material.color = _playerColor;

        return piece;
    }

    private IEnumerator DeletePieces()
    {
        yield return new WaitForSeconds(3);
        foreach (GameObject piece in _pieces)
        {
            Destroy(piece);
        }
    }
}
