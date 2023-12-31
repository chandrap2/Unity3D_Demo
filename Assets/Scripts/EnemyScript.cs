using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float speed;
    
    //private Collider collider;
    private CubicBezier bz3;
    private QuadBezier bz2;

    int dir = 1;

    private void Awake() {
        //collider = GetComponent<Collider>();

        bz3 = new(
            new Vector3(0, 0, 0),
            new Vector3(0, 2, 5),
            new Vector3(5, -1, 0),
            new Vector3(5, 3, 5)
        );

        bz2 = new(
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 8),
            new Vector3(10, 0, 0)
        );
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = bz.MoveAlongCurve(speed * Time.deltaTime);
        transform.position = bz3.MoveAlongCurve(dir * speed * Time.deltaTime);

        if (bz3.currTravelState != Bezier.TRAVEL_STATE.TRAVELLED)
            dir *= -1;
    }
}
