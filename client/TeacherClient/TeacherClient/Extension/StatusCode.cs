using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TeacherClient
{
    /// <summary>
    /// 错误码类
    /// </summary>
    public class StatusCode
    {
        public int Status { get; set; }
        public string Msg { get; set; }

        public StatusCode()
        {
            this.Msg = string.Empty;
            this.Status = -1;
        }
    }





    public class StatusCodeManagerHelper
    {
        private static StatusCodeHelper statusCodeHelper;
        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="needread"></param>
        public static void Init(string filepath, bool needread)
        {
            statusCodeHelper = new StatusCodeHelper();
            statusCodeHelper.Init(filepath, needread);
        }
        /// <summary>
        /// 通过配置读取信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetConfigByName(int code)
        {
            return statusCodeHelper.GetConfigByName(code);
        }
    }

    /// <summary>
    /// 系统使用的config类库
    /// </summary>
    public class StatusCodeHelper
    {
        private string filepath;
        private Dictionary<int, string> pathdic;

        /// <summary>
        /// 设置文件地址以及是否读取文件信息
        /// </summary>
        /// <param name="filepath">文件地址</param>
        public void Init(string filepath, bool needread)
        {
            this.filepath = filepath;
            if (pathdic == null)
            {
                this.pathdic = new Dictionary<int, string>();
            }
            if (needread)
            {
                GetAllConfig();
            }
        }

        /// <summary>
        /// 获取所有配置
        /// </summary>
        public void GetAllConfig()
        {
            var doc = new XmlDocument();
            doc.Load(this.filepath);
            var paras = doc.SelectNodes("//paras/para");
            foreach (XmlNode para in paras)
            {
                string codes = para.Attributes["code"].Value;
                var code = Convert.ToInt32(codes);
                var desc = para.Attributes["value"].Value;
                if (pathdic.ContainsKey(code) == false)
                {
                    this.pathdic.Add(code, desc);
                }
            }
        }

        /// <summary>
        /// 通过名称读取文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConfigByName(int code)
        {
            if (this.pathdic != null)
            {
                if (this.pathdic.ContainsKey(code))
                {
                    return this.pathdic[code];
                }
            }
            return string.Empty;
        }

    }
}
