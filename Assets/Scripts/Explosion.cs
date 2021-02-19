using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        var explosionAnimationLenght = _animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, explosionAnimationLenght);
    }
}
