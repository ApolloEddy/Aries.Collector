using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aries.Collector.DataContainer;

namespace Aries.Collector.Extractors
{
	public class HuashitongPicture : Picture
	{
		public string picID { get; set; }
		public int imgCount { get; set; }
		public readonly string imageHost = "https://img2.huashi6.com/";

		public HuashitongPicture(string jsonBlock)
		{
			var tp = new TextParser(jsonBlock);
			title = tp.extractOne("\"title\":\"", "\"");
			artist = tp.extractOne("\"name\":\"", "\"");
			update = DateTime.Parse(tp.extractOne("\"upAt\":\"", "\""));
			picID = tp.extractOne("\"pId\":\"", "\"");
			imgCount = int.Parse(tp.extractOne("\"imageNum\":", ","));
			link = picUrl(tp);
			// Console.WriteLine($"{title}[{artist}]：\n\t链接：{link}\n\t上传时间：{update}\n\t图片ID：{picID}\n\t作品集包含的图片数：{imgCount}");
		}
		public string generateFilename()
		{
			if(picID != string.Empty) { return picID; }
			if (title == string.Empty) { return DateTime.Now.GetHashCode().ToString(); }
			foreach (char ch in System.IO.Path.GetInvalidPathChars())
			{
				if (title.Contains(ch)) { return DateTime.Now.GetHashCode().ToString(); }
			}
			return title;
		}
		private string picUrl(in TextParser tp)
		{
			string link = imageHost + tp.extractOne("\"path\":\"", "\"");
			link = link.Replace("\\u002F", "\u002F");
			return link;
		}
	}
}
