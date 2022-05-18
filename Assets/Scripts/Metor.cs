using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metor : MonoBehaviour
{
    private float speed;
    public int damage = 40;

    public Vector2 speedMinMax = new Vector2(1, 6);
    public Vector2 sizeMinMax = new Vector2(0.3f, 6);

    private Vector3 velocity;
    private Vector3 regularSize;
    private Vector3 depthVelocity;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        transform.position = new Vector3(0, 0, gameManager.spaceDepth);
        this.speed = Random.Range(speedMinMax.x, speedMinMax.y);

        float maxHeight = 2 * Mathf.Tan(0.5f * Camera.main.fieldOfView * Mathf.Deg2Rad) * (gameManager.spaceShipZ - gameManager.SpaceBaseZ);
        float maxWidth = 2 * Mathf.Tan(0.5f * Camera.VerticalToHorizontalFieldOfView( Camera.main.fieldOfView, Camera.main.aspect ) * Mathf.Deg2Rad) * (gameManager.spaceShipZ - gameManager.SpaceBaseZ);

        float maxTheta = Mathf.Atan(maxWidth/ 2 / gameManager.spaceDepth);
        float maxPhi = Mathf.Atan(maxHeight/ 2 / gameManager.spaceDepth);


      /*  Debug.Log("maxTheta: " + maxTheta);
        Debug.Log("maxPhi: " + maxPhi);*/

        float theta = Random.Range(-maxTheta, maxTheta);
        float phi = Random.Range(-maxPhi, maxPhi);

        depthVelocity = new Vector3(Mathf.Sin(theta) * Mathf.Cos(phi), Mathf.Sin(phi), -Mathf.Cos(theta) * Mathf.Cos(phi));

        velocity = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));

        velocity = velocity.normalized;

        // round 
        regularSize = Random.Range(sizeMinMax.x, sizeMinMax.y) * Vector3.one;

        /*regularSize = new Vector3( Random.Range(sizeMinMax.x, sizeMinMax.y), 
            Random.Range(sizeMinMax.x, sizeMinMax.y), 
            Random.Range(sizeMinMax.x, sizeMinMax.y));*/

        float depth = transform.position.z - gameManager.spaceShipZ;
        SetSizeByDepth(depth);
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 transformVelocity = velocity * speed + gameManager.metorDepthSpeed * Vector3.back;
        Vector3 transformVelocity = gameManager.metorDepthSpeed * depthVelocity;

        float depth = transform.position.z - gameManager.spaceShipZ;
        SetSizeByDepth(depth);

        transform.Translate(transformVelocity * Time.deltaTime, Space.World);

        if (transform.position.z + transform.localScale.z < gameManager.SpaceBaseZ)
        {
            Destroy(gameObject);
        }
    }

    void SetSizeByDepth(float depth)
    {
        float sizeRatio = gameManager.GetSizeRatio(depth);
        transform.localScale = regularSize * sizeRatio;
    }
}
