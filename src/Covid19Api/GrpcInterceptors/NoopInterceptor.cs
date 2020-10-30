using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Covid19Api.GrpcInterceptors
{
    public class NoopInterceptor : Interceptor
    {
        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            return continuation(request, context);
        }
    }
}