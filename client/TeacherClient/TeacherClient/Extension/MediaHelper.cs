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
        static MediaInterface m;
        static MediaHelper()
        {
            //FileStream file = new FileStream("play2.bmp", FileMode.OpenOrCreate);
            int ret;
            int ret2, idx, idx2;
            //MediaInterface
            m = new MediaInterface();
            ret = m.init(25, 1024 * 1024, 800, 600, 1920, 1080, 352, 288);
            ret2 = m.startPushStream("rtmp://incastyun.cn/live/media");  
            idx = m.getLocalStreamIndex();
            idx2 = m.getSendStreamIndex();
            m.setShowSmallStream(1, 1);
            int a = 0, b = 0;
            m.getShowSmallStream(out a, out b);
            Console.WriteLine("getLocalStreamIndex:{0}, {1}, {2}, {3}, {4}, {5}", ret, ret2, idx, idx2, a, b);

            //Thread th = new Thread(new ParameterizedThreadStart(cmdThread));
            //th.Start((Object)m);
            //byte[] data = new byte[1920 * 1080 * 3 + 99];
            //int dataSize;
            //bool hasWrite = false;
            //int frameNum = 0;
            //while(true)
            //{
            //    dataSize = m.getPlayData(data);
            //    if (hasWrite == false && dataSize > 0)
            //    {
            //        hasWrite = true;
            //        file.Write(data, 0, dataSize);
            //        file.Flush();
            //        file.Close();
            //    }
            //    Thread.Sleep(10);

            //    //frameNum++;
            //    //if (frameNum > 250)
            //    //{
            //    //    break;
            //    //}
            //}

            //Console.Read();
            //m.stopPushStream();
            //Console.Read();
        }

        public static Stream GetImage()
        {
            byte[] data = new byte[1920 * 1080 * 3 + 99];
            int dataSize;
            dataSize = m.getPlayData(data);
            if (dataSize > 0)
                return new MemoryStream(data);
            return null;
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
