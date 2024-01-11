using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public string characterName;

    public int number;

    public bool isSelected = false, isPotentialLeader = false, isLeader = false;

    public CharacterInfo currentLeader;

    public GroupInfo groupInfo;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetRandomStats(float Speed, float Manoeuvrability, float Stamina)
    {

    }

    public void DisbandPreviousGroup(GroupInfo groupInfo)
    {

    }
}