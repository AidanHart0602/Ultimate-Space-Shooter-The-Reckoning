using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUp : MonoBehaviour
{

    [SerializeField]
    private float _speed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //When the laser is spawned
        //translate laser up
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        //if laser position for y is greater than 8
        //Destroy it
        if (transform.position.y > 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (collision.tag == "Player")
        {
            if (player != null)
            {
                player.Damage();
            }

        }
    }
}
