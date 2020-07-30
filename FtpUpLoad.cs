using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace GDIPlusDemo
{
    class FtpUpLoad
    {
        /// <summary>

        /// 上传文件

        /// </summary>

        /// <param name="localpath">上传文件的全路径 例@"D:\123.txt"</param>

        /// <param name="ftppath"></param>

        /// <returns></returns>

        public bool Upload(string localpath, string ftppath)
        {
            bool bol = false;
            try
            {
                FileInfo fileInf = new FileInfo(localpath);
                //替换符号
                ftppath = ftppath.Replace("\\", "/");
                //组合ftp上传文件路径
                string uri = "ftp://" + ftppath + "/" + fileInf.Name;
               // 根据uri创建FtpWebRequest对象

                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                // 指定数据传输类型

                reqFTP.UseBinary = true;

                // ftp用户名和密码
                string ftpUserID = "ckp";
                string ftpPassword = "19990221";
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                // 默认为true，连接不会被关闭

                // 在一个命令之后被执行

                reqFTP.KeepAlive = false;

                // 指定执行什么命令

                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                // 上传文件时通知服务器文件的大小

                reqFTP.ContentLength = fileInf.Length;
                
                // 缓冲大小设置为kb
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;
                // 打开一个文件流(System.IO.FileStream) 去读上传的文件
                FileStream fs = fileInf.OpenRead();
                try
                {
                    // 把上传的文件写入流
                    Stream strm = reqFTP.GetRequestStream();
                    // 每次读文件流的kb
                    contentLen = fs.Read(buff, 0, buffLength);
                    // 流内容没有结束
                    while (contentLen != 0)
                    {
                        // 把内容从file stream 写入upload stream

                        strm.Write(buff, 0, contentLen);

                        contentLen = fs.Read(buff, 0, buffLength);

                        bol = true;

                    }

                    // 关闭两个流
                    strm.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("上传文件失败，失败原因；" + ex.Message);                  
                }
            }
            catch (Exception ex)

            {

                MessageBox.Show("上传文件失败，失败原因；" + ex.Message);

            }

            return bol;

        }
    }
}
