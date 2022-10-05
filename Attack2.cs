using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2 : MonoBehaviour
{

    private GameObject player;
    private float dist;
    private float _deg;

    public int idx;

    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(Vector3.back * 100f);
        player = GameObject.FindGameObjectWithTag("PLAYER");
        dist = 5.0f;
    }

    private void FixedUpdate()
    {
        _deg = AttackManager.instance.Atk2Deg;
        float rad = Mathf.Deg2Rad * (_deg + (idx * (360 / AttackManager.instance.activeCount)));
        float x = dist * Mathf.Sin(rad);
        float y = dist * Mathf.Cos(rad);
        gameObject.transform.position = player.transform.position + new Vector3(x, y);
        //gameObject.transform.rotation = Quaternion.Euler(0, 0, (_deg + (idx * (360 / AttackManager.instance.activeCount))) * -1);

        //transform.RotateAround(player.transform.position, Vector3.back, speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MONSTER"))
        {
            collision.gameObject.GetComponent<MonsterCtrl>().Damaged(AttackManager.instance.Atk2Damage);

            SoundManager.instance.PlaySFXSound("Attack2", 0.3f);
            //Debug.Log("disable by trigger / " + AM.ObjPools[PrefabName].Count + " / " + gameObject.name);
        }
    }
}
