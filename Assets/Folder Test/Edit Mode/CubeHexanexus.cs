using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using System.Collections.Generic;
using System.Reflection;

public class UT_TRubik_7
{
    [UnityTest]
    public IEnumerator Hexanexus_Test()
    {
        var go = new GameObject();
        var controller = go.AddComponent<HexanexusController>();

        controller.pivotCenter = new GameObject().transform;
        controller.CubicPar = new GameObject().transform;

        // Create cubes
        for (int i = 0; i < 2; i++)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            cube.SetParent(controller.CubicPar);
            cube.gameObject.AddComponent<Outline>();
        }

        controller.rubikChecker = new GameObject().AddComponent<RubikChecker>();

        yield return null;

        // Force internal logic
        go.SendMessage("SelectAxis", 0);
        go.SendMessage("AttachCubesToPivot");

        // Run coroutine safely
        var method = typeof(HexanexusController)
            .GetMethod("RotatePivot", BindingFlags.NonPublic | BindingFlags.Instance);

        var enumerator = (IEnumerator)method.Invoke(controller, new object[] { 90f });

        while (enumerator.MoveNext()) { }

        Assert.Pass();
    }
}
