using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEnemy : MonoBehaviour
{
    [SerializeField]
    private float _cycleSpeed = 1.0f;
    private float _amplitude = 2.0f;
    private float _frequency = 3f;
    private float _laserCooldown = 2;
    private bool _laserTrigger = true;
    private Vector3 _axis;
    private Vector3 _pos;



    //Handles from other Scripts
    private Player _player;
    private Animator _anim;
    private SpawnManager _spawnManager;

    //Variables that access Information from the object
    private Collider2D _collider2D;
    [SerializeField]
    private GameObject _doubleLaserPrefab;

    //Audio
    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {

        _pos = transform.position;
        _axis = transform.right;

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _audioSource = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("PLAYER IS NULL");
        }
        _collider2D = GetComponent<Collider2D>();
        if (_collider2D == null)
        {
            Debug.LogError("COLLIDER COMPONENT IS NULL");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("ANIMATIONS ARE NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AUDIO SOURCE FOR THE ENEMY IS NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        StrafingMovement();
        LaserSpawner();
    }

    void StrafingMovement()
    {
        //Strafe at a speed of 4
        _pos += Vector3.down * _cycleSpeed * Time.deltaTime;
        transform.position = _pos + _axis * Mathf.Sin(Time.time * _frequency) * _amplitude;

        //When at the bottom of the screen destroy the object
        if (transform.position.y < -6)
        {
            Destroy(this.gameObject);
        }
    }

    private void LaserSpawner()
    {
        if (Time.time > _laserCooldown  && _laserTrigger == true) 
        {
            float _fireRate = Random.Range(1.5f, 2.0f);
            _laserCooldown = Time.time + _fireRate;
            GameObject DoubleEnemyLaser = Instantiate(_doubleLaserPrefab, transform.position, Quaternion.identity);
            Laser[] doubleLaser = DoubleEnemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < doubleLaser.Length; i++)
            {
                doubleLaser[i].EnemyLaserTriggerTrue();
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            
            if (_player != null)
            {
                if (_anim != null)
                {
                    _collider2D.enabled = !_collider2D.enabled;
                    _cycleSpeed = 0;
                    _amplitude = 0;
                    _frequency = 0;
                    _anim.SetTrigger("enemyDeathTrigger");
                    _spawnManager.enemiesLeft--;
                    _laserTrigger = false;
                    _player.Damage();
                    _audioSource.Play();
                    Destroy(this.gameObject, 2.6f);
                }
            }
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _collider2D.enabled = !_collider2D.enabled;
                _cycleSpeed = 0;
                _amplitude = 0;
                _anim.SetTrigger("enemyDeathTrigger");
                _laserTrigger = false;
                _spawnManager.enemiesLeft--;
                _player.AddScore(10);
                _audioSource.Play();
                Destroy(this.gameObject, 2.6f);
            }
            
        }
        if (other.tag == "GiantLaser")
        {
            if (_player != null)
            {
                _collider2D.enabled = !_collider2D.enabled;
                _spawnManager.enemiesLeft--;
                _cycleSpeed = 0;
                _amplitude = 0;
                _frequency = 0;
                _laserTrigger = false;
                _anim.SetTrigger("enemyDeathTrigger");
                _player.AddScore(10);
                _audioSource.Play();
                Destroy(this.gameObject, 2.6f);
            }

        }
        if (other.tag == "HomingMissile")
        {
            if (_player != null)
            {
                _collider2D.enabled = !_collider2D.enabled;
                _spawnManager.enemiesLeft--;
                _cycleSpeed = 0;
                _amplitude = 0;
                _frequency = 0;
                _laserTrigger = false;
                _anim.SetTrigger("enemyDeathTrigger");
                _player.AddScore(10);
                _audioSource.Play();
                Destroy(other.gameObject);
                Destroy(this.gameObject, 2.6f);
            }


        }
    }

}
