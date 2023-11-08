using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _powerUpBaseSpeed = 3.0f;
    [SerializeField]
    private float _powerUpCurrentSpeed = 3.0f;

    [SerializeField]
    private int powerUpID;
    [SerializeField]
    private AudioClip _collectedAudio;
    private Enemy _enemy;

    // Update is called once per frame    

    void Update()
    { 
       transform.Translate(Vector3.down * _powerUpCurrentSpeed * Time.deltaTime);

        if(transform.position.y < -7)
        {
            Destroy(gameObject);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _powerUpCurrentSpeed *= 2f;
        }
        
        else if (Input.GetKeyUp(KeyCode.C))
        {
            _powerUpCurrentSpeed = _powerUpBaseSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            AudioSource.PlayClipAtPoint(_collectedAudio, transform.position);
            switch (powerUpID)
            {
                case 0:
                    player.TripleShotActive();   
                    break;
                case 1:
                    player.SpeedBoostActive();
                    break;
                case 2:
                    player.ShieldActive();
                    break;
                case 3:
                    TripleLaserActive();
                    break;
                case 4:
                    player.AmmoPowerUpActive();
                    break;
                case 5:
                    player.MedKitActive();
                    break;
                case 6:
                    player.GiantLaserActive();
                    break;
                case 7:
                    player.HomingMissileActive();
                    break;
                default:
                    Debug.Log("Default value");
                    break;
            }
            Destroy(this.gameObject);
        }
        if (other.CompareTag("ELaser")) 
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void TripleLaserActive()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].EnemyTripleLaserActive();
        }
    }

}