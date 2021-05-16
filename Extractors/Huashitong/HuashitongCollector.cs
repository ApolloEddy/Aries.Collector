using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aries.Collector.DataContainer;
using Aries.Collector.contact;

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
		public long _tsParam
		{
			get
			{
				var origin = DateTime.Parse("1970/01/01").ToUniversalTime().AddHours(8.0);
				long diff = DateTime.Now.ToUniversalTime().Ticks - origin.Ticks;
				return diff;
			}
		}
		private Messenger msg = new Messenger();

		public string json(string title, int index = 1)
		{
			const string url = "https://rt.huashi6.com/front/works/search?_ts_={ts}&index={index}&title={title}";
			title = WebProtocol.EscapeDataString(title);
			var web = new WebProtocol(url.Replace("{index}", index.ToString()).Replace("{title}", title).Replace("{ts}",_tsParam.ToString()));
			return web.contentDocument;
		}
		public List<HuashitongPicture> pictureList(string json)
		{
			var ret = new List<HuashitongPicture>();
			var tp = new TextParser(json);
			jsonCount = int.Parse(tp.extractOne("\"pageCount\":", ","));
			// var array = tp.extractOne("\"datas\":[", "]");
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
			msg.startProcess(0, picture.imgCount, 1, $"正在下载 {picture.title}({picture.picID})({picture.artist})...");
			string url;
			for(int i = 0; i < picture.imgCount; i++)
			{
				try 
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
					msg.stepProcess();
				}
				catch(Exception ex)
				{
					msg.puts();
					msg.error(ex.Message);
				}
			}
			msg.puts();
			// msg.puts($"完成！ 共[{picture.imgCount}]张图片");
		}
		public void download(List<HuashitongPicture> list, string dirpath)
		{
			Directory.CreateDirectory(dirpath);
			msg.success($"已创建路径：\"{dirpath}\"!");
			foreach (HuashitongPicture item in list)
			{
				// Console.WriteLine(list.IndexOf(item));
				download(item, dirpath);
			}
		}
		public void download(string dirpath) { download(pictureInfo.items, dirpath); }
		public void download(string url, string dirpath)
		{
			msg.put("正在分析页面...");
			var pic = singlePictre(url);
			msg.puts("完成!");
			msg.success("为你采集到以下信息：");
			msg.puts($"{pic.title}[{pic.artist}]：\n　　链接：{pic.link}\n　　上传时间：{pic.update}\n　　图片ID：{pic.picID}\n　　作品集包含的图片数：{pic.imgCount}");
			download(pic, dirpath);
		}
		public void catchRank(string dirpath)
		{
			dirpath += $"\\{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}\\";
			var list = rankList();
			download(list, dirpath);
		}
		public void downloadByTitle(string title, string dirpath)
		{
			pictureList(json(title, 1)); // 为了界面，牺牲一下效率 (约500ms)
			int jsonCount = this.jsonCount;
			msg.startProcess(0, jsonCount, 1, "正在分析页面信息...");
			HuashitongPicture pic = new HuashitongPicture(title) { title = title};
			dirpath += $"{pic.generateFilename()}\\";
			for(int index = 1; index <= jsonCount; index++)
			{
				var json = this.json(title, index);
				pictureList(json);
				msg.stepProcess();
			}
			msg.puts();
			msg.success("成功获取页面信息，即将开始下载...");
			download(pictureInfo.items, dirpath); 
		}
	}
}
