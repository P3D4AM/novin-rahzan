using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Dashboard;

[JsonObject]
[RefreshProperties(RefreshProperties.All)]
public class StiElementLayout : IStiJsonReportObject, ICloneable
{
	private IStiElement element;

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (FullScreenButton)
			{
				return SaveButton;
			}
			return false;
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	public bool FullScreenButton
	{
		get
		{
			if (Layout == null)
			{
				return true;
			}
			return Layout.ShowFullScreenButton;
		}
		set
		{
			if (Layout != null)
			{
				Layout.ShowFullScreenButton = value;
			}
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	public bool SaveButton
	{
		get
		{
			if (Layout == null)
			{
				return true;
			}
			return Layout.ShowSaveButton;
		}
		set
		{
			if (Layout != null)
			{
				Layout.ShowSaveButton = value;
			}
		}
	}

	private IStiInteractionLayout Layout => (element as IStiElementInteraction)?.DashboardInteraction as IStiInteractionLayout;

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "FullScreenButton", FullScreenButton, true, false);
		JsonExtensions.AddPropertyBool(val, "SaveButton", SaveButton, true, false);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (!(name == "FullScreenButton"))
			{
				if (name == "SaveButton")
				{
					SaveButton = StiJsonReportObjectHelper.DeserializeBool(item);
				}
			}
			else
			{
				FullScreenButton = StiJsonReportObjectHelper.DeserializeBool(item);
			}
		}
	}

	internal static StiElementLayout CreateFromJsonObject(JObject jObject, IStiElement element)
	{
		StiElementLayout stiElementLayout = new StiElementLayout(element);
		stiElementLayout.LoadFromJsonObject(jObject);
		return stiElementLayout;
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	public StiElementLayout()
	{
	}

	public StiElementLayout(IStiElement element)
	{
		this.element = element;
	}
}
