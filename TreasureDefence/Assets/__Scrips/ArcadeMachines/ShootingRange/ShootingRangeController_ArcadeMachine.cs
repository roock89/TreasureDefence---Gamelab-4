using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ShootingRangeController_ArcadeMachine : ArcadeMachine
{   // Mikkel tried to make this.
    // Henrik Revamped this
    public GameObject Target;
    [SerializeField] Rigidbody targetRB;
    public int HitTarget;
    [Tooltip("The Speed of the Target")]
    public float TargetMovementSpeed;
    public float t, StartTimer;
    float timeLeft;
    [SerializeField] Transform leftPos, rightPos;
    bool leftHasChanged, rightHasChanged;
    Vector3 velocity;
    [SerializeField] GameObject startGameBottle;
    [SerializeField] Animator animator;
    [SerializeField] Transform rugTransform;
    [SerializeField] VR_PlayerController VRPlayer;
    bool holdingGun, shouldTeleport;
    [SerializeField] StudioEventEmitter BottleHit;
    [SerializeField] GameObject BottleShardsParticle, BottleHalfPrefab;
    [SerializeField] Vector3 brokenBottleVelocity;

    // Base Arcade Behaviour
    // Start the Game. What it costs.
    public override void StartSetup()
    {
        base.StartSetup();
        // Teleport To rug, disable movement
        BottleHit.Play();
        shouldTeleport = true;
        // arcade setup
        HitTarget = 0;
        timeLeft = StartTimer;
        randomizeLeftPos();
        randomizeRightPos();
        float random = Random.Range(0,1);
        if(random == 0)
            velocity = new Vector3(0, 0, TargetMovementSpeed);
        else
            velocity = new Vector3(0, 0, -TargetMovementSpeed);
        startGameBottle.SetActive(false);
        Target.SetActive(true);
    }

    public override void isPlayingUpdate()
    {
        t += TargetMovementSpeed * Time.deltaTime;
        Vector3 pos = Target.transform.localPosition;

        if(pos.z < leftPos.localPosition.z)
            randomizeRightPos();
        if(pos.z > rightPos.localPosition.z)
            randomizeLeftPos();
        targetRB.velocity = velocity;
        timeLeft -= Time.deltaTime;
    }

    void LateUpdate()
    {
        if(shouldTeleport)
        {
            VRPlayer.transform.position = rugTransform.position;
            VRPlayer.transform.rotation = rugTransform.rotation;
            shouldTeleport = false;
            VRPlayer.canMove = false;
            VRPlayer.canTeleport = false;
        }
    }


    public override bool WinCondition()
    {

        if (HitTarget >= 5)
        {
            return true;
        }
        return false;
    }

    public override bool LooseCondition()
    {
        // Debug.Log("Time ran out.");
        if (timeLeft <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Reward()
    {
        GameManager.instance.SpawnTower(towerRewardPrefab, spawnPoint.position);
        Instantiate(BottleShardsParticle, Target.transform.position, Quaternion.identity);
        GameObject bottle = Instantiate(BottleHalfPrefab, Target.transform.position, Quaternion.identity);
        Rigidbody rb = bottle.GetComponent<Rigidbody>();
        rb.velocity = brokenBottleVelocity;
        // Debug.Break();
    }

    public override void Reset()
    {
        HitTarget = 0;
        targetRB.velocity = new Vector3(0, 0, 0);
        startGameBottle.SetActive(true);
        Target.SetActive(false);
        if(animator.GetBool("IsOpen") && !holdingGun)
        {
            animator.SetBool("IsOpen", false);
        }
        VRPlayer.canMove = true;
        VRPlayer.canTeleport = true;
    }

    public void Hit()
    {
        HitTarget++;
        BottleHit.Play();
        Instantiate(BottleShardsParticle, Target.transform.position, Quaternion.identity);
    }

    //While Playing.  Damit Rune.
    void randomizeLeftPos()
    {
        if(!leftHasChanged)
        {
            Vector3 pos = leftPos.localPosition;
            pos.z = Random.Range(-1.0f, 0.0f);
            leftPos.localPosition = pos;
            leftHasChanged = true;
            rightHasChanged = false;
            velocity = new Vector3(0, 0, -TargetMovementSpeed);
        }
    }

    void randomizeRightPos()
    {
        if(!rightHasChanged)
        {
            Vector3 pos = rightPos.localPosition;
            pos.z = Random.Range(0.0f, 1.0f);
            rightPos.localPosition = pos;
            rightHasChanged = true;
            leftHasChanged = false;
            velocity = new Vector3(0, 0, TargetMovementSpeed);
        }
    }
    
    public void setIsOpen(bool state)
    {
        if(!isPlaying)
            animator.SetBool("IsOpen", state);
    }

    public void isHoldingGun(bool state)
    {
        holdingGun = state;
    }
}