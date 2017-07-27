# MS ToDo API #

> REST Api with single endpoint that returns all tasks (due today and overdues) and events for today from Outlook.com

[![Build Status](https://travis-ci.org/ziyasal/mstodoapi.svg?branch=master)](https://travis-ci.org/ziyasal/mstodoapi)

## Dev Environment ##

```sh
.NET Command Line Tools (1.0.4)

Product Information:
 Version:            1.0.4
 Commit SHA-1 hash:  af1e6684fd

Runtime Environment:
 OS Name:     Mac OS X
 OS Version:  10.12
 OS Platform: Darwin
 RID:         osx.10.12-x64
 Base Path:   /usr/local/share/dotnet/sdk/1.0.4
```

## Tech Stack ##

* AspNet Core v1.1.2

## Sample Request to `todos` endpoint ##

```sh
GET /api/todos HTTP/1.1
Host: localhost:5000
X-MSOutlookAPI-Token: <access_token here>
```

## Implementation Summary ##
* Separate [task](https://github.com/ziyasal/mstodoapi/blob/master/MSTodoApi/Infrastructure/Http/TasksClient.cs#L11) and [event](https://github.com/ziyasal/mstodoapi/blob/master/MSTodoApi/Infrastructure/Http/EventsClient.cs#L11) clients implemented to communicate with outlook API
* [`TokenMiddleware`](https://github.com/ziyasal/mstodoapi/blob/master/MSTodoApi/Infrastructure/Auth/TokenMiddleware.cs#L6) checks token key presented or not.
* [`TokenProvider`](https://github.com/ziyasal/mstodoapi/blob/master/MSTodoApi/Infrastructure/Auth/TokenProvider.cs#L8) (per request) gets token from header and supply to consumers which are clients to forward it to outlook.com 
* [`ToDoService`](https://github.com/ziyasal/mstodoapi/blob/master/MSTodoApi/Infrastructure/TodoService.cs#L10) implementation uses them and performs api calls in parallel to combine responses as required
* Implemented [configurable `retry` handler](https://github.com/ziyasal/mstodoapi/blob/master/MSTodoApi/Infrastructure/Http/HttpRetryMessageHandler.cs#L12) (DelegatingHandler) using `Polly` for `HttpClient` to retry failed requests with given retrycount

### Integration Testing ###
* Implemented  [`refresh token` handler](https://github.com/ziyasal/mstodoapi/blob/master/MSTodoApi.IntegrationTests/Infrastructure/RefreshTokenHandler.cs) (DelegatingHandler) in integration tests for `HttpClient` to refresh access token when request failed with http status `Unauthorized`
* Implemented [custom `TestServer`](https://github.com/ziyasal/mstodoapi/blob/master/MSTodoApi.IntegrationTests/Infrastructure/CustomTestServer.cs#L13) (starts server using `Startup` class) and [creates client with additional http handlers](https://github.com/ziyasal/mstodoapi/blob/master/MSTodoApi.IntegrationTests/Infrastructure/CustomTestServer.cs#L41) (i.e refresh token handler) to perform requests to api

### Known Issues ###

```sh
#TODO:
```

@z i λ a s a l
