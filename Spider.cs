using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aries;

namespace Aries.Collector
{
	public abstract class Spider
	{
		public string HOST { get; }
		public string siteName { get; }

		/// <summary>
		/// 获取页面响应文本
		/// </summary>
		/// <param name="url">链接</param>
		/// <returns>页面的响应文本<see cref="string"/></returns>
		public string page(string url)
		{
			if(!vertify(url)) { /*错误处理*/ };
			var web = new WebProtocol(url);
			return web.contentDocument;
		}
		/// <summary>
		/// 获取json数据报文
		/// </summary>
		/// <param name="url">请求链接</param>
		/// <returns>返回的json数据报文<see cref="string"/></returns>
		public string json(string url)
		{
			var web = new WebProtocol(url);
			return web.contentDocument;
		}
		/// <summary>
		/// 验证主机是否一致
		/// </summary>
		/// <param name="url">链接</param>
		/// <returns></returns>
		public bool vertify(string url)
		{
			var host = new Uri(url).Host;
			if(host == HOST) { return true; }
			return false;
		}
	}
}
