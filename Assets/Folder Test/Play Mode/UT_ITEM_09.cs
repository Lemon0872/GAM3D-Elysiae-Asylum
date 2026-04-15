

using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class UT_ITEM_09
{
    private GameObject plateObj;
    private PressurePlate plate;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // ===== Create Plate =====
        plateObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        plateObj.transform.position = Vector3.zero;

        plate = plateObj.AddComponent<PressurePlate>();

        yield return null;
    }

    GameObject CreateFallingItem(Vector3 pos)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = pos;

        obj.AddComponent<Rigidbody>(); // REQUIRED for collision

        return obj;
    }

    [UnityTest]
    public IEnumerator Plate_Detects_Generic_Item()
    {
        // Create item above plate
        var item = CreateFallingItem(new Vector3(0, 3, 0));

        // Wait for physics
        yield return new WaitForSeconds(1f);

        // Assert
        Assert.IsTrue(plate.isPressed,
            "Plate should detect ANY item placed on it");
    }

    [UnityTest]
    public IEnumerator Plate_Detects_Multiple_Items()
    {
        // Create multiple items
        var item1 = CreateFallingItem(new Vector3(0, 3, 0));
        var item2 = CreateFallingItem(new Vector3(0.5f, 4, 0));

        yield return new WaitForSeconds(1f);

        // Assert
        Assert.IsTrue(plate.isPressed,
            "Plate should still be pressed with multiple items");
    }

    [UnityTest]
    public IEnumerator Plate_Does_Not_Trigger_When_No_Item()
    {
        yield return new WaitForSeconds(0.5f);

        Assert.IsFalse(plate.isPressed,
            "Plate should NOT be pressed when nothing is on it");
    }
}