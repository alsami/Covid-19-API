# Covid19Api

Covid19Api serving most recent data related to corona virus based on the data available on [worldometers](https://www.worldometers.info/coronavirus/).

While `worldometers` is only displaying recent data, this API is providing historical data starting from today (2020-03-14, 21:00 GMT+1).

## Available endpoints

The API is hosted on azure using an app-service. All times returned from the server are in UTC.

Api base url:
https://covid19-api.azurewebsites.net/

|API-Endpoint|Description|Response|
|------------|-----------|--------|
|`/api/v1/stats`|Returns the latest overall numbers.| `{"total":155840,"recovered":74438,"deaths":5814,"fetchedAt":"2020-03-14T20:23:34.44Z"}`|
|`/api/v1/cases/active`|Returns the latest overall numbers of active cases.| `{"total":75593,"mild":69685,"serious":5908,"fetchedAt":"2020-03-14T20:42:27.703Z","mildPercentage":92.18446152421520511158440596,"seriousPercentage":7.8155384757847948884155940400}`|
|`/api/v1/cases/closed`|Returns the latest overall numbers of closed cases.| `{"total":80252,"recovered":74438,"deaths":5814,"fetchedAt":"2020-03-14T20:49:31.146Z","recoveredPercentage":92.75532073967003937596570802,"deathsPercentage":7.2446792603299606240342919800}`|
