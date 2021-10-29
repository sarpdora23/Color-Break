using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager level_Manager = null;
    [SerializeField]
    private List<Floor> floors = new List<Floor>();
    private Material _mat1, _mat2;
    private int floor_destroy_count;
    [SerializeField]
    private GameObject floor_go;
    [SerializeField]
    private Transform level_parent; 
    [SerializeField]
    private GameObject invinsible_prefab;
    public static LevelManager level_Manager_Instance
    {
        get
        {
            if (level_Manager == null)
            {
                level_Manager = new GameObject("GameManager").AddComponent<LevelManager>();
            }
            return level_Manager;
        }
    }
    private void OnEnable()
    {
        level_Manager = this;
    }
    public void GenerateLevel()
    {
        floor_destroy_count = 0;
        foreach (Floor floor  in floors)
        {
            floor.SetFloorMaterial(_mat1,_mat2);
        }
    }
    public void SetLevelManager(Material mat1, Material mat2)
    {
        _mat1 = mat1;
        _mat2 = mat2;
    }
    public void DestroyFloor(Floor.Cube_Position_Type cube_Position_Type)
    {
        floors.ToArray()[0].DestroyCubes(cube_Position_Type);
        GameObject old_floor = floors[0].gameObject;
        floors.Remove(floors.ToArray()[0]);
        StartCoroutine(DestroyDelay(old_floor));
        floor_destroy_count++;
        UIManager.ui_Manager_Instance.UpdateScore(floor_destroy_count);
        if (floor_destroy_count % 3 == 0)
        {
            if(!PlayerManager.player_Manager_Instance.IsInvinsible()){
                PlayerManager.player_Manager_Instance.FasterSpeed();
            }
        }
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            DestroyFloor(Floor.Cube_Position_Type.LEFT);
        }
    }
    public void GenerateNewFloor()
    {
        Vector3 new_pos = floors.ToArray()[floors.Count - 1].transform.position;
        new_pos.y -= 6;
        int inv = Random.Range(0,13);
        GameObject new_floor = GameObject.Instantiate(floor_go, new_pos, Quaternion.identity);
        new_floor.transform.parent = level_parent;
        new_floor.name = "Floor" + (floor_destroy_count + 10);
        new_floor.transform.localScale = Vector3.one;
        Floor new_floor_script = new_floor.GetComponent<Floor>();
        if(inv == 1){
            if(!PlayerManager.player_Manager_Instance.IsInvinsible()){
                GameObject invinsible_object = GameObject.Instantiate(invinsible_prefab, new_floor.transform);
                invinsible_object.transform.SetParent(new_floor.transform);
                invinsible_object.transform.position = new_floor_script.GetInvinsibleSpawnPosition(Random.Range(0,2));
                
            }
        }
        floors.Add(new_floor_script);
        new_floor_script.SetFloorMaterial(_mat1, _mat2);
    }
    IEnumerator DestroyDelay(GameObject old_go)
    {
        yield return new WaitForSeconds(1.8f);
        GameObject.Destroy(old_go);
    }
}
