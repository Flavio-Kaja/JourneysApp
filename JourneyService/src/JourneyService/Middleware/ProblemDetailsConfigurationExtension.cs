namespace JourneyService.Middleware
{
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;
    using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;
    using JourneyService.Exceptions;

    /// <summary>
    /// Contains extension methods for the ProblemDetails library's ProblemDetailsOptions.
    /// </summary>
    public static class ProblemDetailsConfigurationExtension
    {
        /// <summary>
        /// Configures mappings from certain exception types to HTTP status codes.
        /// </summary>
        /// <param name="options">The ProblemDetailsOptions to configure.</param>
        public static void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.IncludeExceptionDetails = (ctx, ex) => false;
            // Map exceptions to specific HTTP status codes
            options.MapFluentValidationException();
            options.MapValidationException();
            options.MapToStatusCode<ForbiddenAccessException>(StatusCodes.Status401Unauthorized);
            options.MapToStatusCode<NoRolesAssignedException>(StatusCodes.Status403Forbidden);
            options.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);
            options.MapToStatusCode<InvalidSmartEnumPropertyName>(StatusCodes.Status422UnprocessableEntity);

            // Map certain unhandled exceptions to specific HTTP status codes
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

            // Catch-all mapping for any exception not specifically handled above
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Maps ValidationException to a custom ProblemDetails response.
        /// </summary>
        /// <param name="options">The ProblemDetailsOptions to configure.</param>
        private static void MapFluentValidationException(this ProblemDetailsOptions options) =>
            options.Map<FluentValidation.ValidationException>((ctx, ex) =>
            {
                // Get the factory service to create ProblemDetails instances
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                // Group validation errors by property name
                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.ErrorMessage).ToArray());

                // Return a ValidationProblemDetails instance with the grouped errors
                return factory.CreateValidationProblemDetails(ctx, errors);
            });

        /// <summary>
        /// Maps ValidationException to a custom ProblemDetails response.
        /// </summary>
        /// <param name="options">The ProblemDetailsOptions to configure.</param>
        private static void MapValidationException(this ProblemDetailsOptions options) =>
            options.Map<ValidationException>((ctx, ex) =>
            {
                // Get the factory service to create ProblemDetails instances
                var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

                // Group validation errors by key
                var errors = ex.Errors
                    .GroupBy(x => x.Key)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Select(x => x.Value.ToString()).ToArray());

                // Return a ValidationProblemDetails instance with the grouped errors
                return factory.CreateValidationProblemDetails(ctx, errors);
            });
    }
}
