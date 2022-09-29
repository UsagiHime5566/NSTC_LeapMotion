using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    private UDP _udpReceive;
    public GameObject[] _handPoints;
    // Start is called before the first frame update
    void Start()
    {
        _udpReceive = GetComponent<UDP>();
    }

    // Update is called once per frame
    void Update()
    {

        string data = _udpReceive.data;
        if (data.Length > 2)
        {
            data = data.Remove(0, 1);
            data = data.Remove(data.Length - 1, 1);
            print(data);
            string[] points = data.Split(',');
            print(points[1]);

            for (int i = 0; i < 21; i++)
            {

                float x = 7 - float.Parse(points[i * 3]) / 100;
                float y = float.Parse(points[i * 3 + 1]) / 100;
                float z = float.Parse(points[i * 3 + 2]) / 100;

                _handPoints[i].transform.localPosition = new Vector3(x, y, z);

            }


        }


    }
}
