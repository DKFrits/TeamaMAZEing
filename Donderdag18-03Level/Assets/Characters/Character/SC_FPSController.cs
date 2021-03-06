﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{

    public int health;

    public Text healthTxt;

    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float rayDistance = 10f;
    public Light flashlight;
    public GameObject NESController;
    private int zombiesTurned = 0;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        HUDInit();

        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }


        // GAME OVER criteria
        if (health <= 0)
        {
            SceneManager.LoadScene("GAMEOVER");
        }

        Interact();
        Flashlight();
    }

    public void reduceHealthByOne()
    {
        health -= 1;
        UpdateHealthBar();
    }

    public void addOneHealth()
    {
        health += 1;
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        var newText = "health: " + health;
        healthTxt.GetComponent<Text>().text = newText;
    }

    private void HUDInit()
    {
        var newText = "health: " + health;
        healthTxt.GetComponent<Text>().text = newText;
    }

    public void AddController()
    {
        Debug.Log("xd");
        NESController.SetActive(true);
    }

    private void Interact()
    {
        if (Input.GetButtonDown("Submit"))
        {
            var ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.transform.tag == "Door")
                {
                    Animator doorAnimator = hit.collider.GetComponentInParent<Animator>();
                    doorAnimator.SetBool("open", !doorAnimator.GetBool("open"));
                }

                if (hit.transform.tag == "Zombie")
                {
                    if (NESController.activeSelf)
                    {
                        var zombieObject = hit.collider.GetComponent<Zombie>();
                        zombieObject.TurnZombie();
                        zombiesTurned++;
                        if (zombiesTurned > 1)
                        {
                            healthTxt.text = "YOU WIN!";
                        }
                    }
                }
            }
        }
    }

    private void Flashlight()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}