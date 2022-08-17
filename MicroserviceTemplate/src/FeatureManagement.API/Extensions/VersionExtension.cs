namespace FeatureManagement.API.Extensions
{
    public static class VersionExtension
    {
        public static void AddApiVersioningSupport(this IServiceCollection services)
        {
            services.AddApiVersioning()
                    .AddApiExplorer(
                        options =>
                        {
                            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                            // note: the specified format code will format the version as "'v'major[.minor][-status]"
                            options.GroupNameFormat = "'v'VVV";

                            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                            // can also be used to control the format of the API version in route templates
                            options.SubstituteApiVersionInUrl = true;
                        });
        }
    }
}