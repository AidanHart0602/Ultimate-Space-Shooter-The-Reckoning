using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    //Number Variables
    [SerializeField]
    private float _smartEnemySpeed = 1.0f;
    [SerializeField]
    private float _dodgeSpeed = 1f;
    private float _randomX = 0;
    private float _smartLaserCooldown;

    //Handles from other Scripts
    private Player _player;
    private Animator _anim;
    private SpawnManager _spawnManager;
    private Laser _laser;

    //Variables that access Information from the object
    private Collider2D _collider2D;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _enemyTripleLaserPrefab;
    [SerializeField]
    private GameObject _laserUpPrefab;
    [SerializeField]
    private GameObject _enemyDodgeCollider;

    //Bools
    private bool _laserBool = true;
    private static bool _tripleLaserShotTrigger = false;
    private bool _canFire = true;
    private bool _dodge = true;

    //Vector3 Variables
    private Vector3 _movement;

    //Audio
    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        _movement = Vector3.down * _smartEnemySpeed * Time.deltaTime;

        _collider2D = GetComponent<Collider2D>();

        _player = GameObject.Find("Player").GetComponent<Player>();

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _audioSource = GetComponent<AudioSource>();

        _laser = GameObject.FindAnyObjectByType<Laser>();

        if (_collider2D == null)
        {
            Debug.LogError("COLLIDER COMPONENT IS NULL");
        }
        if (_player == null)
        {
            Debug.LogError("PLAYER IS NULL");
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
        EnemyMovement();
        if (_player.transform.position.y > transform.position.y) 
        {
            SmartLaser();
        }
        RegularLaser();

    }

    void EnemyMovement()
    {
        transform.Translate(_movement);
        //When at the bottom of the screen, respawn at top
        if (transform.position.y < -5)
        {
            _randomX = (Random.Range(-10.3f, 10.3f));
            transform.position = new Vector3(_randomX, 6.5f, 0);
        }
    }

    void SmartLaser()
    {
            if (Time.time > _smartLaserCooldown && _laserBool == true)
            {
                float _fireRate = Random.Range(1.5f, 2.0f);
                _smartLaserCooldown = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_laserUpPrefab, transform.position, Quaternion.identity);
                Laser[] laserUp = enemyLaser.GetComponentsInChildren<Laser>();
                for (int i = 0; i < laserUp.Length; i++)
                {
                    laserUp[i].EnemyLaserTriggerTrue();
                }
                return;
            }
    }

    private void RegularLaser()
    {
        if (_tripleLaserShotTrigger == true)
        {

            if (Time.time > _smartLaserCooldown && _tripleLaserShotTrigger == true && _canFire == true)
            {
                float _fireRate = Random.Range(1.5f, 2.0f);
                _smartLaserCooldown = Time.time + _fireRate;
                GameObject TripleEnemyLaser = Instantiate(_enemyTripleLaserPrefab, transform.position, Quaternion.identity);
                Laser[] tripleLaser = TripleEnemyLaser.GetComponentsInChildren<Laser>();
                for (int i = 0; i < tripleLaser.Length; i++)
                {
                    tripleLaser[i].EnemyLaserTriggerTrue();
                }
                return;
            }
        }

        else
        {
            if (Time.time > _smartLaserCooldown && _laserBool == true)
            {
                float _fireRate = Random.Range(1.5f, 2.0f);
                _smartLaserCooldown = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].EnemyLaserTriggerTrue();
                }
                return;
            }
        }
    }
    public void EnemyTripleLaserActive()
    {
        StartCoroutine(TripleLaserShotCooldown());
    }

    public void enableDodge()
    {
        int randomNum = Random.Range(0, 2);
        while (_dodge == true)
        {
            if (randomNum == 0)
            {
                _movement = Vector3.left * _dodgeSpeed * Time.deltaTime;
            }

            if (randomNum == 1)
            {
                _movement = Vector3.right * _dodgeSpeed * Time.deltaTime;
            }

            StartCoroutine(DodgeCooldown());
        }
    }
    IEnumerator DodgeCooldown()
    {
        _dodge = false;
        yield return new WaitForSeconds(1.0f);
        _movement = Vector3.down * _smartEnemySpeed * Time.deltaTime;
        _dodge = true;
    }
    IEnumerator TripleLaserShotCooldown()
    {
        _tripleLaserShotTrigger = true;
        yield return new WaitForSeconds(3f);
        _tripleLaserShotTrigger = false;
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

                    _canFire = false;
                    _laserBool = false;
                    _smartEnemySpeed = 0;
                    _anim.SetTrigger("enemyDeathTrigger");
                    _spawnManager.enemiesLeft--;
                    _player.Damage();
                    _audioSource.Play();
                    Destroy(gameObject, 2.6f);
                }
            }
            
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _collider2D.enabled = !_collider2D.enabled;
                _laserBool = false;
                _canFire = false;
                _smartEnemySpeed = 0;
                _anim.SetTrigger("enemyDeathTrigger");
                _spawnManager.enemiesLeft--;
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
                _canFire = false;
                _laserBool = false;
                _spawnManager.enemiesLeft--;
                _smartEnemySpeed = 0;
                _anim.SetTrigger("enemyDeathTrigger");
                _player.AddScore(10);
                _audioSource.Play();
                Destroy(other.gameObject);
                Destroy(this.gameObject, 2.6f);
            }
        }
        if (other.tag == "GiantLaser")
        {
            if (_player != null)
            {
                _collider2D.enabled = !_collider2D.enabled;
                _canFire = false;
                _laserBool = false;
                _spawnManager.enemiesLeft--;
                _smartEnemySpeed = 0;
                _anim.SetTrigger("enemyDeathTrigger");
                _player.AddScore(10);
                _audioSource.Play();
                Destroy(this.gameObject, 2.6f);
            }

        }
    }

}
