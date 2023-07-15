using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArmSequenceController : MonoBehaviour
{
    private Animator animator;

    private List<FilterType> alreadyHitFilters = new List<FilterType>();

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTargetHit(FilterType type)
    {
        if (alreadyHitFilters.Contains(type)) 
            return;

        alreadyHitFilters.Add(type);
        animator.SetTrigger("TrNextState");
    }

    public void SetDiamondHit()
    {
        SetTargetHit(FilterType.Diamond);
    }

    public void SetEmeraldHit()
    {
        SetTargetHit(FilterType.Emerald);
    }

    public void SetRubyHit()
    {
        SetTargetHit(FilterType.Ruby);
    }

    public void SetTopasHit()
    {
        SetTargetHit(FilterType.Topas);
    }
}
