using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Functional;

public class BossPhases : MonoBehaviour
{
    [SerializeField] float spinAngle = 30f;

    [SerializeField] uint phase = 1;
    [SerializeField] bool roundDone = false;

    [Header("Incoming")]
    [SerializeField] float startMoveSpeed = 4f;
    [SerializeField] float startRotationSpeed = 4f;
    [SerializeField] GameObject startTarget;
    [SerializeField] float toSetAngle = -180f;

    [Header("Body Parts")]
    [SerializeField] GameObject hangarParts;
    [SerializeField] GameObject upperRocketParts;
    [SerializeField] GameObject lowerRocketParts;
    [SerializeField] GameObject bodyParts;

    [Header("Break Explosion Locations")]
    [SerializeField] Transform explosionRocketL;
    [SerializeField] Transform explosionRocketU;
    [SerializeField] Transform explosionHangar;

    [Header("Durations & Delays")]
    [SerializeField] float rocketDuration = 4f;
    [SerializeField] float hangarDuration = 4f;
    [SerializeField] float spawnEnemiesDelayBefore = 1f;
    [SerializeField] float spawnEnemiesDelayBetween = 1f;
    [SerializeField] float spawnRocketsDelayBefore1 = 2.5f;
    [SerializeField] float spawnRocketsDelayBefore2 = 2f;
    [SerializeField] float spawnRocketsDelayBetween = 1f;
    [SerializeField] float waitUntilShootPhase3 = 1.5f;

    [Header("Distances & Speed")]
    [SerializeField] float farDistance = 125f;
    [SerializeField] float closeDistance = 100f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float gravityForce = 2.5f;
    [SerializeField] float farDistancePhase3 = 125f;
    [SerializeField] float closeDistancePhase3 = 50f;
    [SerializeField] float nearEnoughMargin = .1f;

    [Header("Healths")]
    [SerializeField] float bodyHealth = 100f;
    [SerializeField] float partHealth = 50f;

    [Header("Misc")]
    [SerializeField] Vector3 spawnOffset;
    [SerializeField] float hangarOffAngle = 80f;
    [SerializeField] GameObject ground;
    [SerializeField] ParticleSystem explosionDeath;

    float dir = 0f;

    bool hangarOff = false;
    ushort destroyedBodyParts = 0;

    BossHealth bossHealth;
    FlyToLinear flyToLinear;

    RotateAroundLinear rocket1;
    RotateAroundLinear rocket2;
    RotateAroundLinear hangar;

    GameObject rocket1Spawn;
    GameObject rocket2Spawn;
    GameObject hangarSpawn;

    Transform bossBodyTransform;

    float initMoveSpeed;
    float initRotationSpeed;

    RotateAroundLinear spinning;
    Spinning spinAround;
    Floating floating;
    EnemyShooting enemyShooting;

    bool phase3Forward;
    GameObject phase3Backend;
    GameObject phase3Frontend;
    public float phase3Velocity;
    private ParticleSystem explosionDeathInstance;

    bool phase4once = false;

    public class WaitThenData
    {
        public float time;
        public Action fun;
        public WaitThenData(float time, Action fun)
        {
            this.time = time;
            this.fun = fun;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        phase3Backend = new GameObject();
        phase3Frontend = new GameObject();

        explosionDeathInstance = Instantiate(
            explosionDeath,
            Vector3.zero,
            explosionDeath.transform.rotation
        );

        InitVulnerableParts();

        flyToLinear = GetComponent<FlyToLinear>();
        bossBodyTransform = transform.Find("Boss Body");
        spinning = GetComponent<RotateAroundLinear>();
        spinAround = GetComponent<Spinning>();
        enemyShooting = GetComponent<EnemyShooting>();
        floating = bossBodyTransform.GetComponent<Floating>();

        rocket1Spawn = lowerRocketParts.transform.Find("Lower Rocket Jet").gameObject;
        rocket2Spawn = upperRocketParts.transform.Find("Upper Rocket Jet").gameObject;
        hangarSpawn = hangarParts.transform.Find("Hangar Opening").gameObject;

        rocket1 = lowerRocketParts.transform.Find("Lower Rocket Open").GetComponent<RotateAroundLinear>();
        rocket2 = upperRocketParts.transform.Find("Upper Rocket Open").GetComponent<RotateAroundLinear>();
        hangar = hangarParts.transform.Find("Hangar Door").GetComponent<RotateAroundLinear>();

        roundDone = true;

        initMoveSpeed = moveSpeed;
        initRotationSpeed = spinning.speed;
    }

    void InitVulnerableParts()
    {
        bossHealth = BossHealth.SharedComponent;

        var data = new Dictionary<string, BossHealthData>(){
            { "body" , new BossHealthData(()=>{ }, bodyParts, bodyHealth ) },
            { "hangar" , new BossHealthData(BreakHangarOff, hangarParts, partHealth ) },
            { "rocket1" , new BossHealthData(BreakRockets2Off, lowerRocketParts, partHealth ) },
            { "rocket2" , new BossHealthData(BreakRockets1Off, upperRocketParts, partHealth ) }
        };

        bossHealth.ApplyBossData(data);
        bossHealth.onDie = OnDie;
    }

    void BreakAndFallOff(GameObject parts, Transform explosionLocation)
    {
        var e = SpawnManager.SharedInstance
                .GetInstance("Small Explosion Particle");
        e.transform.position = explosionLocation.position;

        //var coord = transform.TransformPoint(parts.transform.position);
        parts.transform.parent = null;
        //parts.transform.position = coord;
        var s = parts.GetComponent<Spinning>();
        var m = parts.GetComponent<MoveForward>();
        var cf = parts.GetComponent<ComeFrontSelf>();
        if (s && m & cf) {
            s.enabled = true;
            m.enabled = true;
            cf.enabled = true;
        }

        destroyedBodyParts += 1;
    }

    void BreakHangarOff()
    {
        hangarOff = true;
        BreakAndFallOff(hangarParts, explosionHangar);
    }

    void BreakRockets1Off()
    {
        BreakAndFallOff(upperRocketParts, explosionRocketU);
    }
    void BreakRockets2Off()
    {
        BreakAndFallOff(lowerRocketParts, explosionRocketL);
    }

    string[] GetStrings(Dictionary<string, float> d)
    {
        var keys = d.Keys;
        var strings = new string[d.Count];

        int i = 0;
        foreach (var key in keys)
        {
            strings[i] = key;
            ++i;
        }

        return strings;
    }

    private void FixedUpdate()
    {
        DoMovement();
        HandlePhases();
    }

    void DoMovement()
    {
        if (dir == 0f)
        {
            return;
        }

        var oldPos = transform.localPosition;

        //Debug.Log(oldPos.z - Time.deltaTime * moveSpeed);

        transform.localPosition = new Vector3(
            oldPos.x,
            oldPos.y,
            dir > 0 ? Mathf.Min(
                farDistance,
                oldPos.z + Time.deltaTime * moveSpeed
            ) :
            Mathf.Max(
                closeDistance,
                oldPos.z - Time.deltaTime * moveSpeed
            )
        );
    }

    void MoveTowardTarget(GameObject target)
    {
        var delta = target.transform.position - transform.localPosition;
        var step = Time.deltaTime * moveSpeed;

        if (delta.magnitude < step)
        {
            transform.localPosition = target.transform.position;
        }

        transform.localPosition +=
            delta.normalized *
            step
        ;

    }

    // Update is called once per frame
    void HandlePhases()
    {
        if (destroyedBodyParts == 3)
        {
            Phase3Init();
        }

        if (phase == 3)
        {
            Phase3Add();
        }

        if (!roundDone)
        {
            return;
        }

        if (phase == 0)
        {
            Debug.Log("Start Phase 0");
            Phase0();
        }
        else if (phase == 1)
        {
            Debug.Log("Start Phase 1");
            Phase1();
        }
        else if (phase == 2){
            Debug.Log("Start Phase 2");
            Phase2();
        }
        else if (phase == 3)
        {
            Debug.Log("Start Phase 3");
            Phase3();
        }
        else if (phase == 4)
        {
            Debug.Log("Start Phase 4");
            Phase4();
        }
    }

    void SetRockets(bool state)
    {
        rocket1.activated = state;
        rocket2.activated = state;
        bossHealth.data["rocket1"].active = state;
        bossHealth.data["rocket2"].active = state;
    }

    void SetHangar(bool state)
    {
        bossHealth.data["hangar"].active = state;
        hangar.activated = state;
    }

    IEnumerator WaitForThenCoroutine(WaitThenData data)
    {
        yield return new WaitForSeconds(data.time);
        Debug.Log("Time passed");
        data.fun();
    }

    void MoveBackward()
    {
        dir = 1f;
    }

    void MoveForward()
    {
        dir = -1f;
    }

    void StopMovement()
    {
        dir = 0f;
    }

    void WaitForThen(float time, Action fun)
    {
        StartCoroutine("WaitForThenCoroutine", new WaitThenData(time, fun));
    }

    void SpawnObjects(
        float before,
        float between,
        string name,
        GameObject spawnSource
    )
    {
        WaitForThen(before, () =>
        {
            SpawnObject(name, spawnSource);
            WaitForThen(between, () =>
            {
                SpawnObject(name, spawnSource);
                WaitForThen(between, () =>
                {
                    SpawnObject(name, spawnSource);
                });
            });
        });
    }

    void SpawnObject(string name, GameObject spawnSource)
    {

        var enemy = SpawnManager.SharedInstance.GetInstance(name);

        enemy.transform.position = spawnSource.transform.position + spawnOffset;
        enemy.GetComponent<EnemyVulnerable>().Revive();
        enemy.GetComponent<EnemyFlying>().MakeReady();
        var shoot = enemy.GetComponent<EnemyShooting>();
        if (shoot)
        {
            shoot.ReadyToShoot();
        }
    }

    void Phase0()
    {
        roundDone = false;
        spinning.activated = true;
        moveSpeed = startMoveSpeed;
        spinning.speed = startRotationSpeed;

        MoveTowardTarget(startTarget);
        spinning.angle = toSetAngle;
        if ( Mathf.Abs(spinning.currentAngle - toSetAngle) < nearEnoughMargin)
        {
            phase = 1;
        }

        roundDone = true;
    }

    void Phase1()
    {
        roundDone = false;
        transform.position = startTarget.transform.position;
        phase = 2;
        roundDone = true;
    }

    void Phase2()
    {
        roundDone = false;
        spinning.activated = true;

        moveSpeed = initMoveSpeed;
        spinning.speed = initRotationSpeed;

        //show Hangar
        SetHangar(true);
        SetRockets(false);
        spinning.angle = (hangarOff ? hangarOffAngle : spinAngle) + toSetAngle;
        MoveBackward();

        SpawnObjects(
            spawnEnemiesDelayBefore,
            spawnEnemiesDelayBetween,
            "Enemy Ship Boss",
            hangarSpawn
        );

        WaitForThen(hangarDuration, () =>
        {
            SpawnObjects(
                spawnRocketsDelayBefore1,
                spawnRocketsDelayBetween,
                "Rocket",
                rocket1Spawn
            );

            SpawnObjects(
                spawnRocketsDelayBefore2,
                spawnRocketsDelayBetween,
                "Rocket",
                rocket2Spawn
            );

            MoveForward();

            //show Rockets
            SetHangar(false);
            SetRockets(true);
            spinning.angle = -spinAngle + toSetAngle;

            WaitForThen(rocketDuration, () =>
            {
                //rerun
                roundDone = true;
            });
        });
    }

    void Phase3Init()
    {
        StopMovement();
        destroyedBodyParts = 0;
        phase = 3;
        roundDone = true;
        StopAllCoroutines();

        Phase3Forward();
        flyToLinear.onTargetReached = Phase3Forward;
        flyToLinear.enabled = true;

        bossHealth.data["body"].active = true;
    }

    void Phase3()
    {
        roundDone = false;
        spinning.enabled = false;
    }

    void Phase3Add()
    {
        var player = PlayerController.SharedComponent.gameObject;
        phase3Backend.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            farDistancePhase3
        );

        phase3Frontend.transform.position = new Vector3(
            player.transform.position.x,
            transform.position.y,
            closeDistancePhase3
        );
    }

    void Phase3Forward(Transform target = null)
    {

        phase3Forward = !phase3Forward;


        if (phase3Forward)
        {
            flyToLinear.target = phase3Frontend.transform;

            enemyShooting.enabled = true;
            enemyShooting.ReadyToShoot();
            return;
        }

        enemyShooting.enabled = false;

        flyToLinear.target = phase3Backend.transform;
    }

    void OnDie()
    {
        Debug.Log("Boss defeated.");

        phase = 4;
        StopAllCoroutines();
        flyToLinear.enabled = false;
        floating.enabled = false;
        spinAround.enabled = true;
        roundDone = true;
    }

    void Phase4()
    {
        if (!phase4once)
        {
            GetComponent<AudioSource>().Play();
            MusicController.SharedComponent.StopMusic();
            phase4once = true;
        }
        phase3Velocity += gravityForce * Time.deltaTime;

        var timedJumpForce = jumpForce * Time.deltaTime - phase3Velocity;
        transform.position += timedJumpForce * Vector3.up;
        if (transform.position.y < ground.transform.position.y)
        {
            Explode();
            phase = 5;
        }
    }

    void Explode()
    {
        explosionDeathInstance.transform.position = transform.position;
        explosionDeathInstance.Play();
        explosionDeathInstance.GetComponent<AudioSource>().Play();
        bossBodyTransform.gameObject.SetActive(false);
        Debug.Log("Explosion");
        WaitForThen(5f, () =>
        {
            Transitions.SharedComponent.FadeOut(() =>
            {
                SceneManager.LoadScene("End");
            });
        });
    }
}
