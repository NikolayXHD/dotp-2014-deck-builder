using System;
using System.Collections.Generic;
using System.ComponentModel;
using Be.Timvw.Framework.Collections.Generic;

// Riiak Shi Nal: Modified to support sorting on multiple fields.
//	No filtering yet.
namespace Be.Timvw.Framework.ComponentModel
{
	public class SortableBindingList<T> : BindingList<T>, IBindingListView
	{
		private readonly Dictionary<Type, PropertyComparer<T>> comparers;
		private bool isSorted;
		private ListSortDirection listSortDirection;
		private PropertyDescriptor propertyDescriptor;
		private List<PropertyComparer<T>> m_lstComparers;
		private ListSortDescriptionCollection m_lsdcSort;

		public SortableBindingList()
			: base(new List<T>())
		{
			this.comparers = new Dictionary<Type, PropertyComparer<T>>();
		}

		public SortableBindingList(IEnumerable<T> enumeration)
			: base(new List<T>(enumeration))
		{
			this.comparers = new Dictionary<Type, PropertyComparer<T>>();
		}

		protected override bool SupportsSortingCore
		{
			get { return true; }
		}

		protected override bool IsSortedCore
		{
			get { return this.isSorted; }
		}

		protected override PropertyDescriptor SortPropertyCore
		{
			get { return this.propertyDescriptor; }
		}

		protected override ListSortDirection SortDirectionCore
		{
			get { return this.listSortDirection; }
		}

		protected override bool SupportsSearchingCore
		{
			get { return true; }
		}

		protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
		{
			List<T> itemsList = (List<T>)this.Items;

			Type propertyType = property.PropertyType;
			PropertyComparer<T> comparer;
			if (!this.comparers.TryGetValue(propertyType, out comparer))
			{
				comparer = new PropertyComparer<T>(property, direction);
				this.comparers.Add(propertyType, comparer);
			}

			comparer.SetPropertyAndDirection(property, direction);
			itemsList.Sort(comparer);

			this.propertyDescriptor = property;
			this.listSortDirection = direction;
			this.isSorted = true;

			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		protected override void RemoveSortCore()
		{
			this.isSorted = false;
			this.propertyDescriptor = base.SortPropertyCore;
			this.listSortDirection = base.SortDirectionCore;

			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		protected override int FindCore(PropertyDescriptor property, object key)
		{
			int count = this.Count;
			for (int i = 0; i < count; ++i)
			{
				T element = this[i];
				if (property.GetValue(element).Equals(key))
				{
					return i;
				}
			}

			return -1;
		}

		private int CompareMultiple(T x, T y)
		{
			if (x == null)
				return (y == null ? 0 : -1);
			else
			{
				if (y == null)
					return 1;
				else
				{
					foreach (PropertyComparer<T> pcComp in m_lstComparers)
					{
						int nRet = pcComp.Compare(x, y);
						if (nRet != 0)
							return nRet;
					}
					return 0;
				}
			}
		}

		public void ApplySort(ListSortDescriptionCollection sorts)
		{
			List<T> itemsList = (List<T>)this.Items;

			m_lstComparers = new List<PropertyComparer<T>>();
			foreach (ListSortDescription lsdSort in sorts)
				m_lstComparers.Add(new PropertyComparer<T>(lsdSort.PropertyDescriptor, lsdSort.SortDirection));
			itemsList.Sort(CompareMultiple);

			this.m_lsdcSort = sorts;
			this.propertyDescriptor = m_lsdcSort[0].PropertyDescriptor;
			this.listSortDirection = m_lsdcSort[0].SortDirection;
			this.isSorted = true;

			this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		public string Filter
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public void RemoveFilter()
		{
			throw new NotImplementedException();
		}

		public ListSortDescriptionCollection SortDescriptions
		{
			get { return m_lsdcSort; }
		}

		public bool SupportsAdvancedSorting
		{
			get { return true; }
		}

		public bool SupportsFiltering
		{
			get { return false; }
		}
	}
}