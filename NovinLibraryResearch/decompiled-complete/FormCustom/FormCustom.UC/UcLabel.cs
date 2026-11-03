using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.UC;

public class UcLabel : UserControl
{
	public delegate void OnClickEventHandler(object sender, EventArgs e);

	private Color _backColor = Color.Transparent;

	private IContainer components;

	private SplitContainer splitContainer1;

	private Label lblTitle;

	private Label lblValue;

	public string Title
	{
		get
		{
			return ((Control)lblTitle).Text;
		}
		set
		{
			((Control)lblTitle).Text = value;
		}
	}

	public string Value
	{
		get
		{
			return ((Control)lblValue).Text;
		}
		set
		{
			((Control)lblValue).Text = value;
		}
	}

	public Color TitleForeColor
	{
		get
		{
			return ((Control)lblTitle).ForeColor;
		}
		set
		{
			((Control)lblTitle).ForeColor = value;
		}
	}

	public Color ValueForeColor
	{
		get
		{
			return ((Control)lblValue).ForeColor;
		}
		set
		{
			((Control)lblValue).ForeColor = value;
		}
	}

	public Color BackColor
	{
		get
		{
			return _backColor;
		}
		set
		{
			_backColor = value;
			((Control)splitContainer1).BackColor = value;
		}
	}

	public ContentAlignment TitleTextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return lblTitle.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			lblTitle.TextAlign = value;
		}
	}

	public ContentAlignment ValueTextAlign
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return lblValue.TextAlign;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			lblValue.TextAlign = value;
		}
	}

	public event OnClickEventHandler OnClick;

	public UcLabel()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void lblValue_Click(object sender, EventArgs e)
	{
		this.OnClick?.Invoke(this, e);
	}

	private void lblValue_MouseEnter(object sender, EventArgs e)
	{
		try
		{
			if (this.OnClick != null)
			{
				Label obj = lblValue;
				Cursor cursor = (((Control)lblTitle).Cursor = Cursors.Hand);
				((Control)obj).Cursor = cursor;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblValue_MouseEnter", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcLabel.cs", 46);
		}
	}

	private void lblValue_MouseLeave(object sender, EventArgs e)
	{
		try
		{
			if (this.OnClick != null)
			{
				Label obj = lblValue;
				Cursor cursor = (((Control)lblTitle).Cursor = Cursors.Default);
				((Control)obj).Cursor = cursor;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "lblValue_MouseLeave", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\UcLabel.cs", 55);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		splitContainer1 = new SplitContainer();
		lblTitle = new Label();
		lblValue = new Label();
		((ISupportInitialize)splitContainer1).BeginInit();
		((Control)splitContainer1.Panel1).SuspendLayout();
		((Control)splitContainer1.Panel2).SuspendLayout();
		((Control)splitContainer1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)splitContainer1).BackColor = Color.Transparent;
		splitContainer1.Dock = (DockStyle)5;
		((Control)splitContainer1).Location = new Point(0, 0);
		((Control)splitContainer1).Name = "splitContainer1";
		((ScrollableControl)splitContainer1.Panel1).AutoScroll = true;
		((Control)splitContainer1.Panel1).Controls.Add((Control)(object)lblTitle);
		((Control)splitContainer1.Panel1).RightToLeft = (RightToLeft)1;
		((ScrollableControl)splitContainer1.Panel2).AutoScroll = true;
		((Control)splitContainer1.Panel2).Controls.Add((Control)(object)lblValue);
		((Control)splitContainer1.Panel2).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).RightToLeft = (RightToLeft)1;
		((Control)splitContainer1).Size = new Size(162, 26);
		splitContainer1.SplitterDistance = 55;
		((Control)splitContainer1).TabIndex = 0;
		((Control)lblTitle).BackColor = Color.Transparent;
		((Control)lblTitle).Dock = (DockStyle)5;
		((Control)lblTitle).ForeColor = SystemColors.AppWorkspace;
		((Control)lblTitle).Location = new Point(0, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(55, 26);
		((Control)lblTitle).TabIndex = 1;
		((Control)lblTitle).Text = "Title:";
		lblTitle.TextAlign = (ContentAlignment)64;
		((Control)lblTitle).Click += lblValue_Click;
		((Control)lblTitle).MouseEnter += lblValue_MouseEnter;
		((Control)lblTitle).MouseLeave += lblValue_MouseLeave;
		((Control)lblValue).BackColor = Color.Transparent;
		((Control)lblValue).Dock = (DockStyle)5;
		((Control)lblValue).ForeColor = SystemColors.ActiveCaption;
		((Control)lblValue).Location = new Point(0, 0);
		((Control)lblValue).Name = "lblValue";
		((Control)lblValue).Size = new Size(103, 26);
		((Control)lblValue).TabIndex = 0;
		((Control)lblValue).Text = "1234";
		lblValue.TextAlign = (ContentAlignment)16;
		((Control)lblValue).Click += lblValue_Click;
		((Control)lblValue).MouseEnter += lblValue_MouseEnter;
		((Control)lblValue).MouseLeave += lblValue_MouseLeave;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(9f, 23f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)splitContainer1);
		((Control)this).Font = new Font("B Titr", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcLabel";
		((Control)this).Size = new Size(162, 26);
		((Control)splitContainer1.Panel1).ResumeLayout(false);
		((Control)splitContainer1.Panel2).ResumeLayout(false);
		((ISupportInitialize)splitContainer1).EndInit();
		((Control)splitContainer1).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
