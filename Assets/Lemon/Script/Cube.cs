using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HexanexusController : MonoBehaviour
{
    [Header("Setup")]
    public RubikChecker rubikChecker;
    public Transform pivotCenter;
    public Transform CubicPar; // Empty ở tâm
    public List<Transform> cubes = new List<Transform>();     // 8 cube
    public float rotationSpeed = 200f;

    private bool isRotating = false;
    private int currentAxis = -1;
    private Transform pivot;

    public string sceneName;
    void Awake()
    {
        foreach (Transform t in CubicPar) cubes.Add(t);
        sceneName=gameObject.scene.name;
    }

    void Start()
    {
        pivot = new GameObject("Pivot").transform;
        pivot.SetParent(transform);
        pivot.localPosition = Vector3.zero;
    }

    void Update()
    {
        HandleAxisSelection();
        HandleRotation();
        OutlineCubes();
    }
    void OutlineCubes()
    {
        foreach (Transform child in cubes)
        {
            if (child.parent == pivot)
                child.GetComponent<Outline>().enabled = true;
            else child.GetComponent<Outline>().enabled = false;
        }
    }
    void HandleAxisSelection()
    {
        if (isRotating) return;
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectAxis(0); // X+
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectAxis(1); // X-
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectAxis(2); // Y+
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectAxis(3); // Y-
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectAxis(4); // Z+
        if (Input.GetKeyDown(KeyCode.Alpha6)) SelectAxis(5); // Z-
        if (Input.GetKeyDown(KeyCode.Alpha7)) SelectAxis(6); // X=
        if (Input.GetKeyDown(KeyCode.Alpha8)) SelectAxis(7); // Y=
        if (Input.GetKeyDown(KeyCode.Alpha9)) SelectAxis(8); // Z=
    }

    void SelectAxis(int index)
    {
        currentAxis = index;
        Debug.Log($"Selected axis: {index}");
        AttachCubesToPivot();
    }

    void HandleRotation()
    {
        if (currentAxis == -1 || isRotating) return;

        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(RotatePivot(-90));
        if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(RotatePivot(90));
    }

    void AttachCubesToPivot()
    {
        // Bỏ parenting cũ
        foreach (Transform cube in cubes)
            if (cube.parent == pivot) cube.SetParent(CubicPar);

        pivot.position = pivotCenter.position;
        pivot.rotation = Quaternion.identity;

        foreach (Transform cube in cubes)
        {
            Vector3 pos = cube.position - pivotCenter.position;
            if (cubes.Count != 8)
            {
                switch (currentAxis)
                {
                    case 0: // X+
                        if (pos.x > 0) cube.SetParent(pivot);
                        break;
                    case 1: // X-
                        if (pos.x == 0) cube.SetParent(pivot);
                        break;
                    case 2: // Y+
                        if (pos.x < 0) cube.SetParent(pivot);
                        break;
                    case 3: // Y-
                        if (pos.y > 0) cube.SetParent(pivot);
                        break;
                    case 4: // Z+
                        if (pos.y == 0) cube.SetParent(pivot);
                        break;
                    case 5: // Z-
                        if (pos.y < 0) cube.SetParent(pivot);
                        break;
                    case 6: // X=
                        if (pos.z > 0) cube.SetParent(pivot);
                        break;
                    case 7: // Y=
                        if (pos.z == 0) cube.SetParent(pivot);
                        break;
                    case 8: // Z=
                        if (pos.z < 0) cube.SetParent(pivot);
                        break;
                }
            }
            else
            {
                switch (currentAxis)
                {
                    case 0: // X+
                        if (pos.x > 0) cube.SetParent(pivot);
                        break;
                    case 1: // X-
                        if (pos.x < 0) cube.SetParent(pivot);
                        break;
                    case 2: // Y+
                        if (pos.y > 0) cube.SetParent(pivot);
                        break;
                    case 3: // Y-
                        if (pos.y < 0) cube.SetParent(pivot);
                        break;
                    case 4: // Z+
                        if (pos.z > 0) cube.SetParent(pivot);
                        break;
                    case 5: // Z-
                        if (pos.z < 0) cube.SetParent(pivot);
                        break;
                }
            }
        }

        Debug.Log($"Pivot attached {pivot.childCount} cubes.");
    }


    IEnumerator RotatePivot(float targetAngle)
    {
        isRotating = true;
        Vector3 axis = Vector3.zero;

        if (cubes.Count!=8)
        {
            switch (currentAxis)
            {
                case 0:
                case 1:
                case 2:
                    axis = Vector3.right;
                    break;
                case 3:
                case 4:
                case 5:
                    axis = Vector3.up;
                    break;
                case 6:
                case 7:
                case 8:
                    axis = Vector3.forward;
                    break;
            }
        }
        else
        {
            switch (currentAxis)
            {
                case 0:
                case 1:
                    axis = Vector3.right;
                    break;
                case 2:
                case 3:
                    axis = Vector3.up;
                    break;
                case 4:
                case 5:
                    axis = Vector3.forward;
                    break;
            }
        }

        float rotated = 0f;
        while (rotated < Mathf.Abs(targetAngle))
        {
            float step = rotationSpeed * Time.deltaTime;
            pivot.Rotate(axis, Mathf.Sign(targetAngle) * step, Space.World);
            rotated += step;
            yield return null;
        }

        // làm tròn góc
        pivot.Rotate(axis, Mathf.Sign(targetAngle) * (Mathf.Abs(targetAngle) - rotated), Space.World);

        // tháo parenting
        
        isRotating = false;
        AttachCubesToPivot(); // cập nhật lại nhóm mới

        // Tính toán lại vị trí & xoay cho từng cube trước khi tách pivot
        while (pivot.childCount > 0)
        {
            Transform cube = pivot.GetChild(0);

            // Lưu vị trí và hướng trong không gian thế giới
            Vector3 worldPos = cube.position;
            Quaternion worldRot = cube.rotation;

            // Tháo ra
            cube.SetParent(CubicPar, false);

            // Gán lại vị trí & hướng chính xác
            cube.position = worldPos;
            cube.rotation = worldRot;

            // Làm tròn vị trí để tránh sai số trôi (rất quan trọng!)
            cube.localPosition = new Vector3(
                Mathf.Round(cube.localPosition.x * 10000f) / 10000f,
                Mathf.Round(cube.localPosition.y * 10000f) / 10000f,
                Mathf.Round(cube.localPosition.z * 10000f) / 10000f
            );
        }
        foreach (Transform cube in cubes)
        {
            Vector3 pos = cube.position - pivotCenter.position;
            if (cubes.Count != 8)
            {
                switch (currentAxis)
                {
                    case 0: if (pos.x > 0) cube.SetParent(pivot); break;
                    case 1: if (pos.x == 0) cube.SetParent(pivot); break;
                    case 2: if (pos.x < 0) cube.SetParent(pivot); break;
                    case 3: if (pos.y > 0) cube.SetParent(pivot); break;
                    case 4: if (pos.y == 0) cube.SetParent(pivot); break;
                    case 5: if (pos.y < 0) cube.SetParent(pivot); break;
                    case 6: if (pos.z > 0) cube.SetParent(pivot); break;
                    case 7: if (pos.z == 0) cube.SetParent(pivot); break;
                    case 8: if (pos.z < 0) cube.SetParent(pivot); break;
                }
            }
            else
            {
                switch (currentAxis)
                {
                    case 0: if (pos.x > 0) cube.SetParent(pivot); break;
                    case 1: if (pos.x < 0) cube.SetParent(pivot); break;
                    case 2: if (pos.y > 0) cube.SetParent(pivot); break;
                    case 3: if (pos.y < 0) cube.SetParent(pivot); break;
                    case 4: if (pos.z > 0) cube.SetParent(pivot); break;
                    case 5: if (pos.z < 0) cube.SetParent(pivot); break;
                }
            }
            
        }

        if (rubikChecker.CheckWin())
        {
            Debug.Log("aaaaaaaaaaaa");
            Time.timeScale=1;
            StartCoroutine(Return());
        }
    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(3.5f);
        SceneReturnManager.Instance.Return();
    }
}
