using System;
using System.Text;
using System.Windows.Forms;

namespace RSN.DotP
{
	public class ColumnSort
	{
		public string Key;
		public string DataProperty;
		public SortOrder SortDirection;

		public ColumnSort()
			: this(string.Empty, string.Empty, SortOrder.None)
		{ }

		public ColumnSort(string strKey, string strDataProp, SortOrder soDirection)
		{
			Key = strKey;
			DataProperty = strDataProp;
			SortDirection = soDirection;
		}
	}
}
