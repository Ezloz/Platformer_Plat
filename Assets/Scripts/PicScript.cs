using UnityEngine;

public class PicScript : MonoBehaviour
{
    [SerializeField] private GameObject respawnPoint;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerActions>() != null)
        {
            GameObject player = collision.gameObject;
            player.transform.position = respawnPoint.transform.position;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        }
    }
}
