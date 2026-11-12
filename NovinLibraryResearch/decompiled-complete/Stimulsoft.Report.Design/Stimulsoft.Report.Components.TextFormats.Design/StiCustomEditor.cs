using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.TextFormats.Design;

[ToolboxItem(false)]
public class StiCustomEditor : StiFormatEditor
{
	private StiCustomFormatService service;

	private IContainer components;

	private StiListBoxV2 listBoxFormats;

	private StiLabelUX lbFormatMask;

	private StiTextBox textBoxFormatMask;

	public StiCustomEditor(StiCustomFormatService service, StiDesignerControl designer)
		: base(designer)
	{
		this.service = service;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((ListControl)listBoxFormats).SelectedIndex = Build(service, "custom", ((ListBox)listBoxFormats).Items);
			((Control)textBoxFormatMask).Text = service.StringFormat;
			Localize();
		}
	}

	private void Localize()
	{
		((Control)lbFormatMask).Text = Loc.GetCleaned("FormFormatEditor", "FormatMask");
	}

	protected override int Build(StiFormatService service, string category, ObjectCollection items)
	{
		CultureInfo cultureInfo = null;
		try
		{
			if (!string.IsNullOrWhiteSpace(base.Designer?.Report?.Culture))
			{
				cultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo(base.Designer.Report.Culture);
			}
			int num = 0;
			string[] keys = StiLocalization.GetKeys("Formats");
			string[] array = keys;
			foreach (string text in array)
			{
				if (text.IndexOf(category, StringComparison.InvariantCulture) <= -1)
				{
					continue;
				}
				string text2 = StiLocalization.Get("Formats", text);
				if (text2 != null)
				{
					if (text2.Length > 0 && text2[0] == '*')
					{
						base.Formats.Add(text2.Substring(1));
						items.Add((object)("*" + service.Format(text2.Substring(1), service.Sample)));
					}
					else
					{
						base.Formats.Add(text2);
						items.Add((object)text2);
					}
				}
			}
			foreach (string format in base.Formats)
			{
				if (format == service.StringFormat)
				{
					return num;
				}
				num++;
			}
			return -1;
		}
		finally
		{
			if (cultureInfo != null)
			{
				Thread.CurrentThread.CurrentCulture = cultureInfo;
			}
		}
	}

	private void ListBoxFormats_SelectedIndexChanged(object sender, EventArgs e)
	{
		((Control)textBoxFormatMask).Text = ((ListBox)listBoxFormats).SelectedItem as string;
		service.StringFormat = base.Formats[((ListControl)listBoxFormats).SelectedIndex];
		InvokeUpdateSample();
	}

	private void TextBoxFormatMask_TextChanged(object sender, EventArgs e)
	{
		service.StringFormat = ((Control)textBoxFormatMask).Text;
		InvokeUpdateSample();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		listBoxFormats = new StiListBoxV2();
		lbFormatMask = new StiLabelUX();
		textBoxFormatMask = new StiTextBox();
		((Control)this).SuspendLayout();
		((Control)listBoxFormats).Anchor = (AnchorStyles)15;
		((Control)listBoxFormats).BackColor = SystemColors.Window;
		((Control)listBoxFormats).ForeColor = SystemColors.ControlText;
		((Control)listBoxFormats).Location = new Point(0, 44);
		((Control)listBoxFormats).Name = "listBoxFormats";
		((Control)listBoxFormats).Size = new Size(256, 140);
		((Control)listBoxFormats).TabIndex = 0;
		((ListBox)listBoxFormats).SelectedIndexChanged += ListBoxFormats_SelectedIndexChanged;
		((Control)lbFormatMask).ForeColor = Color.DimGray;
		((Control)lbFormatMask).Location = new Point(12, 15);
		((Control)lbFormatMask).Name = "lbFormatMask";
		((Control)lbFormatMask).Size = new Size(80, 16);
		((Control)lbFormatMask).TabIndex = 1;
		((Control)lbFormatMask).Text = "FormatMask";
		((Label)lbFormatMask).TextAlign = (ContentAlignment)16;
		((Control)textBoxFormatMask).Anchor = (AnchorStyles)13;
		((Control)textBoxFormatMask).BackColor = Color.White;
		((Control)textBoxFormatMask).Location = new Point(96, 12);
		((Control)textBoxFormatMask).Name = "textBoxFormatMask";
		((Control)textBoxFormatMask).Size = new Size(160, 21);
		((Control)textBoxFormatMask).TabIndex = 2;
		((Control)textBoxFormatMask).TextChanged += TextBoxFormatMask_TextChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)textBoxFormatMask);
		((Control)this).Controls.Add((Control)(object)lbFormatMask);
		((Control)this).Controls.Add((Control)(object)listBoxFormats);
		((Control)this).Margin = new Padding(3);
		((Control)this).Name = "StiCustomEditor";
		((Control)this).Size = new Size(256, 184);
		((Control)this).ResumeLayout(false);
	}
}
