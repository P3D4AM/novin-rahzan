using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public abstract class TypeBase : ConventionAnnotatable, IMutableTypeBase, ITypeBase, IAnnotatable, IMutableAnnotatable, IConventionTypeBase, IConventionAnnotatable
{
	private ConfigurationSource _configurationSource;

	private readonly Dictionary<string, ConfigurationSource> _ignoredMembers = new Dictionary<string, ConfigurationSource>(StringComparer.Ordinal);

	private Dictionary<string, PropertyInfo> _runtimeProperties;

	private Dictionary<string, FieldInfo> _runtimeFields;

	public virtual Type ClrType
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual Model Model
	{
		[DebuggerStepThrough]
		get;
	}

	public virtual string Name
	{
		[DebuggerStepThrough]
		get;
	}

	IModel ITypeBase.Model
	{
		[DebuggerStepThrough]
		get
		{
			return Model;
		}
	}

	IMutableModel IMutableTypeBase.Model
	{
		[DebuggerStepThrough]
		get
		{
			return Model;
		}
	}

	IConventionModel IConventionTypeBase.Model
	{
		[DebuggerStepThrough]
		get
		{
			return Model;
		}
	}

	Type ITypeBase.ClrType
	{
		[DebuggerStepThrough]
		get
		{
			return ClrType;
		}
	}

	protected TypeBase([JetBrains.Annotations.NotNull] string name, [JetBrains.Annotations.NotNull] Model model, ConfigurationSource configurationSource)
		: this(model, configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Name = name;
	}

	protected TypeBase([JetBrains.Annotations.NotNull] Type clrType, [JetBrains.Annotations.NotNull] Model model, ConfigurationSource configurationSource)
		: this(model, configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Name = model.GetDisplayName(clrType);
		ClrType = clrType;
	}

	private TypeBase([JetBrains.Annotations.NotNull] Model model, ConfigurationSource configurationSource)
	{
		Model = model;
		_configurationSource = configurationSource;
	}

	[DebuggerStepThrough]
	public virtual ConfigurationSource GetConfigurationSource()
	{
		return _configurationSource;
	}

	public virtual void UpdateConfigurationSource(ConfigurationSource configurationSource)
	{
		_configurationSource = configurationSource.Max(_configurationSource);
	}

	public virtual IReadOnlyDictionary<string, PropertyInfo> GetRuntimeProperties()
	{
		if (ClrType == null)
		{
			return null;
		}
		if (_runtimeProperties == null)
		{
			Dictionary<string, PropertyInfo> dictionary = new Dictionary<string, PropertyInfo>(StringComparer.Ordinal);
			foreach (PropertyInfo runtimeProperty in ClrType.GetRuntimeProperties())
			{
				if (!runtimeProperty.IsStatic() && !dictionary.ContainsKey(runtimeProperty.Name))
				{
					dictionary[runtimeProperty.Name] = runtimeProperty;
				}
			}
			Interlocked.CompareExchange(ref _runtimeProperties, dictionary, null);
		}
		return _runtimeProperties;
	}

	public virtual IReadOnlyDictionary<string, FieldInfo> GetRuntimeFields()
	{
		if (ClrType == null)
		{
			return null;
		}
		if (_runtimeFields == null)
		{
			Dictionary<string, FieldInfo> dictionary = new Dictionary<string, FieldInfo>(StringComparer.Ordinal);
			foreach (FieldInfo runtimeField in ClrType.GetRuntimeFields())
			{
				if (!runtimeField.IsStatic && !dictionary.ContainsKey(runtimeField.Name))
				{
					dictionary[runtimeField.Name] = runtimeField;
				}
			}
			Interlocked.CompareExchange(ref _runtimeFields, dictionary, null);
		}
		return _runtimeFields;
	}

	public virtual void SetPropertyAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("PropertyAccessMode", propertyAccessMode, configurationSource);
	}

	public virtual void SetNavigationAccessMode(PropertyAccessMode? propertyAccessMode, ConfigurationSource configurationSource)
	{
		this.SetOrRemoveAnnotation("NavigationAccessMode", propertyAccessMode, configurationSource);
	}

	public virtual void ClearCaches()
	{
		_runtimeProperties = null;
		_runtimeFields = null;
		Thread.MemoryBarrier();
	}

	public virtual void AddIgnored([JetBrains.Annotations.NotNull] string name, ConfigurationSource configurationSource)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		if (_ignoredMembers.TryGetValue(name, out var value))
		{
			_ignoredMembers[name] = configurationSource.Max(value);
			return;
		}
		_ignoredMembers[name] = configurationSource;
		OnTypeMemberIgnored(name);
	}

	public abstract void OnTypeMemberIgnored([JetBrains.Annotations.NotNull] string name);

	public virtual IReadOnlyList<string> GetIgnoredMembers()
	{
		return _ignoredMembers.Keys.ToList();
	}

	public virtual ConfigurationSource? FindDeclaredIgnoredConfigurationSource([JetBrains.Annotations.NotNull] string name)
	{
		if (!_ignoredMembers.TryGetValue(Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name"), out var value))
		{
			return null;
		}
		return value;
	}

	public virtual ConfigurationSource? FindIgnoredConfigurationSource(string name)
	{
		return FindDeclaredIgnoredConfigurationSource(name);
	}

	public virtual bool IsIgnored(string name)
	{
		return FindIgnoredConfigurationSource(name).HasValue;
	}

	public virtual void RemoveIgnored(string name)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(name, "name");
		_ignoredMembers.Remove(name);
	}

	void IMutableTypeBase.AddIgnored(string name)
	{
		AddIgnored(name, ConfigurationSource.Explicit);
	}

	void IConventionTypeBase.AddIgnored(string name, bool fromDataAnnotation)
	{
		AddIgnored(name, fromDataAnnotation ? ConfigurationSource.DataAnnotation : ConfigurationSource.Convention);
	}
}
