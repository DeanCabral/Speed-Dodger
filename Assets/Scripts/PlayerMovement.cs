using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private GameManager GM;
    private GameObject endPortal;
    public float defaultSpeed;
    public float forwardSpeed;
    public float turnSpeed;

	// Use this for initialization
	void Start () {

        GM = FindObjectOfType<GameManager>();
        endPortal = GameObject.Find("EndPortal");        

        defaultSpeed = 20.0f;
        turnSpeed = 15.0f;
        forwardSpeed = defaultSpeed;
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.name == "EndPortal")
        {
            transform.position = new Vector3(transform.position.x, 1, -130);
            GM.NextWave();
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Obstacle")
        {
            StartCoroutine(GM.DecreaseLife());
        }
    }

    // Update is called once per frame
    void Update () {

        if (!GM.IsDead)
        {
            MoveForward();
            CheckInput();
        }        
    }

    private void MoveForward()
    {
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
    }

    private void CheckInput()
    {

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * turnSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * turnSpeed * Time.deltaTime;
        }
    }

    public void Respawn()
    {
        transform.position = new Vector3(0, 1, 2);
    }
}
