using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Components;

[Editor("Stimulsoft.Report.Components.Design.StiExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
[TypeConverter(typeof(StiExpressionConverter))]
public class StiExpression : ICloneable, IStiJsonReportObject
{
	private string val;

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public virtual string Value
	{
		get
		{
			return val;
		}
		set
		{
			if (!(val == value))
			{
				if (ParentComponent != null && ParentComponent is StiText)
				{
					StiOptions.Engine.GlobalEvents.InvokeTextChanged(ParentComponent, new StiTextChangedEventArgs(val, value));
				}
				val = value;
			}
		}
	}

	internal object ParentComponent { get; set; }

	[Browsable(false)]
	public virtual bool FullConvert => true;

	[Browsable(false)]
	public virtual bool ApplyFormat => true;

	[Browsable(false)]
	public virtual bool GenAddEvent => true;

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Value", Value);
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Value")
			{
				Value = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public virtual StiEvent GetDefaultEvent()
	{
		return new StiGetValueEvent();
	}

	public static implicit operator string(StiExpression exp)
	{
		return exp?.val;
	}

	public static implicit operator StiExpression(string s)
	{
		return new StiExpression(s);
	}

	public override string ToString()
	{
		return Value;
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiExpression()
		: this(string.Empty)
	{
	}

	public StiExpression(string value)
	{
		val = value;
	}
}
