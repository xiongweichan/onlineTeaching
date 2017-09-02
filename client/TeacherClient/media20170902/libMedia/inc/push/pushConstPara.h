#pragma once

#include "common.h"

#ifdef __cplusplus
extern "C" {
#endif

#include "rtmp_sys.h"
#include "rtmp.h"
#include "log.h"

#pragma comment(lib, "zlibstat.lib")
#pragma comment(lib, "libeay32.lib")
#pragma comment(lib, "ssleay32.lib")
#pragma comment(lib, "librtmp.lib")

#ifdef __cplusplus
}
#endif


class PushConstPara
{
public:
	PushConstPara();
	~PushConstPara();

	const int g_rtmpLinkTimeOut;
	const int64_t g_sendThreadWaitTime;

	const unsigned int g_rtmp_video_package_max_size;
	const unsigned int g_rtmp_audio_package_max_size;
	const unsigned int g_clockTicks;

	const int g_video_channel_number;
	const int g_audio_channel_number;

	const unsigned int g_minVideoFrameRate;
	const unsigned int g_maxVideoFrameRate;

	const unsigned int g_aacSampleRateFrequency[18];
	const unsigned int g_sampleRate;
	const unsigned int g_samplePerSecond;
	const unsigned char g_audioObjectType;			//5bit

	const unsigned char g_channelConfiguration;		//4bit
	const unsigned char g_frameLengthFlag;			//1bit	
	const unsigned char g_dependsOnCoreCoder;		//1bit	
	const unsigned char g_extensionFlag;			//1bit	
	const unsigned int g_audioSpecificConfigSize;	//above add together, bytes

	const unsigned int g_maxDisConnectTimes;
	const unsigned int g_reconnectCheckTimeInterval;

	enum
	{
		NAL_SLICE = 1,
		NAL_DPA,
		NAL_DPB,
		NAL_DPC,
		NAL_IDR_SLICE,
		NAL_SEI,
		NAL_SPS,
		NAL_PPS,
		NAL_AUD,
		NAL_END_SEQUENCE,
		NAL_END_STREAM,
		NAL_FILLER_DATA,
		NAL_SPS_EXT,
		NAL_AUXILIARY_SLICE = 19
	};

	typedef enum
	{
		eVideoHead = 0,
		eAudioHead,
		eVideoData,
		eAudioData,
		eMaxPackageType,
	}PackageType;

	typedef struct _push_rtmp_info
	{
		char rtmp_url[1024];
		RTMP *rtmp_obj;
		unsigned long start_time;
		pthread_t thread_id;
		unsigned int threadIsRunning;
		BlockInfo packageBlock;

		RTMPPacket *video_packet_obj;
		unsigned int videoFrameRate;
		unsigned char sps[128];
		unsigned char pps[128];
		unsigned int sps_size;
		unsigned int pps_size;
		unsigned int videoFrameIndex;
		unsigned int videoFrameTick;
		unsigned char hasSendVideoHead;
		int video_channel_number;
		unsigned int lastVideoTimeStamp;
		unsigned int makeUpVideoTimeStamp;
		unsigned int makeUpVideoInternal;
		unsigned int nowMakeUpVideoTimeStamp;

		RTMPPacket *audio_packet_obj;
		unsigned int audioFrameRate;
		unsigned int audioFrameIndex;
		unsigned int audioFrameTick;	//44100Hz, 1024sample per frame
		unsigned char hasSendAudioHead;
		unsigned char audioSpecificConfig[2];
		unsigned int audioSpecificConfigSize;
		int audio_channel_number;
		unsigned int lastAudioTimeStamp;
		unsigned int makeUpAudioTimeStamp;
		unsigned int makeUpAudioInternal;
		unsigned int nowMakeUpAudioTimeStamp;

		unsigned int disConnectErrTimes;
	}PUSH_RTMP_INFO;
};
