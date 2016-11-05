using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DF.Finance.WeiXin.Extra;
using DF.Finance.Model;
using DF.Finance.WeiXin.Model;
using System.Threading;
using WeiXin.Public.Common;
namespace DF.Finance.WeiXin.API
{
    /// <summary>
    /// 微信的一些常用的API接口
    /// </summary>
    public class WeiXinApi
    {
        /// <summary>
        /// 构造PC端网页登陆授权url
        /// </summary>
        /// <param name="sessionState">传入唯一的标识回调使用</param>
        /// <returns></returns>
        public string BuildLoginUrl(string sessionState)
        {
            return string.Format("https://open.weixin.qq.com/connect/qrconnect?appid={0}&redirect_uri={1}&scope=snsapi_login&response_type=code&state={2}#wechat_redirect",
                WenXinGlobal.GetWeiXinSetting.WXLogin.AppId,
                 WenXinGlobal.GetWeiXinSetting.WXLogin.CallBackUrl.UrlEncode(),
                 sessionState
                );
        }
        /// <summary>
        /// 构造微信客户端登录授权地址
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="sessionState">传入唯一的标识回调使用</param>
        /// <param name="callBackUrl">默认使用配置文件中的回调地址,传入就是传入的地址</param>
        /// <returns></returns>
        public string BuildOAuthUrl(Emums.OAuthScope scope, string sessionState, string callBackUrl = null)
        {
            string s_weixin_oauth_format_url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_{2}&state={3}#wechat_redirect";
            return String.Format(s_weixin_oauth_format_url,
                 WenXinGlobal.GetWeiXinSetting.WXLogin.AppId,
                 string.IsNullOrEmpty(callBackUrl) ? WenXinGlobal.GetWeiXinSetting.WXLogin.CallBackUrl.UrlEncode() : callBackUrl.UrlEncode(),
                 scope.ToString().ToLower(),
                 sessionState
                );
        }
        /// <summary>
        /// 回调授权可以 成功取到openid和access_token
        /// </summary>
        /// <param name="code">code</param> 
        /// <returns></returns>
        public bool IsAuthorized(string code, out WebAuthorizeEntity webAuthorizeEntity)
        {
            webAuthorizeEntity = null;
            if (string.IsNullOrEmpty(code)) throw new Exception("code为空");
            bool IsAuthorized = false;
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                WenXinGlobal.GetWeiXinSetting.WXLogin.AppId,
               WenXinGlobal.GetWeiXinSetting.WXLogin.AppSecret,
                 code
                );
            var token = WebApiHelper.GetAsync<WebAuthorizeEntity>(url).Result;
            if (token != null
                && token.expires_in == "7200"
                && !string.IsNullOrEmpty(token.access_token))
            {
                IsAuthorized = true;
            }
            else
            {
                //过期刷新一次
                token.access_token = Refresh_AccessToken(token.access_token);
            }
            webAuthorizeEntity = token;
            return IsAuthorized;
        }

        /// <summary>
        /// 获取访问者AccessToken
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}",
                WenXinGlobal.GetWeiXinSetting.WXLogin.AppId,
               WenXinGlobal.GetWeiXinSetting.WXLogin.AppSecret
                 );
            var tempToken = WebApiHelper.GetAsync<WebAuthorizeEntity>(url).Result;
            return tempToken.access_token;
        }
        /// <summary>
        /// 超时后刷新 AccessToken 返回新的access_token
        /// </summary>
        public string Refresh_AccessToken(string access_token)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}",
                   WenXinGlobal.GetWeiXinSetting.WXLogin.AppId,
                  access_token
                );
            var newTolen = WebApiHelper.GetAsync<WebAuthorizeEntity>(url).Result;
            if (string.IsNullOrEmpty(newTolen.access_token)) newTolen.access_token = access_token;
            return newTolen.access_token;
        }

        /// <summary>
        /// 获取微信服务器IP地址
        /// </summary>
        /// <param name="access_token">访问者token</param>
        /// <returns></returns>
        public WxIpList GetWxServerIp(string access_token)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token={0}", access_token);
            var result = WebApiHelper.InvokeApi<WxIpList>(url);
            return result;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="access_token">访问者token</param>
        /// <returns></returns>
        public Union GetUserInfo(string access_token, string openId)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}",
               access_token,
                openId
                );
            return WebApiHelper.GetAsync<Union>(url).Result;
        }

        #region 批量获取
        private int _MaxOpenIdCount = 10000;
        private int _MaxUserCount = 100;
        /// <summary>
        /// 批量获取openId 一次最多 10000个
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="startOpenId"></param>
        /// <returns></returns>
        private UserList GetUserOpenIdList(string access_token, string startOpenId)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}",
                   access_token,
                   startOpenId
               );
            UserList userList = WebApiHelper.GetAsync<UserList>(url).Result;
            return userList;
        }
        /// <summary>
        /// 获取所有的OpenId
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="startOpenId"></param>
        /// <returns></returns>
        public List<UserList> GetAllUserOpenIdList(string access_token, string startOpenId)
        {
            List<UserList> result = new List<UserList>();
            UserList userList = GetUserOpenIdList(access_token, startOpenId);
            result.Add(userList);
            if (userList.total > _MaxOpenIdCount && userList.count >= _MaxOpenIdCount)
            {
                var temp = GetAllUserOpenIdList(access_token, userList.next_openid);
                result.AddRange(temp.ToArray());
            }
            return result;
        }

        /// <summary>
        /// 获取所有的用户信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="OpenIdList"></param>
        /// <returns></returns>
        public AllUnion GetAllUserInfoList(string access_token, List<string> openIdList)
        {
            var result = new List<Union>();
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token={0}", access_token);
            int total = openIdList.Count;
            int start = 0;
            while (start < total)
            {
                if ((total - start) >= _MaxUserCount)
                {
                    var list = openIdList.GetRange(start, _MaxUserCount);
                    result.AddRange(GetUserInfoList(access_token, list));
                    start += _MaxUserCount;
                }
                else
                {
                    var list = openIdList.GetRange(start, (total - start));
                    result.AddRange(GetUserInfoList(access_token, list));
                    start += (total - start);
                }
                Thread.Sleep(1);
            }
            return new AllUnion()
            {
                user_info_list = result.ToArray()
            };
        }
        /// <summary>
        /// 批量获取用户信息 100个
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openIdList"></param>
        /// <returns></returns>
        private Union[] GetUserInfoList(string access_token, List<string> openIdList)
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info/batchget?access_token={0}", access_token);
            var list = new List<wxParam>();
            openIdList.ForEach(p =>
            {
                list.Add(new wxParam()
                {
                    openid = p
                });
            });
            var postData = new wxRQUser()
            {
                user_list = list.ToArray()
            };
            AllUnion result = WebApiHelper.InvokeAxApi<AllUnion>(url, postData, true);
            return result.user_info_list;
        }
        #endregion
    }
}
