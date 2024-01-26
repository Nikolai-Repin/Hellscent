using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Item is required to have the component RigidBody2D to be able to be picked up
[RequireComponent(typeof(Rigidbody2D))]
public class UI_Items : MonoBehaviour
{
    public ItemData data;

    [HideInInspector] public Rigidbody2D rb2d;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
    }
}