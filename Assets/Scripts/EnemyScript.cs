using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Collider collider;
    private CubicBezier bz;
    private Vector3[] path;
    private int currI;

    private void Awake() {
        collider = GetComponent<Collider>();

        bz = new(
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 5),
            new Vector3(5, 2, 0),
            new Vector3(5, 3, 5)
        );
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = bz.MoveAlongCurve(speed * Time.deltaTime, transform.position);
    }
}
