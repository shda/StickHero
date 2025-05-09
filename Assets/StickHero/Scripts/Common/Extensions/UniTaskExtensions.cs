using System;
using Cysharp.Threading.Tasks;

namespace CoffeeShopGame.Scripts.Common.Extensions
{
    public static class UniTaskExtensions
    {
        public static bool IsTaskComplete(this UniTask task)
        {
            return task.Status != UniTaskStatus.Pending;
        }

        public static UniTask WaitComplete(this UniTask task)
        {
            try
            {
                if (IsTaskComplete(task))
                {
                    return UniTask.CompletedTask;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return UniTask.CompletedTask;
        }
    }
}