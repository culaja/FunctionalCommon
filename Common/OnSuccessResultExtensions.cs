using System;

namespace Common
{
    public static class OnSuccessResultExtensions
    {
        public static Result<K> OnSuccess<T, K>(this Result<T> result, Func<T, Result<K>> funcHandler)
        {
            if (result.IsSuccess)
            {
                return funcHandler(result.Value);
            }

            return Result<K>.Failure(result.Error);
        }
    }
}
