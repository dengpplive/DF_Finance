using DF.Finance.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace DF.Finance.Web.WebApi
{
    public class TestController : ApiController
    {
        /// <summary>
        /// 测试ajax
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public RetObj TestAjax()
        {
            Thread.Sleep(2000);
            GlobalConfiguration gc = new GlobalConfiguration();
            gc.HomeId = "a";
            gc.Title = "-金融业代";
            List<PageLayout> listPL = new List<PageLayout>();

            PageLayout model = new PageLayout()
            {
                Id = "a",
                PageLinks = "/test/a.html",
                Title = "测试a页面",
                Preloading = "/test/b.html,/test/c.html",
                AreReacquire = false
            };
            listPL.Add(model);
            PageLayout modelb= new PageLayout()
            {
                Id = "b",
                PageLinks = "/test/b.html",
                Title = "测试b页面",
                Preloading = "/test/f.html",
                AreReacquire = false
            };
            PageLayout modelc = new PageLayout()
            {
                Id = "c",
                PageLinks = "/test/c.html",
                Title = "测试c页面",
                Preloading = "",
                AreReacquire = false
            };
            PageLayout modelf = new PageLayout()
            {
                Id = "f",
                PageLinks = "/test/f.html",
                Title = "测试f页面",
                Preloading = "",
                AreReacquire = false
            };
            listPL.Add(modelb);
            listPL.Add(modelc);
            listPL.Add(modelf);

            return new RetObj(new { GlobalConfiguration=gc, ListPage = listPL });
        }
        /// <summary>
        /// 页面配置
        /// </summary>
        private class PageLayout
        {
            /// <summary>
            /// 页面标识，建议与页面名称一致，必须唯一【必须写此参数】
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// 指向页面
            /// </summary>
            public string PageLinks { get; set; }
            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 预加载页面编号 多个 , 分割
            /// a,b
            /// </summary>
            public string Preloading { get; set; }
            /// <summary>
            /// 是否每次进入页面都重新获取最新数据
            /// [每次进入页面是否需要重新拉取数据，如果false 则不重新拉取]
            /// </summary>
            public bool AreReacquire { get; set; }
           
        }
        /// <summary>
        /// 全局配置
        /// </summary>
        private class GlobalConfiguration
        {
            /// <summary>
            /// 标题，设置此值将所有页面标题后面都会追加此标题 
            /// 例如a页面标题叫:资料录入
            /// Title：-金融业代
            /// 最终显示：资料录入-金融业代
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 模板首次加载的Id
            /// 不要增加 package_
            /// </summary>
            public string HomeId { get; set; }
        }
    }
    
}
