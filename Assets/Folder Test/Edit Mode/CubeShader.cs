using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UT_TRubik_72
{
    [Test]
    public void CubeShader_Blink()
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var config = go.AddComponent<cubeconfig>();

        config.lunkMaterial = new Material(Shader.Find("Standard"));
        config.nonLunkMaterial = new Material(Shader.Find("Standard"));

        config.isLunk = true;
        config.ApplyConfiguration();

        config.isLunk = false;
        config.ApplyConfiguration();

        Assert.Pass();
    }
}
