using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class UT_ITEM_03
{
    private GameObject item;
    private ItemInteractor interactor;
    private GameObject holder;

    // ✅ Load fresh empty scene BEFORE each test
    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.CreateScene("TempTestScene");
    SceneManager.SetActiveScene(SceneManager.GetSceneByName("TempTestScene"));
        yield return null;

        // ===== Create camera (IMPORTANT for Place) =====
        var cam = new GameObject("MainCamera");
        cam.tag = "MainCamera";
        cam.AddComponent<Camera>();

        // ===== Create holder =====
        holder = new GameObject("Holder");
        holder.tag = "Holder";

        // ===== Create item =====
        item = GameObject.CreatePrimitive(PrimitiveType.Cube);
        item.AddComponent<Rigidbody>();

        interactor = item.AddComponent<ItemInteractor>();

        yield return null;
    }

    // ✅ Cleanup AFTER each test (extra safety)
    [TearDown]
    public void Cleanup()
    {
        foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
        {
            GameObject.DestroyImmediate(obj);
        }
    }

    [UnityTest]
    public IEnumerator Raptor_Can_Be_Picked_Up()
    {
        ((IInteractable)interactor).Interact(holder.transform);
        yield return null;

        Assert.IsTrue(interactor.IsHeld());
        Assert.AreEqual(holder.transform, item.transform.parent);
    }

    [UnityTest]
    public IEnumerator Raptor_Can_Be_Released()
    {
        ((IInteractable)interactor).Interact(holder.transform);
        yield return null;

        interactor.Test_Place();
        yield return null;

        Assert.IsFalse(interactor.IsHeld());
        Assert.IsNull(item.transform.parent);
    }

    [UnityTest]
    public IEnumerator Raptor_Dont_Fall_Off_Map()
    {
        // ===== Floor =====
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = Vector3.zero;

        // ===== Camera =====
        var cam = new GameObject("MainCamera");
        cam.tag = "MainCamera";
        cam.AddComponent<Camera>();
        cam.transform.position = new Vector3(0, 5, -5);
        cam.transform.forward = Vector3.forward;

        // ===== Item =====
        var item = GameObject.CreatePrimitive(PrimitiveType.Cube);
        item.transform.position = new Vector3(0, 2, 0);
        var rb = item.AddComponent<Rigidbody>();

        var interactor = item.AddComponent<ItemInteractor>();

        // ===== Pickup =====
        var holder = new GameObject("Holder");
        holder.tag = "Holder";

        ((IInteractable)interactor).Interact(holder.transform);
        yield return null;

        // ===== Drop =====
        interactor.Test_Place();

        yield return new WaitForSeconds(1f);

        float yPos = item.transform.position.y;

        Assert.Greater(yPos, -0.5f, "Item fell through the floor!");
    }
}