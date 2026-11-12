using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.TextFormats.Design;

[ToolboxItem(false)]
public class StiTimeEditor : StiFormatEditor
{
	private StiTimeFormatService service;

	private IContainer components;

	private StiListBoxV2 listBoxFormats;

	public StiTimeEditor(StiTimeFormatService service, StiDesignerControl designer)
		: base(designer)
	{
		this.service = service;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			((ListControl)listBoxFormats).SelectedIndex = Build(service, "time", ((ListBox)listBoxFormats).Items);
		}
	}

	private void ListBoxFormats_SelectedIndexChanged(object sender, EventArgs e)
	{
		service.StringFormat = base.Formats[((ListControl)listBoxFormats).SelectedIndex];
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
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		listBoxFormats = new StiListBoxV2();
		((Control)this).SuspendLayout();
		((Control)listBoxFormats).Anchor = (AnchorStyles)15;
		((Control)listBoxFormats).Location = new Point(0, 12);
		((Control)listBoxFormats).Margin = new Padding(2);
		((Control)listBoxFormats).Name = "listBoxFormats";
		((Control)listBoxFormats).Size = new Size(162, 188);
		((Control)listBoxFormats).TabIndex = 0;
		((ListBox)listBoxFormats).SelectedIndexChanged += ListBoxFormats_SelectedIndexChanged;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)listBoxFormats);
		((Control)this).Margin = new Padding(1);
		((Control)this).Name = "StiTimeEditor";
		((Control)this).Size = new Size(162, 200);
		((Control)this).ResumeLayout(false);
	}
}
