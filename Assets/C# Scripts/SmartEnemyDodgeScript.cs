using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemyDodgeScript : MonoBehaviour
{
    private SmartEnemy _smartEnemy;
    private bool _cooldown = false;

    void Start()
    {
        _smartEnemy = GetComponentInParent<SmartEnemy>();
        if (_smartEnemy == null)
        {
            Debug.LogError("HANDLE FOR THE SMART ENEMY IN THE DODGE SCRIPT IS NULL!");
        }
    }

    private void OnTriggerEnter2D(Collider2D dodge)
    {
        if (dodge.tag == "Laser" || dodge.tag == "HomingMissile")
        {
            if (_cooldown == false)
            {
                _smartEnemy.enableDodge();
            }
        }
    }
}
