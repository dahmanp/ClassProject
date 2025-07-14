using UnityEngine;

public class Pickups : MonoBehaviour
{
    // reference to player
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        // grab ref to player
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        // if the player collides with coin, increase points and destroy
        if (other.name == "Player")
        {
            player.coinCount++;
            Destroy(this.gameObject);
        }
    }
}
