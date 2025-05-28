using StackExchange.Redis;
using System.Text.Json;
using ImagineHubAPI.Interfaces;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;

    public RedisService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiration);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }
    
    public async Task RemoveByPatternAsync(string pattern)
    {
        var endpoints = _db.Multiplexer.GetEndPoints();
        var server = _db.Multiplexer.GetServer(endpoints.First());

        foreach (var key in server.Keys(pattern: pattern))
        {
            await _db.KeyDeleteAsync(key);
        }
    }

}