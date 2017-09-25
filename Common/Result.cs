using System;

namespace Common
{
    public struct Result<T>
    {
        private readonly T _value;
        private Error _error;

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public T Value
        {
            get
            {
                if (IsFailure)
                {
                    throw new InvalidOperationException("Result is failure. Value not available.");
                }

                return _value;
            }
        }

        public Error Error
        {
            get
            {
                if (IsSuccess)
                {
                    throw new InvalidOperationException("Result is success. Error not available");
                }

                if (_error == null)
                {
                    _error = new GenericError("Default constructor should not be used.");
                }

                return _error;
            }
        }

        private Result(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            IsSuccess = true;
            _value = value;
            _error = default(Error);
        }

        private Result(Error error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            IsSuccess = false;
            _value = default(T);
            _error = error;
        }

        public static Result<T> Ok(T value) => new Result<T>(value);

        public static Result<T> Failure(Error error) => new Result<T>(error);

        public static Result<T> Failure(string message) => new Result<T>(new GenericError(message));

        public Result<T> OnFailure(Action<string> errorHandler)
        {
            if (IsFailure)
            {
                errorHandler(Error.Message);
            }

            return this;
        }

        public Result<T> OnFailure<K>(Action<K> errorHandler) where K : Error
        {
            if (IsFailure)
            {
                var kError = Error as K;
                if (kError != null)
                {
                    errorHandler(kError);
                }
            }

            return this;
        }

        public Result<T> OnSuccess(Action<T> onSuccessHandler)
        {
            if (IsSuccess)
            {
                onSuccessHandler(Value);
            }

            return this;
        }
    }
}
