# Covid19Api

[![Build Status](https://travis-ci.com/alsami/Covid19Api.svg?branch=master)](https://travis-ci.com/alsami/Covid19Api)

Covid19Api serving most recent data related to corona virus based on the data available on [worldometers](https://www.worldometers.info/coronavirus/).

While `worldometers` is only displaying recent data, this API is providing historical data starting from today (2020-03-15, 06:30 GMT+1).

## Available endpoints

The API is hosted on azure using an app-service. All times returned from the server are in UTC.

Api base url:
https://covid19-api.azurewebsites.net/

|API-Endpoint|Description|Response|
|------------|-----------|--------|
|`/api/v1/stats`|Returns the latest overall numbers.| `{"total":155840,"recovered":74438,"deaths":5814,"fetchedAt":"2020-03-14T20:23:34.44Z"}`|
|`/api/v1/cases/active`|Returns the latest overall numbers of active cases.| `{"total":75593,"mild":69685,"serious":5908,"fetchedAt":"2020-03-14T20:42:27.703Z","mildPercentage":92.18446152421520511158440596,"seriousPercentage":7.8155384757847948884155940400}`|
|`/api/v1/cases/active/history`|Returns the past seven days overall numbers of active cases.| `[{"total":74990,"mild":69338,"serious":5652,"fetchedAt":"2020-03-15T05:41:10.544Z","mildPercentage":92.46299506600880117348979864,"seriousPercentage":7.5370049339911988265102013600},{"total":74970,"mild":69318,"serious":5652,"fetchedAt":"2020-03-15T05:05:08.257Z","mildPercentage":92.46098439375750300120048019,"seriousPercentage":7.5390156062424969987995198100}]`|
|`/api/v1/cases/closed`|Returns the latest overall numbers of closed cases.| `{"total":80252,"recovered":74438,"deaths":5814,"fetchedAt":"2020-03-14T20:49:31.146Z","recoveredPercentage":92.75532073967003937596570802,"deathsPercentage":7.2446792603299606240342919800}`|
|`/api/v1/cases/closed/history`|Returns the past seven days overall numbers of closed cases.| `[{"total":81776,"recovered":75937,"deaths":5839,"fetchedAt":"2020-03-15T05:41:10.544Z","recoveredPercentage":92.85976325572295049892388965,"deathsPercentage":7.1402367442770495010761103500},{"total":81775,"recovered":75936,"deaths":5839,"fetchedAt":"2020-03-15T05:05:08.257Z","recoveredPercentage":92.85967594007948639559767655,"deathsPercentage":7.1403240599205136044023234500}]`|

