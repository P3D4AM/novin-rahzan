using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Services;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Components.ShapeTypes.Design;

public class StiShapeTypeServiceEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override void PaintValue(PaintValueEventArgs e)
	{
		if (e.Value is StiShapeTypeService stiShapeTypeService)
		{
			Rectangle rectangle = new Rectangle(e.Bounds.X + StiScale.I2, e.Bounds.Y + StiScale.I2, e.Bounds.Width - StiScale.I5, e.Bounds.Height - StiScale.I5);
			StiShape stiShape = (StiShape)e.Context.Instance;
			stiShapeTypeService.Paint(e.Graphics, stiShape, rectangle, 1f / stiShape.Size);
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	internal void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		StiShapeTypeService stiShapeTypeService = (StiShapeTypeService)value;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		if (!StiOptions.Services.Shapes.Any((StiShapeTypeService s) => s.ServiceEnabled))
		{
			return value;
		}
		ImageList val = new ImageList
		{
			ColorDepth = (ColorDepth)32,
			ImageSize = new Size(16, 16),
			TransparentColor = Color.Magenta
		};
		StiListBox stiListBox = new StiListBox();
		((ListBox)stiListBox).BorderStyle = (BorderStyle)0;
		((ListBox)stiListBox).ItemHeight = StiScale.YYI(19.0);
		((Control)stiListBox).Width = StiScale.XXI(200.0);
		stiListBox.ImageList = val;
		StiListBox stiListBox2 = stiListBox;
		((Control)stiListBox2).Height = ((ListBox)stiListBox2).ItemHeight * StiOptions.Services.Shapes.Count((StiShapeTypeService s) => s.ServiceEnabled) + StiScale.YYI(20.0);
		if (((Control)stiListBox2).Height > StiScale.YYI(300.0))
		{
			((Control)stiListBox2).Height = StiScale.YYI(300.0);
		}
		int selectedIndex = 0;
		int num = 0;
		foreach (StiShapeTypeService item in StiOptions.Services.Shapes.Where((StiShapeTypeService s) => s.ServiceEnabled))
		{
			StiServiceBitmapAttribute stiServiceBitmapAttribute = item.GetType().GetCustomAttributes(typeof(StiServiceBitmapAttribute), inherit: true).FirstOrDefault() as StiServiceBitmapAttribute;
			stiServiceBitmapAttribute.BitmapName = stiServiceBitmapAttribute.BitmapName.Substring(0, stiServiceBitmapAttribute.BitmapName.Length - 4) + ".png";
			val.Images.Add(StiImageResizer.GetImage((Image)(object)stiServiceBitmapAttribute.GetImage()));
			((ListBox)stiListBox2).Items.Add((object)item);
			if (item.GetType() == stiShapeTypeService.GetType())
			{
				selectedIndex = num;
			}
			num++;
		}
		((ListControl)stiListBox2).SelectedIndex = selectedIndex;
		((Control)stiListBox2).DoubleClick += ListBox_DoubleClick;
		((ListBox)stiListBox2).Click += ListBox_DoubleClick;
		edSvc.DropDownControl((Control)(object)stiListBox2);
		if (stiShapeTypeService.GetType() != ((ListBox)stiListBox2).Items[((ListControl)stiListBox2).SelectedIndex].GetType())
		{
			return StiActivator.CreateObject(((ListBox)stiListBox2).Items[((ListControl)stiListBox2).SelectedIndex].GetType());
		}
		return stiShapeTypeService;
	}
}
