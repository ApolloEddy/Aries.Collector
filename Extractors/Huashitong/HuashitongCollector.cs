using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aries.Collector.DataContainer;

namespace Aries.Collector.Extractors
{
	public class HuashitongCollector : Spider
	{
		/// <summary>
		/// https://www.huashi6.com/
		/// </summary>
		public new string HOST
		{
			get
			{
				return new Uri("https://www.huashi6.com/").Host;
			}
		}
		/// <summary>
		/// P站-[pixiv]-画师分享平台 - 画师通
		/// </summary>
		public new string siteName { get { return "P站-[pixiv]-画师分享平台 - 画师通"; } }
		public HuashitongPictureInfo pictureInfo = new HuashitongPictureInfo();
		private int jsonCount;

		public string json(string title, int index = 1)
		{
			const string url = "https://rt.huashi6.com/front/works/search?index={index}&title={title}";
			title = WebProtocol.EscapeDataString(title);
			var web = new WebProtocol(url.Replace("{index}", index.ToString()).Replace("{title}", title));
			return web.contentDocument;
		}
		public List<HuashitongPicture> pictureList(string json)
		{
			var ret = new List<HuashitongPicture>();
			var tp = new TextParser(json);
			jsonCount = int.Parse(tp.extractOne("\"pageCount\":", ","));
			var array = tp.extractOne("\"datas\":[", "]");
			var blocklist = tp.regParse("{\"bornAt\"(.+?)(viewNum)(.+?)}");
			foreach (string item in blocklist)
			{
				ret.Add(new HuashitongPicture(item));
			}
			ret.Distinct();
			pictureInfo.addlist(ret);
			return ret;
		}
		public HuashitongPicture singlePictre(string url)
		{
			var html = page(url);
			var tp = new TextParser(html);
			tp.extractOne("window\\.__INITIAL_STATE__=", "\\(function\\(\\)", true);
			var block = tp.extractOne("\"worksDetailData\":{", "{\"bornAt\"");
			var picture = new HuashitongPicture(block);
			pictureInfo.add(picture);
			return picture;
		}
		public List<HuashitongPicture> rankList()
		{
			// 每天的排行榜只有50个，可以指定一个json页面返回的数量
			string link = $"https://rt.huashi6.com/front/works/rank_page?index=1&size=50&date={DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
			var web = new WebProtocol(link);
			web.Accept = "application/json, text/plain, */*";
			web.Headers.Add("sec-fetch-site", "same-site");
			web.Headers.Add("sec-fetch-mode", "cors");
			web.Headers.Add("sec-fetch-dest", "empty");
			var json = web.contentDocument;
			return pictureList(json);
		}
		public void download(HuashitongPicture picture, string dirpath)
		{
			WebProtocol web;
			//if (dirpath.LastIndexOf('.') > dirpath.LastIndexOf('\\'))
			//{ dirpath = new DirectoryInfo(dirpath).Parent.Name; }
			Console.WriteLine($"正在下载 {picture.title}({picture.picID})({picture.artist})");
			string url;
			for(int i = 0;i < picture.imgCount; i++)
			{
				url = picture.link.Replace("p0.jpg", $"p{i}.jpg").Replace("p0.png", $"p{i}.png");
				web = new WebProtocol(url);
				web.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
				web.Headers.Add("pragma", "no-cache");
				web.Headers.Add("sec-fetch-dest", "document");
				web.Headers.Add("sec-fetch-mode", "navigate");
				web.Headers.Add("sec-fetch-site", "none");
				web.Headers.Add("sec-fetch-user", "?1");
				var bytes = web.contentBytes;
				var path = dirpath + $"{picture.generateFilename()}_p{i}.{Path.GetExtension(url)}";
				File.WriteAllBytes(path, bytes);
			}
		}
		public void download(List<HuashitongPicture> list, string dirpath)
		{
			Directory.CreateDirectory(dirpath);
			foreach (HuashitongPicture item in list)
			{
				// Console.WriteLine(list.IndexOf(item));
				download(item, dirpath);
			}
		}
		public void download(string dirpath) { download(pictureInfo.items, dirpath); }
		public void download(string url, string dirpath)
		{
			var pic = singlePictre(url);
			download(pic, dirpath);
		}
		public void catchRank(string dirpath)
		{
			dirpath += $"\\{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}\\";
			var list = rankList();
			download(list, dirpath);
		}
	}
}
