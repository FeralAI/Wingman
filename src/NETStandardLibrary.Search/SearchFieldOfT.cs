using System;

namespace NETStandardLibrary.Search
{
	public class SearchField<T> : SearchField
	{
		public override Type ValueType
		{
			get => typeof(T);
		}
	}
}
