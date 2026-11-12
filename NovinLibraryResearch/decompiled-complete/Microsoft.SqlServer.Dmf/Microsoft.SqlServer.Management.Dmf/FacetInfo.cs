using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using Microsoft.SqlServer.Management.Facets;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Dmf;

public sealed class FacetInfo : IDisposable, IComparable
{
	private Type facetType;

	private LocalizableTypeConverter typeConverter;

	private FacetEvaluationContext context;

	private object lastTarget;

	private string displayName;

	private string description;

	public string Name
	{
		get
		{
			if (FacetType != null)
			{
				return FacetType.Name;
			}
			return string.Empty;
		}
	}

	public string DisplayName
	{
		get
		{
			if (displayName == null)
			{
				displayName = string.Empty;
				if (TypeConverter != null)
				{
					LocalizableMemberDescriptor typeMemberDescriptor = TypeConverter.GetTypeMemberDescriptor(facetType);
					displayName = typeMemberDescriptor.DisplayName;
				}
			}
			return displayName;
		}
	}

	public string Description
	{
		get
		{
			if (description == null)
			{
				description = string.Empty;
				if (TypeConverter != null)
				{
					LocalizableMemberDescriptor typeMemberDescriptor = TypeConverter.GetTypeMemberDescriptor(facetType);
					description = typeMemberDescriptor.Description;
				}
			}
			return description;
		}
	}

	public Type FacetType => facetType;

	public ReadOnlyCollection<Type> TargetTypes => new ReadOnlyCollection<Type>(FacetRepository.GetFacetSupportedTypes(FacetType));

	public AutomatedPolicyEvaluationMode EvaluationMode => FacetRepository.GetFacetEvaluationMode(FacetType);

	public PropertyDescriptorCollection FacetPropertyDescriptors
	{
		get
		{
			PropertyDescriptorCollection result = null;
			if (TypeConverter != null)
			{
				result = TypeConverter.GetProperties(FacetRepository.GetFacetProperties(FacetType));
			}
			return result;
		}
	}

	public ReadOnlyCollection<PropertyInfo> FacetProperties => new ReadOnlyCollection<PropertyInfo>(FacetRepository.GetFacetProperties(FacetType));

	private LocalizableTypeConverter TypeConverter
	{
		get
		{
			if (facetType != null && typeConverter == null)
			{
				typeConverter = TypeDescriptor.GetConverter(facetType) as LocalizableTypeConverter;
			}
			return typeConverter;
		}
	}

	internal FacetInfo()
	{
	}

	internal FacetInfo(Type facetType)
	{
		this.facetType = facetType;
	}

	internal FacetInfo(string facetShortName)
		: this(FacetRepository.GetFacetType(facetShortName))
	{
	}

	public override string ToString()
	{
		return DisplayName;
	}

	public object GetTargetProperty(string propName, object target)
	{
		return GetAdapter(target).GetPropertyValue(propName);
	}

	private FacetEvaluationContext GetAdapter(object target)
	{
		if (target == lastTarget)
		{
			return context;
		}
		context = FacetEvaluationContext.GetFacetEvaluationContext(FacetType, target);
		lastTarget = target;
		return context;
	}

	void IDisposable.Dispose()
	{
		lastTarget = null;
		context = null;
		typeConverter = null;
		displayName = null;
		description = null;
	}

	int IComparable.CompareTo(object other)
	{
		return DisplayName.CompareTo(((FacetInfo)other).DisplayName);
	}
}
