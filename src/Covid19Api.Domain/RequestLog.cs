
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Covid19Api.Domain;

public class RequestLog
{
    public RequestLog(Guid id, DateTime loggedAt, RequestInfo requestInfo)
    {
        this.Id = id;
        this.LoggedAt = loggedAt;
        this.RequestInfo = requestInfo;
    }

    public Guid Id { get; private set; }
    public DateTime LoggedAt { get; private set; }
    public RequestInfo RequestInfo { get; private set; }
}