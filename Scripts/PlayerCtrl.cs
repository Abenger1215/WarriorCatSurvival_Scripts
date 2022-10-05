using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    private MonsterCtrl _NearestMonster;
    public MonsterCtrl NearestMonster {
        get => _NearestMonster;
        set => _NearestMonster = value;
    }
    private JoystickCtrl joystick;

    private void Awake()
    {
        joystick = FindObjectOfType<JoystickCtrl>();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, NearestMonster.transform.position, Color.blue);
    }

    void FixedUpdate()
    {
        float x = joystick.Horizontal * Time.fixedDeltaTime * GameManager.instance.MoveSpeed;
        float y = joystick.Vertical * Time.fixedDeltaTime * GameManager.instance.MoveSpeed;

        transform.Translate(new Vector3(x, y, 0));
    }
}
