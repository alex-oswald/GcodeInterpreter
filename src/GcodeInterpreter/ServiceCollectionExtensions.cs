using Microsoft.Extensions.DependencyInjection;

namespace GcodeInterpreter
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGcodeInterpreter(this IServiceCollection services)
        {
            services.AddSingleton<IGcodeInterpreter, Interpreter>();

            services.AddSingleton<ICommentRemover, AfterSemiColonCommentRemover>();
            services.AddSingleton<ICommentRemover, InPerenthesesisCommentRemover>();

            return services;
        }
    }
}