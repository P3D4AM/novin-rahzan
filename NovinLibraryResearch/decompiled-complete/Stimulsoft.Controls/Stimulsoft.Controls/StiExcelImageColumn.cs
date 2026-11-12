using System.Windows.Forms;

namespace Stimulsoft.Controls;

public class StiExcelImageColumn : DataGridViewImageColumn
{
	public StiExcelImageColumn()
	{
		((DataGridViewColumn)this).CellTemplate = (DataGridViewCell)(object)new StiExcelImageCell();
	}
}
