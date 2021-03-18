using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{

    public string itemName;

    private AudioSource audioSource;
    public AudioClip itemPickUpSound;

    public GameObject player;

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    public virtual void PickUp()
    {
        AddHealthToPlayer();
        PlayPickupSound();
        Destroy(gameObject);
        
    }

    void PlayPickupSound()
    {
        var gameManager = GameObject.Find("GameManager");
        var audioSource = gameManager.GetComponent<AudioSource>();
        audioSource.clip = itemPickUpSound;
        audioSource.Play();
    }

    private void AddHealthToPlayer()
    {
        var playerScript = player.GetComponent<SC_FPSController>();
        playerScript.addOneHealth();
    }

}
