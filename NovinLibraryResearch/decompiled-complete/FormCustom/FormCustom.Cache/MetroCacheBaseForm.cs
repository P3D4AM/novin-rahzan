using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Metro;

namespace FormCustom.Cache;

public class MetroCacheBaseForm : MetroForm
{
	private IContainer components;

	public CacheContext Context { get; private set; }

	public string ContextGuid => Context?.ContextGuid;

	public MetroCacheBaseForm()
	{
	}

	public MetroCacheBaseForm(string contextGuid)
	{
		Context = new CacheContext(contextGuid);
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(391, 411);
		((Control)this).Name = "MetroCacheBaseForm";
		((Control)this).Text = "MetroCacheBaseForm";
		((Control)this).ResumeLayout(false);
	}
}
