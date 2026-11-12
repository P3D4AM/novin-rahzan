using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Components.Button;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiButtonElementIconSet : IStiButtonElementIconSet, IStiJsonReportObject, IStiDefault, ICloneable
{
	private StiFontIcons? icon;

	private StiFontIcons? checkedIcon;

	private StiFontIcons? ucheckedIcon;

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (!Icon.HasValue && !CheckedIcon.HasValue)
			{
				return !UncheckedIcon.HasValue;
			}
			return false;
		}
	}

	[DefaultValue(null)]
	[StiSerializable]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiOrder(1)]
	public StiFontIcons? Icon
	{
		get
		{
			return icon;
		}
		set
		{
			if (icon != value)
			{
				icon = value;
				InvokeRefreshControl("Icon");
			}
		}
	}

	[DefaultValue(null)]
	[StiSerializable]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiOrder(2)]
	public StiFontIcons? CheckedIcon
	{
		get
		{
			return checkedIcon;
		}
		set
		{
			if (checkedIcon != value)
			{
				checkedIcon = value;
				InvokeRefreshControl("CheckedIcon");
			}
		}
	}

	[DefaultValue(null)]
	[StiSerializable]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiOrder(3)]
	public StiFontIcons? UncheckedIcon
	{
		get
		{
			return ucheckedIcon;
		}
		set
		{
			if (ucheckedIcon != value)
			{
				ucheckedIcon = value;
				InvokeRefreshControl("UncheckedIcon");
			}
		}
	}

	[Browsable(false)]
	internal StiButtonElement Parent { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		if (IsDefault)
		{
			return null;
		}
		JObject jObject = new JObject();
		jObject.AddPropertyEnum("Icon", Icon);
		jObject.AddPropertyEnum("CheckedIcon", CheckedIcon);
		jObject.AddPropertyEnum("UncheckedIcon", UncheckedIcon);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Icon":
				Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "CheckedIcon":
				CheckedIcon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "UncheckedIcon":
				UncheckedIcon = item.DeserializeEnum<StiFontIcons>();
				break;
			}
		}
	}

	protected void InvokeRefreshControl(string name)
	{
		Parent?.InvokeRefreshControl(name);
	}

	public StiButtonElementIconSet()
	{
	}

	[StiUniversalConstructor("IconSet")]
	public StiButtonElementIconSet(StiFontIcons? checkedIcon, StiFontIcons? uncheckedIcon)
	{
		CheckedIcon = checkedIcon;
		UncheckedIcon = uncheckedIcon;
	}
}
