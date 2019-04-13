# HostFun

This is an example of using `HostBuilder` and `IHostedService` for hosting [Hangfire](https://www.hangfire.io) in a non-web-app while hosting the Hangfire dashboard in a separate web app.

## How to run

- `dotnet run --project HostFun` - This will start the standalone Hangfire Server
- In another terminal: `dotnet run --project HostFun.Web` - This will start the Hangfire Dashboard
- In a third terminal: `dotnet run --project HostFun.SecondApp` - This will start a console app that queues a single job

Now you should be able to see 1 succeeded job in the dashboard.
