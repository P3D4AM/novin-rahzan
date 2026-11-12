using System;
using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Services;

[StiServiceBitmap(typeof(StiService), "Stimulsoft.Base.Bmp.Service.bmp")]
[StiServiceCategoryBitmap(typeof(StiService), "Stimulsoft.Base.Bmp.Service.bmp")]
public abstract class StiService : ICloneable
{
	private StiRepositoryItems properties;

	[Browsable(false)]
	[JsonIgnore]
	public StiRepositoryItems Properties
	{
		get
		{
			return properties ?? (properties = new StiRepositoryItems());
		}
		set
		{
			properties = value;
		}
	}

	[Browsable(false)]
	[JsonIgnore]
	public virtual string ServiceCategory => "Misc";

	[Browsable(false)]
	[JsonIgnore]
	public virtual string ServiceName => GetType().Name;

	[Browsable(false)]
	[JsonIgnore]
	public virtual string ServiceInfo => string.Empty;

	[Browsable(false)]
	[JsonIgnore]
	public abstract Type ServiceType { get; }

	[StiServiceParam]
	[Browsable(false)]
	[DefaultValue(true)]
	[StiCategory("Parameters", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool ServiceEnabled
	{
		get
		{
			return Properties.GetBool("ServiceEnabled", defaultValue: true);
		}
		set
		{
			Properties.SetBool("ServiceEnabled", value, defaultValue: true);
		}
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	public bool IsPropertyPresent(object key)
	{
		if (properties != null)
		{
			return properties.IsPresent(key);
		}
		return false;
	}

	protected bool IsPropertiesInitializedProtected()
	{
		return properties != null;
	}

	public virtual void PackService()
	{
	}
}
