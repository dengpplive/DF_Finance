using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DF.Finance.Common
{
    public class DownloadHelper
    {
        private delegate void DownloadDelegate(string downloadUrl, string savePath);
        public static void DownloadAsync(string downloadUrl, string savePath)
        {
            DownloadDelegate download = new DownloadDelegate(Download);
            download.BeginInvoke(downloadUrl, savePath, null, null);
        }
        /// <summary>
        /// 下载一个文件到本地
        /// </summary>
        /// <param name="downloadUrl">要下载的文件地址</param>
        /// <param name="savePath">保存到该地址</param>
        public static void Download(string downloadUrl, string savePath)
        {
            WebClient wcClient = new WebClient();
            //对远程文件发送一个请求
            WebRequest webReq = WebRequest.Create(downloadUrl);
            //接收远程WEB服务器发回的响应
            using (WebResponse webRes = webReq.GetResponse())
            {
                //获取文件长度
                long fileLength = webRes.ContentLength;
                //创建一个文件流，接收返回的流信息
                Stream srm = webRes.GetResponseStream();
                //使用特殊编码读取流信息
                StreamReader srmReader = new StreamReader(srm);
                //定义缓冲区
                byte[] bufferbyte = new byte[fileLength];
                //缓冲区长度
                int allByte = (int)bufferbyte.Length;
                int startByte = 0;
                //读取缓冲区内容
                while (fileLength > 0)
                {
                    int downByte = srm.Read(bufferbyte, startByte, allByte);
                    if (downByte == 0) { break; };
                    startByte += downByte;
                    allByte -= downByte;
                }
                //创建一个文件流，将处理的文件流写入磁盘
                FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Write(bufferbyte, 0, bufferbyte.Length);
                srm.Close();
                srmReader.Close();
                fs.Close();
                webRes.Close();
            }
        }
    }
}
