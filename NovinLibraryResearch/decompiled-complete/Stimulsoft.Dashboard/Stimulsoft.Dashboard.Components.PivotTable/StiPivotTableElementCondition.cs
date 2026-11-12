using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Image.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Dashboard.Components.PivotTable;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiPivotTableElementCondition : IStiPivotTableElementCondition, ICloneable, IStiJsonReportObject
{
	private byte[] customIcon;

	private Size? iconSize;

	private float iconFontSize => 13f;

	[StiSerializable]
	[Description("Gets or sets a color to draw text.")]
	public Color TextColor { get; set; } = Color.Black;


	[StiSerializable]
	[Description("Gets or sets a color to draw background of text.")]
	public Color BackColor { get; set; } = Color.Transparent;


	[StiSerializable]
	[Description("Gets or sets font of text.")]
	public Font Font { get; set; } = new Font("Arial", 8f);


	[StiSerializable]
	[DefaultValue(StiConditionPermissions.All)]
	public StiConditionPermissions Permissions { get; set; } = StiConditionPermissions.All;


	[StiSerializable]
	[DefaultValue(null)]
	[Browsable(false)]
	[TypeConverter(typeof(StiImageBytesConverter))]
	public byte[] CustomIcon
	{
		get
		{
			return customIcon;
		}
		set
		{
			customIcon = value;
			iconSize = null;
		}
	}

	[StiSerializable]
	[DefaultValue(StiFontIcons.Adjust)]
	public StiFontIcons Icon { get; set; } = StiFontIcons.Adjust;


	[StiSerializable]
	[DefaultValue(StiIconAlignment.None)]
	public StiIconAlignment IconAlignment { get; set; }

	[DefaultValue(Stimulsoft.Report.Components.StiFilterCondition.EqualTo)]
	[StiSerializable]
	public Stimulsoft.Report.Components.StiFilterCondition Condition { get; set; }

	[DefaultValue(Stimulsoft.Report.Components.StiFilterDataType.Numeric)]
	[StiSerializable]
	public Stimulsoft.Report.Components.StiFilterDataType DataType { get; set; } = Stimulsoft.Report.Components.StiFilterDataType.Numeric;


	[DefaultValue("")]
	[StiSerializable]
	public string Value { get; set; }

	[DefaultValue("")]
	[StiSerializable]
	public string KeyValueMeter { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Browsable(false)]
	public Color IconColor { get; set; } = Color.Maroon;


	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			if (TextColor == Color.Black && Condition == Stimulsoft.Report.Components.StiFilterCondition.EqualTo && DataType == Stimulsoft.Report.Components.StiFilterDataType.Numeric && Value == "")
			{
				return KeyValueMeter == "";
			}
			return false;
		}
	}

	public Size? IconSize
	{
		get
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			try
			{
				if (!iconSize.HasValue && customIcon != null)
				{
					using MemoryStream memoryStream = new MemoryStream(customIcon);
					Image val = Image.FromStream((Stream)memoryStream);
					try
					{
						iconSize = val.Size;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				else if (!iconSize.HasValue)
				{
					double factor = StiScale.Factor;
					Bitmap val2 = new Bitmap(1, 1);
					try
					{
						Graphics val3 = Graphics.FromImage((Image)(object)val2);
						try
						{
							Font val4 = new Font(StiFontIconsHelper.GetFontFamilyIcons(), iconFontSize);
							try
							{
								SizeF sizeF = StiTextDrawing.MeasureString(val3, StiFontIconsHelper.GetContent(Icon), val4);
								sizeF.Width /= (float)factor;
								sizeF.Height /= (float)factor;
								iconSize = sizeF.ToSize();
							}
							finally
							{
								((IDisposable)val4)?.Dispose();
							}
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
			}
			catch
			{
			}
			return iconSize;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyIdent("Ident", GetType().Name);
		jObject.AddPropertyStringNullOrEmpty("KeyValueMeter", KeyValueMeter);
		jObject.AddPropertyEnum("Condition", Condition, StiFilterCondition.EqualTo);
		jObject.AddPropertyEnum("DataType", DataType, StiFilterDataType.String);
		jObject.AddPropertyStringNullOrEmpty("Value", Value);
		jObject.AddPropertyColor("TextColor", TextColor, Color.Black);
		jObject.AddPropertyColor("BackColor", BackColor, Color.Transparent);
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyEnum("Permissions", Permissions, StiConditionPermissions.All);
		jObject.AddPropertyEnum("IconAlignment", IconAlignment, StiIconAlignment.None);
		jObject.AddPropertyColor("IconColor", IconColor, Color.Maroon);
		jObject.AddPropertyEnum("Icon", Icon);
		if (CustomIcon != null)
		{
			jObject.AddPropertyByteArray("CustomIcon", CustomIcon);
		}
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Condition":
				Condition = item.DeserializeEnum<Stimulsoft.Report.Components.StiFilterCondition>();
				break;
			case "DataType":
				DataType = item.DeserializeEnum<Stimulsoft.Report.Components.StiFilterDataType>();
				break;
			case "KeyValueMeter":
				KeyValueMeter = item.DeserializeString();
				break;
			case "Value":
				Value = item.DeserializeString();
				break;
			case "TextColor":
				TextColor = item.DeserializeColor();
				break;
			case "BackColor":
				BackColor = item.DeserializeColor();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "Permissions":
				Permissions = item.DeserializeEnum<StiConditionPermissions>();
				break;
			case "IconAlignment":
				IconAlignment = item.DeserializeEnum<StiIconAlignment>();
				break;
			case "Icon":
				Icon = item.DeserializeEnum<StiFontIcons>();
				break;
			case "CustomIcon":
				CustomIcon = item.DeserializeByteArray();
				break;
			case "IconColor":
				IconColor = item.DeserializeColor();
				break;
			}
		}
	}

	public virtual object Clone()
	{
		return MemberwiseClone();
	}

	public static StiPivotTableElementCondition LoadFromJson(JObject json)
	{
		StiPivotTableElementCondition stiPivotTableElementCondition = new StiPivotTableElementCondition();
		stiPivotTableElementCondition.LoadFromJsonObject(json);
		return stiPivotTableElementCondition;
	}

	public byte[] GetIcon(bool isExporting)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		if (CustomIcon != null)
		{
			if (StiScale.Factor == 1.0 || isExporting)
			{
				return CustomIcon;
			}
			try
			{
				using MemoryStream memoryStream = new MemoryStream(customIcon);
				Image val = Image.FromStream((Stream)memoryStream);
				try
				{
					Bitmap image = StiImageUtils.ResizeImage(val, (int)((double)val.Width * StiScale.Factor), (int)((double)val.Height * StiScale.Factor));
					return StiImageConverter.ImageToBytes((Image)(object)image);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception)
			{
				return null;
			}
		}
		try
		{
			double num = (isExporting ? 1.0 : StiScale.Factor);
			Bitmap val2 = new Bitmap((int)((double)IconSize.Value.Width * num), (int)((double)IconSize.Value.Height * num), (PixelFormat)2498570);
			try
			{
				Graphics val3 = Graphics.FromImage((Image)(object)val2);
				try
				{
					val3.TextRenderingHint = (TextRenderingHint)3;
					Font val4 = new Font(StiFontIconsHelper.GetFontFamilyIcons(), (float)(int)((double)iconFontSize / (isExporting ? StiScale.Factor : 1.0)));
					try
					{
						SolidBrush val5 = new SolidBrush(IconColor);
						try
						{
							val3.DrawString(StiFontIconsHelper.GetContent(Icon), val4, (Brush)(object)val5, 0f, 0f);
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
					return StiImageConverter.ImageToBytes((Image)(object)val2);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		catch
		{
			return null;
		}
	}

	public virtual int GetUniqueCode()
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected I4, but got Unknown
		int num = ((KeyValueMeter != null) ? KeyValueMeter.GetHashCode() : 0);
		num = (num * 397) ^ TextColor.GetHashCode();
		num = (num * 397) ^ BackColor.GetHashCode();
		num = (num * 397) ^ ((int)Condition * 23);
		num = (num * 397) ^ ((int)DataType * 876);
		if (!string.IsNullOrEmpty(Value))
		{
			num = (num * 397) ^ Value.GetHashCode();
		}
		if (Font != null)
		{
			num = (num * 397) ^ Font.Name.GetHashCode();
			num = (num * 397) ^ Font.Size.GetHashCode();
			num = (num * 397) ^ (Font.Style * 876);
		}
		if (Permissions != 0)
		{
			num = (num * 397) ^ ((int)Permissions * 876);
		}
		if (customIcon != null)
		{
			num = (num * 397) ^ CustomIcon.GetHashCode();
		}
		num = (num * 397) ^ ((int)IconAlignment * 654);
		num = (num * 397) ^ ((int)Icon * 623);
		return (num * 397) ^ IconColor.GetHashCode();
	}

	public StiPivotTableElementCondition()
	{
	}//IL_0021: Unknown result type (might be due to invalid IL or missing references)
	//IL_002b: Expected O, but got Unknown


	[StiUniversalConstructor("Condition")]
	public StiPivotTableElementCondition(string keyValueMeter, Stimulsoft.Report.Components.StiFilterDataType dataType, Stimulsoft.Report.Components.StiFilterCondition condition, string value, Font font, Color textColor, Color backColor, StiConditionPermissions permissions, StiFontIcons icon, StiIconAlignment iconAlignment, byte[] customIcon, Color iconColor)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		KeyValueMeter = keyValueMeter;
		DataType = dataType;
		Condition = condition;
		Value = value;
		Font = font;
		TextColor = textColor;
		BackColor = backColor;
		Permissions = permissions;
		Icon = icon;
		IconAlignment = iconAlignment;
		CustomIcon = customIcon;
		IconColor = iconColor;
	}
}
