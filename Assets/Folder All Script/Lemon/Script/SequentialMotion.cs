using UnityEngine;
using System;
using System.Collections.Generic;

public class SequentialMotion : MonoBehaviour
{
    [Serializable]
    public class MotionPhase
    {
        [Header("Move (Absolute From Play Base)")]
        public bool useMove;
        public Vector3 moveOffset;
        public float moveTime = 1f;
        public LeanTweenType moveEase = LeanTweenType.easeInOutSine;

        [Header("Rotate (360° Absolute From Play Base)")]
        public bool useRotate;
        public Vector3 rotateAxis = Vector3.up;
        public float rotateAngle = 0f; // theo thang 360°
        public float rotateTime = 1f;
        public LeanTweenType rotateEase = LeanTweenType.easeInOutSine;
    }

    [Header("Motion Phases (Execute Once Per Play)")]
    public List<MotionPhase> phases = new List<MotionPhase>();
    [Header("Standalone Motions (PlayAlone)")]
    public List<MotionPhase> standaloneMotions = new List<MotionPhase>();

    [Header("Play Call Control")]
    public bool allowMultiplePlayCalls = true;
    [Min(1)]
    public int maxPlayCalls = 1;

    private int playCallCount = 0;

    private Vector3 basePosition;
    private Quaternion baseRotation;

    private int currentPhase;

    // =============================

    public void Play()
    {
        // Kiểm soát số lần gọi Play()
        if (!allowMultiplePlayCalls && playCallCount > 0)
            return;

        if (playCallCount >= maxPlayCalls)
            return;

        playCallCount++;

        // Base được lấy tại thời điểm gọi Play()
        // => đảm bảo cộng dồn giữa các lần Play()
        basePosition = transform.position;
        baseRotation = transform.rotation;

        currentPhase = 0;
        PlayNext();
    }
    public void PlayAlone(int index)
    {
        if (index < 0 || index >= standaloneMotions.Count)
            return;

        MotionPhase phase = standaloneMotions[index];

        // Base lấy tại thời điểm gọi
        Vector3 basePos = transform.position;
        Quaternion baseRot = transform.rotation;

        // ===== MOVE =====
        if (phase.useMove)
        {
            Debug.Log("dang di chuyen");
            Vector3 target = basePos + phase.moveOffset;

            LeanTween.move(gameObject, target, phase.moveTime)
                .setEase(phase.moveEase);
        }

        // ===== ROTATE =====
        if (phase.useRotate)
        {
            Debug.Log("dang xoay");
            Vector3 axis = phase.rotateAxis.normalized;
            float targetAngle = phase.rotateAngle;

            LeanTween.value(gameObject, 0f, targetAngle, phase.rotateTime)
                .setEase(phase.rotateEase)
                .setOnUpdate((float angle) =>
                {
                    transform.rotation =
                        baseRot *
                        Quaternion.AngleAxis(angle, axis);
                })
                .setOnComplete(() =>
                {
                    transform.rotation =
                        baseRot *
                        Quaternion.AngleAxis(targetAngle, axis);
                });
        }
    }

    // =============================

    private void PlayNext()
    {
        if (currentPhase >= phases.Count)
            return;

        MotionPhase phase = phases[currentPhase++];
        int tweenCount = 0;
        int completed = 0;

        Action onComplete = () =>
        {
            completed++;
            if (completed >= tweenCount)
                PlayNext();
        };

        // ===== MOVE =====
        if (phase.useMove)
        {
            tweenCount++;

            Vector3 target = basePosition + phase.moveOffset;

            LeanTween.move(gameObject, target, phase.moveTime)
                .setEase(phase.moveEase)
                .setOnComplete(onComplete);
        }

        // ===== ROTATE (Không drift, 360° chuẩn) =====
        if (phase.useRotate)
        {
            tweenCount++;

            Vector3 axis = phase.rotateAxis.normalized;
            float targetAngle = phase.rotateAngle;

            LeanTween.value(gameObject, 0f, targetAngle, phase.rotateTime)
                .setEase(phase.rotateEase)
                .setOnUpdate((float angle) =>
                {
                    transform.rotation =
                        baseRotation *
                        Quaternion.AngleAxis(angle, axis);
                })
                .setOnComplete(() =>
                {
                    // Ép giá trị chính xác để tránh 89° thay vì 90°
                    transform.rotation =
                        baseRotation *
                        Quaternion.AngleAxis(targetAngle, axis);

                    onComplete();
                });
        }

        if (tweenCount == 0)
            PlayNext();
    }

    // =============================
    // Optional: reset bộ đếm nếu cần

    public void ResetPlayCallCounter()
    {
        playCallCount = 0;
    }
}
