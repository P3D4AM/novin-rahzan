using System;
using System.Collections.Generic;
using System.Globalization;
using Serilog.Exceptions.Destructurers;
using Serilog.Exceptions.Filters;

namespace Serilog.Exceptions.Core;

public class DestructuringOptionsBuilder : IDestructuringOptions
{
	public static readonly IExceptionDestructurer[] DefaultDestructurers = new IExceptionDestructurer[9]
	{
		new ExceptionDestructurer(),
		new ArgumentExceptionDestructurer(),
		new ArgumentOutOfRangeExceptionDestructurer(),
		new AggregateExceptionDestructurer(),
		new RegexMatchTimeoutExceptionDestructurer(),
		new ReflectionTypeLoadExceptionDestructurer(),
		new OperationCanceledExceptionDestructurer(),
		new TaskCanceledExceptionDestructurer(),
		new SocketExceptionDestructurer()
	};

	public static readonly IExceptionPropertyFilter IgnoreStackTraceAndTargetSiteExceptionFilter = new IgnorePropertyByNameExceptionFilter("StackTrace", "TargetSite");

	private readonly List<IExceptionDestructurer> destructurers = new List<IExceptionDestructurer>();

	public string RootName { get; private set; } = "ExceptionDetail";


	public int DestructuringDepth { get; private set; } = 10;


	public bool DisableReflectionBasedDestructurer { get; private set; }

	public IEnumerable<IExceptionDestructurer> Destructurers => destructurers;

	public IExceptionPropertyFilter? Filter { get; private set; }

	public DestructuringOptionsBuilder WithDestructurers(IEnumerable<IExceptionDestructurer> destructurers)
	{
		if (destructurers == null)
		{
			throw new ArgumentNullException("destructurers");
		}
		this.destructurers.AddRange(destructurers);
		return this;
	}

	public DestructuringOptionsBuilder WithDefaultDestructurers()
	{
		return WithDestructurers(DefaultDestructurers);
	}

	public DestructuringOptionsBuilder WithFilter(IExceptionPropertyFilter filter)
	{
		if (Filter != null)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Resources.FilterAlreadySet, "CompositeExceptionPropertyFilter"));
		}
		Filter = filter;
		return this;
	}

	public DestructuringOptionsBuilder WithIgnoreStackTraceAndTargetSiteExceptionFilter()
	{
		return WithFilter(IgnoreStackTraceAndTargetSiteExceptionFilter);
	}

	public DestructuringOptionsBuilder WithRootName(string rootName)
	{
		if (rootName == null)
		{
			throw new ArgumentNullException("rootName");
		}
		if (rootName.Length == 0)
		{
			throw new ArgumentException(Resources.CannotBeEmpty, "rootName");
		}
		RootName = rootName;
		return this;
	}

	public DestructuringOptionsBuilder WithDestructuringDepth(int destructuringDepth)
	{
		if (destructuringDepth <= 0)
		{
			throw new ArgumentOutOfRangeException("destructuringDepth", destructuringDepth, Resources.DestructuringDepthMustBeMoreThanZero);
		}
		DestructuringDepth = destructuringDepth;
		return this;
	}

	public DestructuringOptionsBuilder WithoutReflectionBasedDestructurer()
	{
		DisableReflectionBasedDestructurer = true;
		return this;
	}
}
