using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace GenSort.Infrastructure
{
	[ExcludeFromCodeCoverage]
	public class TypeRegistrar : ITypeRegistrar
	{
		private readonly IServiceCollection _services;

		public TypeRegistrar(IServiceCollection services)
		{
			_services = services;
		}

		public ITypeResolver Build() => new TypeResolver(_services.BuildServiceProvider());

		public void Register(Type service, Type implementation)
		{
			_services.AddTransient(service, implementation);
		}

		public void RegisterInstance(Type service, object implementation)
		{
			_services.AddSingleton(service, implementation);
		}

		public void RegisterLazy(Type service, Func<object> factory)
		{
			_services.AddTransient(service, _ => factory());
		}
	}


	[ExcludeFromCodeCoverage]
	public class TypeResolver : ITypeResolver
	{
		private readonly IServiceProvider _provider;

		public TypeResolver(IServiceProvider provider)
		{
			_provider = provider;
		}

		public object? Resolve(Type? type)
		{
			if (type != null) return _provider.GetService(type);

			throw new TypeLoadException();
		}
	}

}
