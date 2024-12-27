using UnityEngine;

public class MoveToObject : MonoBehaviour
{

    public GameObject Sphere;
    public GameObject Cube;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Sphere.transform.position = Vector3.MoveTowards(Sphere.transform.position, Cube.transform.position, speed);
    }
}
