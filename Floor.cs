using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Floor : MonoBehaviour
{
    [SerializeField]
    List<Cube> floor_cube_list = new List<Cube>();
    private Animator anim;
    private Dictionary<Cube_Position_Type, Material> material_dictionary = new Dictionary<Cube_Position_Type, Material>();
    [SerializeField]
    private Transform invinsible_left,invinsible_right;
    private void Awake() {
        anim = gameObject.GetComponent<Animator>();
    }
    private void Start()
    {
        DOTween.Init();
    }
    public enum Cube_Position_Type
    {
        LEFT,
        RIGHT
    }
    public void SetFloorMaterial(Material material_left, Material material_right)
    {
        int i = Random.Range(1, 3);
        if (i == 1)
        {
            material_dictionary.Add(Cube_Position_Type.LEFT, material_left);
            material_dictionary.Add(Cube_Position_Type.RIGHT, material_right);
        }
        else
        {
            material_dictionary.Add(Cube_Position_Type.LEFT, material_right);
            material_dictionary.Add(Cube_Position_Type.RIGHT, material_left);
        }
        SetCubesMaterial();
    }
    private void SetCubesMaterial()
    {
        foreach (Cube cube in floor_cube_list)
        {
            
            if (cube.is_small)
            {
                cube.gameObject.SetActive(true);
                cube.SetCubeMaterial(material_dictionary[cube.GetCubePosition()]);
                cube.gameObject.SetActive(false);
            }
            else
            {
                cube.SetCubeMaterial(material_dictionary[cube.GetCubePosition()]);
            }
        }
    }
    public void DestroyCubes(Cube_Position_Type broken_part)
    {
        if(broken_part == Cube_Position_Type.LEFT){
            anim.Play("LeftBreak");
        }
        else if(broken_part == Cube_Position_Type.RIGHT){
            anim.Play("RightBreak");
        }   
    }
    public Vector3 GetInvinsibleSpawnPosition(int rand){
        if(rand == 1){
            return invinsible_left.position;
        }
        else{
            return invinsible_right.position;
        }
    }
}
