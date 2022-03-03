﻿namespace TweetBook.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object? response, TimeSpan timeTimeLive);

        Task<string?> GetCachedResponseAsync(string cacheKey);
    }
}
