using System.Diagnostics;
using System.Text.Json;
using MediatR;
using ServiceName.Core.Common.Interfaces;

namespace ServiceName.Core.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        ILoggingService _loggingService;
        private readonly Stopwatch _timer;

        public LoggingBehaviour(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _timer = new Stopwatch();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var correlationId = Guid.NewGuid();
            var requestName = typeof(TRequest).Name;

            await _loggingService.LogInformationAsync(@"correlationId {correlationId} 
                                                        requestName {requestName}
                                                        type {requestType}
                                                        payload {requestPayload}", correlationId, requestName, "Request", JsonSerializer.Serialize(request));

            _timer.Start();
            var response = await next();
            _timer.Stop();

            await _loggingService.LogInformationAsync(@"correlationId {correlationId}
                                                        requestName {requestName}
                                                        type {requestType}
                                                        payload {requestPayload} {elapsedMilliseconds}", correlationId, requestName, "Response", JsonSerializer.Serialize(response), _timer.ElapsedMilliseconds);

            return response;
        }
    }


}
