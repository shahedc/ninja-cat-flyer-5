﻿using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;

    public float interval = 0.5f;
    public float rotationSpeed = 0.0f;

    private bool isLaserOn = true;
    private float timeUntilNextToggle;

	// Use this for initialization
	void Start () {
        timeUntilNextToggle = interval;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        timeUntilNextToggle -= Time.fixedDeltaTime;

        if (timeUntilNextToggle <= 0)
        {

            isLaserOn = !isLaserOn;
            GetComponent<Collider2D>().enabled = isLaserOn;

            SpriteRenderer spriteRenderer = ((SpriteRenderer)this.GetComponent<Renderer>());
            if (isLaserOn)
                spriteRenderer.sprite = laserOnSprite;
            else
                spriteRenderer.sprite = laserOffSprite;

            timeUntilNextToggle = interval;
        }
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.fixedDeltaTime);
    }
}
