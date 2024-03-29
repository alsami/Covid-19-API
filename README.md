# Covid-19-API

[![Build Application](https://github.com/alsami/Covid-19-API/actions/workflows/push.yml/badge.svg)](https://github.com/alsami/Covid-19-API/actions/workflows/push.yml) [![codecov](https://codecov.io/gh/alsami/Covid-19-API/branch/main/graph/badge.svg?token=UDYQ2H8MV8)](https://codecov.io/gh/alsami/Covid-19-API)

Covid19Api serving most recent data related to corona virus based on the data available on [worldometers](https://www.worldometers.info/coronavirus/).

While `worldometers` is only displaying recent data, this API is providing historical data starting from today (2020-03-15, 06:30 AM, GMT+1).

## Available endpoints

The API is hosted on azure using an app-service. All times returned from the server are in UTC.

Api base url:
https://api.alsami-covid19-statistics.dev/

A swagger definition can be found [here](https://api.alsami-covid19-statistics.dev/swagger/index.html) for testing the API.

## Sample App

There is a PWA available [here](https://app.alsami-covid19-statistics.dev). The code is located [here](https://github.com/alsami/Covid19-Statistics).

