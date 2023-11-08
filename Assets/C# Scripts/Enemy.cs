using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour

{
    //Number Variables
    [SerializeField]
    private float _speed = 4.0f;
    private float _randomX = 0;
    private float _laserCooldown;


    //Handles from other Scripts
    private Player _player;
    private Animator _anim;
    private SpawnManager _spawnManager;

    //Variables that access Information from the object
    private Collider2D _collider2D;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    [SerializeField]
    private GameObject _enemyTripleLaserPrefab;
    [SerializeField]
    private GameObject _enemyShield;

    //Bools
    private bool _laserTrigger = true;
    private bool _tripleLaserShotTrigger = false;
    private bool _shield = true;
    private bool _trackPlayer = true;

    //Ray casting variables
    private bool _rayCastTrigger;
    private bool _rayCanFire = true;

    //Aggressive Enemy Variables
    private float _distance;
    private float _ramSpeed = 2.5f;
    private float _ramRange = 3f;

    //Audio
    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        _enemyShield.SetActive(true);

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _audioSource = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();

        _collider2D = GetComponent<Collider2D>();


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
        EnemyLaser();
        RayShot();
        AggressiveTargeting();
        if (_rayCastTrigger == true)
        {
            ShootPowerUp();
        }
    }

    void EnemyMovement()
    {
        //Move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //When at the bottom of the screen, respawn at top
        BoundsCheck();
    }

    void BoundsCheck()
    {
        if (transform.position.y < -6)
        {
            _randomX = (Random.Range(-10.3f, 10.3f));
            transform.position = new Vector3(_randomX, 6.5f, 0);
        }
        if (transform.position.x > 10.7f || transform.position.x < -10.7f)
        {
            _randomX = (Random.Range(-10.3f, 10.3f));
            transform.position = new Vector3(_randomX, 6.5f, 0);
        }
    }

    void RayShot()
    {
        Debug.DrawRay(transform.position, new Vector3(0, -10, 0));
        RaycastHit2D[] Detected = Physics2D.RaycastAll(transform.position, new Vector3(0, -10, 0));
        foreach (RaycastHit2D hitDetect in Detected)
        {
            if (hitDetect.collider.CompareTag("Power Up"))
            {
                Debug.Log("DETECTED");
                _rayCastTrigger = true;
            }
        }
    }

    void AggressiveTargeting()
    {
        if (_trackPlayer == true)
        {
            _distance = Vector3.Distance(_player.transform.position, this.transform.position);
            if (_distance < _ramRange)
            {
                Vector3 Direction = this.transform.position - _player.transform.position;
                Direction = Direction.normalized;
                this.transform.position -= Direction * Time.deltaTime * _ramSpeed;
            }
        }


    }
    void ShootPowerUp()
    {
        if (_rayCanFire == true)
        {
            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            StartCoroutine(EnemyLaserCooldown());
        }
    }
    void EnemyLaser()
    {
        if (Time.time > _laserCooldown && _laserTrigger == true)
        {

            GameObject ToInstantiate = _tripleLaserShotTrigger == true ? _enemyTripleLaserPrefab : _enemyLaserPrefab;
            float _fireRate = Random.Range(1.5f, 2.0f);
            _laserCooldown = Time.time + _fireRate;
            GameObject EnemyLasers = Instantiate(ToInstantiate, transform.position, Quaternion.identity);
            Laser[] Lasers = EnemyLasers.GetComponentsInChildren<Laser>();
            for (int i = 0; i < Lasers.Length; i++)
            {
                Lasers[i].EnemyLaserTriggerTrue();
            }
        }
    }

    public void EnemyTripleLaserActive()
    {
        StartCoroutine(TripleLaserShotCooldown());
    }

    IEnumerator EnemyLaserCooldown()
    {
        _rayCastTrigger = false;
        _rayCanFire = false;
        yield return new WaitForSeconds(5.0f);
        _rayCanFire = true;
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
                _trackPlayer = false;
                _collider2D.enabled = !_collider2D.enabled;
                _enemyShield.SetActive(false);
                _laserTrigger = false;
                _speed = 0;
                _anim.SetTrigger("enemyDeathTrigger");
                _spawnManager.enemiesLeft--;
                _player.Damage();
                _audioSource.Play();
                Destroy(this.gameObject, 2.6f);

            }

        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_shield == true)
            {
                _enemyShield.SetActive(false);
                _shield = false;
                return;
            }

            if (_player != null)
            {
                _trackPlayer = false;
                _collider2D.enabled = !_collider2D.enabled;
                _laserTrigger = false;
                _speed = 0;
                _anim.SetTrigger("enemyDeathTrigger");
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
                _trackPlayer = false;
                _enemyShield.SetActive(false);
                _collider2D.enabled = !_collider2D.enabled;
                _laserTrigger = false;
                _spawnManager.enemiesLeft--;
                _speed = 0;
                _anim.SetTrigger("enemyDeathTrigger");
                _player.AddScore(10);
                _audioSource.Play();
                Destroy(this.gameObject, 2.6f);
            }

        }
        if (other.tag == "HomingMissile")
        {
            _trackPlayer = false;
            _enemyShield.SetActive(false);
            _collider2D.enabled = !_collider2D.enabled;
            _laserTrigger = false;
            _spawnManager.enemiesLeft--;
            _speed = 0;
            _anim.SetTrigger("enemyDeathTrigger");
            _player.AddScore(10);
            _audioSource.Play();
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.6f);
        }

    }

}