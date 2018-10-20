using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerabound : MonoBehaviour {
    public BoxCollider2D bound;
    private Vector3 minBound;
    private Vector3 maxBound;
    private float height;
    private float width;
    public Transform lookat;
    public float x;
    public float y;
    // Use this for initialization
	void Start () {
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        height = gameObject.GetComponent<Camera>().orthographicSize;
        width = height * Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
       /* Vector3 delta = Vector3.zero;
        float dx = lookat.position.x - transform.position.x;
        if (Mathf.Abs(dx) > x)
        {
            if (transform.position.x < lookat.position.x)
            {
                delta.x = dx - x;
            }
            else
            {
                delta.x = dx + x;
            }
        }
        float dy = lookat.position.y - transform.position.y;
        if (Mathf.Abs(dy) > y)
        {
            if (transform.position.y < lookat.position.y)
            {
                delta.y = dy - y;
            }
            else
            {
                delta.y = dy + y;
            }
        }

        transform.position = transform.position + delta;*/
        float clampX = Mathf.Clamp(transform.position.x, minBound.x + width, maxBound.x - width);
        float clampY = Mathf.Clamp(transform.position.y, minBound.y + height, maxBound.y - height);
        transform.position = new Vector3(clampX, clampY, transform.position.z);
    }
}
