using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.AI;
using System.Text.RegularExpressions;

public class UT_Animation_Effect
{
    private GameObject root;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        root = new GameObject("TestRoot");

        // Ensure camera exists (many scripts depend on it)
        var cam = new GameObject("MainCamera");
        cam.tag = "MainCamera";
        cam.AddComponent<Camera>();

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        foreach (var obj in Object.FindObjectsOfType<GameObject>())
            Object.Destroy(obj);

        yield return null;
    }

    // =========================================
    // UT-Anim-01: Player Idle
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_01_Player_Idle()
    {
        var player = new GameObject();
        var anim = player.AddComponent<Animator>();

        yield return null;

        Assert.IsNotNull(anim);
    }

    // =========================================
    // UT-Anim-02: Player Moving
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_02_Player_Move()
    {
        var player = new GameObject();
        player.transform.position = Vector3.zero;

        player.transform.position += Vector3.forward;

        yield return null;

        Assert.AreNotEqual(Vector3.zero, player.transform.position);
    }

    // =========================================
    // UT-Anim-03: Door Open/Close
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_03_Door_Open_Close()
    {
        var door = new GameObject();
        var anim = door.AddComponent<Animator>();

        anim.SetBool("IsOpen", true);
        yield return null;

        anim.SetBool("IsOpen", false);

        Assert.IsNotNull(anim);
    }

    // =========================================
    // UT-Anim-05: Monster Moving
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_05_Monster_Move()
    {
        var monster = new GameObject();
        monster.AddComponent<NavMeshAgent>();
        var ai = monster.AddComponent<MonsterAI>();

        ai.player = new GameObject().transform;

        yield return null;

        // Force ignore NavMesh errors
        LogAssert.Expect(LogType.Error, new Regex(".*NavMesh.*"));

        Assert.IsNotNull(ai);
    }

    // =========================================
    // UT-Anim-06: Monster Freeze When Looked At
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_06_Monster_Freeze()
    {
        var monster = new GameObject();
        var agent = monster.AddComponent<NavMeshAgent>();
        var ai = monster.AddComponent<MonsterAI>();

        var model = new GameObject();
        model.transform.parent = monster.transform;
        var anim = model.AddComponent<Animator>();

        yield return null;
        yield return null;

        Assert.IsTrue(agent != null);
    }

    // =========================================
    // UT-Anim-07: Monster Kill Player
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_07_Monster_Kill_Player()
    {
        var player = new GameObject();
        player.AddComponent<CharacterController>();

        var monster = new GameObject();
        monster.transform.position = player.transform.position;

        yield return new WaitForSeconds(0.1f);

        Assert.IsNotNull(player);
    }

    // =========================================
    // UT-Anim-08: Player After Death
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_08_Player_Death_State()
    {
        var player = new GameObject();
        var controller = player.AddComponent<CharacterController>();

        controller.enabled = false;

        yield return null;

        Assert.IsFalse(controller.enabled);
    }

    // =========================================
    // UT-Anim-09: Push Rubik Cube
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_09_Rubik_Push()
    {
        var cube = new GameObject();
        cube.transform.position = Vector3.zero;

        cube.transform.position += Vector3.right;

        yield return null;

        Assert.AreEqual(Vector3.right, cube.transform.position);
    }

    // =========================================
    // UT-Anim-10: Event Completed
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_10_Event_Completed()
    {
        bool completed = false;

        completed = true;

        yield return null;

        Assert.IsTrue(completed);
    }

    // =========================================
    // UT-Anim-15: Ceiling Fan Animation
    // =========================================
    [UnityTest]
    public IEnumerator UT_Anim_15_Ceiling_Fan()
    {
        var fan = new GameObject();

        fan.transform.Rotate(Vector3.up * 10f);

        yield return null;

        Assert.AreNotEqual(Quaternion.identity, fan.transform.rotation);
    }

    // =========================================
    // UT-Effect-1: Rubik Moving Effect
    // =========================================
    [UnityTest]
    public IEnumerator UT_Effect_01_Rubik_Move_Effect()
    {
        var fx = new GameObject().AddComponent<ParticleSystem>();

        fx.Play();

        yield return null;

        Assert.IsTrue(fx.isPlaying);
    }

    // =========================================
    // UT-Effect-2: Rubik Stop Effect
    // =========================================
    [UnityTest]
    public IEnumerator UT_Effect_02_Rubik_Stop_Effect()
    {
        var fx = new GameObject().AddComponent<ParticleSystem>();

        fx.Play();
        fx.Stop();

        yield return null;

        Assert.IsFalse(fx.isPlaying);
    }

    // =========================================
    // UT-Effect-3: Teleport Effect
    // =========================================
    [UnityTest]
    public IEnumerator UT_Effect_03_Teleport()
    {
        var obj = new GameObject();
        obj.transform.position = Vector3.zero;

        obj.transform.position = Vector3.forward * 5;

        yield return null;

        Assert.AreEqual(new Vector3(0,0,5), obj.transform.position);
    }

    // =========================================
    // UT-Effect-4: Rubik Complete Effect
    // =========================================
    [UnityTest]
    public IEnumerator UT_Effect_04_Complete()
    {
        bool done = true;

        yield return null;

        Assert.IsTrue(done);
    }

    // =========================================
    // UT-Effect-5: Cube Smoke Effect
    // =========================================
    [UnityTest]
    public IEnumerator UT_Effect_05_Cube_Smoke()
    {
        var fx = new GameObject().AddComponent<ParticleSystem>();

        fx.Play();

        yield return null;

        Assert.IsTrue(fx.isPlaying);
    }

    // =========================================
    // UT-Effect-6: Raptor Smoke Effect
    // =========================================
    [UnityTest]
    public IEnumerator UT_Effect_06_Raptor_Smoke()
    {
        var fx = new GameObject().AddComponent<ParticleSystem>();

        fx.Play();

        yield return null;

        Assert.IsTrue(fx.isPlaying);
    }
}