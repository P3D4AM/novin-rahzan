namespace DevComponents.DotNetBar.Ribbon;

internal class Ყ : SubItemsCollection
{
	private Ꮮ Ꮾ;

	public Ყ(Ꮮ qatToolbar)
		: base(null)
	{
		Ꮾ = qatToolbar;
	}

	protected override void OnInsert(int index, object value)
	{
	}

	protected override void OnInsertComplete(int index, object value)
	{
		if (!m_IgnoreEvents)
		{
			if (Ꮾ.ᯥ.Count <= index)
			{
				Ꮾ.ᯥ.Add(value as BaseItem);
			}
			else
			{
				Ꮾ.ᯥ.Insert(index, value as BaseItem);
			}
		}
	}

	protected override void RemoveInternal(int index, object value)
	{
		if (!m_IgnoreEvents)
		{
			Ꮾ.ᯥ.Remove(value as BaseItem);
		}
	}

	protected override void RemoveCompleteInternal(int index, object value)
	{
	}
}
