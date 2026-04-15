using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class UT_ITEM_04_05
{
    private GameObject pedestalObj;
    private PedestalEquip pedestal;

    private GameObject player;
    private GameObject pedestalStick;
    private GameObject playerStick;
    private Canvas canvas;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Create Player
        player = new GameObject("Player");
        player.transform.position = Vector3.zero;

        // Create Pedestal
        pedestalObj = new GameObject("Pedestal");
        pedestalObj.transform.position = Vector3.forward * 1f; // within range

        // Create sticks
        pedestalStick = new GameObject("PedestalStick");
        playerStick = new GameObject("PlayerStick");

        // Create canvas
        canvas = new GameObject("Canvas").AddComponent<Canvas>();

        // Add script
        pedestal = pedestalObj.AddComponent<PedestalEquip>();

        // Assign references via reflection (since fields are private)
        SetPrivateField(pedestal, "player", player.transform);
        SetPrivateField(pedestal, "pedestalStick", pedestalStick);
        SetPrivateField(pedestal, "playerStick", playerStick);
        SetPrivateField(pedestal, "promptCanvas", canvas);

        yield return null; // wait for Start()
    }

    // Helper to set private serialized fields
    private void SetPrivateField(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        field.SetValue(obj, value);
    }

    [UnityTest]
    public IEnumerator Fish_Equip_04()
    {
        // Act (simulate press E)
        pedestal.Interact(player.transform);

        yield return null;

        // Assert
        Assert.IsTrue(playerStick.activeSelf, "Player should have stick");
        Assert.IsFalse(pedestalStick.activeSelf, "Pedestal should be empty");
    }

    [UnityTest]
    public IEnumerator Fish_Drop_04()
    {
        // First interact (pick up)
        pedestal.Interact(player.transform);
        yield return null;

        // Second interact (drop)
        pedestal.Interact(player.transform);
        yield return null;

        // Assert
        Assert.IsFalse(playerStick.activeSelf, "Player should NOT have stick");
        Assert.IsTrue(pedestalStick.activeSelf, "Pedestal should have stick back");
    }

    [UnityTest]
    public IEnumerator Fish_In_Range_06()
    {
        // Arrange
        pedestalObj.transform.position = Vector3.zero;
        player.transform.position = Vector3.forward * 2f; // within default 3f range

        yield return null; // wait 1 frame for Update()

        // Assert
        Assert.IsTrue(canvas.enabled, "Canvas should be visible when player is in range");
    }

    [UnityTest]
    public IEnumerator Fish_Not_In_Range_06()
    {
        // Arrange
        pedestalObj.transform.position = Vector3.zero;
        player.transform.position = Vector3.forward * 10f; // outside range

        yield return null;

        // Assert
        Assert.IsFalse(canvas.enabled, "Canvas should be hidden when player is out of range");
    }

    [UnityTest]
    public IEnumerator Fish_Boundary_06()
    {
        // Arrange
        pedestalObj.transform.position = Vector3.zero;

        float detectDistance = 3f;
        SetPrivateField(pedestal, "detectDistance", detectDistance);

        player.transform.position = Vector3.forward * detectDistance;

        yield return null;

        // Assert
        Assert.IsTrue(canvas.enabled, "Canvas should be visible at boundary distance");
    }

    [UnityTest]
public IEnumerator Fish_Dont_Fall_Off_Map_07()
{
    // ===== Floor =====
    var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
    floor.transform.position = Vector3.zero;

    // ===== Player =====
    var player = new GameObject("Player");
    player.transform.position = Vector3.zero;

    // ===== Pedestal =====
    var pedestalObj = new GameObject("Pedestal");
    pedestalObj.transform.position = new Vector3(0, 0, 2);

    var pedestal = pedestalObj.AddComponent<PedestalEquip>();

    // ===== Stick objects =====
    var pedestalStick = GameObject.CreatePrimitive(PrimitiveType.Cube);
    pedestalStick.transform.position = new Vector3(0, 1, 2);

    var playerStick = GameObject.CreatePrimitive(PrimitiveType.Cube);
    playerStick.transform.position = player.transform.position + Vector3.up;

    playerStick.SetActive(false);

    // ===== Canvas =====
    var canvas = new GameObject("Canvas").AddComponent<Canvas>();

    // ===== Inject private fields =====
    SetPrivateField(pedestal, "player", player.transform);
    SetPrivateField(pedestal, "pedestalStick", pedestalStick);
    SetPrivateField(pedestal, "playerStick", playerStick);
    SetPrivateField(pedestal, "promptCanvas", canvas);

    yield return null;

    // ===== Pick up stick =====
    pedestal.Interact(player.transform);
    yield return null;

    // ===== Drop stick =====
    pedestal.Interact(player.transform);
    yield return null;

    // ===== Wait physics =====
    yield return new WaitForSeconds(1f);

    // ===== Assert =====
    Assert.Greater(pedestalStick.transform.position.y, -0.5f,
        "Pedestal stick fell through the floor!");
}
}