using Riptide;
using UnityEngine;

public class Multi_Movement : Player_Movement
{
    public bool Is_Main;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Is_Main)
        {
            // Get the mouse position and send it to the server
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
            mousePosition = new Vector3(
                Mathf.Clamp(mousePosition.x, bottomLeftLimit.x, topRightLimit.x),
                Mathf.Clamp(mousePosition.y, bottomLeftLimit.y, topRightLimit.y),
                mousePosition.z
            );

            Message message = Message.Create(MessageSendMode.Unreliable, (ushort)(ClientToServerId.playerData));
            message.AddVector3(new Vector3(
                (mousePosition.x - bottomLeftLimit.x) / (topRightLimit.x - bottomLeftLimit.x),
                (mousePosition.y - bottomLeftLimit.y) / (topRightLimit.y - bottomLeftLimit.y),
                mousePosition.z
            ));
            NetworkManager.Singleton.Client.Send(message);
        }
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = new Vector3(
            position.x * (topRightLimit.x - bottomLeftLimit.x) + bottomLeftLimit.x,
            position.y * (topRightLimit.y - bottomLeftLimit.y) + bottomLeftLimit.y,
            position.z
        );
    }

    public bool IsMain()
    {
        return Is_Main;
    }
}
