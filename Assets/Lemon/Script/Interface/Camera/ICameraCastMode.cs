using UnityEngine;
using System.Collections;

public interface ICameraCastMode
{
    IEnumerator Execute(Camera cam, CameraCutsceneBinding binding);
}