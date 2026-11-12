using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.Options;

public class OptionsValidationException : Exception
{
	public string OptionsName { get; }

	public Type OptionsType { get; }

	public IEnumerable<string> Failures { get; }

	public override string Message => string.Join("; ", Failures);

	public OptionsValidationException(string optionsName, Type optionsType, IEnumerable<string>? failureMessages)
	{
		System.ThrowHelper.ThrowIfNull(optionsName, "optionsName");
		System.ThrowHelper.ThrowIfNull(optionsType, "optionsType");
		Failures = failureMessages ?? new List<string>();
		OptionsType = optionsType;
		OptionsName = optionsName;
	}
}
