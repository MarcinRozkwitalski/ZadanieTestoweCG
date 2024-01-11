using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementManager : MonoBehaviour
{
    public bool canMove, isMoving, hasReachedDestination;

    public float InitialSpeed, CurrentSpeed, Manoeuvrability, Stamina;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public IEnumerator MoveToDestination(Vector3 position)
    {
        yield return new WaitForSeconds(1.0f);
    }

    public IEnumerator FollowLeader(CharacterInfo currentLeader)
    {
        yield return new WaitForSeconds(1.0f);
    }

    public void CheckIfAnyGroupMemberIsSlowerThanLeader(CharacterInfo currentLeader, float InitialSpeed)
    {

    }

    public void AdjustSpeedToSlowestGroupMember(CharacterInfo currentLeader, float CurrentSpeed)
    {

    }

    public void ResetCurrentSpeed(float InitialSpeed, float CurrentSpeed)
    {

    }

    public void DecreaseStaminaWhenMoving(float Stamina)
    {

    }

    public void RegenerateStaminaWhenIdle(float Stamina)
    {

    }

    public void StopEverybodyInGroup(CharacterInfo currentLeader)
    {

    }

    public void AlertLeaderAboutStaminaDepletion(CharacterInfo currentLeader)
    {

    }

    public IEnumerator AlertLeaderAboutFullStamina(CharacterInfo currentLeader, float Stamina)
    {
        yield return new WaitForSeconds(1.0f);
    }
}