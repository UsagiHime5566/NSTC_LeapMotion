using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail_2 : MonoBehaviour
{
    public float _MaxActiveTime = 2f;
    [Header("Mesh Related")]
    public float _meshRefreshRate = 0.1f;
    public float _meshDestroyDelay = 3f;

    [Header("Shader Related")]
    public Material _mat;
    public string _shaderVarRef;
    public float _shaderVarRate = 0.1f;
    public float _shaderVarRefeshRate = 0.05f;

    public Transform _positionToSpawnRef
    {
        get { return _positionToSpawn; }
        set { _positionToSpawn = value; }
    }

    private float _activeTime;
    private Transform _positionToSpawn;
    private SkinnedMeshRenderer _skinnedMeshRen;
    public Boolean _isTrailActiveRef
    {
        get { return _isTrailActive; }
        set { _isTrailActive = value; }
    }
    private bool _isTrailActive = false;


    void Start()
    {
        _activeTime = _MaxActiveTime;
        _positionToSpawnRef = GetComponent<Transform>();
        _skinnedMeshRen = GetComponent<SkinnedMeshRenderer>();
    }

    public void StartTrail()
    {
        if (!_isTrailActive && transform.GetComponentInParent<Leap.Unity.RiggedHand>() != null)
        {
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

            GameObject gObj = new GameObject();

            gObj.transform.SetPositionAndRotation(_positionToSpawn.position, _positionToSpawn.rotation);

            MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
            MeshFilter mf = gObj.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();

            _skinnedMeshRen.BakeMesh(mesh);

            mf.mesh = mesh;
            mr.material = _mat;

            StartCoroutine(AnimateMatFloat(mr.material, 0, _shaderVarRate, _shaderVarRefeshRate));

            Destroy(gObj, _meshDestroyDelay);
            yield return new WaitForSeconds(_meshRefreshRate);
        }
        _isTrailActive = false;

        _skinnedMeshRen.enabled = true;
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
