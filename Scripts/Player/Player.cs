using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Monster nearestMonster;
    private Joystick joystick;
    public static Player instance;
    [SerializeField]
    private List<Sprite> sprites;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        joystick = FindObjectOfType<Joystick>();
    }

    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (GameManager.instance.characterName == "warrior")
        {
            spriteRenderer.sprite = sprites[0];
        }
        else if (GameManager.instance.characterName == "mage")
        {
            spriteRenderer.sprite = sprites[1];
        }
        else if (GameManager.instance.characterName == "pirate")
        {
            spriteRenderer.sprite = sprites[2];
        }
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, nearestMonster.transform.position, Color.blue);
    }

    void FixedUpdate()
    {
        float x = joystick.Horizontal * Time.fixedDeltaTime * GameManager.instance.MoveSpeed;
        float y = joystick.Vertical * Time.fixedDeltaTime * GameManager.instance.MoveSpeed;

        transform.Translate(new Vector3(x, y, 0));
    }
}
