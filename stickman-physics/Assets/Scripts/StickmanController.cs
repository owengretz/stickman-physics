using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanController : MonoBehaviour
{
    public bool dummy;
    public int playerNumber;


    [Header("input")]
    private string horizontalAxisName;
    private float horizInput;
    private string verticalAxisName;
    private float vertInput;

    [HideInInspector] public string leftClickName;

    [HideInInspector] public string rightClickName;

    [HideInInspector] public string useWeaponButtonName;

    private string jumpButtonName;

    [HideInInspector] public string horizControllerHandAimName;
    [HideInInspector] public string vertControllerHandAimName;

    //private string jumpButtonName;

    //private string duckButtonName;


    [Header("walking")]
    public _Muscle[] muscles;
    public Rigidbody2D rightLeg;
    public Rigidbody2D leftLeg;
    public Rigidbody2D torso;

    public Vector2 walkVector;
    private Vector2 walkRightVector;
    private Vector2 walkLeftVector;

    public float walkInterpValue;
    public float walkInterpThreshold;

    private bool stepRightLeg;
    private float moveDelayPointer;
    public float moveDelay;


    [Header("jumping")]
    public float jumpVelocity;
    public float airHandling;
    public float jumpCheckDist;

    [HideInInspector] public bool grounded = true;


    [Header("ducking")]
    public float duckForce;
    private bool ducking;
    private float[] originalBodyRotations = new float[6];
    private float[] duckingBodyRotations = new float[6];


    [Header("knockout")]
    public float knockoutTime;
    [HideInInspector] public bool ragdoll = false;
    private List<float> muscleValues = new List<float>();


    [Header("other")]
    public GameObject testCube;

    public Collider2D[] cols;
    private GameObject[] oneWayPlatforms;
    private bool fallThroughOneWayPlatforms;


    private void Awake()
    {
        horizontalAxisName = "horizontal" + playerNumber;
        verticalAxisName = "vertical" + playerNumber;
        leftClickName = "left click" + playerNumber;
        rightClickName = "right click" + playerNumber;
        useWeaponButtonName = "use weapon" + playerNumber;
        jumpButtonName = "jump" + playerNumber;
        horizControllerHandAimName = "right stick horiz" + playerNumber;
        vertControllerHandAimName = "right stick vert" + playerNumber;


        walkRightVector = walkVector;
        walkLeftVector = new Vector2(-walkVector.x, walkVector.y);

        for (int i = 0; i < muscles.Length; i++)
        {
            muscleValues.Add(muscles[i].force);
        }


        for (int i = 0; i < 6; i++)
        {
            originalBodyRotations[i] = muscles[i].restRotation;

            if (i == 1 || i == 2)
            {
                duckingBodyRotations[i] = -50f;
            }
            else if (i == 3 || i == 4)
            {
                duckingBodyRotations[i] = 50f;
            }
            else
            {
                duckingBodyRotations[i] = muscles[i].restRotation;
            }
        }

        
    }

    private void Start()
    {
        oneWayPlatforms = GameObject.FindGameObjectsWithTag("1 way platform");
    }

    private void Update()
    {
        grounded = Physics2D.OverlapArea(new Vector2(torso.transform.position.x - 1f, torso.transform.position.y - jumpCheckDist),
            new Vector2(torso.transform.position.x + 1f, torso.transform.position.y - jumpCheckDist / 2), 512);

        if (dummy || ragdoll)
            return;

        horizInput = Input.GetAxis(horizontalAxisName);
        vertInput = Input.GetAxis(verticalAxisName);

        //torso.GetComponent<Animator>().enabled = !grounded;

        if (Input.GetButtonDown(jumpButtonName))
        {
            if (grounded && !ducking)
            {
                Jump();
            }
        }

        if (vertInput < -0.5f)
        {
            ducking = true;

            
            if (fallThroughOneWayPlatforms == false)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    Collider2D firstCollider = cols[i];
                    for (int j = 0; j < oneWayPlatforms.Length; j++)
                    {
                        Collider2D secondCollider = oneWayPlatforms[j].GetComponent<Collider2D>();
                        Physics2D.IgnoreCollision(firstCollider, secondCollider);
                    }
                }
                fallThroughOneWayPlatforms = true;
                StartCoroutine(ReenableCollisionsStandOnPlatforms());
            }
        }
        else
        {
            ducking = false;
        }
    }

    private void FixedUpdate()
    {
        if (ragdoll)
            return;

        foreach (_Muscle muscle in muscles)
        {
            muscle.ActivateMuscle();
        }

        if (dummy)
            return;


        // walking
        if (grounded && !ducking)
        {
            if (horizInput > 0.1f)
            {
                if (torso.velocity.x < walkInterpThreshold)
                {
                    torso.velocity = new Vector2(Mathf.Lerp(torso.velocity.x, walkInterpThreshold, walkInterpValue), torso.velocity.y);
                }

                if (Time.time > moveDelayPointer)
                {
                    Step(walkRightVector, rightLeg, leftLeg);
                    moveDelayPointer = Time.time + moveDelay;
                }
            }
            else if (horizInput < -0.1f)
            {
                if (torso.velocity.x > -walkInterpThreshold)
                {
                    torso.velocity = new Vector2(Mathf.Lerp(torso.velocity.x, -walkInterpThreshold, walkInterpValue), torso.velocity.y);
                }

                if (Time.time > moveDelayPointer)
                {
                    Step(walkLeftVector, leftLeg, rightLeg);
                    moveDelayPointer = Time.time + moveDelay;
                }
            }
            else if (Mathf.Abs(torso.velocity.x) > 0.05f)
            {
                torso.velocity = new Vector2(Mathf.Lerp(torso.velocity.x, 0f, walkInterpValue), torso.velocity.y);
            }
        }
        else
        {
           if (horizInput > 0.1f && horizInput * airHandling > torso.velocity.x || horizInput < -0.1f && horizInput * airHandling < torso.velocity.x)
           {
                torso.velocity = new Vector2(Mathf.Lerp(torso.velocity.x, horizInput * airHandling, 0.3f), torso.velocity.y);
           }
        }

        //ducking
        if (ducking)
        {
            torso.AddForce(Vector2.down * duckForce * Time.fixedDeltaTime * 50f);

            for (int i = 0; i < 6; i++)
            {
                muscles[i].restRotation = duckingBodyRotations[i];
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                muscles[i].restRotation = originalBodyRotations[i];
            }
        }
    }

    private void Step(Vector2 vector, Rigidbody2D steppingFoot, Rigidbody2D otherFoot)
    {
        if (stepRightLeg)
        {
            steppingFoot.AddForce(vector, ForceMode2D.Impulse);
            otherFoot.AddForce(vector * -0.5f, ForceMode2D.Impulse);
            stepRightLeg = false;
        }
        else
        {
            otherFoot.AddForce(vector, ForceMode2D.Impulse);
            steppingFoot.AddForce(vector * -0.5f, ForceMode2D.Impulse);
            stepRightLeg = true;
        }
    }

    private void Jump()
    {
        foreach (_Muscle muscle in muscles)
        {
            muscle.bone.velocity = new Vector2(muscle.bone.velocity.x, jumpVelocity);
        }
    }

    public void Ragdoll()
    {
        ragdoll = true;
        StartCoroutine(Unragdoll(0.75f, 1f));
    }
    private IEnumerator Unragdoll(float restorePartway, float restoreFully)
    {
        float timer = knockoutTime;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        ragdoll = false;

        for (int i = 0; i < muscles.Length; i++)
        {
            muscles[i].force = 0f;
        }
        timer = restorePartway;
        while (timer > 0f && !ragdoll)
        {
            for (int i = 0; i < muscles.Length; i++)
            {
                muscles[i].force += (muscleValues[i] / (restorePartway * 10)) * Time.deltaTime;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        timer = restoreFully;
        while (timer > 0f && !ragdoll)
        {
            for (int i = 0; i < muscles.Length; i++)
            {
                muscles[i].force += (muscleValues[i] * (1 - (restoreFully / 10f))) * Time.deltaTime;
            }
            timer -= Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < muscles.Length; i++)
        {
            muscles[i].force = muscleValues[i];
        }
    }

    private IEnumerator ReenableCollisionsStandOnPlatforms()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < 2; i++)
        {
            Collider2D firstCollider = cols[i];
            for (int j = 0; j < oneWayPlatforms.Length; j++)
            {
                Collider2D secondCollider = oneWayPlatforms[j].GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(firstCollider, secondCollider, false);
            }
        }

        fallThroughOneWayPlatforms = false;

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < cols.Length; i++)
        {
            Collider2D firstCollider = cols[i];
            for (int j = 2; j < oneWayPlatforms.Length; j++)
            {
                Collider2D secondCollider = oneWayPlatforms[j].GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(firstCollider, secondCollider, false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0);
        //Gizmos.DrawCube(new Vector2(torso.transform.position.x, torso.transform.position.y - jumpCheckDist * 3/4f),
        //new Vector2(2f, jumpCheckDist/2));



        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = (mousePos - torso.position).normalized;
        //Vector2 target = torso.position + direction * 2f;
        //Gizmos.DrawSphere(target, distUntilDamp);
    }
}


[System.Serializable]
public class _Muscle
{
    public Rigidbody2D bone;
    public float restRotation;
    public float force;

    public void ActivateMuscle()
    {
        bone.MoveRotation(Mathf.LerpAngle(bone.rotation, restRotation, force * Time.deltaTime));
    }
}