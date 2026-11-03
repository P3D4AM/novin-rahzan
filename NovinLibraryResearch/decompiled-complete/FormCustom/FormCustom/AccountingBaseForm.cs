using FormCustom.Forms;
using FormCustom.Froms.LkManage;

namespace FormCustom;

public class AccountingBaseForm : LkUiFactory
{
	private new static BaseFormConfig Config => new BaseFormConfig
	{
		ChangeFont = false,
		ChangeBackColor = false
	};

	protected AccountingBaseForm(string contextGuid, BaseFormConfig _config = null)
		: base(contextGuid, _config)
	{
	}

	public AccountingBaseForm(string contextGuid)
		: base(contextGuid, Config)
	{
	}

	public AccountingBaseForm()
		: base("")
	{
	}
}
