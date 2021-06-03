using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Aircraft : MonoBehaviour
{
    private float rollInput;
    private float pitchInput;
    private float speed;
    private float minSpeed;
    private Vector3 movCam;
    private float bias;
    private Scene currentScene;
    private RaycastHit hit;
    private bool crashDown;
    private bool crashForward;
    private bool runUpdate;
    // Start is called before the first frame update
    void Start()
    {
        runUpdate = true;
        crashForward = false;
        crashDown = false;
        speed = 90.0f;
        minSpeed = 35.0f;
    }

    // Update is called once per frame
    void Update()
    {
        rollInput = Input.GetAxis("Horizontal");
        pitchInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (!runUpdate)
        {
            return;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            crashDown = checkRayHit(hit);
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            crashForward = checkRayHit(hit);
        }

        if (crashForward || crashDown)
        {
            Explode();
            runUpdate = false;
            return;
        }

        bias = 0.85f;
        movCam = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
        Camera.main.transform.position = Camera.main.transform.position * bias + movCam * (1.0f - bias);
        Camera.main.transform.LookAt(transform.position + transform.forward * 15.0f);
        transform.position += transform.forward * Time.deltaTime * speed;

        speed -= transform.forward.y * Time.deltaTime * 50.0f;

        transform.Rotate(pitchInput, 0.0f, -rollInput);

        if (speed < minSpeed)
        {
            speed = minSpeed;
        }
        
        
        
    }

    private bool checkRayHit(RaycastHit hit)
    {
        bool rayHit = false;
        float distanceToTerrain = hit.distance;

        if (distanceToTerrain < 3.0f)
        {
            Camera.main.transform.position -= transform.forward * 5.0f;
            speed = -speed;
            transform.position += transform.forward * Time.deltaTime * speed;
            rayHit = true;
        }

        return rayHit;
    }

    private void Explode()
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.Play();
        TrailRenderer[] trails = GetComponentsInChildren<TrailRenderer>();
        foreach (TrailRenderer trail in trails)
        {
            Destroy(trail, 0);
        }
        Invoke("RestartScene", ps.main.duration * 0.55f);
    }

    private void RestartScene()
    {
        currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
