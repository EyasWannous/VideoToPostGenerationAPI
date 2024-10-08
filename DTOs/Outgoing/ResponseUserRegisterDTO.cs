﻿namespace VideoToPostGenerationAPI.DTOs.Outgoing;

public record ResponseUserRegisterDTO
{
    public string Message { get; set; } = string.Empty;

    public bool IsSuccess { get; set; }

    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

    public DateTime? ExpireDate { get; set; }

    public string? JwtToken { get; set; }
}
