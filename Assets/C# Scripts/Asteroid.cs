using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotatingSpeed = 2.5f;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("ASTEROID SPAWN MANAGER IS NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotatingSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the laser collides with the asteroid, start spawning enemies.
        if (collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _spawnManager.EndWave();
             Destroy(this.gameObject, 0.2f);

        }
    }
}