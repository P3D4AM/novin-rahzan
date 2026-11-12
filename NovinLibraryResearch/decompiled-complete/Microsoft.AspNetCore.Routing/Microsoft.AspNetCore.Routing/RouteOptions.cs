using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Microsoft.AspNetCore.Routing;

public class RouteOptions
{
	private IDictionary<string, Type> _constraintTypeMap = GetDefaultConstraintMap();

	public bool LowercaseUrls { get; set; }

	public bool AppendTrailingSlash { get; set; }

	public IDictionary<string, Type> ConstraintMap
	{
		get
		{
			return _constraintTypeMap;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("ConstraintMap");
			}
			_constraintTypeMap = value;
		}
	}

	private static IDictionary<string, Type> GetDefaultConstraintMap()
	{
		return new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
		{
			{
				"int",
				typeof(IntRouteConstraint)
			},
			{
				"bool",
				typeof(BoolRouteConstraint)
			},
			{
				"datetime",
				typeof(DateTimeRouteConstraint)
			},
			{
				"decimal",
				typeof(DecimalRouteConstraint)
			},
			{
				"double",
				typeof(DoubleRouteConstraint)
			},
			{
				"float",
				typeof(FloatRouteConstraint)
			},
			{
				"guid",
				typeof(GuidRouteConstraint)
			},
			{
				"long",
				typeof(LongRouteConstraint)
			},
			{
				"minlength",
				typeof(MinLengthRouteConstraint)
			},
			{
				"maxlength",
				typeof(MaxLengthRouteConstraint)
			},
			{
				"length",
				typeof(LengthRouteConstraint)
			},
			{
				"min",
				typeof(MinRouteConstraint)
			},
			{
				"max",
				typeof(MaxRouteConstraint)
			},
			{
				"range",
				typeof(RangeRouteConstraint)
			},
			{
				"alpha",
				typeof(AlphaRouteConstraint)
			},
			{
				"regex",
				typeof(RegexInlineRouteConstraint)
			},
			{
				"required",
				typeof(RequiredRouteConstraint)
			}
		};
	}
}
