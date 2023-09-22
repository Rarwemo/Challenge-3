﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    private bool lowerLimit;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private float upperLimit = 15.0f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
      

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && lowerLimit)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);

        }
        if (transform.position.y >13)
        {
            lowerLimit = false;
        }
        else
        {
            lowerLimit = true;
        }
        //limits the ball from passing the upperlimit
        if (transform.position.y> upperLimit)
        {
            transform.position = new Vector3(transform.position.x, upperLimit, transform.position.z);
            playerRb.velocity = Vector2.zero;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            fireworksParticle.Stop();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
           
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            explosionParticle.Stop();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            playerRb.AddForce(Vector3.up*10, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, 1.5f);
        }

    }

}
