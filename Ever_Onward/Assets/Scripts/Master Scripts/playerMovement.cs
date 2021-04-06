using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private int speed = 5;
    Vector3 movement;
    public SpriteRenderer spriteRenderer;
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        this.transform.position += movement*speed*Time.deltaTime;
        if (Input.GetAxis("Horizontal") > 0) spriteRenderer.sprite = right;
        if (Input.GetAxis("Horizontal") < 0) spriteRenderer.sprite = left;
        if (Input.GetAxis("Vertical") > 0) spriteRenderer.sprite = up;
        if (Input.GetAxis("Vertical") < 0) spriteRenderer.sprite = down;


    }
}
