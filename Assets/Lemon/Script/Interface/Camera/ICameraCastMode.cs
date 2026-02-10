using System.Collections;
using UnityEngine;

public interface ICameraCastMode
{
    IEnumerator Execute(Camera cam, CameraCutsceneData data, Transform target);
}
