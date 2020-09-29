# Covid-19-API

[![Build Status](https://travis-ci.com/alsami/Covid-19-API.svg?branch=master)](https://travis-ci.com/alsami/Covid-19-API) [![codecov](https://codecov.io/gh/alsami/Covid-19-API/branch/master/graph/badge.svg?token=UDYQ2H8MV8)](master)

Covid19Api serving most recent data related to corona virus based on the data available on [worldometers](https://www.worldometers.info/coronavirus/).

While `worldometers` is only displaying recent data, this API is providing historical data starting from today (2020-03-15, 06:30 AM, GMT+1).

## Available endpoints

The API is hosted on azure using an app-service. All times returned from the server are in UTC.

Api base url:
https://app-covid-19-statistics-api.azurewebsites.net

A swagger definition can be found [here](https://app-covid-19-statistics-api.azurewebsites.net/swagger/index.html) for testing the API.

## Sample App

There is a PWA available [here](https://app-covid-19-statistics.azurewebsites.net/). The code is located [here](https://github.com/alsami/Covid19-Statistics).

