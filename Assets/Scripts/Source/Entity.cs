using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EntityMotor motor;

    public Type type;

    public Animator animator;

    public AudioSource audio;

    public AudioClip[] deathSounds;
    public AudioClip[] attackSounds;

    public enum Type
    {
        Swordsman,
        Spearman,
        Hammerman,
        Shieldman,
        Archer
    }

    void Start()
    {
        
    }

    

    public void Attack()
    {
        animator.SetTrigger("attack");

        audio.PlayOneShot(attackSounds[Random.Range(0, attackSounds.Length)]);
    }


    public IEnumerator IEDie()
    {
        yield return new WaitForSeconds(0.1f);

        animator.SetTrigger("death");

        audio.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);

        yield return new WaitForSeconds(0.1f);

        float t = 0;
        Vector3 sPos = transform.position;
        float side = Random.Range(-1, 1f);

        if(side > 0)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }

        motor.enabled = false;

        while (t < 1)
        {
            t += Time.deltaTime;

            Vector2 np = GameUtility.Parabola((Vector2)sPos, (Vector2)sPos+new Vector2(3*side, -2), 1.5f, t);

            transform.eulerAngles = Vector3.Lerp(new Vector3(0,0,0), new Vector3(0, 0, 65*-side), t);
            transform.position =  new Vector3(np.x, np.y, transform.position.z);

            yield return null;
        }

        Destroy(gameObject);
    }
}
