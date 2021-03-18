using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : Interactable
{

    public readonly string itemName = "NEScontroller";

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
        Debug.Log("picking up " + itemName);
        AddControllerToPlayer();
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

    private void AddControllerToPlayer()
    {
        var playerScript = player.GetComponent<SC_FPSController>();
        playerScript.AddController();
    }

}
