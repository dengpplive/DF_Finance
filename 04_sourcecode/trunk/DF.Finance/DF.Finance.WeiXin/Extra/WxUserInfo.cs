﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiXin.Public.Common.Extra
{
    /// <summary>
    /// 粉丝个人信息明细
    /// </summary>
    public class WxUserInfo
    {
        public string City { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }

        [JsonProperty(PropertyName = "fake_id")]
        public string FakeId { get; set; }

        [JsonProperty(PropertyName = "group_id")]
        public int GroupID { get; set; }


        public List<WxUserGroup> Groups { get; set; }

        [JsonProperty(PropertyName = "nick_name")]
        public string NickName { get; set; }


        public string ReMarkName { get; set; }

        /// <summary>
        /// 1：男; 2:女
        /// </summary>
        [JsonProperty(PropertyName = "gender")]
        public string Sex { get; set; }

        /// <summary>
        /// 个人签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 微信登陆名
        /// </summary>
        [JsonProperty(PropertyName = "user_name")]
        public string Username { get; set; }

        #region 额外
        public string OpenId { get; set; }
        #endregion

    }

    //todo:结构有调整
    public class WxUserGroup
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }
    }

    //新调整
    public class WxUserInfoResp
    {
        public object base_resp { get; set; }
        public object groups { get; set; }
        public WxUserInfo contact_info { get; set; }
    }
}