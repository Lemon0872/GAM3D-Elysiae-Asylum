using NUnit.Framework;
using UnityEngine;

public class UT_Penguin
{
    private GameObject spawnerObj;
    private MoleSpawner spawner;

    private GameObject managerObj;
    private GameManager manager;

    private GameObject molePrefab;

    [SetUp]
    public void Setup()
    {
        // ===== GameManager =====
        managerObj = new GameObject("GameManager");
        manager = managerObj.AddComponent<GameManager>();

        manager.targetWord = "LOVE";

        // ===== Spawner =====
        spawnerObj = new GameObject("Spawner");
        spawner = spawnerObj.AddComponent<MoleSpawner>();

        // ===== Fake Mole Prefab =====
        molePrefab = new GameObject("MolePrefab");
        molePrefab.AddComponent<Mole>();

        spawner.molePrefab = molePrefab;
        spawner.spawnCenter = Vector3.zero;
        spawner.spawnRange = Vector3.one * 10;
        spawner.maxMoles = 10;
    }

    // =========================
    // 1. Spawn chim
    // =========================
    [Test]
    public void Mole_Should_Spawn_When_Called()
    {
        spawner.Test_Spawn();

        Assert.Greater(spawner.GetActiveMoleCount(), 0,
            "Mole should spawn");
    }

    // =========================
    // 2. Số lượng chim
    // =========================
    [Test]
    public void Mole_Count_Should_Not_Exceed_Max()
    {
        for (int i = 0; i < 20; i++)
        {
            spawner.Test_Spawn();
        }

        Assert.LessOrEqual(spawner.GetActiveMoleCount(), spawner.maxMoles,
            "Mole count should not exceed max limit");
    }

    // =========================
    // 3. Đánh chim bằng stick
    // =========================
    [Test]
    public void Hit_Mole_With_Letter_Should_Collect_Letter()
    {
        GameObject moleObj = new GameObject("Mole");
        Mole mole = moleObj.AddComponent<Mole>();

        mole.hasLetter = true;
        mole.letter = 'L';

        mole.OnHit();

        Assert.Contains('L', manager.collectedLetters,
            "Letter should be collected when mole is hit");
    }

    // =========================
    // 4. Đánh sai ký tự
    // =========================
    [Test]
    public void Hit_Wrong_Letter_Should_Not_Complete_Word()
    {
        manager.targetWord = "LOVE";

        manager.CollectLetter('X'); // sai

        Assert.IsFalse(manager.IsComplete(),
            "Wrong letters should not complete word");
    }

    // =========================
    // 5. Hoàn thành minigame
    // =========================
    [Test]
    public void Collecting_All_Letters_Should_Complete_Game()
    {
        manager.targetWord = "LOVE";

        manager.CollectLetter('L');
        manager.CollectLetter('O');
        manager.CollectLetter('V');
        manager.CollectLetter('E');

        Assert.IsTrue(manager.IsComplete(),
            "Game should complete when all letters are collected");
    }
}