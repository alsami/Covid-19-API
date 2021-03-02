// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain
{
    public class RequestInfo
    {
        public RequestInfo(string scheme, string protocol, string method, string path, string ipAddress)
        {
            this.Scheme = scheme;
            this.Protocol = protocol;
            this.Method = method;
            this.Path = path;
            this.IpAddress = ipAddress;
        }

        public string Scheme { get; private set; }
        public string Protocol { get; private set; }
        public string Method { get; private set; }
        public string Path { get; private set; }
        public string IpAddress { get; private set; }
    }
}