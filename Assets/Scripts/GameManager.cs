using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterInfo characterOne, characterTwo, characterThree, characterFour;

    public CanvasManager canvasManager;

    public Vector3 destinationForCurrentGroupPosition;

    public List<CharacterInfo> newGroupBuffor = new List<CharacterInfo>();

    public List<GroupInfo> existingGroups = new List<GroupInfo>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnMapClick()
    {
        
    }

    public void ConvertClickToWorldPostion()
    {

    }

    public void MakeNextMemberInPotentialGroupALeader()
    {
        newGroupBuffor[0].isPotentialLeader = true;
        canvasManager.ShowCrownForNextPotentialLeader(newGroupBuffor[0]);
    }

    public void AddCharacterToPotentialGroup(CharacterInfo addCharacter)
    {
        if (!newGroupBuffor.Contains(addCharacter)) 
            newGroupBuffor.Add(addCharacter);
    }

    public void RemoveCharacterFromPotentialGroup(CharacterInfo removeCharacter)
    {
        if (newGroupBuffor.Contains(removeCharacter)) 
            newGroupBuffor.Remove(removeCharacter);
    }

    public void MakeNewGroupOnMapClick(GroupInfo groupInfo)
    {
        
    }
}