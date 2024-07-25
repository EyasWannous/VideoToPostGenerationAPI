﻿using VideoToPostGenerationAPI.Domain.Abstractions.IRepositories;
using VideoToPostGenerationAPI.Domain.Entities;
using VideoToPostGenerationAPI.Presistence.Data;

namespace VideoToPostGenerationAPI.Presistence.Repositories;

public class VideoRepository(AppDbContext context) : BaseRepository<Video>(context) , IVideoRepository
{
}
