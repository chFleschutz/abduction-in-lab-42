using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Post : MonoBehaviour
{
    public List<PostData> data = new List<PostData>();
    public UnityEvent OnPostWasReached;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Transform GetNextPost()
    {
        if(data.Count == 0)
        {
            return null;
        }

        if(data.Count == 1)
        {
            return data[0].Transform;
        }

        int totalProbability = 0;
        foreach (PostData data in data)
        {
            totalProbability += data.Probability;
        }

        int selection = UnityEngine.Random.Range(0, totalProbability);

        foreach(PostData data in data)
        {
            selection -= data.Probability;
            if (selection <= 0) return data.Transform;
        }

        return null;
    }

    public void PostWasReached()
    {
        OnPostWasReached.Invoke();
    }
}
