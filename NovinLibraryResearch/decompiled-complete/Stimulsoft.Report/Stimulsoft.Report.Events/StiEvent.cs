using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.CodeDom;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Report.Events;

[Editor("Stimulsoft.Report.Events.Design.StiEventEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
[TypeConverter(typeof(StiEventConverter))]
public abstract class StiEvent : ICloneable, IStiJsonReportObject
{
	private string script = string.Empty;

	private StiComponent parent;

	[Browsable(false)]
	public string PropertyName => ToString() + "Event";

	[StiSerializable]
	public string Script
	{
		get
		{
			if (parent == null)
			{
				return script;
			}
			return ((StiService)parent).Properties.Get((object)PropertyName, (object)string.Empty) as string;
		}
		set
		{
			if (parent != null)
			{
				((StiService)parent).Properties.Set((object)PropertyName, (object)value, (object)string.Empty);
			}
			else
			{
				script = value;
			}
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		if (string.IsNullOrEmpty(Script))
		{
			return null;
		}
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Script", Script);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			if (item.Name == "Script")
			{
				Script = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public void Set(StiComponent parent, string value)
	{
		this.parent = parent;
		Script = value;
	}

	public virtual StiParameterInfo[] GetParameters()
	{
		return new StiParameterInfo[2]
		{
			new StiParameterInfo(typeof(object), "sender"),
			new StiParameterInfo(typeof(EventArgs), "e")
		};
	}

	public virtual Type GetEventType()
	{
		return typeof(EventHandler);
	}

	public StiEvent()
		: this(string.Empty)
	{
	}

	public StiEvent(string script)
	{
		this.script = script;
	}

	public StiEvent(StiComponent parent)
	{
		this.parent = parent;
	}
}
