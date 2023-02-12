namespace SampleAPI.Infrastructure.Extensions
{
    internal static class TypeExtensions
    {
        internal static IEnumerable<Type> GetImplementations(this Type @interface)
        {
            return @interface.Assembly.GetTypes()
                .Where(type =>
                    type.IsClass && !type.IsAbstract &&
                    (@interface.IsAssignableFrom(type) ||
                    type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == @interface)));
        }
    }
}
