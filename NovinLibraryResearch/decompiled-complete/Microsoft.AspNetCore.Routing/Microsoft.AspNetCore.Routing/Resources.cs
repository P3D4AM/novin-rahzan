using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Microsoft.AspNetCore.Routing;

internal static class Resources
{
	private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.AspNetCore.Routing.Resources", typeof(Resources).GetTypeInfo().Assembly);

	internal static string ArgumentMustBeGreaterThanOrEqualTo => GetString("ArgumentMustBeGreaterThanOrEqualTo");

	internal static string RangeConstraint_MinShouldBeLessThanOrEqualToMax => GetString("RangeConstraint_MinShouldBeLessThanOrEqualToMax");

	internal static string PropertyOfTypeCannotBeNull => GetString("PropertyOfTypeCannotBeNull");

	internal static string NamedRoutes_AmbiguousRoutesFound => GetString("NamedRoutes_AmbiguousRoutesFound");

	internal static string DefaultHandler_MustBeSet => GetString("DefaultHandler_MustBeSet");

	internal static string DefaultInlineConstraintResolver_AmbiguousCtors => GetString("DefaultInlineConstraintResolver_AmbiguousCtors");

	internal static string DefaultInlineConstraintResolver_CouldNotFindCtor => GetString("DefaultInlineConstraintResolver_CouldNotFindCtor");

	internal static string DefaultInlineConstraintResolver_TypeNotConstraint => GetString("DefaultInlineConstraintResolver_TypeNotConstraint");

	internal static string TemplateRoute_CannotHaveCatchAllInMultiSegment => GetString("TemplateRoute_CannotHaveCatchAllInMultiSegment");

	internal static string TemplateRoute_CannotHaveDefaultValueSpecifiedInlineAndExplicitly => GetString("TemplateRoute_CannotHaveDefaultValueSpecifiedInlineAndExplicitly");

	internal static string TemplateRoute_CannotHaveConsecutiveParameters => GetString("TemplateRoute_CannotHaveConsecutiveParameters");

	internal static string TemplateRoute_CannotHaveConsecutiveSeparators => GetString("TemplateRoute_CannotHaveConsecutiveSeparators");

	internal static string TemplateRoute_CatchAllCannotBeOptional => GetString("TemplateRoute_CatchAllCannotBeOptional");

	internal static string TemplateRoute_OptionalCannotHaveDefaultValue => GetString("TemplateRoute_OptionalCannotHaveDefaultValue");

	internal static string TemplateRoute_CatchAllMustBeLast => GetString("TemplateRoute_CatchAllMustBeLast");

	internal static string TemplateRoute_InvalidLiteral => GetString("TemplateRoute_InvalidLiteral");

	internal static string TemplateRoute_InvalidParameterName => GetString("TemplateRoute_InvalidParameterName");

	internal static string TemplateRoute_InvalidRouteTemplate => GetString("TemplateRoute_InvalidRouteTemplate");

	internal static string TemplateRoute_MismatchedParameter => GetString("TemplateRoute_MismatchedParameter");

	internal static string TemplateRoute_RepeatedParameter => GetString("TemplateRoute_RepeatedParameter");

	internal static string RouteConstraintBuilder_ValidationMustBeStringOrCustomConstraint => GetString("RouteConstraintBuilder_ValidationMustBeStringOrCustomConstraint");

	internal static string RouteConstraintBuilder_CouldNotResolveConstraint => GetString("RouteConstraintBuilder_CouldNotResolveConstraint");

	internal static string TemplateRoute_UnescapedBrace => GetString("TemplateRoute_UnescapedBrace");

	internal static string TemplateRoute_OptionalParameterCanbBePrecededByPeriod => GetString("TemplateRoute_OptionalParameterCanbBePrecededByPeriod");

	internal static string TemplateRoute_OptionalParameterHasTobeTheLast => GetString("TemplateRoute_OptionalParameterHasTobeTheLast");

	internal static string AttributeRoute_DifferentLinkGenerationEntries_SameName => GetString("AttributeRoute_DifferentLinkGenerationEntries_SameName");

	internal static string UnableToFindServices => GetString("UnableToFindServices");

	internal static string TemplateRoute_Exception => GetString("TemplateRoute_Exception");

	internal static string FormatArgumentMustBeGreaterThanOrEqualTo(object p0)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("ArgumentMustBeGreaterThanOrEqualTo"), p0);
	}

	internal static string FormatRangeConstraint_MinShouldBeLessThanOrEqualToMax(object p0, object p1)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("RangeConstraint_MinShouldBeLessThanOrEqualToMax"), p0, p1);
	}

	internal static string FormatPropertyOfTypeCannotBeNull(object p0, object p1)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("PropertyOfTypeCannotBeNull"), p0, p1);
	}

	internal static string FormatNamedRoutes_AmbiguousRoutesFound(object p0)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("NamedRoutes_AmbiguousRoutesFound"), p0);
	}

	internal static string FormatDefaultHandler_MustBeSet(object p0)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("DefaultHandler_MustBeSet"), p0);
	}

	internal static string FormatDefaultInlineConstraintResolver_AmbiguousCtors(object p0, object p1)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("DefaultInlineConstraintResolver_AmbiguousCtors"), p0, p1);
	}

	internal static string FormatDefaultInlineConstraintResolver_CouldNotFindCtor(object p0, object p1)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("DefaultInlineConstraintResolver_CouldNotFindCtor"), p0, p1);
	}

	internal static string FormatDefaultInlineConstraintResolver_TypeNotConstraint(object p0, object p1, object p2)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("DefaultInlineConstraintResolver_TypeNotConstraint"), p0, p1, p2);
	}

	internal static string FormatTemplateRoute_CannotHaveCatchAllInMultiSegment()
	{
		return GetString("TemplateRoute_CannotHaveCatchAllInMultiSegment");
	}

	internal static string FormatTemplateRoute_CannotHaveDefaultValueSpecifiedInlineAndExplicitly(object p0)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("TemplateRoute_CannotHaveDefaultValueSpecifiedInlineAndExplicitly"), p0);
	}

	internal static string FormatTemplateRoute_CannotHaveConsecutiveParameters()
	{
		return GetString("TemplateRoute_CannotHaveConsecutiveParameters");
	}

	internal static string FormatTemplateRoute_CannotHaveConsecutiveSeparators()
	{
		return GetString("TemplateRoute_CannotHaveConsecutiveSeparators");
	}

	internal static string FormatTemplateRoute_CatchAllCannotBeOptional()
	{
		return GetString("TemplateRoute_CatchAllCannotBeOptional");
	}

	internal static string FormatTemplateRoute_OptionalCannotHaveDefaultValue()
	{
		return GetString("TemplateRoute_OptionalCannotHaveDefaultValue");
	}

	internal static string FormatTemplateRoute_CatchAllMustBeLast()
	{
		return GetString("TemplateRoute_CatchAllMustBeLast");
	}

	internal static string FormatTemplateRoute_InvalidLiteral(object p0)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("TemplateRoute_InvalidLiteral"), p0);
	}

	internal static string FormatTemplateRoute_InvalidParameterName(object p0)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("TemplateRoute_InvalidParameterName"), p0);
	}

	internal static string FormatTemplateRoute_InvalidRouteTemplate()
	{
		return GetString("TemplateRoute_InvalidRouteTemplate");
	}

	internal static string FormatTemplateRoute_MismatchedParameter()
	{
		return GetString("TemplateRoute_MismatchedParameter");
	}

	internal static string FormatTemplateRoute_RepeatedParameter(object p0)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("TemplateRoute_RepeatedParameter"), p0);
	}

	internal static string FormatRouteConstraintBuilder_ValidationMustBeStringOrCustomConstraint(object p0, object p1, object p2, object p3)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("RouteConstraintBuilder_ValidationMustBeStringOrCustomConstraint"), p0, p1, p2, p3);
	}

	internal static string FormatRouteConstraintBuilder_CouldNotResolveConstraint(object p0, object p1, object p2, object p3)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("RouteConstraintBuilder_CouldNotResolveConstraint"), p0, p1, p2, p3);
	}

	internal static string FormatTemplateRoute_UnescapedBrace()
	{
		return GetString("TemplateRoute_UnescapedBrace");
	}

	internal static string FormatTemplateRoute_OptionalParameterCanbBePrecededByPeriod(object p0, object p1, object p2)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("TemplateRoute_OptionalParameterCanbBePrecededByPeriod"), p0, p1, p2);
	}

	internal static string FormatTemplateRoute_OptionalParameterHasTobeTheLast(object p0, object p1, object p2)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("TemplateRoute_OptionalParameterHasTobeTheLast"), p0, p1, p2);
	}

	internal static string FormatAttributeRoute_DifferentLinkGenerationEntries_SameName(object p0)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("AttributeRoute_DifferentLinkGenerationEntries_SameName"), p0);
	}

	internal static string FormatUnableToFindServices(object p0, object p1, object p2)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("UnableToFindServices"), p0, p1, p2);
	}

	internal static string FormatTemplateRoute_Exception(object p0, object p1)
	{
		return string.Format(CultureInfo.CurrentCulture, GetString("TemplateRoute_Exception"), p0, p1);
	}

	private static string GetString(string name, params string[] formatterNames)
	{
		string text = _resourceManager.GetString(name);
		if (formatterNames != null)
		{
			for (int i = 0; i < formatterNames.Length; i++)
			{
				text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
			}
		}
		return text;
	}
}
