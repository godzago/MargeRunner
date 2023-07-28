using System;
using DG.Tweening;
using UnityEngine;

namespace HyperCasualRunner.Tweening
{
    /// <summary>
    /// Helper class for centralizing DOTween calls.
    /// </summary>
    public static class TweeningHelper
    {
        public static void SpendResource(int requiredResource, int collectedResource, int myResource, out Tween resourceSpendingTween, float spendingSpeed,
                                         Ease resourceSpendEase, TweenCallback<int> onTweenUpdate)
        {
            int remainedMoney = requiredResource - collectedResource;
            int to = myResource >= remainedMoney ? requiredResource : collectedResource + myResource;
            resourceSpendingTween = DOVirtual.Int(collectedResource, to, (float)to / requiredResource * spendingSpeed, onTweenUpdate)
                .SetEase(resourceSpendEase).SetAutoKill().SetRecyclable();
        }

        public static void SetParentAndJump(this Transform transform, Transform to, Action onJumped)
        {
            transform.SetParent(to);
            transform.DOLocalJump(Vector3.zero, 1f, 1, 0.4f).SetRecyclable().SetAutoKill().OnComplete(() => onJumped?.Invoke());
            transform.DOScale(Vector3.kEpsilon, 0.4f).SetEase(Ease.InBack, 5f).SetRecyclable().SetAutoKill().OnComplete(() => transform.gameObject.SetActive(false));
        }

        public static Sequence Jump(this Transform transform, Vector3 targetPoint)
        {
            return transform.DOJump(targetPoint, 1f, 1, 1f).SetRecyclable().SetAutoKill();
        }

        public static Sequence Jump(this Transform transform, Vector3 targetPoint, float jumpPower, int numJumps, float duration)
        {
            return transform.DOJump(targetPoint, jumpPower, numJumps, duration).SetRecyclable().SetAutoKill();
        }

        public static Sequence JumpLocal(this Transform transform, Vector3 targetPoint, float jumpPower, int numJumps, float duration)
        {
            return transform.DOLocalJump(targetPoint, jumpPower, numJumps, duration).SetRecyclable().SetAutoKill();
        }

        public static void Jump(this Transform transform, Transform targetPoint, float jumpPower, int numJumps, float duration, TweenCallback onComplete)
        {
            transform.DOJump(targetPoint.position, jumpPower, numJumps, duration).SetRecyclable().SetAutoKill().OnComplete(onComplete);
        }

        public static void JumpToDisappearSlowly(this Transform transform, Vector3 targetPoint, float jumpPower, int numJumps, float duration)
        {
            transform.Jump(targetPoint, jumpPower, numJumps, duration / 2f).OnComplete(() => transform.DeactivateSlowly(duration / 2f));
        }

        public static void Scale(this Transform transform, Vector3 targetScale, float duration, Ease ease)
        {
            transform.DOScale(targetScale, duration).SetEase(ease).SetAutoKill().SetRecyclable();
        }

        public static void DisappearCallback(this Transform transform, float duration, TweenCallback callback)
        {
            transform.DOScale(Vector3.one * float.Epsilon, duration).SetAutoKill().SetRecyclable().OnComplete(callback);
        }

        public static Tween ShowSmoothly(this Transform transform, float duration)
        {
            transform.gameObject.SetActive(true);
            transform.localScale = Vector3.one * float.Epsilon;
            return transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack, 3f).SetAutoKill();
        }

        public static Tween HideSmoothly(this Transform transform, float duration)
        {
            return transform.DOScale(Vector3.one * float.Epsilon, duration).SetEase(Ease.OutBack, 4f).SetAutoKill().OnComplete(() => transform.gameObject.SetActive(false));
        }

        public static Tweener DeactivateSlowly(this Transform transform, float duration)
        {
            return transform.DOScale(Vector3.one * Mathf.Epsilon, duration).SetAutoKill().SetRecyclable().OnComplete(() => { transform.gameObject.SetActive(false); });
        }

        public static Tweener RotateLocal(this Transform trans, Vector3 target, float duration)
        {
            return trans.DOLocalRotate(target, duration).SetRecyclable();
        }

        public static Tweener MoveLocal(this Transform trans, Vector3 target, float duration, Ease ease)
        {
            return trans.DOLocalMove(target, duration).SetRecyclable().SetEase(ease);
        }
        
        public static Tweener Move(this Transform trans, Vector3 target, float duration, Ease ease)
        {
            return trans.DOMove(target, duration).SetRecyclable().SetEase(ease);
        }

        public static void Kill(this Transform transform)
        {
            transform.DOKill();
        }
    }
}