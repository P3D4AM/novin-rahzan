using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Logging;

[UnsupportedOSPlatform("browser")]
public static class ConsoleLoggerExtensions
{
	internal const string TrimmingRequiresUnreferencedCodeMessage = "TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.";

	[UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "AddConsoleFormatter and RegisterProviderOptions are only dangerous when the Options type cannot be statically analyzed, but that is not the case here. The DynamicallyAccessedMembers annotations on them will make sure to preserve the right members from the different options objects.")]
	public static ILoggingBuilder AddConsole(this ILoggingBuilder builder)
	{
		builder.AddConfiguration();
		builder.AddConsoleFormatter<JsonConsoleFormatter, JsonConsoleFormatterOptions>();
		builder.AddConsoleFormatter<SystemdConsoleFormatter, ConsoleFormatterOptions>();
		builder.AddConsoleFormatter<SimpleConsoleFormatter, SimpleConsoleFormatterOptions>();
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
		LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(builder.Services);
		return builder;
	}

	public static ILoggingBuilder AddConsole(this ILoggingBuilder builder, Action<ConsoleLoggerOptions> configure)
	{
		if (configure == null)
		{
			throw new ArgumentNullException("configure");
		}
		builder.AddConsole();
		builder.Services.Configure(configure);
		return builder;
	}

	public static ILoggingBuilder AddSimpleConsole(this ILoggingBuilder builder)
	{
		return builder.AddFormatterWithName("simple");
	}

	public static ILoggingBuilder AddSimpleConsole(this ILoggingBuilder builder, Action<SimpleConsoleFormatterOptions> configure)
	{
		return builder.AddConsoleWithFormatter("simple", configure);
	}

	public static ILoggingBuilder AddJsonConsole(this ILoggingBuilder builder)
	{
		return builder.AddFormatterWithName("json");
	}

	public static ILoggingBuilder AddJsonConsole(this ILoggingBuilder builder, Action<JsonConsoleFormatterOptions> configure)
	{
		return builder.AddConsoleWithFormatter("json", configure);
	}

	public static ILoggingBuilder AddSystemdConsole(this ILoggingBuilder builder, Action<ConsoleFormatterOptions> configure)
	{
		return builder.AddConsoleWithFormatter("systemd", configure);
	}

	public static ILoggingBuilder AddSystemdConsole(this ILoggingBuilder builder)
	{
		return builder.AddFormatterWithName("systemd");
	}

	internal static ILoggingBuilder AddConsoleWithFormatter<TOptions>(this ILoggingBuilder builder, string name, Action<TOptions> configure) where TOptions : ConsoleFormatterOptions
	{
		if (configure == null)
		{
			throw new ArgumentNullException("configure");
		}
		builder.AddFormatterWithName(name);
		builder.Services.Configure(configure);
		return builder;
	}

	private static ILoggingBuilder AddFormatterWithName(this ILoggingBuilder builder, string name)
	{
		return builder.AddConsole(delegate(ConsoleLoggerOptions options)
		{
			options.FormatterName = name;
		});
	}

	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public static ILoggingBuilder AddConsoleFormatter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFormatter, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this ILoggingBuilder builder) where TFormatter : ConsoleFormatter where TOptions : ConsoleFormatterOptions
	{
		builder.AddConfiguration();
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ConsoleFormatter, TFormatter>());
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<TOptions>, ConsoleLoggerFormatterConfigureOptions<TFormatter, TOptions>>());
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<TOptions>, ConsoleLoggerFormatterOptionsChangeTokenSource<TFormatter, TOptions>>());
		return builder;
	}

	[RequiresUnreferencedCode("TOptions's dependent types may have their members trimmed. Ensure all required members are preserved.")]
	public static ILoggingBuilder AddConsoleFormatter<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TFormatter, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TOptions>(this ILoggingBuilder builder, Action<TOptions> configure) where TFormatter : ConsoleFormatter where TOptions : ConsoleFormatterOptions
	{
		if (configure == null)
		{
			throw new ArgumentNullException("configure");
		}
		builder.AddConsoleFormatter<TFormatter, TOptions>();
		builder.Services.Configure(configure);
		return builder;
	}
}
