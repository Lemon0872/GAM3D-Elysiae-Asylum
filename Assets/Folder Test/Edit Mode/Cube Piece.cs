using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UT_TRubik_5
{
    [Test]
    public void CubePiece()
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var piece = go.AddComponent<CubePiece>();

        piece.SetColored(true);
        piece.UpdateVisual();

        piece.SetColored(false);
        piece.UpdateVisual();

        Assert.Pass();
    }
}
