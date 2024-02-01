using Microsoft.Extensions.DependencyInjection;

namespace TCK.Common.DependencyInjection
{
    public static class DecoratorServiceCollectionExtensions
    {
        public static IServiceCollection AddDecorator<TService, TDecorator>(this IServiceCollection services, Action<IServiceCollection> configureDecoratedServices)
            where TDecorator : class, TService
            where TService : class
        {
            var decoratedServices = new ServiceCollection();

            // This calls back to the configuration lambda.
            configureDecoratedServices(decoratedServices);

            // For now, support defining only single decorated service.
            var decoratedServiceDescriptor = decoratedServices.SingleOrDefault(sd => sd.ServiceType == typeof(TService));

            if (decoratedServiceDescriptor == null)
            {
                throw new InvalidOperationException("No decorated service has been configured!");
            }

            // We will replace this descriptor with a tweaked one later.
            decoratedServices.Remove(decoratedServiceDescriptor);

            // Add all remaining services to main collection.
            foreach (var service in decoratedServices)
            {
                services.Add(service);
            }

            // This factory allows us to pass some dependencies 
            // (the decoratee instance) manually,
            // which is not possible with something like GetRequiredService. 
            var decoratorInstanceFactory = ActivatorUtilities.CreateFactory(typeof(TDecorator), new[] { typeof(TService) });

            var decoratedImplementationType = decoratedServiceDescriptor.GetImplementationType();

#pragma warning disable IDE0039  // Use local function // Surpessing this for readability on this code
            Func<IServiceProvider, TDecorator> decoratorFactory = sp =>
#pragma warning restore IDE0039
            {
                // Note that we query the decoratee by it's implementation type, avoiding any ambiguity. 
                var decoratee = sp.GetRequiredService(decoratedImplementationType);

                // Pass the decoratee manually. All other dependencies are resolved as usual.
                var decorator = (TDecorator)decoratorInstanceFactory(sp, new[] { decoratee });

                return decorator;
            };

            // Decorator inherits decoratee's lifetime.
            var decoratorDescriptor = ServiceDescriptor.Describe(typeof(TService), decoratorFactory, decoratedServiceDescriptor.Lifetime);

            // Re-create the decoratee without original service type (interface).
            // This allows to create decoratee instances via service provider, utilizing its lifetime scope control finctionality.
            decoratedServiceDescriptor = RefactorDecoratedServiceDescriptor(decoratedServiceDescriptor);

            services.Add(decoratedServiceDescriptor);
            services.Add(decoratorDescriptor);

            return services;
        }

        /// <summary>
        /// Infers the implementation type for any kind of service descriptor (i.e. even when implementation type is not specified explicitly).
        /// </summary>
        private static Type GetImplementationType(this ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor.ImplementationType != null)
            {
                return serviceDescriptor.ImplementationType;
            }

            if (serviceDescriptor.ImplementationInstance != null)
            {
                return serviceDescriptor.ImplementationInstance.GetType();
            }

            // Get the type from the return type of the factory delegate.
            // Due to covariance, the delegate object can have more concrete type than the factory delegate defines (object).
            if (serviceDescriptor.ImplementationFactory != null)
            {
                return serviceDescriptor.ImplementationFactory.GetType().GenericTypeArguments[1];
            }

            // This should not be possible, but just in case.
            throw new InvalidOperationException("No way to get the decoratee implementation type.");
        }

        /// <summary>
        /// The goal of this method is to replace the service type (interface) with the implementation type in any kind of service descriptor.
        /// Actually, we build new service descriptor.
        /// </summary>
        private static ServiceDescriptor RefactorDecoratedServiceDescriptor(ServiceDescriptor decoratedServiceDescriptor)
        {
            var decoratedImplementationType = decoratedServiceDescriptor.GetImplementationType();

            if (decoratedServiceDescriptor.ImplementationFactory != null)
            {
                decoratedServiceDescriptor = ServiceDescriptor.Describe(decoratedImplementationType, decoratedServiceDescriptor.ImplementationFactory, decoratedServiceDescriptor.Lifetime);
            }
            else if (decoratedServiceDescriptor.ImplementationInstance != null)
            {
                decoratedServiceDescriptor = ServiceDescriptor.Singleton(decoratedImplementationType, decoratedServiceDescriptor.ImplementationInstance);
            }
            else
            {
                decoratedServiceDescriptor = ServiceDescriptor.Describe(decoratedImplementationType, decoratedImplementationType, decoratedServiceDescriptor.Lifetime);
            }

            return decoratedServiceDescriptor;
        }
    }
}
