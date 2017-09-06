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
        public int sendBitRate = 1024 * 1024;
        public int sendWidth = 800;
        public int sendHeight = 600;
        public int localWidth = 1920;
        public int localHeight = 1080;
        public int localSmallWidth = 352;
        public int localSmallHeight = 288;

        MediaInterface m;
        MediaHelper()
        {
            int ret;
            int ret2, idx, idx2;
            m = new MediaInterface();
            ret = m.init(frameRate, sendBitRate, sendWidth, sendHeight, localWidth, localHeight, localSmallWidth, localSmallHeight);
            ret2 = m.startPushStream("rtmp://incastyun.cn/live/media");
            idx = m.getLocalStreamIndex();
            idx2 = m.getSendStreamIndex();
            m.setShowSmallStream(1, 1);
            int a = 0, b = 0;
            m.getShowSmallStream(out a, out b);
            Console.WriteLine("getLocalStreamIndex:{0}, {1}, {2}, {3}, {4}, {5}", ret, ret2, idx, idx2, a, b);

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

        public void Close()
        {
            _instance.m.Dispose();
            _instance = null;
        }

        public Stream GetImage()
        {
            byte[] data = new byte[1920 * 1080 * 3 + 99];
            int dataSize;
            dataSize = m.getPlayData(data);
            if (dataSize > 0)
                return new MemoryStream(data);
            return null;
        }

        public bool SetLocalStreamIndex(int index)
        {
            return m.setLocalStreamIndex(index) == 0;
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
