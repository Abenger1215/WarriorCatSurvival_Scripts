using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : MonoBehaviour
{
    MonsterManager MM;

    public float Hp;
    public float Exp;

    public float initHp;
    public float initDamage;

    private Transform PlayerTr;
    private Rigidbody2D Rb;
    private bool isDelay;
    private int Damage;

    public float MoveSpeed = 9.0f;

    private string PrefabName;

    private void Awake()
    {
        PlayerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        Rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        MM = MonsterManager.instance;
        PrefabName = gameObject.name.Split('_')[0];
    }

    private void OnEnable()
    {
        UpgradeMonster();
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    private void OnDisable()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.transform.position = new Vector3(0, 0, float.MaxValue);
    }

    private void Update()
    {
       /* Hp -= 0.005f;
        if(Hp <= 0)
        {
            ReturnPool();
        } */
    }

    private void FixedUpdate()
    {
        Vector2 dir = PlayerTr.position - transform.position;
        Rb.MovePosition(Rb.position + dir.normalized * MoveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PLAYER"))
        {
            if(isDelay == false)
            {
                //Debug.Log("Player Hit!");
                SoundManager.instance.PlaySFXSound("Damaged", 0.5f);

                GameManager.instance.Damaged(Damage);
                UIManager.instance.UpdateHpUI();
                isDelay = true;
                StartCoroutine(Delay());
            }
            else
            {
                //Debug.Log("DelayTime");
            }
        }
    }

    public void Damaged(float damage)
    {
        Hp -= damage;
        if(Hp < 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ChangeColorByDamage());
        }
    }

    void Die()
    {
        GameManager.instance.ExpUp(this.Exp);
        //Debug.Log(GameManager.instance.exp);

        StopAllCoroutines();
        ReturnPool();
    }

    void ReturnPool()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(MM.ObjGroups[PrefabName + "Group"].transform);
        MM.ObjPools[PrefabName].Push(gameObject);
        GameManager.instance.score += 0.5f;
    }

    void UpgradeMonster()
    {
        Hp = initHp * (1 + GameManager.instance.PlayTime * 0.05f);
        Exp = 10.0f;
        Damage = (int)(initDamage * (1 + GameManager.instance.PlayTime * 0.001f));
    }
    
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.5f);
        isDelay = false;
    }

    IEnumerator ChangeColorByDamage()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
