using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Components;

[StiV1Builder(typeof(StiSimpleTextV1Builder))]
[StiV2Builder(typeof(StiSimpleTextV2Builder))]
public abstract class StiSimpleText : StiComponent, IStiGlobalizedName, IStiText, IStiEditable, IStiProcessAtEnd, IStiProcessAt, IStiOnlyText
{
	protected static object PropertyGlobalizedName = new object();

	protected static object PropertyProcessingDuplicates = new object();

	protected static object PropertyMaxNumberOfLines = new object();

	protected static object PropertyOnlyText = new object();

	protected static object PropertyProcessAt = new object();

	private StiExpression text = new StiExpression();

	private string textValue;

	private static readonly object EventGetValue = new object();

	private static readonly object EventTextProcess = new object();

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[DefaultValue("")]
	[ParenthesizePropertyName(true)]
	[Description("Gets or sets special identificator which will be used for report globalization.")]
	[Editor("Stimulsoft.Report.Design.StiGlobabalizationManagerEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string GlobalizedName
	{
		get
		{
			return ((StiService)this).Properties.Get(PropertyGlobalizedName, (object)string.Empty) as string;
		}
		set
		{
			((StiService)this).Properties.Set(PropertyGlobalizedName, (object)value, (object)string.Empty);
		}
	}

	[StiSerializable]
	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value indicates that it is necessary to lines of underline.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPenStyle LinesOfUnderline { get; set; } = (StiPenStyle)6;


	[StiNonSerialized]
	[Browsable(false)]
	public virtual bool LinesOfUnderlining
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			return (int)LinesOfUnderline != 6;
		}
		set
		{
			LinesOfUnderline = (StiPenStyle)((!value) ? 6 : 0);
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiShowInContextMenu]
	[Description("Gets or sets value indicates that no need show zeroes.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool HideZeros { get; set; }

	[StiNonSerialized]
	[Browsable(false)]
	[Obsolete("Use ProcessingDuplicates property")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual bool MergeDuplicates
	{
		get
		{
			return ProcessingDuplicates == StiProcessingDuplicatesType.Merge;
		}
		set
		{
			ProcessingDuplicates = StiProcessingDuplicatesType.Merge;
		}
	}

	[DefaultValue(StiProcessingDuplicatesType.None)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(230)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value which indicates how report engine processes duplicated values.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiProcessingDuplicatesType ProcessingDuplicates
	{
		get
		{
			return (StiProcessingDuplicatesType)((StiService)this).Properties.Get(PropertyProcessingDuplicates, (object)StiProcessingDuplicatesType.None);
		}
		set
		{
			((StiService)this).Properties.Set(PropertyProcessingDuplicates, (object)value, (object)StiProcessingDuplicatesType.None);
		}
	}

	[DefaultValue(0)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(190)]
	[Description("Gets or sets maximum number of lines which specify the limit of the height stretch.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int MaxNumberOfLines
	{
		get
		{
			return ((StiService)this).Properties.GetInt(PropertyMaxNumberOfLines, 0);
		}
		set
		{
			((StiService)this).Properties.SetInt(PropertyMaxNumberOfLines, value, 0);
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiShowInContextMenu]
	[Description("Gets or sets value indicates that the text expression contains a text only.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool OnlyText
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyOnlyText, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyOnlyText, value, false);
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets value indicates that a component can be edited in the window of viewer.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Editable { get; set; }

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(220)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value indicates that a text is processed at the end of the report execution.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public virtual bool ProcessAtEnd
	{
		get
		{
			return ProcessAt == StiProcessAt.EndOfReport;
		}
		set
		{
			if (value)
			{
				ProcessAt = StiProcessAt.EndOfReport;
			}
			else
			{
				ProcessAt = StiProcessAt.None;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiProcessAt.None)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value indicates whether to process a text expression of this component at the end of the page rendering or at the end of the report rendering.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiEngine(/*Could not decode attribute arguments.*/)]
	public virtual StiProcessAt ProcessAt
	{
		get
		{
			return (StiProcessAt)((StiService)this).Properties.Get(PropertyProcessAt, (object)StiProcessAt.None);
		}
		set
		{
			((StiService)this).Properties.Set(PropertyProcessAt, (object)value, (object)StiProcessAt.None);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable]
	[Description("Gets or sets text expression.")]
	[Editor("Stimulsoft.Report.Components.Design.StiTextExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiExpression Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
			if (text != null)
			{
				text.ParentComponent = this;
			}
		}
	}

	[Browsable(false)]
	[Description("Gets or sets text value.")]
	public virtual string TextValue
	{
		get
		{
			return textValue;
		}
		set
		{
			if (textValue != value)
			{
				StiOptions.Engine.GlobalEvents.InvokeTextChanged(this, new StiTextChangedEventArgs(textValue, value));
				textValue = value;
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the text is being prepared for rendering.")]
	public StiGetValueEvent GetValueEvent
	{
		get
		{
			return new StiGetValueEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	public virtual string TotalValueHelp { get; set; }

	public event StiGetValueEventHandler GetValue
	{
		add
		{
			base.Events.AddHandler(EventGetValue, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetValue, value);
		}
	}

	public event StiValueEventHandler TextProcess
	{
		add
		{
			base.Events.AddHandler(EventTextProcess, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventTextProcess, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "GlobalizedName", GlobalizedName);
		JsonExtensions.AddPropertyEnum(val, "LinesOfUnderline", (Enum)(object)LinesOfUnderline, (Enum)(object)(StiPenStyle)6);
		JsonExtensions.AddPropertyBool(val, "HideZeros", HideZeros, false, false);
		JsonExtensions.AddPropertyEnum(val, "ProcessingDuplicates", (Enum)ProcessingDuplicates, (Enum)StiProcessingDuplicatesType.None);
		JsonExtensions.AddPropertyInt(val, "MaxNumberOfLines", MaxNumberOfLines, 0);
		JsonExtensions.AddPropertyBool(val, "OnlyText", OnlyText, false, false);
		JsonExtensions.AddPropertyBool(val, "Editable", Editable, false, false);
		JsonExtensions.AddPropertyBool(val, "ProcessAtEnd", ProcessAtEnd, false, false);
		JsonExtensions.AddPropertyEnum(val, "ProcessAt", (Enum)ProcessAt, (Enum)StiProcessAt.None);
		JsonExtensions.AddPropertyJObject(val, "Text", Text.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetValueEvent", GetValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TotalValueHelp", TotalValueHelp);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "GlobalizedName":
				GlobalizedName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "LinesOfUnderline":
				LinesOfUnderline = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "HideZeros":
				HideZeros = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ProcessingDuplicates":
				ProcessingDuplicates = StiJsonReportObjectHelper.DeserializeEnum<StiProcessingDuplicatesType>(item);
				break;
			case "MaxNumberOfLines":
				MaxNumberOfLines = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "OnlyText":
				OnlyText = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Editable":
				Editable = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ProcessAtEnd":
				ProcessAtEnd = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ProcessAt":
				ProcessAt = StiJsonReportObjectHelper.DeserializeEnum<StiProcessAt>(item);
				break;
			case "Text":
			{
				StiExpression stiExpression = new StiExpression();
				stiExpression.LoadFromJsonObject((JObject)item.Value);
				text = stiExpression;
				break;
			}
			case "GetValueEvent":
			{
				StiGetValueEvent stiGetValueEvent = new StiGetValueEvent();
				stiGetValueEvent.LoadFromJsonObject((JObject)item.Value);
				GetValueEvent = stiGetValueEvent;
				break;
			}
			case "TotalValueHelp":
				TotalValueHelp = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override void SaveState(string stateName)
	{
		base.SaveState(stateName);
		base.States.Push(stateName, this, "textValue", textValue);
	}

	public override void RestoreState(string stateName)
	{
		if (base.States.IsExist(stateName, this))
		{
			textValue = (string)base.States.Pop(stateName, this, "textValue");
		}
		base.RestoreState(stateName);
	}

	public override object Clone(bool cloneProperties)
	{
		StiSimpleText stiSimpleText = (StiSimpleText)base.Clone(cloneProperties);
		if (text != null)
		{
			stiSimpleText.text = (StiExpression)text.Clone();
		}
		else
		{
			stiSimpleText.text = null;
		}
		return stiSimpleText;
	}

	public string GetTextWithoutZero(string text)
	{
		if (!HideZeros || text == null)
		{
			return text;
		}
		if (text == "0")
		{
			if (!CanShrink)
			{
				return " ";
			}
			return "";
		}
		string text2 = text.Trim();
		if (text2.Length == 0)
		{
			return text;
		}
		if (text2.IndexOf('0') == -1)
		{
			return text;
		}
		if (text2.IndexOfAny(new char[9] { '1', '2', '3', '4', '5', '6', '7', '8', '9' }) != -1)
		{
			return text;
		}
		string numberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		try
		{
			string text3 = "$";
			if (this is StiText && (this as StiText).TextFormat is StiCurrencyFormatService)
			{
				text3 = ((this as StiText).TextFormat as StiCurrencyFormatService).Symbol;
			}
			if (text2.StartsWith(text3))
			{
				text2 = text2.Substring(text3.Length);
			}
			if (text2.EndsWith(text3))
			{
				text2 = text2.Substring(0, text2.Length - text3.Length);
			}
			text2 = text2.Replace(".", ",").Replace(",", numberDecimalSeparator);
			if (decimal.TryParse(text2, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out var result))
			{
				if (result == 0m)
				{
					return CanShrink ? "" : " ";
				}
				return text;
			}
			text2 = text2.Replace("%", string.Empty);
			if (decimal.TryParse(text2, NumberStyles.Number, NumberFormatInfo.CurrentInfo, out result))
			{
				if (result == 0m)
				{
					return CanShrink ? "" : " ";
				}
				return text;
			}
		}
		catch
		{
		}
		Report?.WriteToReportRenderingMessages(Name + " HideZero Format Exception");
		return text;
	}

	public void SetText(StiGetValue getValue)
	{
		SetText(getValue, null);
	}

	public void SetExcelText(StiGetExcelValue getValue)
	{
		SetText(getValue, null);
	}

	public void SetText(string value)
	{
		SetText(null, value);
	}

	public void SetText(StiGetValue getValue, bool isRunning)
	{
		SetText(getValue, null, isRunning);
	}

	public void SetExcelText(StiGetExcelValue getValue, bool isRunning)
	{
		SetText(getValue, null, isRunning);
	}

	public virtual void SetText(object getValue, string value, bool isRunning = false)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Invalid comparison between Unknown and I4
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Invalid comparison between Unknown and I4
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Invalid comparison between Unknown and I4
		StiGetValue stiGetValue = getValue as StiGetValue;
		StiGetExcelValue stiGetExcelValue = getValue as StiGetExcelValue;
		if (this.text != null)
		{
			string text = Name;
			if (stiGetExcelValue != null)
			{
				text += "Excel";
			}
			if (Report.Totals[text] is ArrayList { Count: >0 } arrayList)
			{
				Hashtable hashtable = new Hashtable();
				foreach (StiRuntimeVariables item2 in arrayList)
				{
					int num = item2.PageIndex;
					if (Report.Engine != null)
					{
						Report.Engine.IndexPageForPageTotal = num - 1;
					}
					if ((int)Report.EngineVersion == 2)
					{
						num--;
					}
					if (Report.RenderedPages.CacheMode && num < Report.RenderedPages.Count)
					{
						StiPage pageWithoutCache = Report.RenderedPages.GetPageWithoutCache(num);
						if (Report.RenderedPages.IsNotSavedPage(pageWithoutCache) && !hashtable.ContainsKey(num))
						{
							SetTextTo(item2.TextBox, item2, getValue, value);
							continue;
						}
						if (StiOptions.Engine.ReportCache.OptimizeEndRenderSetText)
						{
							Hashtable hashtable2 = Report.Totals["%%%=Pages=%%%"] as Hashtable;
							if (hashtable2 == null)
							{
								hashtable2 = new Hashtable();
								Report.Totals["%%%=Pages=%%%"] = hashtable2;
							}
							List<DictionaryEntry> list = hashtable2[num] as List<DictionaryEntry>;
							if (list == null)
							{
								list = new List<DictionaryEntry>();
								hashtable2[num] = list;
							}
							DictionaryEntry item = new DictionaryEntry(item2, getValue);
							list.Add(item);
							continue;
						}
						hashtable[num] = null;
						Report.RenderedPages.GetPage(pageWithoutCache);
						StiComponentsCollection components = pageWithoutCache.GetComponents();
						foreach (StiComponent item3 in components)
						{
							if (item3 is StiSimpleText stiSimpleText && item3.Name == Name && stiSimpleText.TotalValueHelp == null)
							{
								SetTextTo(item3, item2, getValue, value);
								break;
							}
						}
						if ((int)Report.EngineVersion == 1 && !Report.RenderedPages.IsNotSavedPage(pageWithoutCache))
						{
							Report.RenderedPages.SavePage(pageWithoutCache);
						}
						if ((int)Report.EngineVersion == 2)
						{
							Report.RenderedPages.MarkPageAsNotSaved(pageWithoutCache);
						}
					}
					else
					{
						SetTextTo(item2.TextBox, item2, getValue, value);
					}
				}
				hashtable.Clear();
				arrayList.Clear();
			}
			else if (value != null)
			{
				this.text = value;
			}
			else
			{
				try
				{
					if (stiGetValue != null)
					{
						this.text = stiGetValue(this);
					}
					if (stiGetExcelValue != null)
					{
						((StiText)this).ExcelDataValue = stiGetExcelValue(this);
					}
				}
				catch (Exception ex)
				{
					string text2 = "Expression in Text property of '" + Name + "' can't be evaluated! " + ex.Message;
					StiLogService.Write(((object)this).GetType(), text2);
					StiLogService.Write(((object)this).GetType(), ex.Message);
					Report.WriteToReportRenderingMessages(text2);
				}
			}
		}
		if (stiGetExcelValue == null)
		{
			textValue = ProcessText(GetTextWithoutZero(this.text));
		}
	}

	public static void CheckEndRenderRuntimes(StiReport report)
	{
		if (report == null || report.Totals == null || report.Totals.Count == 0)
		{
			return;
		}
		Hashtable hashtable = new Hashtable(report.Totals);
		foreach (DictionaryEntry item in hashtable)
		{
			if (item.Key is string text && text.StartsWith("#%#"))
			{
				try
				{
					string text2 = text.Substring(3);
					StiText stiText = ((StiService)report.GetComponentByName(text2)).Clone() as StiText;
					StiGetValue getValue = (StiGetValue)Delegate.CreateDelegate(typeof(StiGetValue), report, text2 + "_GetValue_End");
					stiText.SetText(getValue, null);
				}
				catch
				{
				}
			}
		}
	}

	public static void ProcessEndRenderSetText(StiReport report)
	{
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Invalid comparison between Unknown and I4
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Invalid comparison between Unknown and I4
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Invalid comparison between Unknown and I4
		if (report == null || report.Totals == null || !(report.Totals["%%%=Pages=%%%"] is Hashtable { Count: not 0 } hashtable))
		{
			return;
		}
		for (int i = 0; i < report.RenderedPages.Count; i++)
		{
			if (!(hashtable[i] is List<DictionaryEntry> { Count: not 0 } list))
			{
				continue;
			}
			StiPage stiPage = report.RenderedPages[i];
			report.RenderedPages.GetPage(stiPage);
			StiComponentsCollection components = stiPage.GetComponents();
			foreach (DictionaryEntry item in list)
			{
				StiRuntimeVariables stiRuntimeVariables = item.Key as StiRuntimeVariables;
				object value = item.Value;
				string text = stiRuntimeVariables.TextBox.Name;
				foreach (StiComponent item2 in components)
				{
					if (item2 is StiSimpleText stiSimpleText && item2.Name == text && stiSimpleText.TotalValueHelp == null)
					{
						if ((int)report.EngineVersion == 2)
						{
							stiRuntimeVariables.CurrentPrintPage = i + 1;
							stiRuntimeVariables.TextBox = null;
						}
						stiSimpleText.SetTextTo(item2, stiRuntimeVariables, value, null);
						break;
					}
				}
			}
			if ((int)report.EngineVersion == 1 && !report.RenderedPages.IsNotSavedPage(stiPage))
			{
				report.RenderedPages.SavePage(stiPage);
			}
			if ((int)report.EngineVersion == 2)
			{
				report.RenderedPages.MarkPageAsNotSaved(stiPage);
			}
		}
		hashtable.Clear();
	}

	internal void SetTextTo(StiComponent comp, StiRuntimeVariables runtime, object getValue, string value)
	{
		if (!(comp is IStiText stiText))
		{
			return;
		}
		StiGetValue stiGetValue = getValue as StiGetValue;
		StiGetExcelValue stiGetExcelValue = getValue as StiGetExcelValue;
		((StiSimpleText)comp).TotalValueHelp = "1";
		int pageNumber = Report.PageNumber;
		StiRuntimeVariables stiRuntimeVariables = new StiRuntimeVariables(Report);
		runtime.SetVariables(Report);
		string value2 = string.Empty;
		string value3 = "-";
		bool flag = true;
		if (Report != null)
		{
			flag = Report.CalculationMode == StiCalculationMode.Compilation;
		}
		if (value != null)
		{
			this.text = value;
		}
		else
		{
			if (flag)
			{
				if (stiGetValue != null)
				{
					try
					{
						value2 = stiGetValue(comp);
					}
					catch (Exception ex)
					{
						string text = "Expression in Text property of '" + Name + "' can't be evaluated! " + ex.Message;
						StiLogService.Write(((object)this).GetType(), text);
						StiLogService.Write(((object)this).GetType(), ex.Message);
						Report.WriteToReportRenderingMessages(text);
					}
				}
			}
			else
			{
				try
				{
					StiText stiText2 = comp as StiText;
					bool flag2 = stiText2 != null && stiText2.ExcelValue.Value != null && stiText2.ExcelValue.Value.Length > 0;
					string inputExpression = stiText2.Text.Value;
					if (Report.Totals.Contains("#%#" + comp.Name))
					{
						inputExpression = Report.Totals["#%#" + comp.Name] as string;
					}
					bool storeToPrint = false;
					object obj = StiParser.ParseTextValue(inputExpression, this, stiText2, ref storeToPrint, executeIfStoreToPrint: true);
					value2 = ((stiText2 == null || stiText2.Format == null || !(stiText2.Format != "G")) ? Report.ToString(comp, obj, !flag2) : (flag2 ? stiText2.TextFormat.Format(obj) : stiText2.TextFormat.Format(Report.CheckExcelValue(comp, obj))));
				}
				catch (Exception ex2)
				{
					string text2 = "Expression in Text property of '" + Name + "' can't be evaluated! " + ex2.Message;
					StiLogService.Write(((object)this).GetType(), text2);
					StiLogService.Write(((object)this).GetType(), ex2.Message);
					Report.WriteToReportRenderingMessages(text2);
				}
			}
			if (stiGetExcelValue != null)
			{
				try
				{
					value3 = stiGetExcelValue(comp);
				}
				catch (Exception ex3)
				{
					string text3 = "Expression in Text property of '" + Name + "' can't be evaluated! " + ex3.Message;
					StiLogService.Write(((object)this).GetType(), text3);
					StiLogService.Write(((object)this).GetType(), ex3.Message);
					Report.WriteToReportRenderingMessages(text3);
				}
			}
		}
		stiRuntimeVariables.SetVariables(Report);
		Report.PageNumber = pageNumber;
		if (flag)
		{
			if (stiGetValue != null)
			{
				StiGetValueEventArgs stiGetValueEventArgs = new StiGetValueEventArgs();
				stiGetValueEventArgs.Value = value2;
				((StiSimpleText)stiText).InvokeGetValue(stiText as StiText, stiGetValueEventArgs);
				if (stiGetValueEventArgs.StoreToPrinted && (string.IsNullOrEmpty(stiGetValueEventArgs.Value) || stiGetValueEventArgs.Value.StartsWith("#%#", StringComparison.InvariantCulture)))
				{
					stiGetValueEventArgs.Value = value2;
				}
				StiValueEventArgs stiValueEventArgs = new StiValueEventArgs(stiGetValueEventArgs.Value);
				InvokeTextProcess(comp, stiValueEventArgs);
				string text4 = ProcessText(GetTextWithoutZero(stiValueEventArgs.Value as string));
				if (stiText is StiTextInCells)
				{
					StiContainer cont = StiTextInCells.SplitByCells(this as StiTextInCells, comp, text4);
					StiTextInCells.ReplaceContainerWithContentCells(comp, cont);
				}
				else
				{
					stiText.SetTextInternal(text4);
				}
			}
		}
		else
		{
			StiGetValueEventArgs stiGetValueEventArgs2 = new StiGetValueEventArgs();
			stiGetValueEventArgs2.Value = value2;
			((StiSimpleText)stiText).InvokeGetValue(stiText as StiText, stiGetValueEventArgs2);
			if (stiGetValueEventArgs2.StoreToPrinted && (string.IsNullOrEmpty(stiGetValueEventArgs2.Value) || stiGetValueEventArgs2.Value.StartsWith("#%#", StringComparison.InvariantCulture)))
			{
				stiGetValueEventArgs2.Value = value2;
			}
			StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs(stiGetValueEventArgs2.Value);
			InvokeTextProcess(comp, stiValueEventArgs2);
			string text5 = ProcessText(GetTextWithoutZero(stiValueEventArgs2.Value as string));
			if (stiText is StiTextInCells)
			{
				StiContainer cont2 = StiTextInCells.SplitByCells(this as StiTextInCells, comp, text5);
				StiTextInCells.ReplaceContainerWithContentCells(comp, cont2);
			}
			else
			{
				stiText.SetTextInternal(text5);
			}
		}
		if (stiGetExcelValue != null)
		{
			StiGetExcelValueEventArgs stiGetExcelValueEventArgs = new StiGetExcelValueEventArgs();
			stiGetExcelValueEventArgs.Value = value3;
			((StiText)stiText).InvokeGetExcelValue(stiText as StiText, stiGetExcelValueEventArgs);
			if (stiGetExcelValueEventArgs.StoreToPrinted && (string.IsNullOrEmpty(stiGetExcelValueEventArgs.Value) || StringExt.StartsWithInvariant(stiGetExcelValueEventArgs.Value, "#%#")))
			{
				stiGetExcelValueEventArgs.Value = value3;
			}
			((StiText)stiText).ExcelDataValue = stiGetExcelValueEventArgs.Value;
		}
	}

	public virtual string ProcessText(string text)
	{
		if (StiOptions.Engine.ForceNormalizeEndOfLineSymbols)
		{
			string text2 = NormalizeEndOfLineSymbols(text);
			if (text2 != null)
			{
				return text2;
			}
		}
		return text;
	}

	string IStiEditable.SaveState()
	{
		return GetTextInternal();
	}

	void IStiEditable.RestoreState(string value)
	{
		SetTextInternal(value);
	}

	public virtual void InvokeRenderTo(StiSimpleText textBox)
	{
	}

	private string GetFields()
	{
		if (Text == null || GetTextInternal() == null)
		{
			return string.Empty;
		}
		int num = GetTextInternal().LastIndexOf('.');
		if (num == -1)
		{
			return string.Empty;
		}
		int num2 = GetTextInternal().LastIndexOf('}');
		if (num2 == -1)
		{
			return string.Empty;
		}
		if (num2 - num - 1 < 1)
		{
			return string.Empty;
		}
		if (GetTextInternal().Length <= num + 1)
		{
			return string.Empty;
		}
		if (GetTextInternal().Length <= num2 - num - 1)
		{
			return string.Empty;
		}
		if (num + 1 < 0)
		{
			return string.Empty;
		}
		if (num2 - num - 1 < 0)
		{
			return string.Empty;
		}
		return GetTextInternal().Substring(num + 1, num2 - num - 1);
	}

	public override string GetQuickInfo()
	{
		switch (Report.Info.QuickInfoType)
		{
		case StiQuickInfoType.ShowAliases:
			if (!string.IsNullOrEmpty(Alias))
			{
				return Alias;
			}
			return GetTextInternal();
		case StiQuickInfoType.ShowFields:
		{
			string fields = GetFields();
			if (fields.Length == 0)
			{
				return Text;
			}
			return fields;
		}
		case StiQuickInfoType.ShowFieldsOnly:
			return GetFields();
		case StiQuickInfoType.ShowContent:
			return GetTextInternal();
		default:
			return base.GetQuickInfo();
		}
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (GetValueEvent != null)
		{
			stiEventsCollection.Add(GetValueEvent);
		}
		return stiEventsCollection;
	}

	public virtual string GetTextInternal()
	{
		return Text.Value;
	}

	public virtual void SetTextInternal(string value)
	{
		Text.Value = value;
	}

	protected virtual void OnGetValue(StiGetValueEventArgs e)
	{
	}

	public virtual void InvokeGetValue(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetValue(e);
			StiReport report = Report;
			if (report != null && report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = sender as StiText;
				bool flag = stiText != null && !string.IsNullOrEmpty(stiText.ExcelValue.Value);
				bool flag2 = stiText != null && stiText.ProcessAt == StiProcessAt.EndOfPage;
				StiParserParameters stiParserParameters = new StiParserParameters
				{
					ExecuteIfStoreToPrint = flag2
				};
				object obj = null;
				if (sender is StiRichText stiRichText)
				{
					if (stiRichText.OnlyText)
					{
						obj = XmlConvert.DecodeName(stiRichText.Text.Value).Replace('\0', ' ');
					}
					else if (Report.Engine != null)
					{
						object obj2 = Report.Engine.ParserConversionStore["*StiRichTextExpression*" + sender.Name];
						if (obj2 != null)
						{
							obj = StiParser.ParseTextValue("{" + (string)obj2 + "}", stiRichText, stiParserParameters);
						}
					}
				}
				else if (this is StiText { Type: StiSystemTextType.DataColumn } stiText2 && !string.IsNullOrEmpty(stiText2.NullValue))
				{
					string value = stiText.Text.Value;
					if (StringExt.StartsWithInvariant(value, "{") && StringExt.EndsWithInvariant(value, "}") && value.Length > 2)
					{
						value = value.Substring(1, value.Length - 2);
						obj = (StiNullValuesHelper.IsNull(Report, value) ? stiText2.NullValue : StiParser.ParseTextValue(stiText.Text.Value, stiText, stiParserParameters));
					}
				}
				else
				{
					obj = StiParser.ParseTextValue(stiText.Text.Value, stiText, stiParserParameters);
				}
				if (stiParserParameters.StoreToPrint && !flag2)
				{
					e.StoreToPrinted = true;
					string value2 = stiText.Text.Value;
					if (value2 == null || !value2.StartsWith("#%#"))
					{
						obj = "#%#" + value2;
					}
					flag = true;
				}
				if (stiText != null && stiText.Format != null && stiText.Format != "G")
				{
					object arg = (flag ? obj : Report.CheckExcelValue(sender, obj));
					if (stiText.TextFormat is StiCustomFormatService stiCustomFormatService && !string.IsNullOrWhiteSpace(stiCustomFormatService.StringFormat) && stiCustomFormatService.StringFormat.Contains("{"))
					{
						string stringFormat = stiCustomFormatService.StringFormat;
						try
						{
							stiCustomFormatService.StringFormat = System.Convert.ToString(StiParser.ParseTextValue(stringFormat, stiText, stiParserParameters));
							e.Value = stiCustomFormatService.Format(arg);
						}
						finally
						{
							stiCustomFormatService.StringFormat = stringFormat;
						}
					}
					else
					{
						e.Value = stiText.TextFormat.Format(arg);
					}
				}
				else
				{
					e.Value = Report.ToString(sender, obj, !flag);
				}
			}
			if (base.Events[EventGetValue] is StiGetValueEventHandler stiGetValueEventHandler)
			{
				stiGetValueEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetValueEvent, e);
			StiOptions.Engine.GlobalEvents.InvokeGetValue(sender, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in Text property of '" + Name + "' can't be evaluated! " + ex.Message;
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	public void CheckDuplicates(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			StiText stiText = sender as StiText;
			StiProcessingDuplicatesType processingDuplicates = ProcessingDuplicates;
			if (processingDuplicates == StiProcessingDuplicatesType.BasedOnValueAndTagMerge || processingDuplicates == StiProcessingDuplicatesType.BasedOnValueAndTagHide || processingDuplicates == StiProcessingDuplicatesType.BasedOnValueRemoveText)
			{
				string tag = ((processingDuplicates == StiProcessingDuplicatesType.BasedOnValueAndTagMerge || processingDuplicates == StiProcessingDuplicatesType.BasedOnValueAndTagHide) ? System.Convert.ToString(stiText.TagValue) : null);
				if (Report.Engine.CheckForDuplicate(Name, e.Value, tag))
				{
					e.Value = string.Empty;
					if (sender is StiText stiText2 && !string.IsNullOrWhiteSpace(stiText2.Format) && stiText2.Format != "G")
					{
						stiText2.ExcelDataValue = null;
					}
				}
			}
			if (processingDuplicates != StiProcessingDuplicatesType.GlobalBasedOnValueAndTagMerge && processingDuplicates != StiProcessingDuplicatesType.GlobalBasedOnValueAndTagHide && processingDuplicates != StiProcessingDuplicatesType.GlobalBasedOnValueRemoveText)
			{
				return;
			}
			string tag2 = ((processingDuplicates == StiProcessingDuplicatesType.GlobalBasedOnValueAndTagMerge || processingDuplicates == StiProcessingDuplicatesType.GlobalBasedOnValueAndTagHide) ? System.Convert.ToString(stiText.TagValue) : null);
			if (Report.Engine.CheckForDuplicate($"{Left}_{Width}", e.Value, tag2))
			{
				e.Value = string.Empty;
				if (sender is StiText stiText3 && !string.IsNullOrWhiteSpace(stiText3.Format) && stiText3.Format != "G")
				{
					stiText3.ExcelDataValue = null;
				}
			}
		}
		catch
		{
		}
	}

	protected virtual void OnTextProcess(StiValueEventArgs e)
	{
	}

	public void InvokeTextProcess(StiComponent sender, StiValueEventArgs e)
	{
		try
		{
			OnTextProcess(e);
			bool flag = true;
			if (Report != null)
			{
				flag = Report.CalculationMode == StiCalculationMode.Compilation;
			}
			if (flag)
			{
				if (base.Events[EventTextProcess] is StiValueEventHandler stiValueEventHandler)
				{
					stiValueEventHandler(sender, e);
				}
				return;
			}
			if (base.Events[EventTextProcess] is StiValueEventHandler stiValueEventHandler2)
			{
				stiValueEventHandler2(sender, e);
			}
			if (Report == null || Report.Engine == null)
			{
				return;
			}
			object obj = Report.Engine.ParserConversionStore["*StiConditionExpression*" + Name];
			if (obj != null)
			{
				Report.Engine.LastInvokeTextProcessValueEventArgsValue = e.Value;
				ApplyConditionsAssignExpression(sender, obj as ArrayList);
				if (this is StiCrossCell)
				{
					ApplyConditions(sender, obj as ArrayList, e);
				}
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), Name + " InvokeTextProcess...ERROR");
			StiLogService.Write(((object)this).GetType(), Name + " " + ex.Message);
			Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
		}
	}

	private void ApplyConditionsAssignExpression(object sender, ArrayList conditions)
	{
		if (conditions == null)
		{
			return;
		}
		foreach (DictionaryEntry condition in conditions)
		{
			StiCondition stiCondition = condition.Key as StiCondition;
			if (stiCondition.CanAssignExpression && stiCondition.AssignExpression != null && stiCondition.AssignExpression.Trim().Length > 0)
			{
				object obj = StiParser.ParseTextValue((string)condition.Value, this, sender);
				if (obj is bool && (bool)obj)
				{
					object obj2 = StiParser.ParseTextValue("{" + stiCondition.AssignExpression + "}", this, sender);
					((StiText)sender).TextValue = Report.ToString(sender, obj2, allowExcelCheck: true);
				}
			}
		}
	}

	private static string NormalizeEndOfLineSymbols(string inputText)
	{
		if (inputText == null || inputText.Length < 2 || (inputText.IndexOf('\r') == -1 && inputText.IndexOf('\n') == -1))
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < inputText.Length; i++)
		{
			char c = inputText[i];
			if (c == '\r' || c == '\n')
			{
				if (i + 1 < inputText.Length)
				{
					char c2 = inputText[i + 1];
					if ((c2 == '\r' || c2 == '\n') && c2 != c)
					{
						i++;
					}
				}
				stringBuilder.Append("\r\n");
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		if (stringBuilder.Length == inputText.Length)
		{
			return null;
		}
		return stringBuilder.ToString();
	}

	public StiSimpleText()
		: this(RectangleD.Empty, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiSimpleText(RectangleD rect)
		: this(rect, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiSimpleText(RectangleD rect, string text)
		: base(rect)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		SetTextInternal(text);
		PlaceOnToolbox = false;
	}
}
