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
public class StiFormatEditor : UserControl
{
	private IContainer components;

	protected StiStringCollection Formats { get; } = new StiStringCollection();


	internal StiDesignerControl Designer { get; private set; }

	public event EventHandler UpdateSample;

	public StiFormatEditor()
	{
		InitializeComponent();
	}

	public StiFormatEditor(StiDesignerControl designer)
	{
		InitializeComponent();
		Designer = designer;
	}

	protected virtual int Build(StiFormatService service, string category, ObjectCollection items)
	{
		CultureInfo cultureInfo = null;
		try
		{
			if (!string.IsNullOrWhiteSpace(Designer?.Report?.Culture))
			{
				cultureInfo = Thread.CurrentThread.CurrentCulture;
				Thread.CurrentThread.CurrentCulture = new CultureInfo(Designer.Report.Culture);
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
						Formats.Add(text2.Substring(1));
						string str = "*" + service.Format(text2.Substring(1), service.Sample);
						str = Process(text2, str);
						items.Add((object)str);
					}
					else
					{
						Formats.Add(text2);
						string str2 = service.Format(text2, service.Sample);
						str2 = Process(text2, str2);
						items.Add((object)str2);
					}
				}
			}
			foreach (string format in Formats)
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

	private string Process(string key, string str)
	{
		if (key.StartsWith("*"))
		{
			key = key.Substring(1);
		}
		string text = Loc.Get("DatePickerRanges", "Year");
		string text2 = Loc.Get("DatePickerRanges", "Quarter");
		string text3 = Loc.Get("DatePickerRanges", "Index");
		return key switch
		{
			"Q" => str + " - (" + text2 + ")", 
			"QI" => str + " - (" + text2 + " " + text3 + ")", 
			"YQ" => str + " - (" + text + "-" + text2 + ")", 
			"YQI" => str + " - (" + text + "-" + text2 + " " + text3 + ")", 
			_ => str, 
		};
	}

	public virtual void ApplyChanges()
	{
	}

	protected void InvokeUpdateSample()
	{
		this.UpdateSample?.Invoke(this, EventArgs.Empty);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Name = "StiFormatEditor";
		((Control)this).ResumeLayout(false);
	}
}
