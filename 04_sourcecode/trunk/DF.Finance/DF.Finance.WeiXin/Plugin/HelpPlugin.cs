﻿using WeiXin.Public.Message;

namespace WeiXin.Public.Plugin
{
    public class HelpPlugin : TextPlugin
    {
        /// <summary>
        /// 默认匹配。可匹配关键字“帮助”，“help”，“h”。英文不区分大小写。
        /// </summary>
        public const string DefaultPattern = @"^\s*帮助|help|h\s*$";

        private bool _asDefault;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="asDefault">是否作为转为插件，处理所有消息</param>
        public HelpPlugin(string pattern, bool asDefault = false)
            : base(pattern)
        {
            _asDefault = asDefault;
        }

        public override bool CanProcess(PluginContext ctx)
        {
            return _asDefault || base.CanProcess(ctx);
        }

        public override ResponseMessage Execute(PluginContext ctx)
        {
            var seller = ctx.Seller;
            var rep = ctx.ReceiveMessage.GetTextResponse("系统使用帮助");
            return rep;
        }
    }
}