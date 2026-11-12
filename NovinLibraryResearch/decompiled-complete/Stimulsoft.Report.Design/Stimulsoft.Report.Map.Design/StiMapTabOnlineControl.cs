using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design.Controls;
using Stimulsoft.Report.Maps;

namespace Stimulsoft.Report.Map.Design;

[ToolboxItem(false)]
public class StiMapTabOnlineControl : UserControl
{
	private StiMapDesignerForm designerForm;

	private StiMap map;

	private IContainer components;

	private Label labelLatitude;

	public StiMeterListBox listBoxLatitude;

	private Label labelLongitude;

	public StiMeterListBox listBoxLongitude;

	public StiMapTabOnlineControl(StiMapDesignerForm designerForm, StiMap map)
	{
		InitializeComponent();
		if (!((Component)this).DesignMode)
		{
			this.designerForm = designerForm;
			this.map = map;
			listBoxLatitude.Designer = designerForm.DesignerControl;
			listBoxLongitude.Designer = designerForm.DesignerControl;
			if (!string.IsNullOrEmpty(this.map.Latitude))
			{
				listBoxLatitude.AddItem(new StiMeterItem(this.map.Latitude));
			}
			if (!string.IsNullOrEmpty(this.map.Longitude))
			{
				listBoxLongitude.AddItem(new StiMeterItem(this.map.Longitude));
			}
			listBoxLatitude.ElementsChanged += ListBoxLatitude_ElementsChanged;
			listBoxLongitude.ElementsChanged += ListBoxLongitude_ElementsChanged;
			Localize();
		}
	}

	private void Localize()
	{
		((Control)labelLatitude).Text = StiLocalization.Get("PropertyMain", "Latitude");
		((Control)labelLongitude).Text = StiLocalization.Get("PropertyMain", "Longitude");
	}

	private void ListBoxLatitude_ElementsChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxLatitude).Items.Count == 0)
		{
			map.Latitude = string.Empty;
		}
		else
		{
			StiMeterItem stiMeterItem = ((ListBox)listBoxLatitude).Items[0] as StiMeterItem;
			map.Latitude = stiMeterItem.Text;
		}
		((Control)designerForm.DesignerControl).Refresh();
	}

	private void ListBoxLongitude_ElementsChanged(object sender, EventArgs e)
	{
		if (((ListBox)listBoxLongitude).Items.Count == 0)
		{
			map.Longitude = string.Empty;
		}
		else
		{
			StiMeterItem stiMeterItem = ((ListBox)listBoxLongitude).Items[0] as StiMeterItem;
			map.Longitude = stiMeterItem.Text;
		}
		((Control)designerForm.DesignerControl).Refresh();
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		labelLatitude = new Label();
		listBoxLatitude = new StiMeterListBox();
		labelLongitude = new Label();
		listBoxLongitude = new StiMeterListBox();
		((Control)this).SuspendLayout();
		((Control)labelLatitude).BackColor = Color.White;
		((Control)labelLatitude).ForeColor = Color.Black;
		((Control)labelLatitude).Location = new Point(9, 10);
		((Control)labelLatitude).Margin = new Padding(4, 0, 4, 0);
		((Control)labelLatitude).Name = "labelLatitude";
		((Control)labelLatitude).Size = new Size(250, 16);
		((Control)labelLatitude).TabIndex = 0;
		((Control)labelLatitude).Text = "Latitude";
		((Control)listBoxLatitude).AllowDrop = true;
		((Control)listBoxLatitude).Anchor = (AnchorStyles)13;
		((Control)listBoxLatitude).Location = new Point(12, 30);
		((Control)listBoxLatitude).Margin = new Padding(4, 4, 4, 4);
		((Control)listBoxLatitude).Name = "listBoxLatitude";
		((Control)listBoxLatitude).Size = new Size(276, 40);
		((Control)listBoxLatitude).TabIndex = 1;
		((Control)labelLongitude).BackColor = Color.White;
		((Control)labelLongitude).ForeColor = Color.Black;
		((Control)labelLongitude).Location = new Point(9, 80);
		((Control)labelLongitude).Margin = new Padding(4, 0, 4, 0);
		((Control)labelLongitude).Name = "labelLongitude";
		((Control)labelLongitude).Size = new Size(250, 16);
		((Control)labelLongitude).TabIndex = 2;
		((Control)labelLongitude).Text = "Longitude";
		((Control)listBoxLongitude).AllowDrop = true;
		((Control)listBoxLongitude).Anchor = (AnchorStyles)13;
		((Control)listBoxLongitude).Location = new Point(12, 100);
		((Control)listBoxLongitude).Margin = new Padding(4, 4, 4, 4);
		((Control)listBoxLongitude).Name = "listBoxLongitude";
		((Control)listBoxLongitude).Size = new Size(276, 40);
		((Control)listBoxLongitude).TabIndex = 3;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Controls.Add((Control)(object)listBoxLongitude);
		((Control)this).Controls.Add((Control)(object)labelLongitude);
		((Control)this).Controls.Add((Control)(object)listBoxLatitude);
		((Control)this).Controls.Add((Control)(object)labelLatitude);
		((Control)this).Name = "StiMapTabOnlineControl";
		((Control)this).Size = new Size(300, 300);
		((Control)this).ResumeLayout(false);
	}
}
