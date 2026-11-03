using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EntityCache.Business.Accounting.Note;
using FormCustom.Properties;
using PacketParser.Entities;

namespace FormCustom.UC.Dashboard;

public class UcNote : UserControl
{
	private IContainer components;

	private PanelEx pnlHeader;

	private Label lblTitle;

	private Label label5;

	private Label lblPriority;

	private Label lblSarresid;

	private Label lblDesc;

	private Label label7;

	private PictureBox picRead;

	private PictureBox picClose;

	public NoteBusiness _note { get; private set; }

	public event Action<Guid> OnNoteSelected;

	public void SetNote(NoteBusiness note)
	{
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (note != null)
			{
				_note = note;
				((Control)lblSarresid).Text = note.TimeSarResid + " - " + note.DateSarResidSh;
				((Control)lblPriority).Text = note.PirorityName;
				((Control)lblTitle).Text = note.Title;
				((Control)lblDesc).Text = note.Text;
				SetLabelColor(Color.White);
				Size size = TextRenderer.MeasureText(note.Text, ((Control)lblDesc).Font);
				_ = size.Width;
				if (note.Text.Length > 80)
				{
					size.Height += size.Height;
					((Control)this).Height = size.Height + note.Text.Length / 2;
				}
				if (note.Pirority != 0 && note.Pirority != NotePirority.Mohem && note.Pirority != NotePirority.Mamoli)
				{
					note.Pirority = NotePirority.Mamoli;
				}
				switch (note.Pirority)
				{
				case NotePirority.Zarori:
					SetColor(Color.Yellow);
					SetLabelColor(Color.Black);
					break;
				case NotePirority.Mohem:
					SetColor(Color.YellowGreen);
					SetLabelColor(Color.Black);
					break;
				case NotePirority.Mamoli:
					SetColor(Color.FromArgb(241, 245, 254));
					SetLabelColor(Color.Black);
					break;
				default:
					SetLabelColor(Color.Gray);
					SetColor(Color.White);
					break;
				}
				ToolTip val = new ToolTip();
				val.SetToolTip((Control)(object)picClose, "تبدیل به یادداشت بسته شده");
				val.SetToolTip((Control)(object)picRead, "تبدیل به یادداشت خوانده شده");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetNote", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcNote.cs", 60);
		}
	}

	private void SetColor(Color color)
	{
		try
		{
			pnlHeader.Style.BackColor1.Color = color;
			pnlHeader.Style.BackColor2.Color = color;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcNote.cs", 69);
		}
	}

	private void SetLabelColor(Color color)
	{
		try
		{
			Label obj = lblDesc;
			Color foreColor = (((Control)lblPriority).ForeColor = color);
			((Control)obj).ForeColor = foreColor;
			Label obj2 = lblSarresid;
			foreColor = (((Control)lblTitle).ForeColor = color);
			((Control)obj2).ForeColor = foreColor;
			Label obj3 = label5;
			foreColor = (((Control)label7).ForeColor = color);
			((Control)obj3).ForeColor = foreColor;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetLabelColor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcNote.cs", 79);
		}
	}

	public UcNote()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void lblDesc_Click(object sender, EventArgs e)
	{
		this.OnNoteSelected?.Invoke(_note?.Guid ?? Guid.Empty);
	}

	private async void picClose_Click(object sender, EventArgs e)
	{
		try
		{
			if (_note != null)
			{
				_note.Status = NoteStatus.Closed;
				await _note.SaveAsync(EnSystem.UserInterface, send2Server: true, default(CancellationToken), awaitOnSemaphore: true, await4Send: false, Priority.Low, 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picClose_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcNote.cs", 95);
		}
	}

	private async void picRead_Click(object sender, EventArgs e)
	{
		try
		{
			if (_note != null)
			{
				_note.Status = NoteStatus.Read;
				await _note.SaveAsync(EnSystem.UserInterface, send2Server: true, default(CancellationToken), awaitOnSemaphore: true, await4Send: false, Priority.Low, 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "picRead_Click", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UC\\Dashboard\\UcNote.cs", 105);
		}
	}

	protected override void Dispose(bool disposing)
	{
		components?.Dispose();
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Expected O, but got Unknown
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Expected O, but got Unknown
		//IL_05ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Expected O, but got Unknown
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0979: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		pnlHeader = new PanelEx();
		picRead = new PictureBox();
		picClose = new PictureBox();
		lblDesc = new Label();
		label7 = new Label();
		lblTitle = new Label();
		label5 = new Label();
		lblPriority = new Label();
		lblSarresid = new Label();
		((Control)pnlHeader).SuspendLayout();
		((ISupportInitialize)picRead).BeginInit();
		((ISupportInitialize)picClose).BeginInit();
		((Control)this).SuspendLayout();
		pnlHeader.CanvasColor = Color.Transparent;
		((Control)pnlHeader).Controls.Add((Control)(object)picRead);
		((Control)pnlHeader).Controls.Add((Control)(object)picClose);
		((Control)pnlHeader).Controls.Add((Control)(object)lblDesc);
		((Control)pnlHeader).Controls.Add((Control)(object)label7);
		((Control)pnlHeader).Controls.Add((Control)(object)lblTitle);
		((Control)pnlHeader).Controls.Add((Control)(object)label5);
		((Control)pnlHeader).Controls.Add((Control)(object)lblPriority);
		((Control)pnlHeader).Controls.Add((Control)(object)lblSarresid);
		pnlHeader.DisabledBackColor = Color.Empty;
		((Control)pnlHeader).Dock = (DockStyle)5;
		((Control)pnlHeader).Location = new Point(0, 0);
		((Control)pnlHeader).Margin = new Padding(4, 5, 4, 5);
		((Control)pnlHeader).Name = "pnlHeader";
		((Control)pnlHeader).Size = new Size(421, 59);
		pnlHeader.Style.BackColor1.Color = Color.White;
		pnlHeader.Style.BackColor2.Color = Color.White;
		pnlHeader.Style.Border = eBorderType.SingleLine;
		pnlHeader.Style.BorderColor.Color = Color.White;
		pnlHeader.Style.BorderWidth = 0;
		pnlHeader.Style.CornerDiameter = 10;
		pnlHeader.Style.CornerType = eCornerType.Rounded;
		((Control)pnlHeader).TabIndex = 143;
		((Control)picRead).Anchor = (AnchorStyles)6;
		((Control)picRead).Cursor = Cursors.Hand;
		picRead.Image = (Image)(object)FormCustom.Properties.Resources.read;
		((Control)picRead).Location = new Point(4, 33);
		((Control)picRead).Name = "picRead";
		((Control)picRead).Size = new Size(27, 23);
		picRead.SizeMode = (PictureBoxSizeMode)4;
		picRead.TabIndex = 16;
		picRead.TabStop = false;
		((Control)picRead).Click += picRead_Click;
		((Control)picClose).Anchor = (AnchorStyles)6;
		((Control)picClose).Cursor = Cursors.Hand;
		picClose.Image = (Image)(object)FormCustom.Properties.Resources.closedNote;
		((Control)picClose).Location = new Point(4, 7);
		((Control)picClose).Name = "picClose";
		((Control)picClose).Size = new Size(27, 23);
		picClose.SizeMode = (PictureBoxSizeMode)4;
		picClose.TabIndex = 15;
		picClose.TabStop = false;
		((Control)picClose).Click += picClose_Click;
		((Control)lblDesc).Anchor = (AnchorStyles)15;
		((Control)lblDesc).BackColor = Color.Transparent;
		((Control)lblDesc).Cursor = Cursors.Hand;
		((Control)lblDesc).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblDesc).ForeColor = Color.Gray;
		((Control)lblDesc).Location = new Point(38, 36);
		((Control)lblDesc).Margin = new Padding(4, 0, 4, 0);
		((Control)lblDesc).Name = "lblDesc";
		((Control)lblDesc).Size = new Size(332, 23);
		((Control)lblDesc).TabIndex = 14;
		((Control)lblDesc).Text = "---";
		((Control)lblDesc).Click += lblDesc_Click;
		((Control)label7).Anchor = (AnchorStyles)9;
		((Control)label7).AutoSize = true;
		((Control)label7).BackColor = Color.Transparent;
		((Control)label7).Cursor = Cursors.Hand;
		((Control)label7).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label7).ForeColor = Color.Gray;
		((Control)label7).Location = new Point(378, 38);
		((Control)label7).Margin = new Padding(4, 0, 4, 0);
		((Control)label7).Name = "label7";
		((Control)label7).Size = new Size(30, 17);
		((Control)label7).TabIndex = 13;
		((Control)label7).Text = "[شرح]:";
		label7.TextAlign = (ContentAlignment)16;
		((Control)label7).Click += lblDesc_Click;
		((Control)lblTitle).Anchor = (AnchorStyles)13;
		((Control)lblTitle).BackColor = Color.Transparent;
		((Control)lblTitle).Cursor = Cursors.Hand;
		((Control)lblTitle).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblTitle).ForeColor = Color.Gray;
		((Control)lblTitle).Location = new Point(38, 20);
		((Control)lblTitle).Margin = new Padding(4, 0, 4, 0);
		((Control)lblTitle).Name = "lblTitle";
		((Control)lblTitle).Size = new Size(287, 20);
		((Control)lblTitle).TabIndex = 12;
		((Control)lblTitle).Text = "---";
		lblTitle.TextAlign = (ContentAlignment)16;
		((Control)lblTitle).Click += lblDesc_Click;
		((Control)label5).Anchor = (AnchorStyles)9;
		((Control)label5).AutoSize = true;
		((Control)label5).BackColor = Color.Transparent;
		((Control)label5).Cursor = Cursors.Hand;
		((Control)label5).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)label5).ForeColor = Color.Gray;
		((Control)label5).Location = new Point(333, 22);
		((Control)label5).Margin = new Padding(4, 0, 4, 0);
		((Control)label5).Name = "label5";
		((Control)label5).Size = new Size(75, 17);
		((Control)label5).TabIndex = 11;
		((Control)label5).Text = "[عنوان یادداشت]:";
		label5.TextAlign = (ContentAlignment)16;
		((Control)label5).Click += lblDesc_Click;
		((Control)lblPriority).BackColor = Color.Transparent;
		((Control)lblPriority).Cursor = Cursors.Hand;
		((Control)lblPriority).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblPriority).ForeColor = Color.Gray;
		((Control)lblPriority).Location = new Point(154, 1);
		((Control)lblPriority).Margin = new Padding(4, 0, 4, 0);
		((Control)lblPriority).Name = "lblPriority";
		((Control)lblPriority).Size = new Size(113, 20);
		((Control)lblPriority).TabIndex = 10;
		((Control)lblPriority).Text = "---";
		lblPriority.TextAlign = (ContentAlignment)16;
		((Control)lblPriority).Click += lblDesc_Click;
		((Control)lblSarresid).Anchor = (AnchorStyles)13;
		((Control)lblSarresid).BackColor = Color.Transparent;
		((Control)lblSarresid).Cursor = Cursors.Hand;
		((Control)lblSarresid).Font = new Font("B Yekan", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)lblSarresid).ForeColor = Color.Gray;
		((Control)lblSarresid).Location = new Point(275, 1);
		((Control)lblSarresid).Margin = new Padding(4, 0, 4, 0);
		((Control)lblSarresid).Name = "lblSarresid";
		((Control)lblSarresid).Size = new Size(133, 20);
		((Control)lblSarresid).TabIndex = 8;
		((Control)lblSarresid).Text = "---";
		lblSarresid.TextAlign = (ContentAlignment)16;
		((Control)lblSarresid).Click += lblDesc_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Transparent;
		((Control)this).Controls.Add((Control)(object)pnlHeader);
		((Control)this).Font = new Font("B Yekan", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)178);
		((Control)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "UcNote";
		((Control)this).RightToLeft = (RightToLeft)1;
		((Control)this).Size = new Size(421, 59);
		((Control)pnlHeader).ResumeLayout(false);
		((Control)pnlHeader).PerformLayout();
		((ISupportInitialize)picRead).EndInit();
		((ISupportInitialize)picClose).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
