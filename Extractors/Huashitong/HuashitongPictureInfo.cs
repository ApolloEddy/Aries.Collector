using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aries.Collector.DataContainer;

namespace Aries.Collector.Extractors
{
	public class HuashitongPictureInfo : PictureInfo
	{
		public HuashitongPictureInfo(List<HuashitongPicture> pictures) { items = pictures; }
		public HuashitongPictureInfo() { items = new List<HuashitongPicture>(); }
		public new List<HuashitongPicture> items { get; set; }
		public void add(HuashitongPicture item) { items.Add(item); items.Distinct(); }
		public void addlist(List<HuashitongPicture> list) { items.AddRange(list); items.Distinct(); }
	}
}
