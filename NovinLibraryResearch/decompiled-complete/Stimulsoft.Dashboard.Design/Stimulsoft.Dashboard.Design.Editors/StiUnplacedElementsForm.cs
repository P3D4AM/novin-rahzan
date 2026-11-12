using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Design.Controls;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Dashboard.Helper;
using Stimulsoft.Report.Help;
using Stimulsoft.Report.Surface.Editor;

namespace Stimulsoft.Dashboard.Design.Editors;

public class StiUnplacedElementsForm : StiElementEditorForm, IStiUnplacedElementsForm
{
	private StiDesignerControl designer;

	private IContainer components;

	private ToolTip toolTip;

	private StiElementListBox listBoxElements;

	internal override StiDesignerControl Designer => designer;

	internal StiElementListBox ElementListBox => listBoxElements;

	public override bool AllowResize => false;

	public StiUnplacedElementsForm()
		: this(null)
	{
	}

	public StiUnplacedElementsForm(List<IStiElement> elements)
	{
		InitializeComponent();
		if (StiOptions.Appearance.ShowFormsHelp)
		{
			((Form)this).HelpButton = true;
		}
		designer = elements.FirstOrDefault().Report.Designer.DesignerControl as StiDesignerControl;
		listBoxElements.Editor = this;
		foreach (StiComponent element in elements)
		{
			((ListBox)listBoxElements).Items.Add((object)element);
		}
		LocalizeInternal();
		DoTheming();
		DoScaling();
		((Control)listBoxElements).DragLeave += ListBoxElements_DragLeave;
		StiUnplacedElementsHelper.ElementsForm = this;
	}

	public void RemoveElement(StiComponent component)
	{
		if (component != null)
		{
			((ListBox)listBoxElements).Items.Remove((object)component);
			if (((ListBox)listBoxElements).Items.Count == 0)
			{
				((Form)this).Close();
			}
		}
	}

	public override void Localize()
	{
		LocalizeInternal();
		((Control)listBoxElements).Invalidate();
	}

	private void LocalizeInternal()
	{
		((Control)this).Text = Loc.Get("Report", "Components");
	}

	private void DoTheming()
	{
		if (StiUX.IsDark)
		{
			((Control)this).BackColor = StiUX.BackgroundDark;
		}
	}

	private void DoScaling()
	{
	}

	protected override void OnGlobalElementChanged(object sender, EventArgs e)
	{
		if (sender == base.Element)
		{
			ReadPropertiesFromElement();
		}
	}

	private void ListBoxElements_DragLeave(object sender, EventArgs e)
	{
		if (Designer.PageView.DraggingComponent == null)
		{
			Designer.PageView.DraggingComponent = ((ListBox)listBoxElements).SelectedItem as StiComponent;
		}
	}

	private void This_HelpButtonClicked(object sender, CancelEventArgs e)
	{
		StiHelpProvider.ShowHelpViewer("user-manual/dashboards.htm");
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		base.OnFormClosed(e);
		listBoxElements.DisposeCache();
		StiUnplacedElementsHelper.ElementsForm = null;
		designer?.RibbonDesignerUserControl.RefreshPage();
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		toolTip = new ToolTip(components);
		listBoxElements = new StiElementListBox();
		((Control)this).SuspendLayout();
		((Control)listBoxElements).AllowDrop = true;
		((Control)listBoxElements).Dock = (DockStyle)5;
		((Control)listBoxElements).Location = new Point(32, 29);
		((Control)listBoxElements).Margin = new Padding(5);
		((Control)listBoxElements).Name = "listBoxFilters";
		((Control)listBoxElements).Size = new Size(795, 874);
		((Control)listBoxElements).TabIndex = 1;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(16f, 31f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(859, 932);
		((Control)this).Controls.Add((Control)(object)listBoxElements);
		((Control)this).Name = "StiUnplacedElementsForm";
		((Control)this).Padding = new Padding(32, 29, 32, 29);
		((Control)this).Text = "Elements";
		((Form)this).HelpButtonClicked += This_HelpButtonClicked;
		((Control)this).ResumeLayout(false);
	}

	DialogResult IStiUnplacedElementsForm.ShowDialog()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return ((Form)this).ShowDialog();
	}
}
