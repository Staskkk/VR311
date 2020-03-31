using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerScript : MonoBehaviour
{
    public Cluster cluster;

    public Material templateMaterial;

    private Material material;

    public Color color;

    public float width;

    public float height;

    public void SetParams(Color color, float width, float height)
    {
        this.color = color;
        this.width = width;
        this.height = height;
        material.color = color;
        transform.localScale = new Vector3(transform.localScale.x * width, transform.localScale.y * height, transform.localScale.z * width);
    }

    void Awake()
    {
        material = new Material(templateMaterial);
        gameObject.GetComponent<Renderer>().material = material;
    }
}
