# MS ToDo API #

REST Api with single endpoint that returns all tasks and events for today from Outlook.com.


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

## Implementation Summary ##

* Separate task and event clients implemented to communicate outlook API
* `ToDoService` implementation uses them and performs api calls in parallel to combine responses as required
* There is `refresh token` handler (DelegatingHandler) for `HttpClient` to refresh access token when request failed with http status `Unauthorized`
* There is configurable `retry` handler (DelegatingHandler) using `Polly` for `HttpClient` to retry failed requests with given retrycount


@z i λ a s a l
