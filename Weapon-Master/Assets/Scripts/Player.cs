using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public static Player self;
    public float speed = 5.0f;

    public float horizontal;
    public float vertical;

    public bool moveBool = true;
    void Start()
    {
        if (self == null)
            self = this;

    }

    public void PlayerMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
    }

    void Update()
    {
        if (moveBool)
            PlayerMove();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Door")
        {
            GameObject nextRoom = collision.gameObject.transform.parent.GetComponent<Door>().nextRoom;
            Door nextDoor = collision.gameObject.transform.parent.GetComponent<Door>().SideDoor;

            // ���� ������ �˸� �����ذ�
            if (nextDoor.doorType == Door.DoorType.left)
            {
                Debug.Log("��");
                Vector3 currPos = new Vector3(nextDoor.transform.position.x + 1.5f, 0.5f, nextDoor.transform.position.z);
                Player.Instance.transform.position = currPos;
            }
            else if (nextDoor.doorType == Door.DoorType.right)
            {
                Debug.Log("��");

                Vector3 currPos = new Vector3(nextDoor.transform.position.x - 1.5f, 0.5f, nextDoor.transform.position.z);
                Player.Instance.transform.position = currPos;
            }
            else if (nextDoor.doorType == Door.DoorType.top)
            {
                Debug.Log("��");

                Vector3 currPos = new Vector3(nextDoor.transform.position.x, 0.5f, nextDoor.transform.position.z - 1.5f);
                Player.Instance.transform.position = currPos;
            }
            else
            {
                Debug.Log("�Ʒ�");

                Vector3 currPos = new Vector3(nextDoor.transform.position.x , 0.5f, nextDoor.transform.position.z + 1.5f);
                Player.Instance.transform.position = currPos;
            }




        }
    }
}