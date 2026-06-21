using AutoMapper;
using MedicalClinic.ManagementSystem.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace MedicalClinic.ManagementSystem.Tests.Helpers;

public class MapperFactory
{
    public static IMapper Create()
    {
        var configExpression = new MapperConfigurationExpression();
        configExpression.AddProfile<MapperProfile>();

        var config = new MapperConfiguration(configExpression, new LoggerFactory());
        //config.AssertConfigurationIsValid();
        return config.CreateMapper();
    }
}
