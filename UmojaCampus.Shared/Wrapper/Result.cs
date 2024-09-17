﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace UmojaCampus.Shared.Wrapper
{
    public class Result: IResult
    {
        public Result() { }

        public List<string> Messages { get; set; } = [];
        public bool Succeeded { get; set; }

        public static IResult Fail()
        {
            return new Result { Succeeded = false };
        }
        public static IResult Fail(string message)
        {
            return new Result { Succeeded = false, Messages = [message] };
        }

        public static IResult Fail(List<string> messages)
        {
            return new Result { Succeeded = false, Messages = messages };
        }

        public static Task<IResult> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<IResult> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));  
        }

        public static IResult Success()
        {
            return new Result { Succeeded = true };
        }

        public static IResult Success(string message)
        {
            return new Result { Succeeded = true, Messages = [message] };
        }

        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));   
        }   
    }

    public class Result<T> : Result, IResult<T>
    {
        public Result() 
        {
        }

        public T Data { get; set; }

        public static new Result<T> Fail()
        {
            return new Result<T> { Succeeded = false };
        }

        public static Result<T> Fail(T data)
        {
            return new Result<T> { Succeeded = false, Data = data };
        }

        public static Result<T> Fail(T data, List<string> messages)
        {
            return new Result<T> { Succeeded = false, Data = data, Messages = messages };
        }

        public static new Result<T> Fail(string message)
        {
            return new Result<T> { Succeeded = false, Messages = [message] };
        }

        public static new Task<Result<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static new Task<Result<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static new Result<T> Success()
        {
            return new Result<T> { Succeeded = true };
        }
        public static new Result<T> Success(string message)
        {
            return new Result<T> { Succeeded = true, Messages = [message] };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data };
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = [message] };
        }

        public static Result<T> Success(T data, List<string> messages)
        {
            return new Result<T> { Succeeded = true, Data = data, Messages = messages };
        }

        public static new Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static new Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
    }
}
