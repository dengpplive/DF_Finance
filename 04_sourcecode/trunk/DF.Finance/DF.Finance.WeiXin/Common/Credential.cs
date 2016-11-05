﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Web.Configuration;

namespace WeiXin.Public.Common
{
    public class Credential
    {
        public const string Url = "https://api.weixin.qq.com/cgi-bin/token";
        private static string _accessToken;

        /// <summary>
        /// 多access_token缓存（根据appid），满足一个服务器服务于多个微信公号的需求。
        /// </summary>
        private readonly static Dictionary<string, string> MultiTokenCache = new Dictionary<string, string>();

        /// <summary>
        /// 获取缓存的（最后一次获取的）AccessToken。
        /// 注意，即使缓存的token已过期或不存在也不会获取新AccessToken。
        /// </summary>
        public static string CachedAccessToken
        {
            get
            {
                return _accessToken;
            }
        }

        /// <summary>
        /// 获取access_token填写client_credential
        /// </summary>
        public string GrantType
        {
            get
            {
                return "client_credential";
            }
        }

        /// <summary>
        /// 第三方用户唯一凭证
        /// </summary>
        public string Appid { get; set; }

        /// <summary>
        /// 第三方用户唯一凭证密钥，既appsecret
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 获取access_token。会缓存，过期后自动重新获取新的token。
        /// </summary>
        public string AccessToken
        {
            get
            {
                if (!MultiTokenCache.ContainsKey(Appid) || string.IsNullOrEmpty(MultiTokenCache[Appid]))
                {
                    var helper = GetHelper();
                    var ret = helper.Get<Result>(new FormData
                    {
                       { "grant_type",GrantType},
                       { "appid",Appid},
                       { "secret",Secret},
                    });

                    _accessToken = ret.access_token;
                    MultiTokenCache[Appid] = _accessToken;//缓存

                    if (ret.IsSuccess)
                    {
                        var autoEvent = new AutoResetEvent(false);
                        var timer = new Timer(state => { _accessToken = null;
                                                           autoEvent.Set();
                        }, autoEvent, (ret.expires_in - 3)/*避免时间误差*/ * 1000, Timeout.Infinite);
                        timer.Dispose(autoEvent);
                    }
                }

                _accessToken = MultiTokenCache[Appid];//2013-11-28 修复bug：Appid变化后，仍然获取上个成功的access_token

                return _accessToken;
            }
        }

        private HttpHelper GetHelper()
        {
            var ret = new HttpHelper(Url);
            return ret;
        }

        /// <summary>
        /// 获取配置文件中的凭证信息并填充到Credential实例
        /// </summary>
        /// <returns></returns>
        public static Credential Create()
        {
            var ret = new Credential();
            ret.Appid =  WenXinGlobal.GetWeiXinSetting.WXLogin.AppId;
            ret.Secret = WenXinGlobal.GetWeiXinSetting.WXLogin.AppSecret;
            return ret;
        }
    }

    /// <summary>
    /// 获取凭证时的响应数据
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// 错误信息号
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误消息文本
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return !string.IsNullOrEmpty(access_token);
            }
        }
    }
}