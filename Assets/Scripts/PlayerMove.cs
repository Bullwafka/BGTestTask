using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private LayerMask _mask;

    [SerializeField] private Transform _target;
    [SerializeField] private Transform _start;

    [SerializeField] private LineRenderer _linerenderer;

    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _protectionColor;

    public bool EnabelMove;

    private CubeExplosion _explosion;
    private MeshRenderer _meshRenderer;
    private NavMeshAgent _agent;
    private bool _isProtected;


    private void Start()
    {
        _explosion = GetComponent<CubeExplosion>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _linerenderer = GetComponent<LineRenderer>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.SetDestination(_target.position);
        gameObject.transform.position = _startPosition;
        StartCoroutine(CheckPath());
    }
    private void Update()
    {
        if (EnabelMove)
        {
            _agent.SetDestination(_target.position);
        }
        if (_agent.hasPath)
        {
            CalculatePath();
        }
    }

    private void CalculatePath()
    {
        _linerenderer.positionCount = _agent.path.corners.Length;
        _linerenderer.SetPosition(0, transform.position);

        for (int i = 1; i < _agent.path.corners.Length; i++)
        {
            Vector3 position = new Vector3(_agent.path.corners[i].x, 0.3f, _agent.path.corners[i].z);
            _linerenderer.SetPosition(i, position);
        }

        _linerenderer.enabled = true;
    }

    private void GenerateDeadZones()
    {
        List<GameObject> objectsOnPath = new List<GameObject>();
        for (int i = 0; i < _linerenderer.positionCount - 1; i++)
        {
            Ray ray = new Ray(_linerenderer.GetPosition(i), _linerenderer.GetPosition(i + 1));
            if (Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(_linerenderer.GetPosition(i), _linerenderer.GetPosition(i + 1)), _mask))
                objectsOnPath.Add(hit.collider.gameObject);
        }
        _agent.ResetPath();

        Cell randomCell = objectsOnPath[Random.Range(1, objectsOnPath.Count)].GetComponent<Cell>();
        randomCell.ActivateDeadZone();
    }

    public void ApplyProtection()
    {
        _isProtected = true;
        StartCoroutine(ProtectionAnimation(_protectionColor));
    }

    public void RemoveProtection()
    {
        _isProtected = false;
        StartCoroutine(ProtectionAnimation(_normalColor));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (_isProtected == false)
            {
               
                EnabelMove = false;
                _agent.ResetPath();
                _agent.isStopped = false;
                _explosion.Explode();

                StartCoroutine(Restart());
            }
        }
    }

    private IEnumerator CheckPath()
    {
        Debug.Log("StartCoroutine");
        yield return new WaitUntil(() => _agent.hasPath);
        CalculatePath();
        GenerateDeadZones();
        yield break;
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(3);
        gameObject.transform.position = _startPosition;
        gameObject.transform.rotation = Quaternion.identity;
        _meshRenderer.enabled = true;
        _agent.SetDestination(_target.position);
        yield return new WaitForSeconds(3);
        EnabelMove = true;
        yield break;
    }

    private IEnumerator ProtectionAnimation(Color color)
    {
        float t = 0;

        const float DURATION = 1f;

        while (t < 1)
        {
            _meshRenderer.material.color = Color.Lerp(_meshRenderer.material.color, color, t);
            t += Time.deltaTime / DURATION;
            yield return null;
        }
    }
}
