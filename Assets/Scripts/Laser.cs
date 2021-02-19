using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed = 8f;

    [SerializeField] 
    private int _topLimit = 8;

    [SerializeField] bool _invertMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var direction = Vector3.up;

        if (_invertMovement)
            direction *= -1;

        transform.Translate(direction * _movementSpeed * Time.deltaTime);

        if (_invertMovement)
        {
            if (transform.position.y < _topLimit * -1)
                DestroyMe();
        }
        else
        {
            if (transform.position.y > _topLimit)
                DestroyMe();
        }

    }

    private void DestroyMe()
    {
        if (transform.parent != null)
            Destroy(transform.parent.gameObject);

        Destroy(gameObject);
    }
}
