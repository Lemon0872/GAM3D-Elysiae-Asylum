using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Mono.Cecil.Cil;
using UnityEngine;

public class RubikChecker : MonoBehaviour
{
    public Transform rubikParent;
    public Transform goalA, goalB;
    public float endA, endB;
    public Transform PivotCube;

    private List<Transform> rubikCubes = new List<Transform>();
    private List<Transform> aCubes = new List<Transform>();
    private List<Transform> bCubes = new List<Transform>();

    public bool isWin = false;
    [SerializeField] LeanTweenType type;
    void Start()
    {
        foreach (Transform t in rubikParent) rubikCubes.Add(t);
        foreach (Transform t in goalA) aCubes.Add(t);
        foreach (Transform t in goalB) bCubes.Add(t);
    }

    void Update()
    {
        
    }

    public bool CheckWin()
    {
        // chỉ kiểm tra các cube có tag "Lunk" (cube màu)

        foreach (var cube in rubikCubes)
        {
            if (!cube.CompareTag("Lunk"))
                continue; // bỏ qua cube trong suốt

            // Kiểm tra theo ảnh A (song song trục X)
            bool aMatch = CheckAxisMatch(cube, aCubes, Axis.Z);

            // Kiểm tra theo ảnh B (song song trục Z)
            bool bMatch = CheckAxisMatch(cube, bCubes, Axis.X);

            foreach (var block in aCubes)
            {
                if (CheckGoalACompleted()) block.GetComponent<CubeGlow>().SetGlow(true);
                else block.GetComponent<CubeGlow>().SetGlow(false);
            }
            foreach (var block in bCubes)
            {
                if (CheckGoalBCompleted()) block.GetComponent<CubeGlow>().SetGlow(true);
                else block.GetComponent<CubeGlow>().SetGlow(false);
            }

            if (!aMatch || !bMatch)
            {
                Debug.Log("chua win");
                return false;
            }

        }
        StartCoroutine(MoveABatEnd());
        return true;
    }

    enum Axis { X, Z}

    private bool CheckAxisMatch(Transform cube, List<Transform> goalCubes, Axis axis)
    {
        foreach (var goal in goalCubes)
        {
            // chỉ so với cube màu trong ảnh A/B
            if (!goal.CompareTag("Lunk"))
                continue;

            bool sameLine = false;
            if (axis == Axis.Z)
                sameLine = Mathf.Approximately(cube.position.z, goal.position.z) && Mathf.Approximately(cube.position.y, goal.position.y);
            else if (axis == Axis.X)
                sameLine = Mathf.Approximately(cube.position.x, goal.position.x) && Mathf.Approximately(cube.position.y, goal.position.y);

            if (sameLine && cube.tag == goal.tag)
            {
                
                return true;
            }
        }

        return false;
    }
    public bool CheckGoalACompleted()
    {
        return CheckGoalCompleted(aCubes, Axis.Z);
    }

    public bool CheckGoalBCompleted()
    {
        return CheckGoalCompleted(bCubes, Axis.X);
    }
    IEnumerator MoveABatEnd()
    {
        LeanTween.moveZ(goalB.gameObject, endB, 0.7f).setEase(type).setDelay(1.6f);
        LeanTween.moveX(goalA.gameObject, endA, 0.7f).setEase(type).setDelay(1.6f);
        yield return new WaitForSeconds(2.2f);
        foreach (var block in rubikCubes)
            block.GetComponent<CubeGlow>().SetGlow(true);
    }

    private bool CheckGoalCompleted(List<Transform> goalCubes, Axis axis)
    {
        foreach (var goal in goalCubes)
        {
            if (!goal.CompareTag("Lunk"))
                continue; // bỏ qua cube không màu

            bool matched = false;

            foreach (var cube in rubikCubes)
            {
                if (!cube.CompareTag("Lunk"))
                    continue;

                bool sameLine = false;
                if (axis == Axis.Z)
                    sameLine = Mathf.Approximately(cube.position.z, goal.position.z)
                            && Mathf.Approximately(cube.position.y, goal.position.y);
                else if (axis == Axis.X)
                    sameLine = Mathf.Approximately(cube.position.x, goal.position.x)
                            && Mathf.Approximately(cube.position.y, goal.position.y);

                if (sameLine)
                {
                    matched = true;
                    break;
                }
            }

            // Nếu có 1 cube điều kiện không được thỏa
            if (!matched)
                return false;
        }

        // Nếu tất cả cube điều kiện đã được khớp
        return true;
    }

}
