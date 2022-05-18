using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public event System.Action OnSpaceShipDeathAction;
    public float speed = 4;
    public int health = 100;

    public float smoothMoveTime = .1f;

    private float smoothInputMagnitude = 0;
    private float smoothMoveVolecity = 0;

    private Vector3 velocity;

    private GameManager gameManager;
    private bool smokeOn = false;

    Vector3 endpoint;
    new Rigidbody rigidbody;
    ShipStatus shipStatus;

    ParticleSystem smokePs;

    public enum ShipStatus
    {
        Alive,
        Dead,
        Won
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        rigidbody = GetComponent<Rigidbody>();
        // endpoint = new Vector3(0, 0, gameManager.spaceDepth);

        shipStatus = ShipStatus.Alive;

        smokePs = transform.GetChild(0).GetComponent<ParticleSystem>();
        smokePs.Pause();
        smokePs.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (shipStatus == ShipStatus.Alive)
        {
            if (health <= 0)
            {
                OnDeath();
            }

            Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

            float inputMag = inputDirection.magnitude;

            smoothInputMagnitude = Mathf.SmoothDamp(smoothInputMagnitude, inputMag, ref smoothMoveVolecity, smoothMoveTime);

            velocity = inputDirection * speed * smoothInputMagnitude;


            // transform.Translate(velocity * Time.deltaTime, Space.World);
            transform.LookAt(new Vector3(0, 0, gameManager.spaceDepth));
        }
            
                
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Metor")
        {
            GetComponent<Renderer>().material.color = Color.red;

            Metor metor = other.GetComponent<Metor>();

            if (metor)
            {
                LoseHealth(metor.damage);
                Debug.Log("damage: "+metor.damage);
                ActivateSmoke();
            }

            Debug.Log("Hit Metor");
        }
    }

    void ActivateSmoke()
    {
        if (!smokeOn)
        {
            smokeOn = true;
            smokePs.Play();
            smokePs.gameObject.SetActive(true);
        }
    }

    private void LoseHealth(int amount)
    {
        if (health - amount >=0)
        {
            health -= amount;
        }
        else
        {
            health = 0;
        }
    }

    private void OnDeath()
    {
        shipStatus = ShipStatus.Dead;
        OnSpaceShipDeathAction?.Invoke();

        Destroy(gameObject);
    }
}
