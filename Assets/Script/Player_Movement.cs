using UnityEditor;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public GameObject topRightLimitGameobject;
    public GameObject bottomLeftLimitGameobject;

    public Vector3 topRightLimit;
    public Vector3 bottomLeftLimit;

    void Start () {
        // Set the cursor to not be visible
        Cursor.visible = false;
        topRightLimit = topRightLimitGameobject.transform.position;
        bottomLeftLimit = bottomLeftLimitGameobject.transform.position;
    }

    void Update() {
        // Get the mouse position and move the player to that position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;

        transform.position = mousePosition;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
            Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y),
            transform.position.z
        );
    }
}