using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    protected float mp;
    [SerializeField]
    public float maxHp;
    [SerializeField] 
    public float maxMp;
    public float atk;
    public string unitName;
    public Animator animator;
    protected Rigidbody rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        hp = maxHp;
        mp = maxMp;
    }

}
