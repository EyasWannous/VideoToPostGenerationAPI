﻿namespace VideoToPostGenerationAPI.Domain.Entities;

/// <summary>
/// Represents a header entity associated with a post.
/// </summary>
public class Header : BaseEntity
{
    /// <summary>
    /// Gets or sets the title of the header.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the associated post.
    /// </summary>
    public int PostId { get; set; }

    /// <summary>
    /// Gets or sets the post associated with this header.
    /// </summary>
    public Post Post { get; set; }
}
