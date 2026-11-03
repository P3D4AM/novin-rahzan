using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

public class StiSSCC18BarCodeType : StiCode128cBarCodeType
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiSSCC18BarCodeType;

	public override string ServiceName => "SSCC";

	public override string DefaultCodeValue => "(00)~0~0123456~000000001~8";

	[Description("Gets or sets the GS1 Company Prefix (7-10 digits).")]
	[DefaultValue("0123456")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public string CompanyPrefix { get; set; } = "0123456";


	[Description("Gets or sets the Serial Reference Numbers (6-9 digits).")]
	[DefaultValue("000000001")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public string SerialNumber { get; set; } = "000000001";


	[Description("Gets or sets the extension digit.")]
	[DefaultValue("0")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public string ExtensionDigit { get; set; } = "0";


	protected override bool TextSpacing => false;

	public override bool[] VisibleProperties => new bool[27]
	{
		false, false, false, false, false, false, false, false, false, false,
		false, true, false, true, false, false, false, false, false, false,
		false, false, false, false, true, true, true
	};

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyString(val, "CompanyPrefix", CompanyPrefix, "0123456");
		JsonExtensions.AddPropertyString(val, "SerialNumber", SerialNumber, "000000001");
		JsonExtensions.AddPropertyString(val, "ExtensionDigit", ExtensionDigit, "0");
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "CompanyPrefix":
				CompanyPrefix = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "SerialNumber":
				SerialNumber = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ExtensionDigit":
				ExtensionDigit = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[5]
		{
			propertiesHelper.CompanyPrefix(),
			propertiesHelper.ExtensionDigit(),
			propertiesHelper.fHeight(),
			propertiesHelper.Module(),
			propertiesHelper.SerialNumber()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public override string GetCombinedCode()
	{
		return string.Format("(00){0}{1}{0}{2}{0}{3}{0}{4}", '~', ExtensionDigit, CompanyPrefix, SerialNumber, GetCheckDigit(CheckCodeSymbols(ExtensionDigit + CompanyPrefix + SerialNumber + new string('0', 17), "0123456789")));
	}

	private string GetCheckDigit(string input)
	{
		int[] array = new int[17];
		for (int i = 0; i < 17; i++)
		{
			array[i] = int.Parse(input[i].ToString(CultureInfo.InvariantCulture));
		}
		int num = array[1] + array[3] + array[5] + array[7] + array[9] + array[11] + array[13] + array[15] + (array[0] + array[2] + array[4] + array[6] + array[8] + array[10] + array[12] + array[14] + array[16]) * 3;
		int num2 = 10 - num % 10;
		if (num2 == 10)
		{
			num2 = 0;
		}
		return ((char)(num2 + 48)).ToString(CultureInfo.InvariantCulture);
	}

	private bool CheckContens(string inputCode, string[] arr)
	{
		if (arr.Length != 5)
		{
			return true;
		}
		if (arr[0] != "00")
		{
			return true;
		}
		if (arr[1].Length != 1)
		{
			return true;
		}
		int length = arr[2].Length;
		if (length < 7 || length > 10)
		{
			return true;
		}
		int length2 = arr[3].Length;
		if (length2 + length != 16)
		{
			return true;
		}
		if (inputCode.Length != 24)
		{
			return true;
		}
		return false;
	}

	public override void Draw(object context, StiBarCode barCode, RectangleF rect, float zoom)
	{
		string code = GetCode(barCode);
		string text = CheckCodeSymbols(code.Replace('\u001f', '~'), "0123456789~");
		string[] array = text.Split(new char[1] { '~' });
		bool flag = CheckContens(text, array);
		if (flag)
		{
			string[] array2 = new string[(array.Length > 5) ? array.Length : 5];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i];
			}
			array = array2;
			array[0] = "00";
			if (array[1].Length < 1)
			{
				array[1] = "0";
			}
			if (array[1].Length > 1)
			{
				array[1] = array[1].Substring(0, 1);
			}
			if (array[2].Length < 7)
			{
				array[2] += new string('0', 7 - array[2].Length);
			}
			if (array[2].Length > 10)
			{
				array[2] = array[2].Substring(0, 10);
			}
			if (array[3].Length < 16 - array[2].Length)
			{
				array[3] += new string('0', 16 - array[2].Length);
			}
			if (array[3].Length > 16 - array[2].Length)
			{
				array[3] = array[3].Substring(0, 16 - array[2].Length);
			}
		}
		string text2 = array[1] + array[2] + array[3];
		string checkDigit = GetCheckDigit(text2);
		text2 = array[0] + text2 + checkDigit;
		string textString = string.Format("({0}) {1} {2} {3} {4}{5}", array[0], array[1], array[2], array[3], checkDigit, flag ? " *" : "");
		if (barCode.Report != null && barCode.Report.IsDesigning)
		{
			textString = code.Replace('~', ' ');
		}
		List<string> list = new List<string> { text2 };
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < list.Count; j++)
		{
			string text3 = list[j];
			int num = text3.Length / 2;
			for (int k = 0; k < num; k++)
			{
				stringBuilder.Append((char)int.Parse(text3.Substring(k * 2, 2)));
			}
			if (text3.Length % 2 == 1)
			{
				stringBuilder.Append('d');
				stringBuilder.Append((char)(text3[text3.Length - 1] - 32));
				if (j < list.Count - 1)
				{
					stringBuilder.Append('c');
				}
			}
			if (j < list.Count - 1)
			{
				stringBuilder.Append('f');
			}
		}
		int[] array3 = new int[stringBuilder.Length + 4];
		array3[0] = 105;
		array3[1] = 102;
		for (int l = 0; l < stringBuilder.Length; l++)
		{
			array3[l + 2] = stringBuilder[l];
		}
		int num2 = array3[0] + array3[1];
		for (int m = 0; m < array3.Length - 3; m++)
		{
			num2 += array3[m + 2] * (m + 2);
		}
		array3[^2] = num2 % 103;
		array3[^1] = 106;
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int n = 0; n < array3.Length; n++)
		{
			stringBuilder2.Append(CodeToBar(Code128Table[array3[n]]));
		}
		CalculateSizeFull(10f, 10f, 0f, 1f, 45f, 45f, 46f, 8.33f, 55f, 45f, 2f, zoom, text2, textString, stringBuilder2.ToString(), rect, barCode);
		DrawBarCode(context, rect, barCode);
	}

	public override StiBarCodeTypeService CreateNew()
	{
		return new StiSSCC18BarCodeType();
	}

	public StiSSCC18BarCodeType()
		: this(13f, 1f)
	{
	}

	public StiSSCC18BarCodeType(float module, float height)
		: base(module, height)
	{
	}
}
