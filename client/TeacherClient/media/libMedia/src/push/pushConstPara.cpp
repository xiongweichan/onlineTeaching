#include "push/pushConstPara.h"

PushConstPara::PushConstPara() :
	g_rtmpLinkTimeOut(5), g_sendThreadWaitTime(2),
	g_rtmp_video_package_max_size(2 * 1024 * 1024), g_rtmp_audio_package_max_size(2 * 1024 * 1024), g_clockTicks(1000),
	g_video_channel_number(0x04), g_audio_channel_number(0x05),
	g_minVideoFrameRate(10), g_maxVideoFrameRate(100),
	g_aacSampleRateFrequency{ 96000, 88200, 64000, 48000, 44100, 32000, 24000, 22050, 16000, 12000, 11025, 8000, 8000, 8000, 8000, 8000, 8000, 8000 },
	g_sampleRate(44100), g_samplePerSecond(1024),
	g_audioObjectType(2), g_channelConfiguration(2), g_frameLengthFlag(0), g_dependsOnCoreCoder(0), g_extensionFlag(0), g_audioSpecificConfigSize(2),
	g_maxDisConnectTimes(3), g_reconnectCheckTimeInterval(2)
{

}

PushConstPara::~PushConstPara()
{
}
