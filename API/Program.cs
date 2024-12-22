using API;
using Application;
using Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = await HostCreator.CreateHost(args);

var serviceProvider = host.Services;
var app = new App(serviceProvider);
await app.RunAsync(new CancellationToken());