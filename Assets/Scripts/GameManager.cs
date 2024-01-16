using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public CharacterInfo characterOne, characterTwo, characterThree, characterFour;

    public CanvasManager canvasManager;

    public Vector3 destinationForCurrentGroupPosition;

    static GameManager instance;

    public List<CharacterInfo> newGroupBuffor = new List<CharacterInfo>();

    public List<GroupInfo> existingGroups = new List<GroupInfo>();

    private void Awake() 
    {
        instance = this;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        ConvertClickToWorldPostion();
    }

    public void OnMapClick()
    {
        
    }

    public void ConvertClickToWorldPostion()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 worldPosition = hit.point;

                    CharacterInfo leader = newGroupBuffor[0];

                    foreach (CharacterInfo characterInfo in newGroupBuffor)
                    {
                        CharacterMovementManager characterMovementManager = characterInfo.GetComponent<CharacterMovementManager>();
                        Debug.Log("StopAllCoroutines()");
                        characterMovementManager.StopAllCoroutines();
                        characterMovementManager.DequeuePathForCharacter(characterMovementManager.gameObject);

                        if (characterInfo.isPotentialLeader)
                        {
                            characterMovementManager.MoveToDestination(worldPosition);
                        }
                        else
                        {
                            Debug.Log("StopStartCoroutine(FollowLeader)");
                            StartCoroutine(characterMovementManager.FollowLeader(leader));
                        }
                    }
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Main camera not found.");
            }
        }
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