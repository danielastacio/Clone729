using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance; 
    public List<Transform> spawnPoints = new List<Transform>();
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
