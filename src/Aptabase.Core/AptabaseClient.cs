﻿using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace Aptabase.Core;

public class AptabaseClient : IAptabaseClient
{
    private readonly Channel<EventData> _channel;
    private readonly Task _processingTask;
    private readonly AptabaseClientBase _client;
    private readonly ILogger<AptabaseClient>? _logger;
    private readonly CancellationTokenSource _cts;

    public AptabaseClient(string appKey, AptabaseOptions? options, ILogger<AptabaseClient>? logger)
    {
        _client = new AptabaseClientBase(appKey, options, logger);
        _channel = Channel.CreateUnbounded<EventData>();
        _processingTask = Task.Run(ProcessEventsAsync);
        _logger = logger;
        _cts = new CancellationTokenSource();
    }

    public Task TrackEvent(string eventName, Dictionary<string, object>? props = null)
    {
        if (!_channel.Writer.TryWrite(new EventData(eventName, props)))
        {
            _logger?.LogError("Failed to perform TrackEvent");
        }

        return Task.CompletedTask;
    }

    private async Task ProcessEventsAsync()
    {
        try
        {
            while (await _channel.Reader.WaitToReadAsync())
            {
                if (_cts.IsCancellationRequested)
                {
                    break;
                }

                while (_channel.Reader.TryRead(out var eventData))
                {
                    if (_cts.IsCancellationRequested)
                    {
                        break;
                    }

                    try
                    {
                        await _client.TrackEvent(eventData);
                    }
                    catch (Exception ex)
                    {
                        // best effort
                        _logger?.LogError(ex, "Failed to perform TrackEvent");
                    }
                }
            }
        }
        catch (ChannelClosedException)
        {
            // ignore
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            _cts.Cancel();
        }
        catch { }

        _channel.Writer.Complete();

        if (_processingTask?.IsCompleted == false)
        {
            await _processingTask;
        }

        _cts.Dispose();

        await _client.DisposeAsync();

        GC.SuppressFinalize(this);
    }
}