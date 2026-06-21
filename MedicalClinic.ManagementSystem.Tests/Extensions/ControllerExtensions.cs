using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MedicalClinic.ManagementSystem.Tests.Extensions;

public static class ControllerExtensions
{
    public static void SetUserIsAuthenticated(this ControllerBase controller, bool isAuthenticated)
    {
        var httpContext = new Mock<HttpContext>();
        httpContext.SetupGet(x => x.User.Identity!.IsAuthenticated).Returns(isAuthenticated);

        var controllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        controller.ControllerContext = controllerContext;
    }
}
