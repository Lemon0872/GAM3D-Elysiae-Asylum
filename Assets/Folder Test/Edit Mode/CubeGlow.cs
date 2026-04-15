using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UT_TRubik_6
{
    [Test]
    public void CubeGlow_WhenInteract()
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var glow = go.AddComponent<CubeGlow>();

        glow.SetGlow(true);
        glow.SetGlow(false);

        Assert.Pass();
    }
}
