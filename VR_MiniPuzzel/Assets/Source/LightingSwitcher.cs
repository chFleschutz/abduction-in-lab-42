using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LightingSwitcher : MonoBehaviour
{
    [SerializeField] private List<LevelLights> Level;    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetFirstLights());
    }
    IEnumerator SetFirstLights()
    {
        yield return new WaitForSeconds(1);
        SetLevelLights(0);
    }

    // Update is called once per frame
    public void SetLevelLights(int level)
    {
        Level[level].activate();
    }
}
