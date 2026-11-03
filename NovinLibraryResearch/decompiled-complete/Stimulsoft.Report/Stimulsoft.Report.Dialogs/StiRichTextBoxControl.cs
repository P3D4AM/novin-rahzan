using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiTextBoxControl), "Stimulsoft.Report.Dialogs.Bmp.StiRichTextBoxControl.gif")]
[StiDesigner("Stimulsoft.Report.Dialogs.Design.StiRichTextBoxControlDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfRichTextBoxControlDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiRichTextBoxControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiRichTextBoxControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiRichTextBoxControl : StiReportControl, IStiRichTextBoxControl
{
	private Image image;

	private RichTextBox control;

	private string text = "";

	[Browsable(false)]
	public Image Image => image;

	public override StiComponentId ComponentId => StiComponentId.StiRichTextBoxControl;

	public override int ToolboxPosition => 18;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiRichTextBoxControl");

	[Browsable(false)]
	public RichTextBox Control
	{
		get
		{
			return control;
		}
		set
		{
			control = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the current text in the rich text box.")]
	[Editor("Stimulsoft.Report.Components.Design.StiRichTextExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiRichTextExpressionConverter))]
	[DefaultValue("")]
	[StiOrder(730)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
			UpdateReportControl("Text");
		}
	}

	[Browsable(false)]
	public string RtfText
	{
		get
		{
			return StiRichText.UnpackRtf(XmlConvert.DecodeName(Text));
		}
		set
		{
			Text = XmlConvert.EncodeName(StiRichText.PackRtf(value));
		}
	}

	[Browsable(false)]
	public override RightToLeft RightToLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.RightToLeft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.RightToLeft = value;
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 96.0, 96.0);

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Text", Text);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (text == "Text")
			{
				this.text = StiJsonReportObjectHelper.DeserializeString(item);
			}
		}
	}

	public void RenderMetafile()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		if (Page == null || Report == null)
		{
			return;
		}
		bool flag = true;
		int num = 0;
		while (flag && num < 100)
		{
			flag = false;
			try
			{
				StiRichTextBox stiRichTextBox = new StiRichTextBox(BackColor == Color.Transparent);
				try
				{
					string preparedText = GetPreparedText((RichTextBox)(object)stiRichTextBox);
					RectangleD paintRectangle = GetPaintRectangle(convertToHInches: true, convertByZoom: false);
					Graphics val = Graphics.FromHwnd(((Control)stiRichTextBox).Handle);
					try
					{
						IntPtr hdc = val.GetHdc();
						Metafile val2 = new Metafile(hdc, (EmfType)4);
						val.ReleaseHdc(hdc);
						if (BackColor != Color.Transparent)
						{
							((Control)stiRichTextBox).BackColor = BackColor;
						}
						if (preparedText.Length > 0)
						{
							Graphics val3 = Graphics.FromImage((Image)(object)val2);
							try
							{
								int charEnd = 0;
								val3.PageUnit = (GraphicsUnit)2;
								StiRichTextHelper.FormatRange(StiRtfFormatType.DrawRtf, (RichTextBox)(object)stiRichTextBox, new Rectangle(0, 0, (int)((RectangleD)(ref paintRectangle)).Width, (int)((RectangleD)(ref paintRectangle)).Height), val3, 0, preparedText.Length, out charEnd);
							}
							finally
							{
								((IDisposable)val3)?.Dispose();
							}
						}
						if (Image != null)
						{
							Image.Dispose();
						}
						image = (Image)(object)val2;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)stiRichTextBox)?.Dispose();
				}
			}
			catch
			{
				flag = true;
				num++;
			}
		}
	}

	private string GetPreparedText(RichTextBox richTextBox)
	{
		richTextBox.Rtf = StiRichText.UnpackRtf(XmlConvert.DecodeName(Text));
		((TextBoxBase)richTextBox).WordWrap = true;
		string rtf = richTextBox.Rtf;
		if (((Control)richTextBox).Text == string.Empty)
		{
			((Control)richTextBox).Text = " ";
		}
		return rtf;
	}

	public override StiEvent GetDefaultEvent()
	{
		return EnterEvent;
	}

	public override StiComponent CreateNew()
	{
		return new StiRichTextBoxControl();
	}

	public StiRichTextBoxControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiRichTextBoxControl(RectangleD rect)
		: base(rect)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		base.BackColor = SystemColors.Window;
		PlaceOnToolbox = true;
	}
}
