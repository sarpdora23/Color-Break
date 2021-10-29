using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private Floor.Cube_Position_Type cube_Position_Type;
    private MeshRenderer meshRenderer;
    public Rigidbody rb_body;
    public bool is_small;
    public Small_Cube_Part small_cube_part;
    public enum Small_Cube_Part
    {
        ONE,
        TWO
    }
    private void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }
    public Floor.Cube_Position_Type GetCubePosition()
    {
        return cube_Position_Type;
    }
    public void SetCubeMaterial(Material material)
    {
        meshRenderer.material = material;      
    }
}
