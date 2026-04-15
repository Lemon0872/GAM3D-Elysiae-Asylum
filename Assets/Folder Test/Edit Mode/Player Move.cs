using NUnit.Framework;
using UnityEngine;

public class Player_Move
{
    private GameObject player;
    private PlayerController controller;
    private CharacterController characterController;
    private Transform cameraTransform;

    [SetUp]
    public void Setup()
    {
        player = new GameObject("Player");

        // required Unity component
        characterController = player.AddComponent<CharacterController>();

        controller = player.AddComponent<PlayerController>();

        // fake camera
        var camObj = new GameObject("Camera");
        cameraTransform = camObj.transform;
        cameraTransform.forward = Vector3.forward;
        cameraTransform.right = Vector3.right;

        // IMPORTANT: safe initialization instead of reflection guessing
        controller.Test_Initialize(cameraTransform, characterController);

        player.transform.position = Vector3.zero;
    }

    // =========================
    // 1. Move Left
    // =========================
    [Test]
    public void Move_Left()
    {
        Vector3 start = player.transform.position;

        controller.Test_SetInput(new Vector2(-1, 0));
        controller.Test_Move();

        Assert.Less(player.transform.position.x, start.x);
    }

    // =========================
    // 2. Move Right
    // =========================
    [Test]
    public void Move_Right()
    {
        Vector3 start = player.transform.position;

        controller.Test_SetInput(new Vector2(1, 0));
        controller.Test_Move();

        Assert.Greater(player.transform.position.x, start.x);
    }

    // =========================
    // 3. Move Forward
    // =========================
    [Test]
    public void Move_Forward()
    {
        Vector3 start = player.transform.position;

        controller.Test_SetInput(new Vector2(0, 1));
        controller.Test_Move();

        Assert.Greater(player.transform.position.z, start.z);
    }

    // =========================
    // 4. Move Backward
    // =========================
    [Test]
    public void Move_Backward()
    {
        Vector3 start = player.transform.position;

        controller.Test_SetInput(new Vector2(0, -1));
        controller.Test_Move();

        Assert.Less(player.transform.position.z, start.z);
    }

    // =========================
    // 5. Move + Rotate
    // =========================
    [Test]
    public void Move_And_Rotate()
    {
        controller.Test_SetInput(new Vector2(1, 0));
        controller.Test_Move();
        controller.Test_Rotate();

        Assert.AreEqual(Vector3.forward, player.transform.forward);
    }

    // =========================
    // 6. No movement
    // =========================
    [Test]
    public void No_Movement_When_No_Input()
    {
        Vector3 start = player.transform.position;

        controller.Test_SetInput(Vector2.zero);
        controller.Test_Move();

        Assert.AreEqual(start, player.transform.position);
    }

    // =========================
    // 7. CharacterController exists
    // =========================
    [Test]
    public void Character_Controller_Exists()
    {
        Assert.IsNotNull(characterController);
    }

    // =========================
    // 8. Opposite movement cancel
    // =========================
    [Test]
    public void Stop_When_Opposite_Input()
    {
        Vector3 start = player.transform.position;

        controller.Test_SetInput(new Vector2(1, 0));
        controller.Test_Move();

        controller.Test_SetInput(new Vector2(-1, 0));
        controller.Test_Move();

        Assert.AreEqual(start, player.transform.position);
    }
}