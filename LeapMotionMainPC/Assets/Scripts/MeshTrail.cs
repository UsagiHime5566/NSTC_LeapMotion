using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float _MaxActiveTime = 2f;
    [Header("Mesh Related")]
    public float _meshRefreshRate = 0.1f;
    public float _meshDestroyDelay = 3f;
    public GameObject _handModel;

    [Header("Shader Related")]
    public Material _mat;
    public string _shaderVarRef;
    public float _shaderVarRate = 0.1f;
    public float _shaderVarRefeshRate = 0.05f;


    private float _activeTime;
    private Transform _positionToSpawn_L;
    private Transform _positionToSpawn_R;
    private SkinnedMeshRenderer _skinnedMeshRen_L;
    private SkinnedMeshRenderer _skinnedMeshRen_R;
    private bool _isTrailActive = false;

    // Start is called before the first frame update
    void Start()
    {
        _activeTime = _MaxActiveTime;

        //Initialize Transform
        Transform tran_L = _handModel.GetComponentInChildren<Transform>().Find("LoPoly Rigged Hand Left").
        GetComponentInChildren<Transform>().Find("LoPoly_Hand_Mesh_Left");
        _positionToSpawn_L = tran_L;

        Transform tran_R = _handModel.GetComponentInChildren<Transform>().Find("LoPoly Rigged Hand Right").
         GetComponentInChildren<Transform>().Find("LoPoly_Hand_Mesh_Right");
        _positionToSpawn_R = tran_R;

        //Initialize SkinnedMeshRenderer
        _skinnedMeshRen_L = tran_L.gameObject.GetComponent<SkinnedMeshRenderer>();
        _skinnedMeshRen_R = tran_R.gameObject.GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isTrailActive)
        {
            _skinnedMeshRen_L.enabled = false;
            _skinnedMeshRen_R.enabled = false;
            _isTrailActive = true;
            _activeTime = _MaxActiveTime;
            StartCoroutine(ActivateTrail(_activeTime));
        }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (_activeTime > 0)
        {
            _activeTime -= _meshRefreshRate;

            GameObject gObj_L = new GameObject();
            GameObject gObj_R = new GameObject();

            gObj_L.transform.SetPositionAndRotation(_positionToSpawn_L.position, _positionToSpawn_L.rotation);
            gObj_R.transform.SetPositionAndRotation(_positionToSpawn_R.position, _positionToSpawn_R.rotation);

            MeshRenderer mr_L = gObj_L.AddComponent<MeshRenderer>();
            MeshFilter mf_L = gObj_L.AddComponent<MeshFilter>();
            MeshRenderer mr_R = gObj_R.AddComponent<MeshRenderer>();
            MeshFilter mf_R = gObj_R.AddComponent<MeshFilter>();

            Mesh mesh_L = new Mesh();
            Mesh mesh_R = new Mesh();

            _skinnedMeshRen_L.BakeMesh(mesh_L);
            _skinnedMeshRen_R.BakeMesh(mesh_R);

            mf_L.mesh = mesh_L;
            mf_R.mesh = mesh_R;

            mr_L.material = _mat;
            mr_R.material = _mat;


            StartCoroutine(AnimateMatFloat(mr_L.material, 0, _shaderVarRate, _shaderVarRefeshRate));
            StartCoroutine(AnimateMatFloat(mr_R.material, 0, _shaderVarRate, _shaderVarRefeshRate));

            Destroy(gObj_L, _meshDestroyDelay);
            Destroy(gObj_R, _meshDestroyDelay);
            yield return new WaitForSeconds(_meshRefreshRate);
        }
        _isTrailActive = false;

        _skinnedMeshRen_L.enabled = true;
        _skinnedMeshRen_R.enabled = true;
    }

    IEnumerator AnimateMatFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnim = mat.GetFloat(_shaderVarRef);

        while (valueToAnim > goal)
        {
            valueToAnim -= rate;
            mat.SetFloat(_shaderVarRef, valueToAnim);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
