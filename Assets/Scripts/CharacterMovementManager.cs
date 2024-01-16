using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementManager : MonoBehaviour
{
    public bool canMove, isMoving, hasReachedDestination;

    public float InitialSpeed, CurrentSpeed, Manoeuvrability, Stamina;

    public Transform target;
    float speed = 5f;
    Vector3[] path;
    int targetIndex;

    public void MoveToDestination(Vector3 targetPosition)
    {
        PathRequestManager.RequestPath(transform.position, targetPosition, gameObject, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex ++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    public IEnumerator FollowLeader(CharacterInfo currentLeader)
    {
        yield return new WaitForSeconds(0.5f);

        while (gameObject.transform.position != currentLeader.transform.position)
        {
            MoveToDestination(currentLeader.transform.position);

            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("StopCoroutine inside FollowLeader");
        StopCoroutine("FollowLeader");
    }

    public void DequeuePathForCharacter(GameObject requester)
    {
        StopCoroutine("FollowPath");
        StopCoroutine("FollowLeader");
        StopAllCoroutines();
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

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i ++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i-1], path[i]);
                }
            }
        }
    }
}