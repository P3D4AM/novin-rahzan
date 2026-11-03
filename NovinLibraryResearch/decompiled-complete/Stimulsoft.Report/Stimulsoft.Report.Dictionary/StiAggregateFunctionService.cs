using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.CodeDom;

namespace Stimulsoft.Report.Dictionary;

[StiServiceBitmap(typeof(StiAggregateFunctionService), "Stimulsoft.Report.Bmp.AggregateFunction.bmp")]
public abstract class StiAggregateFunctionService : StiService, IStiStateSaveRestore
{
	private StiStatesManager states;

	public sealed override string ServiceCategory => StiLocalization.Get("Services", "categoryDictionary");

	public sealed override Type ServiceType => typeof(StiAggregateFunctionService);

	protected StiStatesManager States => states ?? (states = new StiStatesManager());

	public bool IsFirstInit { get; set; }

	[Browsable(false)]
	public abstract bool RecureParam { get; }

	[Browsable(false)]
	public bool RunningTotal { get; set; }

	public abstract void SaveState(string stateName);

	public abstract void RestoreState(string stateName);

	public virtual void ClearAllStates()
	{
		states = null;
	}

	public override void PackService()
	{
		((StiService)this).PackService();
		states = null;
	}

	public abstract void Init();

	public virtual void Reset()
	{
		bool runningTotal = RunningTotal;
		RunningTotal = false;
		Init();
		RunningTotal = runningTotal;
	}

	public abstract void CalcItem(object value);

	public abstract object GetValue();

	public abstract void SetValue(object value);

	public abstract Type GetResultType();

	private string ConvertTypeToString(Type type, StiReportLanguageType language)
	{
		string text = StiCodeDomSerializator.ConvertTypeToString(type, language);
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return type.ToString();
	}

	public string GetLongFunctionString(StiReportLanguageType language)
	{
		if (language == StiReportLanguageType.CSharp || language == StiReportLanguageType.JS)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(ConvertTypeToString(GetResultType(), language));
			stringBuilder.Append("  ");
			stringBuilder.Append(((StiService)this).ServiceName);
			stringBuilder.Append(" (");
			if (RecureParam)
			{
				Type typeFromHandle = typeof(object);
				string value = "value";
				stringBuilder.Append(ConvertTypeToString(typeFromHandle, language));
				stringBuilder.Append(" ");
				stringBuilder.Append(value);
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder2.Append(((StiService)this).ServiceName);
		stringBuilder2.Append("(");
		Type typeFromHandle2 = typeof(object);
		string value2 = "Value";
		stringBuilder2.Append(value2);
		stringBuilder2.Append(" As ");
		stringBuilder2.Append(ConvertTypeToString(typeFromHandle2, language));
		stringBuilder2.Append(")");
		if (GetResultType() != typeof(void))
		{
			stringBuilder2.Append(" As " + ConvertTypeToString(GetResultType(), language));
		}
		return stringBuilder2.ToString();
	}

	public static StiAggregateFunctionService GetFunction(List<StiAggregateFunctionService> services, string name)
	{
		return services.FirstOrDefault((StiAggregateFunctionService s) => ((StiService)s).ServiceName == name);
	}

	public StiAggregateFunctionService(bool runningTotal)
	{
		RunningTotal = runningTotal;
	}

	public StiAggregateFunctionService()
	{
	}
}
