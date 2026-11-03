using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiWinControl), "Stimulsoft.Report.Images.Components.StiWinControl.png")]
[StiGdiPainter(typeof(StiWinControlGdiPainter))]
[StiV1Builder(typeof(StiWinControlV1Builder))]
[StiV2Builder(typeof(StiWinControlV2Builder))]
[StiToolbox(true)]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiGrowToHeight))]
[StiGuiMode(/*Could not decode attribute arguments.*/)]
public class StiWinControl : StiComponent, IStiExportImageExtended, IStiExportImage
{
	private string typeName = "";

	private Font font = new Font("Microsoft Sans Serif", 8f);

	private Color backColor = SystemColors.Control;

	public override StiComponentId ComponentId => StiComponentId.StiWinControl;

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBorder Border { get; set; } = new StiBorder();


	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
		}
	}

	public override int ToolboxPosition => 115;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override string LocalizedName => StiLocalization.Get("Components", "StiWinControl");

	[Browsable(true)]
	[DefaultValue("")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets type name.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string TypeName
	{
		get
		{
			return typeName;
		}
		set
		{
			typeName = value;
			UpdateControl();
		}
	}

	[Browsable(false)]
	public Control Control { get; set; }

	[StiSerializable]
	[Browsable(true)]
	[Description("Gets or sets the text associated with this control.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Text { get; set; } = "";


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets the foreground color of the control.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color ForeColor { get; set; } = Color.Black;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("The font used to display text in the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Font Font
	{
		get
		{
			return font;
		}
		set
		{
			font = value;
			UpdateControl();
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("The background color of the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color BackColor
	{
		get
		{
			return backColor;
		}
		set
		{
			backColor = value;
			UpdateControl();
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TypeName", TypeName);
		JsonExtensions.AddPropertyString(val, "Text", Text);
		JsonExtensions.AddPropertyColor(val, "ForeColor", ForeColor, Color.Black);
		JsonExtensions.AddPropertyFont(val, "Font", Font, "Microsoft Sans Serif", 8f);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, SystemColors.Control);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "TypeName":
				typeName = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Text":
				Text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ForeColor":
				ForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Font":
				font = StiJsonReportObjectHelper.DeserializeFont(item, font);
				break;
			case "BackColor":
				backColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return null;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		return painter.GetImage(this, ref zoom, format);
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		return true;
	}

	public override List<StiFont> GetFonts()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
		return fonts.Distinct().ToList();
	}

	public Image GetControlImage()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		if (Page == null || Report == null || Control == null)
		{
			return null;
		}
		RectangleD val = Report.Unit.ConvertToHInches(ClientRectangle);
		if (!(((RectangleD)(ref val)).Width > 0.0) || !(((RectangleD)(ref val)).Height > 0.0))
		{
			return null;
		}
		Control.Left = 0;
		Control.Top = 0;
		Control.Width = (int)((RectangleD)(ref val)).Width;
		Control.Height = (int)((RectangleD)(ref val)).Height;
		Control.Text = Text;
		Control.ForeColor = ForeColor;
		Control.BackColor = BackColor;
		Control.Font = Font;
		MemoryStream memoryStream = new MemoryStream();
		Image val2 = null;
		Bitmap val3 = new Bitmap(1, 1);
		try
		{
			Graphics val4 = Graphics.FromImage((Image)(object)val3);
			try
			{
				IntPtr hdc = val4.GetHdc();
				val2 = (Image)new Metafile((Stream)memoryStream, hdc);
				val4.ReleaseHdc(hdc);
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
		Graphics val5 = Graphics.FromImage(val2);
		try
		{
			val5.Clear(Color.White);
			Type type = ((object)Control).GetType();
			MethodInfo method = type.GetMethod("GetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
			object[] parameters = new object[1] { (object)(ControlStyles)65536 };
			bool flag = (bool)method.Invoke(Control, parameters);
			MethodInfo method2 = type.GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
			parameters = new object[2]
			{
				(object)(ControlStyles)65536,
				false
			};
			method2.Invoke(Control, parameters);
			IntPtr hdc2 = val5.GetHdc();
			Message val6 = Message.Create(Control.Handle, 15, hdc2, IntPtr.Zero);
			MethodInfo method3 = type.GetMethod("WndProc", BindingFlags.Instance | BindingFlags.NonPublic);
			parameters = new object[1] { val6 };
			method3.Invoke(Control, parameters);
			val5.ReleaseHdc(hdc2);
			parameters = new object[2]
			{
				(object)(ControlStyles)65536,
				flag
			};
			method2.Invoke(Control, parameters);
			return val2;
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
	}

	private void UpdateControl()
	{
		if (Control != null)
		{
			((Component)(object)Control).Dispose();
			Control = null;
		}
		if (!string.IsNullOrEmpty(TypeName))
		{
			Type type = StiTypeFinder.GetType(TypeName);
			if (type != null)
			{
				object obj = StiActivator.CreateObject(type);
				Control = (Control)((obj is Control) ? obj : null);
			}
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiWinControl();
	}

	public StiWinControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiWinControl(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
