# Implementing Rate Limiting Middleware in ASP.NET Core

Source code used in the article [Implementing Rate Limiting Middleware in ASP.NET Core](https://blouppy.com/blog/2023-07-15-implementing-rate-limiting-middleware/) on the [Blouppy](https://blouppy.com) website.

A small customers API protected by ASP.NET Core's built-in rate limiting middleware (`Microsoft.AspNetCore.RateLimiting`):

- the four algorithms as named policies: `fixed`, `sliding`, `token` and `concurrency`;
- `per-ip`, a policy partitioned by client IP address;
- a `GlobalLimiter` (100 requests per minute per IP) that applies to every request;
- `429 Too Many Requests` with a `Retry-After` header when a request is rejected.

| Endpoint | Rate limiting |
| --- | --- |
| `GET /api/customers` | `[EnableRateLimiting("fixed")]`: 10 requests per 10 seconds, 5 more queued |
| `GET /api/customers/{id}` | `[DisableRateLimiting]`: none, not even the global limiter |
| `GET /api/minimal/customers` | `.RequireRateLimiting("per-ip")`: 5 requests per 10 seconds per IP |

## Project type
- ASP.NET Core Web API (controllers and Minimal APIs)

## Project version
- .NET 10 (C# 14)

## NuGet packages
- **Microsoft.EntityFrameworkCore.Sqlite**: 10.0.14
- **Microsoft.AspNetCore.OpenApi**: 10.0.14

## Run it

```bash
dotnet run --launch-profile http

# the 6th request within 10 seconds is rejected
for i in $(seq 6); do curl -s -o /dev/null -w "%{http_code}\n" http://localhost:5207/api/minimal/customers; done
curl -i http://localhost:5207/api/minimal/customers
```

In Development the SQLite database (`CustomerDb.db`) is recreated and seeded on every start.

## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!

## Support this project! :pray:
If you want to help me to maintain this project or add improvements, it would be a pleasure!
