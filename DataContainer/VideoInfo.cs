using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aries.Collector.DataContainer
{
	public abstract class VideoInfo
	{
		/// <summary>
		/// <see cref="Video"/>选择器，用于获取<see cref="items"/>的成员
		/// </summary>
		/// <param name="index">元素在列表中的索引</param>
		/// <returns><see cref="Video"/></returns>
		public Video this[int index]
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
		protected List<Video> items;

		public void add(Video item) { items.Add(item); }
		public void addlist(List<Video> list) { items.AddRange(list); }
	}
}
