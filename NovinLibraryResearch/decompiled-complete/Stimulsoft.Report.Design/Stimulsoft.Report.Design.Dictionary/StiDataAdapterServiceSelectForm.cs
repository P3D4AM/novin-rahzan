using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiDataAdapterServiceSelectForm : StiFormUX
{
	private class StiAdapterWrapper
	{
		public StiDataAdapterService service;

		public override string ToString()
		{
			return service.ServiceName;
		}

		public StiAdapterWrapper(StiDataAdapterService service)
		{
			this.service = service;
		}
	}

	private IStiDesignerBase Designer;

	private StiDataAdapterService selectedService;

	private IContainer components;

	private StiButtonUX btOk;

	private StiButtonUX btCancel;

	private ImageList ilDataAdapter;

	private StiListBox lbDataAdapter;

	private StiButtonPanelUX stiGroupLine1;

	public StiDataAdapterService SelectedService => selectedService;

	public StiDataAdapterServiceSelectForm(IStiDesignerBase designer)
	{
		((Form)(object)this).InitializeNetCoreFont();
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		if (!((Component)(object)this).DesignMode)
		{
			Designer = designer;
			Build(designer);
			Localize();
			StiSettings.Load();
			StiFormSettings.Load((Form)(object)this);
			((Form)this).ShowIcon = false;
		}
	}

	private void Localize()
	{
		((Control)this).Text = StiLocalization.Get("FormTitles", "DataAdapterServiceSelectForm");
		((Control)btOk).Text = StiLocalization.Get("Buttons", "Ok");
		((Control)btCancel).Text = StiLocalization.Get("Buttons", "Cancel");
	}

	private void btOk_Click(object sender, EventArgs e)
	{
		if (((ListBox)lbDataAdapter).SelectedItem != null)
		{
			selectedService = ((StiAdapterWrapper)((ListBox)lbDataAdapter).SelectedItem).service;
			((Form)this).DialogResult = (DialogResult)1;
		}
	}

	public void Build(IStiDesignerBase designer)
	{
		((ListBox)lbDataAdapter).Items.Clear();
		StiOptions.Services.DataAdapters.Where((StiDataAdapterService s) => s.ServiceEnabled).ToList().ForEach(delegate(StiDataAdapterService a)
		{
			((ListBox)lbDataAdapter).Items.Add((object)new StiAdapterWrapper(a));
		});
		if (((ListBox)lbDataAdapter).Items.Count > 0)
		{
			((ListControl)lbDataAdapter).SelectedIndex = 0;
		}
	}

	private void ThisClosed(object sender, EventArgs e)
	{
		StiFormSettings.Save((Form)(object)this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		components = new Container();
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiDataAdapterServiceSelectForm));
		btOk = new StiButtonUX();
		btCancel = new StiButtonUX();
		ilDataAdapter = new ImageList(components);
		lbDataAdapter = new StiListBox();
		stiGroupLine1 = new StiButtonPanelUX();
		((Control)stiGroupLine1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)btOk).Anchor = (AnchorStyles)10;
		((Control)btOk).Location = new Point(120, 12);
		((Control)btOk).Name = "btOk";
		((Control)btOk).Size = new Size(75, 24);
		((Control)btOk).TabIndex = 1;
		((Control)btOk).Text = "Ok";
		((ButtonBase)btOk).UseVisualStyleBackColor = true;
		((Control)btOk).Click += btOk_Click;
		((Control)btCancel).Anchor = (AnchorStyles)10;
		((Button)btCancel).DialogResult = (DialogResult)2;
		((Control)btCancel).Location = new Point(201, 12);
		((Control)btCancel).Name = "btCancel";
		((Control)btCancel).Size = new Size(75, 24);
		((Control)btCancel).TabIndex = 2;
		((Control)btCancel).Text = "Cancel";
		((ButtonBase)btCancel).UseVisualStyleBackColor = true;
		ilDataAdapter.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("ilDataAdapter.ImageStream");
		ilDataAdapter.TransparentColor = Color.Magenta;
		ilDataAdapter.Images.SetKeyName(0, "");
		((Control)lbDataAdapter).Anchor = (AnchorStyles)15;
		((ListBox)lbDataAdapter).BorderStyle = (BorderStyle)0;
		lbDataAdapter.ImageList = ilDataAdapter;
		((ListBox)lbDataAdapter).IntegralHeight = false;
		((ListBox)lbDataAdapter).ItemHeight = 18;
		((Control)lbDataAdapter).Location = new Point(12, 12);
		((Control)lbDataAdapter).Name = "lbDataAdapter";
		((Control)lbDataAdapter).Size = new Size(264, 364);
		((Control)lbDataAdapter).TabIndex = 0;
		lbDataAdapter.UseFirstImage = true;
		((Control)lbDataAdapter).DoubleClick += btOk_Click;
		((Control)stiGroupLine1).BackColor = Color.White;
		((Control)stiGroupLine1).Controls.Add((Control)(object)btCancel);
		((Control)stiGroupLine1).Controls.Add((Control)(object)btOk);
		((Control)stiGroupLine1).Dock = (DockStyle)2;
		stiGroupLine1.LinePosition = StiLinePosition.Top;
		stiGroupLine1.LineStyle = StiLineStyle.Dot;
		((Control)stiGroupLine1).Location = new Point(0, 392);
		((Control)stiGroupLine1).Name = "stiGroupLine1";
		((Control)stiGroupLine1).Size = new Size(288, 46);
		((Control)stiGroupLine1).TabIndex = 3;
		((Form)this).AcceptButton = (IButtonControl)(object)btOk;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Form)this).CancelButton = (IButtonControl)(object)btCancel;
		((Form)this).ClientSize = new Size(288, 438);
		((Control)this).Controls.Add((Control)(object)stiGroupLine1);
		((Control)this).Controls.Add((Control)(object)lbDataAdapter);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "StiDataAdapterServiceSelectForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)4;
		((Control)this).Text = "Data Adapter Select";
		((Form)this).Closed += ThisClosed;
		((Control)stiGroupLine1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
