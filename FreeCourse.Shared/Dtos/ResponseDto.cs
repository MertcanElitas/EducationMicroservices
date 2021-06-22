using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; set; }

        [JsonIgnore]
        public bool IsSuccess { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public List<string> Errors { get; set; }

        public static Response<T> Success(T data, int statusCode)
        {
            var result = new Response<T>() { Data = data, StatusCode = statusCode, IsSuccess = true };

            return result;
        }

        public static Response<T> Success(int statusCode)
        {
            var result = new Response<T>() { Data = default(T), StatusCode = statusCode, IsSuccess = true };

            return result;
        }

        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            var result = new Response<T>() { Errors = errors, StatusCode = statusCode, IsSuccess = false };

            return result;
        }

        public static Response<T> Fail(string error, int statusCode)
        {
            var result = new Response<T>() { Errors = new List<string> { error }, StatusCode = statusCode, IsSuccess = false };

            return result;
        }
    }
}
