using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.DataContainer
{
	public abstract class PictureInfo
	{
		/// <summary>
		/// <see cref="Picture"/>选择器，用于获取<see cref="items"/>的成员
		/// </summary>
		/// <param name="index">元素在列表中的索引</param>
		/// <returns><see cref="Picture"/></returns>
		public Picture this[int index]
		{
			get
			{
				return items[index];
			}
		}

		public string title { get; }
		public string artist { get; }
		public int count { get { return items.Count; } }
		public string playListUrl { get; }
		protected List<Picture> items;

		/// <summary>
		/// 添加一个元素到实例
		/// </summary>
		/// <param name="item"><see cref="Picture"/>元素</param>
		public void add(Picture item) { items.Add(item); }
		/// <summary>
		/// 将一个列表追加到实例中
		/// </summary>
		/// <param name="list">待加入的列表</param>
		public void addlist(List<Picture> list) { items.AddRange(list); }
	}
}
