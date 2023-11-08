using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurrets : MonoBehaviour
{
    //Other variables
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private bool _beginAttack = false;
    private float _laserCooldown = 2.5f;

    //Variables for the turret rotation
    [SerializeField]
    private Player _player;
    [SerializeField]
    private float _turretRotationSpeed = 1.0f;
    [SerializeField]
    private float _rotationModifier;
    private float _angle;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Update()
    {
        if (_beginAttack == true)
        {
            RotateTurrets();
            FireLasers();
        }
    }

    void RotateTurrets()
    {
        if (_player != null)
        {
            Vector3 LockOnTarget = _player.transform.position - transform.position;
            _angle = Mathf.Atan2(LockOnTarget.y, LockOnTarget.x) * Mathf.Rad2Deg - _rotationModifier;
            Quaternion forward = Quaternion.AngleAxis(_angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, forward, Time.deltaTime * _turretRotationSpeed);
        }
    }
    void FireLasers() 
    {
        if (Time.time > _laserCooldown)
        {
            float fireRate = 2.0f;
            _laserCooldown = Time.time + fireRate;
            Instantiate(_enemyLaserPrefab, transform.position, transform.rotation);
        }
    }

    public void BeginFiring()
    {
        _beginAttack = true;
    }

    public void StopFiring()
    {
        _beginAttack = false;
    }
}
