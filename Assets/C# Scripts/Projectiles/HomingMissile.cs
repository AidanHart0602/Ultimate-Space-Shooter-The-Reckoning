using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private float _homingSpeed = 3.0f;
    [SerializeField]
    private GameObject[] _enemies;
    private GameObject _target = null;
    private float _minDistance;
    private Vector3 _currentPosition;
    // Start is called before the first frame update
    void Start()
    {
        _target = FindClosestEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        ChaseTarget();
        Movement();
    }

    private GameObject FindClosestEnemy()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");

        _minDistance = Mathf.Infinity;
        _currentPosition = transform.position;

        foreach (GameObject enemy in _enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, _currentPosition);
            if (distance < _minDistance)
            {
                _target = enemy;
                _minDistance = distance;
            }
        }
        return _target;
    }

    private void ChaseTarget() 
    {
        if (_target != null) 
        {
            if (Vector3.Distance(transform.position, _target.transform.position) != 0) 
            {
                transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _homingSpeed * Time.deltaTime);

                Vector2 Direction = (transform.position - _target.transform.position).normalized;
                var Angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
                var offset = 90f;
    
                transform.rotation = Quaternion.Euler(Vector3.forward * (Angle + offset));

                if (_target == null)
                {
                    _target = FindClosestEnemy();
                }
            }
        }
    }

    private void Movement() 
    {
        transform.Translate(Vector3.up * _homingSpeed * Time.deltaTime);
        if (transform.position.y >= 6f || transform.position.y <= -6f)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.x >= 11f || transform.position.x <= -11f)
        { 
            Destroy(this.gameObject);
        }
    }
}
