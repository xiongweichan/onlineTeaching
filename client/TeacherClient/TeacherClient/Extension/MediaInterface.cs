using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace TeacherClient
{
    // libMedia.dll interface class
    //  v1.00 for 2017.08.28

    class MediaInterface : IDisposable
    {
        private IntPtr media;
        public MediaInterface()
        {
            media = Media_CreatePtr();
        }

        public void Dispose()
        {
            Media_DeletePtr(media);
        }

        //**********************************************************************************************************************
        //usage: init media module
        //para:
        //[in] frameRate        所有视频的帧率
        //[in] sendBitRate      推送视频的码率
        //[in] sendWidth        推送视频的宽
        //[in] sendHeight       推送视频的高
        //[in] localWidth       本地播放视频的宽
        //[in] localHeight      本地播放视频的高
        //[in] localSmallWidth  本地播放视频的小视频的宽
        //[in] localSmallHeight 本地播放视频的小视频的高
        //return:
        //0为正常，非0为异常

        //      ------------------------------------------------------------------------------- localSmallWidth -----
        //      |                                                                           |                       |
        //      |                                                                           |                       |    
        //      |                                                                           |                 localSmallHeight
        //      |                                                                           |                       |
        //      |                                                                           -------------------------
        //      |                                                                           |                       |
        //  localSmallHeight                                                                |                 localSmallHeight
        //      |                                                                           |                       |
        //      |                                                                           ----localSmallWidth -----
        //      |                                                                                                   |
        //      |                                                                                                   |
        //      |                                                                                                   |
        //      |                                                                                                   |
        //      |                                                                                                   |
        //      |                                                                                                   |
        //      |                                                                                                   |
        //      -------------------------------------localWidth------------------------------------------------------
        //                                          
        //**********************************************************************************************************************
        public int init(int frameRate, int sendBitRate, int sendWidth, int sendHeight, int localWidth, int localHeight, int localSmallWidth, int localSmallHeight)
        {
            return Media_Init(media, frameRate, sendBitRate, sendWidth, sendHeight, localWidth, localHeight, localSmallWidth, localSmallHeight);
        }

        //**********************************************************************************************************************
        //usage: start push stream to media server byte rtmp protocol
        //para:
        //[in] url              推流的url值（rtmp://XXXXXX）
        //return:
        //0为正常，非0为异常                                      
        //**********************************************************************************************************************
        public int startPushStream(string url)
        {
            return Media_StartPushStream(media, url);
        }

        //**********************************************************************************************************************
        //usage: stop push stream to media server byte rtmp protocol
        //para:
        //return:                                     
        //**********************************************************************************************************************
        public void stopPushStream()
        {
            Media_StopPushStream(media);
        }

        //**********************************************************************************************************************
        //usage: 
        //set push video stream index
        //para:
        //[in] index (0:desktop, 1:interCamera, 2:usbCamera)
        //return:
        //0为正常，非0为异常                                      
        //**********************************************************************************************************************
        public int setSendStreamIndex(int index)
        {
            return Media_SetSendStreamIndex(media, index);
        }

        //**********************************************************************************************************************
        //usage: 
        //get push video stream index
        //para:
        //return:
        //0:desktop, 1:interCamera, 2:usbCamera                                     
        //**********************************************************************************************************************
        public int getSendStreamIndex()
        {
            return Media_GetSendStreamIndex(media);
        }

        //**********************************************************************************************************************
        //usage: 
        //set local show video stream index
        //para:
        //[in] index (0:desktop, 1:interCamera, 2:usbCamera)
        //return:
        //0为正常，非0为异常                                      
        //**********************************************************************************************************************
        public int setLocalStreamIndex(int index)
        {
            return Media_SetLocalStreamIndex(media, index);
        }

        //**********************************************************************************************************************
        //usage: 
        //get local show video stream index
        //para:
        //return:
        //0:desktop, 1:interCamera, 2:usbCamera                                     
        //**********************************************************************************************************************
        public int getLocalStreamIndex()
        {
            return Media_GetLocalStreamIndex(media);
        }

        //**********************************************************************************************************************
        //usage: 
        //get local video show data
        //para:
        //[out]data (one video frame data)
        //return:
        //video frame data size (if this value <= 0, it means this act is fail, but you can get continue)                                      
        //**********************************************************************************************************************
        public int getPlayData(byte[] data)
        {
            return Media_GetPlayData(media, data);
        }

        //**********************************************************************************************************************
        //usage: 
        //set local show or hide small video stream
        //para:
        //[in] needShowMain(0 隐藏, 非0 显示，对象是本地播放视频中，主画面)
        //[in] needShowA(0 隐藏, 非0 显示，对象是本地播放视频中，最右上角的小画面)
        //[in] needShowB(0 隐藏, 非0 显示，对象是本地播放视频中，右上角的第二个小画面)
        //return:                                    
        //**********************************************************************************************************************
        public void setShowSmallStream(int needShowMain, int needShowA, int needShowB)
        {
            Media_SetShowSmallStream(media, needShowMain, needShowA, needShowB);
        }

        //**********************************************************************************************************************
        //usage: 
        //get local show or hide small video stream
        //para:
        //[out] needShowMain(0 隐藏, 非0 显示，对象是本地播放视频中，主画面)
        //[out] needShowA(0 隐藏, 非0 显示，对象是本地播放视频中，最右上角的小画面)
        //[out] needShowB(0 隐藏, 非0 显示，对象是本地播放视频中，右上角的第二个小画面)
        //return:                                 
        //**********************************************************************************************************************
        public void getShowSmallStream(out int needShowMain, out int needShowA, out int needShowB)
        {
            needShowMain = 0;
            needShowA = 0;
            needShowB = 0;
            Media_GetShowSmallStream(media, out needShowMain, out needShowA, out needShowB);
        }

        [DllImport("libMedia.dll", EntryPoint = "Media_CreatePtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Media_CreatePtr();
        [DllImport("libMedia.dll", EntryPoint = "Media_DeletePtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Media_DeletePtr(IntPtr m);
        [DllImport("libMedia.dll", EntryPoint = "Media_Init", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Media_Init(IntPtr m, int frameRate, int sendBitRate, int sendWidth, int sendHeight, int localWidth, int localHeight, int localSmallWidth, int localSmallHeight);
        [DllImport("libMedia.dll", EntryPoint = "Media_StartPushStream", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Media_StartPushStream(IntPtr m, string url);
        [DllImport("libMedia.dll", EntryPoint = "Media_StopPushStream", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Media_StopPushStream(IntPtr m);

        [DllImport("libMedia.dll", EntryPoint = "Media_SetSendStreamIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Media_SetSendStreamIndex(IntPtr m, int index);
        [DllImport("libMedia.dll", EntryPoint = "Media_GetSendStreamIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Media_GetSendStreamIndex(IntPtr m);
        [DllImport("libMedia.dll", EntryPoint = "Media_SetLocalStreamIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Media_SetLocalStreamIndex(IntPtr m, int index);
        [DllImport("libMedia.dll", EntryPoint = "Media_GetLocalStreamIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Media_GetLocalStreamIndex(IntPtr m);

        [DllImport("libMedia.dll", EntryPoint = "Media_GetPlayData", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Media_GetPlayData(IntPtr m, byte[] data);

        [DllImport("libMedia.dll", EntryPoint = "Media_SetShowSmallStream", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Media_SetShowSmallStream(IntPtr m, int needShowMain, int needShowA, int needShowB);
        [DllImport("libMedia.dll", EntryPoint = "Media_GetShowSmallStream", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Media_GetShowSmallStream(IntPtr m, out int needShowMain, out int needShowA, out int needShowB);
    }
}
