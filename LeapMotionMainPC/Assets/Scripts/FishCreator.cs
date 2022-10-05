using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FishCreator : MonoBehaviour
{
    public SignalServer server;
    public int eachFishNum = 5;
    public Fish prefab_fish;
    void Start()
    {
        server.OnSignalReceived.AddListener(src => {
            try{
                var s = src.Split(',');
                float x = 0, y = 0;
                float.TryParse(s[0], out x);
                float.TryParse(s[1], out y);
                Vector2 pos = new Vector2(x, y);
                CreateFishGroup(pos);
            } catch {};
        });

        //StartCoroutine(loopCreate());
    }

    void CreateFishGroup(Vector2 pos){
        for (int i = 0; i < eachFishNum; i++)
        {
            Vector3 npos = new Vector3(transform.position.x + pos.x/200, transform.position.y, transform.position.z + pos.y/200);
            var f = Instantiate(prefab_fish, npos, Quaternion.identity, transform);
        }
    }

    IEnumerator loopCreate(){
        while (true)
        {
            CreateFishGroup(new Vector2(0, 0));
            yield return new WaitForSeconds(2);
        }
    }
}
