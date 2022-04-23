using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager i;

    public AnimationCurve yOffset;

    public GameObject[] enemyPrefabs;
    public GameObject[] miscPrefabs;

    public int lastSpawnedEnemy = -1;

    public List<Entity> entities = new List<Entity>();

    public float gameSpeed = 1;

    public float spawnTimer = 0;
    public float spawnMiscTimer = 0;

    public Animator roofAnimator;
    public Animator floorAnimator;

    public SpriteRenderer wave;

    public AudioSource footsteps1;

    [Range(0, 1f)] public float wave_fill = 0;

    public Entity entity
    {
        get {
            if (entities.Count > 0)
                return entities[0];
            return null;
        }
    }

    void Awake()
    {
        i = this;
    }

    void Start()
    {
        spawnTimer = 3;
    }


    void Update()
    {

        float w = (wave.size.x + Time.deltaTime / 2f);
        if (w >= 1.78f * 3f)
            w = 1.78f;
        wave.size = new Vector2(w, 2.56f);
        wave.transform.position = new Vector3(wave.transform.position.x, Mathf.Sin(Time.time)*0.1f + -2.4f + wave_fill * 2f, wave.transform.position.z);


        float s = (GameManager.i.isPlaying) ? LevelManager.i.gameSpeed / 2 : 0;
        roofAnimator.SetFloat("speed", s);
        floorAnimator.SetFloat("speed", s);

        footsteps1.pitch = s*2f;

        if (!GameManager.i.isPlaying)
        {
            return;
        }


        

        spawnTimer -= Time.deltaTime * (0.5f + gameSpeed);
        spawnMiscTimer -= Time.deltaTime * (0.5f + gameSpeed);

        gameSpeed = 0.5f + Mathf.Clamp( Mathf.Pow(GameManager.i.gameTime / 60, 2) , 0, 3);


        


        if (spawnTimer <= 0)
        {
            //Spawn Enemy
            int r;
            if (lastSpawnedEnemy == 4)
                r = Random.Range(0, enemyPrefabs.Length-1);
            else
                r = Random.Range(0, enemyPrefabs.Length);

            GameObject g = (GameObject)Instantiate(enemyPrefabs[r], new Vector3(Random.Range(-0.1f, 0.1f),0,5), Quaternion.identity);
            g.transform.parent = transform;

            Entity e = g.GetComponent<Entity>();

            e.motor.pos = new Vector3(Random.Range(-0.1f, 0.1f), 0, 6);

            entities.Add(e);

            spawnTimer = Random.Range(3f, 5f);
            lastSpawnedEnemy = r;
        }

        if (spawnMiscTimer <= 0)
        {
            //Spawn Misc

            //SpawnMisc(new Vector3(1, 0, 4));
            //SpawnMisc(new Vector3(-1, 0, 4));

            spawnMiscTimer = 4f;
        }
    }

    void SpawnMisc(Vector3 pos)
    {
        GameObject g = (GameObject)Instantiate(miscPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(0, 0, 5), Quaternion.identity);
        g.transform.parent = transform;

        g.GetComponent<EntityMotor>().pos = pos;
        Destroy(g, 10f);
    }

    public void DestroyEntity(Entity e)
    {
        entities.Remove(e);
        //Destroy(e.gameObject);
    }

    public void DestroyAll()
    {
        for (int i = 0; i < entities.Count; i++)
        {
            Destroy(entities[i].gameObject);
        }
        entities.Clear();
    }

    public IEnumerator IEFlood()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 2f;
            wave_fill = t;
            yield return null;
        }
    }
}
