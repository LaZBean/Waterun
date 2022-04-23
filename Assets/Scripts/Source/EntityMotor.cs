using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMotor : MonoBehaviour
{
    public Vector3 pos;

    public float speed = 1;

    
    void Awake()
    {
        pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    
    void Update()
    {
        if (LevelManager.i && GameManager.i.isPlaying)
        {
            float y = LevelManager.i.yOffset.Evaluate(pos.z);

            transform.position = transform.parent.position + new Vector3(Mathf.Sin(pos.x), y, pos.z);
            //transform.localScale = Vector3.one - (Vector3.one * (pos.z / 1));

            pos.z -= Time.deltaTime * (speed) * LevelManager.i.gameSpeed;
        }
    }
}
