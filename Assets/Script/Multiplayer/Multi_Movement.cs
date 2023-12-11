using Riptide;
using UnityEngine;

public class Multi_Movement : MonoBehaviour
{
    [Header("Arena Bounds")]
    [SerializeField]private GameObject topRightLimitGameobject;
    [SerializeField]private GameObject bottomLeftLimitGameobject;

    [Header("Player")]
    [SerializeField] private Multi_Player player;

    public Vector3 TopRightLimit {  get; private set; }
    public Vector3 BottomLeftLimit {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        TopRightLimit = topRightLimitGameobject.transform.position;
        BottomLeftLimit = bottomLeftLimitGameobject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsMain)
        {
            // Get the mouse position and send it to the server
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
            mousePosition = new Vector3(
                Mathf.Clamp(mousePosition.x, BottomLeftLimit.x, TopRightLimit.x),
                Mathf.Clamp(mousePosition.y, BottomLeftLimit.y, TopRightLimit.y),
                mousePosition.z
            );

            Message message = Message.Create(MessageSendMode.Unreliable, (ushort)ClientToServerId.playerData);
            message.AddVector3(new Vector3(
                (mousePosition.x - BottomLeftLimit.x) / (TopRightLimit.x - BottomLeftLimit.x),
                (mousePosition.y - BottomLeftLimit.y) / (TopRightLimit.y - BottomLeftLimit.y),
                mousePosition.z
            ));
            NetworkManager.Singleton.Client.Send(message);
        }
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = new Vector3(
            position.x * (TopRightLimit.x - BottomLeftLimit.x) + BottomLeftLimit.x,
            position.y * (TopRightLimit.y - BottomLeftLimit.y) + BottomLeftLimit.y,
            position.z
        );
    }
}
