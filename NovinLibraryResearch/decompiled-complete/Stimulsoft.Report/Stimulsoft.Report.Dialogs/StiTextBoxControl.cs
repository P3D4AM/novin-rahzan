using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiTextBoxControl), "Stimulsoft.Report.Dialogs.Bmp.StiTextBoxControl.gif")]
[StiGdiPainter(typeof(StiTextBoxControlGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiTextBoxControlWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiTextBoxControl : StiReportControl, IStiTextBoxControl
{
	private TextBox control;

	private string textBinding = "";

	private bool multiline;

	private bool wordWrap;

	private bool acceptsTab;

	private bool acceptsReturn;

	private int maxLength = 32767;

	private char passwordChar;

	private string text = "TextBox";

	public override StiComponentId ComponentId => StiComponentId.StiTextBoxControl;

	public override int ToolboxPosition => 2;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiTextBoxControl");

	[Browsable(false)]
	public TextBox Control
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
	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue("")]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[Description("Gets the data bindings for the text.")]
	[StiOrder(220)]
	public string TextBinding
	{
		get
		{
			return textBinding;
		}
		set
		{
			textBinding = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	public override double Height
	{
		get
		{
			if (!Multiline)
			{
				return 20.0;
			}
			return base.Height;
		}
		set
		{
			if (!Multiline)
			{
				base.Height = 20.0;
			}
			base.Height = value;
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether this is a multiline text box control.")]
	[StiOrder(550)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Multiline
	{
		get
		{
			return multiline;
		}
		set
		{
			multiline = value;
			UpdateReportControl("Multiline");
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Indicates whether a multiline text box control automatically wraps words to the beginning of the next line when necessary.")]
	[StiOrder(820)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool WordWrap
	{
		get
		{
			return wordWrap;
		}
		set
		{
			wordWrap = value;
			UpdateReportControl("WordWrap");
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether pressing the TAB key in a multiline text box control types a TAB character in the control instead of moving the focus to the next control in the tab order.")]
	[StiOrder(110)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool AcceptsTab
	{
		get
		{
			return acceptsTab;
		}
		set
		{
			acceptsTab = value;
			UpdateReportControl("AcceptsTab");
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether pressing ENTER in a multiline TextBox control creates a new line of text in the control or activates the default button for the form.")]
	[StiOrder(100)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool AcceptsReturn
	{
		get
		{
			return acceptsReturn;
		}
		set
		{
			acceptsReturn = value;
			UpdateReportControl("AcceptsReturn");
		}
	}

	[StiSerializable]
	[DefaultValue(32767)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the maximum number of characters the user can type into the text box control.")]
	[StiOrder(520)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int MaxLength
	{
		get
		{
			return maxLength;
		}
		set
		{
			maxLength = value;
			UpdateReportControl("MaxLength");
		}
	}

	[StiSerializable]
	[DefaultValue('\0')]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the character used to mask characters of a password in a single-line TextBox control.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(570)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public char PasswordChar
	{
		get
		{
			return passwordChar;
		}
		set
		{
			passwordChar = value;
			UpdateReportControl("PasswordChar");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Gets or sets the current text in the text box.")]
	[Editor("Stimulsoft.Report.Components.Design.StiExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
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

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextBinding", TextBinding);
		JsonExtensions.AddPropertyBool(val, "Multiline", Multiline, false, false);
		JsonExtensions.AddPropertyBool(val, "WordWrap", WordWrap, false, false);
		JsonExtensions.AddPropertyBool(val, "AcceptsTab", AcceptsTab, false, false);
		JsonExtensions.AddPropertyBool(val, "AcceptsReturn", AcceptsReturn, false, false);
		JsonExtensions.AddPropertyInt(val, "MaxLength", MaxLength, 32767);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "PasswordChar", PasswordChar.ToString());
		JsonExtensions.AddPropertyString(val, "Text", Text);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "TextBinding":
				textBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Multiline":
				multiline = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "WordWrap":
				wordWrap = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AcceptsTab":
				acceptsTab = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AcceptsReturn":
				acceptsReturn = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MaxLength":
				maxLength = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "PasswordChar":
			{
				string text = StiJsonReportObjectHelper.DeserializeString(item);
				passwordChar = text.ToCharArray()[0];
				break;
			}
			case "Text":
				this.text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override StiEvent GetDefaultEvent()
	{
		return EnterEvent;
	}

	public override StiComponent CreateNew()
	{
		return new StiTextBoxControl();
	}

	public StiTextBoxControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiTextBoxControl(RectangleD rect)
		: base(rect)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		base.BackColor = SystemColors.Window;
		PlaceOnToolbox = true;
		Text = LocalizedName;
	}
}
