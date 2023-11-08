using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Vector3 Direction = Vector3.down;
    private float _bossEntrySpeed = 1.5f;
    [SerializeField]
    private float _bossSpeed = 3.0f;
    private bool _enter = true;
    private bool _moving = false;
    private bool _goLeft = true;
    private bool _goRight = false;
    private Animator _animation;

    private Player _player;
    private UiManager _uiManager;
    private BossTurrets _rightTurret;
    private BossTurrets _leftTurret;

    [SerializeField]
    private GameObject _disableRightObject;
    [SerializeField]
    private GameObject _disableLeftTurret;
    [SerializeField]
    private GameObject _leftThruster;
    [SerializeField]
    private GameObject _rightThruster;
    private Collider2D _collider2d;

    private AudioSource _audio;
    private float _bossHealth = 20;


    // Start is called before the first frame update
    void Start()
    {

        _collider2d = GetComponent<Collider2D>();

        _audio = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null) 
        {
            Debug.Log("PLAYER HANDLE IN BOSS SCRIPT IS NULL");
        }

        _uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI MANAGER IS NULL");
        }

        _rightTurret = GameObject.Find("RightTurret").GetComponent<BossTurrets>();

        if (_rightTurret == null)
        {
            Debug.LogError("Right boss turret handle in the boss script is null!");
        }

        _leftTurret = GameObject.Find("LeftTurret").GetComponent<BossTurrets>();

        if (_leftTurret == null)
        {
            Debug.LogError("Left boss turret handle in the boss script is null!");
        }

        _animation = GetComponent<Animator>();

        _player._bossBorderTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (_enter == true)
        {
            MovetoMiddle();
        }

        if(_moving == true)
        { 
            Movement();
        }
    }

    private void MovetoMiddle()
    {
        if (transform.position.y >= 2)
        {
            transform.Translate(Direction * _bossEntrySpeed * Time.deltaTime);
        }

        else
        {  
            _uiManager.ActivateBossSlider();
            _moving = true;
            _enter = false;
            _rightTurret.BeginFiring();
            _leftTurret.BeginFiring();
        }
    }
    private void Movement()
    {

        if (transform.position.x >= 6.5f)
        {
            _goRight = false;
            _goLeft = true;
        }
        if (transform.position.x <= -6.5f)
        {
            _goRight = true;
            _goLeft = false;
        }

        if (transform.position.x < 6.5f && _goRight == true)
        {
            Direction = Vector3.right;
        }

        if (transform.position.x > -6.5f && _goLeft == true)
        {
            Direction = Vector3.left;
        }
        transform.Translate(Direction * _bossSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            _bossHealth--;
            _uiManager.UpdateBossHealth(_bossHealth);
            Destroy(other.gameObject);
        }

        if (other.tag == "HomingMissile")
        {
            _bossHealth = _bossHealth - 0.5f;
            _uiManager.UpdateBossHealth(_bossHealth);

        
        }

        if (_bossHealth == 0)
        {
            _collider2d.enabled = !_collider2d.enabled;
            _bossSpeed = 0;
            _audio.Play();
            DeactivateObjects();
            _player._bossBorderTrigger = false;
            _animation.SetTrigger("BossDeathTrigger");
            Destroy(this.gameObject, 2.6f);
        }
    }

    private void DeactivateObjects()
    {
        _disableLeftTurret.SetActive(false);
        _disableRightObject.SetActive(false);
        _rightThruster.SetActive(false);
        _leftThruster.SetActive(false);
    }
}
