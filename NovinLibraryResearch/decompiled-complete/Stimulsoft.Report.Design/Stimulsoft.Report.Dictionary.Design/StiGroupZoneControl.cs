using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report.Design.Controls;

namespace Stimulsoft.Report.Dictionary.Design;

[ToolboxItem(false)]
public class StiGroupZoneControl : StiZoneControl
{
	private IContainer components;

	private ImageList ilButton;

	private StiHitTestVisibleLabel labelColumn;

	public StiComboBox cbOrder;

	private StiHitTestVisibleLabel labelDirection;

	public StiTreeViewBox comboBoxColumn;

	public StiGroupZoneControl()
	{
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Localize();
		}
	}

	private void Localize()
	{
		((Control)labelColumn).Text = StiLocalization.Get("PropertyMain", "Column");
		((Control)labelDirection).Text = StiLocalization.Get("PropertyMain", "Direction");
	}

	private void LbCondition_MouseDown(object sender, MouseEventArgs e)
	{
		((Control)this).Focus();
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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		components = new Container();
		labelColumn = new StiHitTestVisibleLabel();
		comboBoxColumn = new StiTreeViewBox();
		ilButton = new ImageList(components);
		cbOrder = new StiComboBox();
		labelDirection = new StiHitTestVisibleLabel();
		((Control)this).SuspendLayout();
		((Control)labelColumn).BackColor = Color.Transparent;
		((Control)labelColumn).Location = new Point(8, 8);
		((Control)labelColumn).Margin = new Padding(2, 0, 2, 0);
		((Control)labelColumn).Name = "labelColumn";
		((Control)labelColumn).Size = new Size(136, 17);
		((Control)labelColumn).TabIndex = 14;
		((Control)labelColumn).Text = "Column";
		((Control)labelColumn).MouseDown += new MouseEventHandler(LbCondition_MouseDown);
		((Control)comboBoxColumn).BackColor = SystemColors.Window;
		comboBoxColumn.DropDownWidth = 400;
		comboBoxColumn.Flat = false;
		((Control)comboBoxColumn).ForeColor = SystemColors.ControlText;
		((Control)comboBoxColumn).Location = new Point(8, 25);
		((Control)comboBoxColumn).Margin = new Padding(2);
		comboBoxColumn.MaxDropDownHeight = 300;
		((Control)comboBoxColumn).Name = "comboBoxColumn";
		((Control)comboBoxColumn).Size = new Size(304, 21);
		((Control)comboBoxColumn).TabIndex = 0;
		ilButton.ColorDepth = (ColorDepth)24;
		ilButton.ImageSize = new Size(16, 16);
		ilButton.TransparentColor = Color.Magenta;
		((Control)cbOrder).Anchor = (AnchorStyles)13;
		((Control)cbOrder).BackColor = SystemColors.Window;
		cbOrder.DropDownStyle = (ComboBoxStyle)2;
		((Control)cbOrder).Location = new Point(320, 25);
		((Control)cbOrder).Margin = new Padding(2);
		((Control)cbOrder).Name = "cbOrder";
		((Control)cbOrder).Size = new Size(174, 21);
		((Control)cbOrder).TabIndex = 15;
		((Control)labelDirection).BackColor = Color.Transparent;
		((Control)labelDirection).Location = new Point(320, 8);
		((Control)labelDirection).Margin = new Padding(2, 0, 2, 0);
		((Control)labelDirection).Name = "labelDirection";
		((Control)labelDirection).Size = new Size(136, 17);
		((Control)labelDirection).TabIndex = 16;
		((Control)labelDirection).Text = "Direction";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).Controls.Add((Control)(object)labelDirection);
		((Control)this).Controls.Add((Control)(object)cbOrder);
		((Control)this).Controls.Add((Control)(object)comboBoxColumn);
		((Control)this).Controls.Add((Control)(object)labelColumn);
		((Control)this).Margin = new Padding(1);
		((Control)this).Name = "StiGroupZoneControl";
		base.ShowClose = true;
		((Control)this).Size = new Size(500, 58);
		((Control)this).ResumeLayout(false);
	}
}
