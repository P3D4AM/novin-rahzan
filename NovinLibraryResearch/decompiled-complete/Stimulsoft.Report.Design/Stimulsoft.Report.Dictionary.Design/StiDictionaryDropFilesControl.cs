using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Panels;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dictionary.Design;

[Browsable(false)]
[ToolboxItem(false)]
public class StiDictionaryDropFilesControl : UserControl
{
	private enum PositionState
	{
		None,
		Top,
		Bottom
	}

	private PositionState state;

	private Brush brushDefault;

	private Brush brushSelected;

	private string textDataSourceNew;

	private string textResourceNew;

	private IContainer components;

	public StiDictionaryDropFilesControl()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		InitializeComponent();
		brushDefault = (Brush)new SolidBrush(StiUX.IsDark ? StiUX.Background : Color.FromArgb(255, 213, 225, 242));
		brushSelected = (Brush)new SolidBrush(StiUX.IsDark ? StiUX.Accent : Color.FromArgb(255, 163, 189, 227));
		Localize();
		((Control)this).Paint += new PaintEventHandler(OnPaint);
	}

	public void Localize()
	{
		textDataSourceNew = StiLocalization.Get("MainMenu", "menuEditDataSourceNew").Replace("...", string.Empty);
		textResourceNew = StiLocalization.Get("MainMenu", "menuEditResourceNew").Replace("&", string.Empty).Replace("...", string.Empty);
	}

	public void Reset()
	{
		state = PositionState.None;
		((Control)this).Invalidate();
	}

	public void ProcessDragOver(DragEventArgs e)
	{
		int num = ((Control)this).Height / 2;
		Rectangle rectangle = new Rectangle(20, 20, ((Control)this).Width - 40, num - 30);
		Rectangle rectangle2 = new Rectangle(20, num + 10, ((Control)this).Width - 40, num - 30);
		Point pt = ((Control)this).PointToClient(new Point(e.X, e.Y));
		PositionState positionState = PositionState.None;
		if (rectangle.Contains(pt))
		{
			positionState = PositionState.Top;
		}
		else if (rectangle2.Contains(pt))
		{
			positionState = PositionState.Bottom;
		}
		if (state != positionState)
		{
			state = positionState;
			((Control)this).Invalidate();
		}
	}

	public async Task ProcessDragDropAsync(StiDictionaryPanelService service, DragEventArgs e)
	{
		int num = ((Control)this).Height / 2;
		Rectangle rectangle = new Rectangle(20, 20, ((Control)this).Width - 40, num - 30);
		Rectangle rectangle2 = new Rectangle(20, num + 10, ((Control)this).Width - 40, num - 30);
		Point pt = ((Control)this).PointToClient(new Point(e.X, e.Y));
		if (rectangle.Contains(pt))
		{
			await service.ProcessDragDropAsync(e);
		}
		else if (rectangle2.Contains(pt))
		{
			AddResources(service, e);
		}
	}

	private void AddResources(StiDictionaryPanelService service, DragEventArgs e)
	{
		if (!(e.Data.GetData(DataFormats.FileDrop) is string[] array) || array.Length == 0)
		{
			return;
		}
		StiDesignerControl designer = service.Designer;
		designer.UndoRedoSave(StiLocalization.Get("FormDictionaryDesigner", "ResourceNew"));
		StiResource stiResource = null;
		string[] array2 = array;
		foreach (string text in array2)
		{
			string extension = Path.GetExtension(text).ToLowerInvariant();
			StiResourceType? typeFromExtension = StiResourceTypeHelper.GetTypeFromExtension(extension);
			if (typeFromExtension.HasValue)
			{
				string newResourceName = GetNewResourceName(text, service.Designer.Report);
				StiResource stiResource2 = new StiResource(newResourceName, newResourceName, typeFromExtension.GetValueOrDefault(), File.ReadAllBytes(text));
				designer.Report.Dictionary.Resources.Add(stiResource2);
				stiResource = stiResource2;
			}
		}
		if (stiResource != null)
		{
			service.BuildDictionary();
			if (((TreeView)service.DictionaryControl.tvData).SelectedNode == null || !(((TreeView)service.DictionaryControl.tvData).SelectedNode.Tag is StiResource))
			{
				service.DictionaryControl.SelectNodeByTag("Resources");
			}
			if (((TreeView)service.DictionaryControl.tvData).SelectedNode != null)
			{
				service.DictionaryControl.ExpandTreeNode(((TreeView)service.DictionaryControl.tvData).SelectedNode);
			}
			service.DictionaryControl.SelectNodeByTag(stiResource);
		}
	}

	public static string GetNewResourceName(string file, StiReport report)
	{
		string text = Path.GetFileNameWithoutExtension(file).Normalize();
		if (StiNameCreation.IsResourceNameExists(report, text))
		{
			text = StiNameCreation.CreateResourceName(report, text);
			if (StiNameCreation.IsResourceNameExists(report, text))
			{
				text = StiNameCreation.CreateResourceName(report, text);
			}
		}
		return text;
	}

	private void OnPaint(object sender, PaintEventArgs e)
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		if (((Control)this).Width < 41 || ((Control)this).Height < 81)
		{
			return;
		}
		int num = ((Control)this).Height / 2;
		RectangleF rectangleF = new RectangleF(20f, 20f, ((Control)this).Width - 40, num - 30);
		RectangleF rectangleF2 = new RectangleF(20f, num + 10, ((Control)this).Width - 40, num - 30);
		Graphics graphics = e.Graphics;
		graphics.FillRectangle((state == PositionState.Top) ? brushSelected : brushDefault, rectangleF);
		graphics.FillRectangle((state == PositionState.Bottom) ? brushSelected : brushDefault, rectangleF2);
		graphics.CompositingQuality = (CompositingQuality)2;
		graphics.SmoothingMode = (SmoothingMode)2;
		Font val = new Font(((Control)this).Font.FontFamily, 14f);
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				val2.Alignment = (StringAlignment)1;
				val2.LineAlignment = (StringAlignment)1;
				SolidBrush val3 = new SolidBrush(StiUX.IsDark ? StiUX.InputForeground : Color.Black);
				try
				{
					graphics.DrawString(textDataSourceNew, val, (Brush)(object)val3, rectangleF, val2);
					graphics.DrawString(textResourceNew, val, (Brush)(object)val3, rectangleF2, val2);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
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
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Name = "StiDictionaryDropFilesControl";
		((Control)this).Size = new Size(100, 100);
		((Control)this).ResumeLayout(false);
	}
}
