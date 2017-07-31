using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace TeacherClient.Core
{
    public class ConfigManagerHelper
    {
        private static ConfigHelper confighelper;
        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="needread"></param>
        public static void Init(string filepath, bool needread)
        {
            confighelper = new ConfigHelper();
            confighelper.Init(filepath, needread);
        }
        /// <summary>
        /// 通过配置读取信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetConfigByName(string name)
        {
            return confighelper.GetConfigByName(name);
        }

        /// <summary>
        /// 根据配置项写入配置数据，如无，则重新创建配置项插入
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SetConfigByName(string name, string value)
        {
            return confighelper.SetConfigValue(name, value);
        }


    }

    /// <summary>
    /// 系统使用的config类库
    /// </summary>
    internal class ConfigHelper
    {
        private string filepath;
        private Dictionary<string, string> pathdic;
        XmlDocument document = null;

        /// <summary>
        /// 设置文件地址以及是否读取文件信息
        /// </summary>
        /// <param name="filepath">文件地址</param>
        public void Init(string filepath, bool needread)
        {
            this.filepath = filepath;
            if (pathdic == null)
            {
                this.pathdic = new Dictionary<string, string>();
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
            document = new XmlDocument();
            document.Load(this.filepath);
            XmlNodeList paras = document.SelectNodes("//paras/para");
            foreach (XmlNode para in paras)
            {
                string name = para.Attributes["name"].Value.ToString();
                string value = para.Attributes["value"].Value.ToString();
                if (string.IsNullOrEmpty(name) == false)
                {
                    if (pathdic.ContainsKey(name) == false)
                    {
                        this.pathdic.Add(name, value);
                    }
                }
            }
            //doc.Clone();
        }

        /// <summary>
        /// 通过名称读取文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetConfigByName(string name)
        {
            if (this.pathdic != null)
            {
                if (this.pathdic.ContainsKey(name))
                {
                    return this.pathdic[name];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 设置配置项数值
        /// </summary>
        public bool SetConfigValue(string name, string value)
        {
            /*本类被静态类引用,内存不释放,再次调用GetConfigByName取得旧值*/
            bool result = false;

            // 保存到内存
            if (this.pathdic != null)
            {
                if (this.pathdic.ContainsKey(name))
                {
                    this.pathdic[name] = value;
                    result = true;
                }
            }

            // 保存到文件
            var xpath = string.Format("//paras/para[@name='{0}']", name);
            XmlNode node = document.SelectSingleNode(xpath);
            if (node != null)
            {
                var ele = node as XmlElement;
                ele.SetAttribute("value", value);
                result = true;
            }
            document.Save(this.filepath);

            return result;
        }
    }
}
