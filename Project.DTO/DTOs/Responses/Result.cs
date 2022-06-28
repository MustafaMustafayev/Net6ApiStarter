﻿namespace Project.DTO.DTOs.Responses;

public record Result : IResult
{
    public Result(bool success, string message)
        : this(success)
    {
        Message = message;
    }

    public Result(bool success)
    {
        Success = success;
    }

    public bool Success { get; }

    public string Message { get; }
}