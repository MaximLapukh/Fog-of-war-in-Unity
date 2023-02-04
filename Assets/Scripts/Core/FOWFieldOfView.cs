using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FOWFieldOfView : MonoBehaviour
{
    [Header("Dynamic options")]
    [Range(1, 360)]
    [SerializeField] float _angle = 360;
    [Min(0)]
    [SerializeField] float _distance = 5;
    [SerializeField] LayerMask _maskBlockers;
    [SerializeField] LayerMask _visibleObjects;
    [Header("Init options")]
    [Min(2)]
    [SerializeField] int _startRayCount = 9;

    private float _anglePerRay;
    private int _vertexCount;
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _vertexCount = _startRayCount + 1;
        SetupMesh();
    }

    private void SetupMesh()
    {
        _mesh = new Mesh();
        _vertices = new Vector3[_vertexCount];
        _triangles = new int[((_vertexCount - 2) * 3) + 3];

        for (int i = 0, trG = 0; i < _vertexCount - 2; i++, trG += 3)
        {
            _triangles[trG] = i + 1;
            _triangles[trG + 1] = i + 2;
            _triangles[trG + 2] = 0;
        }

        _vertices[0] = Vector3.zero;
        _meshFilter.mesh = _mesh;
    }
    public void UpdateFieldOfView()
    {
        _anglePerRay = _angle / (_vertexCount - 2);
        Vector3 startDir = Quaternion.Euler(new Vector3(0,(-_angle/2 )- _anglePerRay, 0)) * transform.forward;
       
        for (int i = 1; i < _vertexCount; i++)
        {
            Vector3 direction = Quaternion.Euler(0, _anglePerRay * i, 0) * startDir;

            var hits = Physics.RaycastAll(transform.position, direction, _distance, _maskBlockers);
            RaycastHit hit;
            if (hits.Length > 0)
            {
                Vector3 vertex = Vector3.zero;
                if (hits.Length == 1) hit = hits[0];
                else hit = hits.OrderBy(h => h.distance).First();

                vertex = (hit.point - transform.position);
                _vertices[i] = Quaternion.Euler(-transform.eulerAngles) * vertex;
            }
            else
            {
                _vertices[i] = Quaternion.Euler(-transform.eulerAngles) * (direction*_distance);
            }  
        }
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
    }
    public List<GameObject> GetVisibleObject()
    {
        List<GameObject> visibleObjects = new List<GameObject>();
        foreach (var item in Physics.OverlapSphere(transform.position, _distance, _visibleObjects))
        {

            Vector3 directionToObject = (item.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToObject) < _angle / 2)
            {
                float distanceToObject = Vector3.Distance(transform.position, item.transform.position);
                if (!Physics.Raycast(transform.position, directionToObject, distanceToObject, _maskBlockers))
                {
                    visibleObjects.Add(item.gameObject);
                }
            }
        }
        return visibleObjects;
    }

}
