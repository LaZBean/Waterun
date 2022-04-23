using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;

    public BoxCollider weaponTrigger;

    public int hp = 1;
    public int arrow = 10;
    public int kills = 0;

    public Weapon weapon;

    public enum Weapon
    {
        Sword,
        Spear,
        Hammer,
        Shield,
        Bow,
    }




    public float lastDist = 0;
    Entity targetEnemy;

    public bool attackPrepared = false;
    public bool blockDestroyed = false;

    public Animator charA;
    public Animator charAWeapon;

    public Animator charB;


    public AudioClip[] weaponSounds;
    public AudioSource audio;


    // Start is called before the first frame update
    void Awake()
    {
        i = this;
    }
    private void Start()
    {
        SetWeapon((int)weapon);
        UIManager.i.AddKills();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.i.Pause(GameManager.i.isPlaying);
        }

        if (!GameManager.i.isPlaying)
        {
            return;
        }

        charA.SetFloat("speed", LevelManager.i.gameSpeed / 2);
        charB.SetFloat("speed", LevelManager.i.gameSpeed / 2);

        //Control
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SetWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SetWeapon(2);
        else if (Input.GetKeyDown(KeyCode.Space))
            SetWeapon(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SetWeapon(4);


        //finding closest enemy
        Entity e = LevelManager.i.entity;
        if(targetEnemy != e || targetEnemy == null)
        {
            lastDist = -1;
            targetEnemy = e;

            attackPrepared = false;
            blockDestroyed = false;
        }
        else
        {
            //Check enemy range
            float d = (e.motor.pos.z - transform.position.z);
            int range = Mathf.CeilToInt(d);
            

            if (lastDist != range)
            {
                lastDist = range;
                OnChangedRange(range);
            }
        }
    }

    

    void OnChangedRange(int range)
    {
        if (!targetEnemy) return;

        //Scenarios
        //Swordsman
        if (targetEnemy.type == Entity.Type.Swordsman)
        {
            if (range == 1)
            {
                if (weapon == Weapon.Hammer && attackPrepared)
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                }
                else
                {
                    Death();
                }
            }
            else if(range == 2)
            {
                if(weapon == Weapon.Spear)
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                }
                else if(weapon == Weapon.Hammer)
                {
                    attackPrepared = true;
                    charAWeapon.SetTrigger("prepare");
                }
            }
            else if (range == 3)
            {
                if (weapon == Weapon.Bow && arrow > 0)
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                    arrow--;
                }
            }
        }

        //Spearman
        if (targetEnemy.type == Entity.Type.Spearman)
        {
            if (range == 1)
            {
                if (weapon != Weapon.Sword)
                {
                    Death();
                }
                else
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                }
            }
            else if (range == 2)
            {
                targetEnemy.Attack();
                if (weapon != Weapon.Shield)
                {
                    Death();
                }
                else
                {
                    charAWeapon.SetTrigger("prepare"); PlayWeaponAudio();
                }
            }
            else if (range == 3)
            {
                if (weapon == Weapon.Bow && arrow > 0)
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                    arrow--;
                }
            }
        }

        //Hammerman
        if (targetEnemy.type == Entity.Type.Hammerman)
        {
            if (range == 1)
            {
                Death();
            }
            else if (range == 2)
            {
                
                if (weapon == Weapon.Spear)
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                }
            }
            else if (range == 3)
            {
                if (weapon == Weapon.Bow && arrow > 0)
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                    arrow--;
                }
            }
        }

        //Shielder
        if (targetEnemy.type == Entity.Type.Shieldman)
        {
            if (range == 1)
            {
                if ((weapon == Weapon.Sword && blockDestroyed) || (weapon == Weapon.Hammer && attackPrepared))
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                }
                else
                {
                    Death();
                }
            }
            else if (range == 2)
            {

                if (weapon == Weapon.Spear)
                {
                    if (blockDestroyed)
                    {
                        charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                        Kill();
                    }
                    else
                    {
                        charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                        blockDestroyed = true;
                    }
                }
                else if (weapon == Weapon.Hammer)
                {
                    attackPrepared = true;
                    charAWeapon.SetTrigger("prepare");
                }
            }
            else if (range == 3)
            {
                if (weapon == Weapon.Bow && arrow > 0)
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    blockDestroyed = true;
                    arrow--;
                }
            }

            targetEnemy.animator.SetBool("block", !blockDestroyed);
        }

        //Archer
        if (targetEnemy.type == Entity.Type.Archer)
        {
            if (range == 1)
            {
                if (weapon == Weapon.Sword || (weapon == Weapon.Hammer && attackPrepared))
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                }
                else
                {
                    Death();
                }
            }
            else if (range == 2)
            {

                if (weapon == Weapon.Spear)
                {
                    charAWeapon.SetTrigger("attack"); PlayWeaponAudio();
                    Kill();
                }
                else if (weapon == Weapon.Hammer)
                {
                    attackPrepared = true;
                    charAWeapon.SetTrigger("prepare");
                }
            }
            else if (range == 3)
            {
                targetEnemy.Attack();
                if (weapon != Weapon.Shield)
                {
                    Death();
                }
                else
                {
                    charAWeapon.SetTrigger("prepare"); PlayWeaponAudio();
                }
            }
        }
    }

    void Kill()
    {
        targetEnemy.StartCoroutine(targetEnemy.IEDie());
        LevelManager.i.DestroyEntity(targetEnemy);

        kills++;
        UIManager.i.AddKills();
    }

    void PlayWeaponAudio()
    {
        audio.PlayOneShot(weaponSounds[(int)weapon]);
    }

    public void Death()
    {
        //Failed
        targetEnemy.Attack();

        StartCoroutine(IEDeath());
    }

    public IEnumerator IEDeath()
    {
        GameManager.i.isPlaying = false;

        yield return new WaitForSeconds(1.1f);

        LevelManager.i.StartCoroutine(LevelManager.i.IEFlood());

        charA.SetTrigger("death");
        charB.SetTrigger("death");

        GameManager.i.GameFinish();
    }



    public void SetWeapon(int id)
    {
        weapon = (Weapon)id;

        charAWeapon.SetInteger("id", id);
        charAWeapon.SetTrigger("change");

        //OnChangedRange(Mathf.CeilToInt(lastDist));
        UIManager.i.RefreshItem(id);
    }


  
    /*public IEnumerator Attack()
    {
        yield return new WaitForSeconds(weapon.attackDelayTime);

        //try attack
        Entity target = LevelManager.i.entity;
        if (target != null)
        {
            int v = weapon.Compare(target.weapon);
            if (v == 1)
            {
                LevelManager.i.DestroyEntity(target);
            }
            else if(v == 0)
            {
                hp--;
                if(hp <= 0)
                {
                    Death();
                }
            }
            else
            {
                Death();
            }
        }
    }*/
}
