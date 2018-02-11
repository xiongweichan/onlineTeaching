using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;

namespace TeacherClient
{
    public class MediaHelper
    {
        public int frameRate = 25;
        public int sendBitRate = 128 * 1024;//1024 * 1024;
        public int sendWidth = 352;//800;
        public int sendHeight = 288;//600;
        public int localWidth = 1920;
        public int localHeight = 1080;
        public int localSmallWidth = 352;
        public int localSmallHeight = 288;

        MediaInterface m;
        MediaHelper()
        {
            int ret;
            //int idx, idx2;
            m = new MediaInterface();
            ret = m.init(frameRate, sendBitRate, sendWidth, sendHeight, localWidth, localHeight, localSmallWidth, localSmallHeight);
            //idx = m.getLocalStreamIndex();
            //idx2 = m.getSendStreamIndex();
            m.setShowSmallStream(1, 0, 0);
            //int a = 0, b = 0;
            //m.getShowSmallStream(out a, out b);
            //Console.WriteLine("getLocalStreamIndex:{0}, {1}, {2}, {3}, {4}, {5}", ret, ret2, idx, idx2, a, b);

        }
        public void PushStream(string address)
        {//"rtmp://incastyun.cn/live/media"
            int ret2 = m.startPushStream(address);
        }
        static MediaHelper _instance;
        public static MediaHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MediaHelper();
                return _instance;
            }
        }

        public void Open()
        {
            _instance.m.Start();
            m.setShowSmallStream(1, 0, 0);

        }

        public void Close()
        {
            _instance.m.Stop();
        }

        byte[] data = new byte[1920 * 1080 * 3 + 99];
        MemoryStream stream = new MemoryStream();
        public Stream GetImage()
        {
            int dataSize;
            dataSize = m.getPlayData(data);
            if (dataSize > 0)
            {
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                return stream;
            }
            return null;
        }

        public bool SetLocalStreamIndex(int index)
        {
            return m.setLocalStreamIndex(index) == 0;
        }

        public void SetShowSmallStream(bool needShowMain, bool needShowA, bool needShowB)
        {
            m.setShowSmallStream(needShowMain ? 1 : 0, needShowA ? 1 : 0, needShowB ? 1 : 0);
        }

        static void cmdThread(Object media)
        {
            MediaInterface m = (MediaInterface)media;
            int c;
            string s;
            while (true)
            {
                s = Console.ReadLine();
                c = s[0] - '0';
                Console.WriteLine("read input {0}", c);
                if (c > 2)
                {
                    m.setSendStreamIndex(c - 3);
                }
                else
                {
                    m.setLocalStreamIndex(c);
                }
                Thread.Sleep(2000);
            }
            //return;
        }

    }
}
