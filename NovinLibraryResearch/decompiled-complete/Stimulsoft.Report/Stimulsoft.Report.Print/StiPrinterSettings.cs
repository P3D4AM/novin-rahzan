using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Print;

public class StiPrinterSettings : IStiSerializeToCodeAsClass, IStiJsonReportObject, IStiDefault
{
	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			if ((int)PrintDialogResult == 0 && Copies == 1 && Collate && (int)Duplex == -1 && ShowDialog && PrinterName != null)
			{
				return PrinterName.Length == 0;
			}
			return false;
		}
	}

	[Browsable(false)]
	public DialogResult PrintDialogResult { get; set; }

	[DefaultValue(1)]
	[StiSerializable]
	[Description("Gets or sets copies number of report for printing.")]
	public int Copies { get; set; } = 1;


	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that collate mode of printing will be used or not.")]
	public bool Collate { get; set; } = true;


	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets mode of duplex printing.")]
	public Duplex Duplex { get; set; } = (Duplex)(-1);


	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that print dialog will be shown or not.")]
	public bool ShowDialog { get; set; } = true;


	[DefaultValue("")]
	[StiSerializable]
	[Description("Gets or sets name of printer which will be used for report printing.")]
	[Editor("Stimulsoft.Report.Print.Design.StiPrinterNameEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string PrinterName { get; set; } = "";


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyEnum(val, "PrintDialogResult", (Enum)(object)PrintDialogResult, (Enum)(object)(DialogResult)0);
		JsonExtensions.AddPropertyInt(val, "Copies", Copies, 1);
		JsonExtensions.AddPropertyBool(val, "Collate", Collate, true, false);
		JsonExtensions.AddPropertyEnum(val, "Duplex", (Enum)(object)Duplex, (Enum)(object)(Duplex)(-1));
		JsonExtensions.AddPropertyBool(val, "ShowDialog", ShowDialog, true, false);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "PrinterName", PrinterName);
		if (((JContainer)val).Count == 0)
		{
			return null;
		}
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "PrintDialogResult":
				PrintDialogResult = StiJsonReportObjectHelper.DeserializeEnum<DialogResult>(item);
				break;
			case "Copies":
				Copies = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Collate":
				Collate = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Duplex":
				Duplex = StiJsonReportObjectHelper.DeserializeEnum<Duplex>(item);
				break;
			case "ShowDialog":
				ShowDialog = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "PrinterName":
				PrinterName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}
}
