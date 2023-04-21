using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware;

public class StatisticMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IStatisticService _statisticService;

    public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService)
    {
        _next = next;
        _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;

        Task staticRegTask = Task.Run(() => _statisticService.RegisterVisitAsync(path));
        await staticRegTask;
        Console.WriteLine(staticRegTask.Status); // just for debugging purposes

        await Task.Run(() => UpdateHeaders(context));
 
        await _next(context);
    }

	private async Task UpdateHeaders(HttpContext context)
	{
        long visitsCount = await _statisticService.GetVisitsCountAsync(context.Request.Path);
        context.Response.Headers.Add(CustomHttpHeaders.TotalPageVisits, visitsCount.ToString());
    }
}
