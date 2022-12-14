using Booking.API;
using Booking.API.Controllers;
using Booking.API.CronJob;
using Booking.Domain.Models;
using Booking.Middleware;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using Microsoft.AspNetCore.HttpOverrides;
using Quartz;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddSignalR();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    var jobKey = new JobKey("MyCronJob");
    q.AddJob<BookingOutOfDayJob>(opts => opts.WithIdentity(jobKey));
    q.AddJob<RemindPaymentDailyJob>(opts => opts.WithIdentity("RemindPayment"));
    q.AddJob<ExtendDueBookingJob>(opts => opts.WithIdentity("ExtendDueBooking"));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("MyCronJob-trigger")
        .WithCronSchedule("0 0 0/3 1/1 * ? *"));
    q.AddTrigger(opts => opts
        .ForJob("RemindPayment")
        .WithIdentity("RemindPayment-trigger")
        .WithCronSchedule("0 0 0/12 1/1 * ? *"));
    q.AddTrigger(opts => opts
        .ForJob("ExtendDueBooking")
        .WithIdentity("ExtendDueBooking-trigger")
        .WithCronSchedule("0 0 0/8 1/1 * ? *"));

});
    

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddCors();

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables().Build();

configuration.GetSection("AppSettings").Get<AppSettings>(options => options.BindNonPublicProperties = true);
configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>(options => options.BindNonPublicProperties = true);

//var hcBuilder = builder.Services.AddHealthChecks();

//hcBuilder.AddRabbitMQ($"amqp://localhost", name: "rabbitmq", tags: new string[] { "rabbitmqbus" });
var forwardingOptions = new ForwardedHeadersOptions() 
{ 
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.All 
};
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.ConfigureEventBus(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                                                    //.AllowCredentials()
                .WithExposedHeaders("*")
                .SetPreflightMaxAge(TimeSpan.FromSeconds(600)));

app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<RequestLoggerMiddleware>();
app.UseForwardedHeaders(new ForwardedHeadersOptions() 
{ 
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.All 
});
app.MapControllers();
app.MapHub<SignalHub>("/hub");

app.Run();
