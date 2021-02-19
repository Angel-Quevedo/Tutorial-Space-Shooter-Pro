using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] float duration = 3f;
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float limit;
    [SerializeField] int powerupId;
    [SerializeField] AudioClip _collectedAudioClip;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);

        if (transform.position.y < limit)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            switch (powerupId)
            {
                case 1:
                    player.ActivateTripleShot(duration);
                    break;
                case 2:
                    player.ActivateSpeed(duration);
                    break;
                case 3:
                    player.ActivateShields();
                    break;
                default:
                    break;
            }
            AudioSource.PlayClipAtPoint(_collectedAudioClip, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
