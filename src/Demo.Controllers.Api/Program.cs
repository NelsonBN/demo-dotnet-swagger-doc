using System;
using System.IO;
using System.Reflection;
using Demo.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc(
            "v1",
            new OpenApiInfo
            {
                Version = $"v{Assembly.GetEntryAssembly()?.GetName()?.Version ?? new Version(0, 0, 1)}",
                Title = "Demo - Title",
                Description = "Demo - Description",
                TermsOfService = new Uri("http://demo.fk"),
                Contact = new OpenApiContact
                {
                    Name = "Demo - Contact - Name",
                    Email = "Demo - Contact - Email",
                    Url = new Uri("http://demo.fk/Contact")
                },
                License = new OpenApiLicense
                {
                    Name = "Demo - License - Name",
                    Url = new Uri("http://demo.fk/License")
                },
            });

        // Include XML documentation in swagger
        // Prepared when the solution contains multi projects
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        foreach(var fileName in Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories))
        {
            options.IncludeXmlComments(fileName);
        }
    });

builder.Services.AddControllers();


var app = builder.Build();

app.UseSwagger()
   .UseSwaggerUI();

app.UseRouting();
app.MapControllers();

await app.RunAsync();


[ApiController]
[Route("demo")]
public class DemoController : ControllerBase
{
    /// <summary>
    /// Get demo request
    /// </summary>
    /// 
    /// <remarks>
    /// A code example to test
    /// </remarks>
    /// 
    /// <response code="200">Demo 200 response</response>
    [HttpGet]
    [ProducesResponseType(typeof(DemoResponse), StatusCodes.Status200OK)]
    public IActionResult Get()
        => Ok(new DemoResponse
        {
            Message = "Demo response"
        });

    /// <summary>
    /// Post demo request
    /// </summary>
    /// 
    /// <remarks>
    /// The payload should be a json
    /// </remarks>
    /// 
    /// <param name="request">Demo payload</param>
    /// 
    /// <response code="201">When the request is created</response>
    /// <response code="400">When the request is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Post(DemoRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.MyRequest))
        {
            return BadRequest();
        }

        return Ok(new DemoResponse
        {
            Message = request.MyRequest
        });
    }
}
