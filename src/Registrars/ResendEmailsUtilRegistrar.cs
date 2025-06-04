using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Resend.ClientUtil.Registrars;
using Soenneker.Resend.Emails.Abstract;

namespace Soenneker.Resend.Emails.Registrars;

/// <summary>
/// A utility library for email related Resend operations
/// </summary>
public static class ResendEmailsUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IResendEmailsUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddResendEmailsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddResendClientUtilAsSingleton().TryAddSingleton<IResendEmailsUtil, ResendEmailsUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IResendEmailsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddResendEmailsUtilAsScoped(this IServiceCollection services)
    {
        services.AddResendClientUtilAsSingleton().TryAddScoped<IResendEmailsUtil, ResendEmailsUtil>();

        return services;
    }
}
