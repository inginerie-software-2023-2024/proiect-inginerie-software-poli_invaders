using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float yDistance;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position + Vector3.up * yDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = player.transform.position + Vector3.up * yDistance;
    }
}
