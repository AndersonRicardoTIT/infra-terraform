using System;
using System.Collections.Generic;
using System.Linq;

namespace Usadosbr.Contas.Core.Common.Result
{
    public class Result<T> : IResult
    {
        public T? Data { get; private set; }

        public ResultStatus Status { get; private set; }

        public object? GetData()
        {
            return Data;
        }

        public IReadOnlyList<ValidationError> Errors => _errors.ToList();
        private IList<ValidationError> _errors = Array.Empty<ValidationError>();

        public static implicit operator T(Result<T> result) => result.Data!;
        public static implicit operator Result<T>(T value) => Ok(value);

        private Result(ResultStatus status)
        {
            Status = status;
        }

        public static Result<T> Ok(T data)
        {
            return new Result<T>(ResultStatus.Ok)
            {
                Data = data
            };
        }

        public static Result<T> Created(T data)
        {
            return new Result<T>(ResultStatus.Created)
            {
                Data = data
            };
        }

        public static Result<T> Error(params string[] errors)
        {
            var e = errors.Select(e => new ValidationError(string.Empty, new List<string> {e}));

            return new Result<T>(ResultStatus.Error)
            {
                _errors = e.ToArray()
            };
        }

        public static Result<T> Invalid(params ValidationError[] validations)
        {
            return new Result<T>(ResultStatus.Invalid)
            {
                _errors = validations
            };
        }

        public static Result<T> Invalid(params Notification[] notifications)
        {
            var errors = notifications
                .Select(x => new ValidationError(x.Key, new List<string> {x.Message}))
                .ToArray();

            return Invalid(errors);
        }

        public static Result<T> NotFound()
        {
            return new Result<T>(ResultStatus.NotFound);
        }
    }
}