#include "media.h"

using namespace std;

#ifndef __iob_func_fix_
#define __iob_func_fix_
extern "C" { FILE __iob_func[3] = { *stdin,*stdout,*stderr }; }
#endif


Media::Media()
{
}

Media::~Media()
{
	exit();
	//cout << "exit " << __FUNCTION__ << endl;
}

int Media::init(const int frameRate, const int sendBitRate, \
	const int sendWidth, const int sendHeight, const int localWidth, const int localHeight, \
	const int localSmallWidth, const int localSmallHeight)
{
	initPara(frameRate);
	getMediaDeviceName();

	int ret;
	ret = p.init();
	if (ret < 0)
	{
		cout << "push->init fail ! ret = " << ret << endl;
		return -2;
	}

	ret = v.init(frameRate,sendBitRate,sendWidth,sendHeight,localWidth,localHeight,localSmallWidth,localSmallHeight,pushVideo,&p,desktopName,interCameraName,externCameraName);
	if (ret < 0)
	{
		cout << "video->init fail ! ret = " << ret << endl;
		//return -3;
	}

	ret = a.init(pushAudio, &p, micName, externMicName);
	if (ret < 0)
	{
		cout << "audio.init fail ! ret = " << ret << endl;
		//return -4;
	}

	//sleep(5);
	return 0;
}

void Media::initPara(const int frameRate)
{
	fr = frameRate;
	memset(videoDeviceName, 0, sizeof(videoDeviceName));
	memset(audioDeviceName, 0, sizeof(audioDeviceName));
	videoDeviceNum = 0;
	audioDeviceNum = 0;
	memset(desktopName, 0, sizeof(desktopName));
	memset(interCameraName, 0, sizeof(interCameraName));
	memset(externCameraName, 0, sizeof(externCameraName));
	memset(micName, 0, sizeof(micName));
	memset(externMicName, 0, sizeof(externMicName));
}

void Media::getMediaDeviceName()
{
	av_register_all();
	avformat_network_init();
	avdevice_register_all();

	show_dshow_device();
	show_dshow_device_option();
	show_vfw_device();

	getVideoDeviceNames(videoDeviceName, videoDeviceNum);
	getAudioDeviceNames(audioDeviceName, audioDeviceNum);

	strcpy(desktopName, "desktop");
	getName("Camera", true, interCameraName);
	getName("C920", true, externCameraName);
	getName("Realtek", false, micName);
	getName("C920", false, externMicName);

	char u[128];
	cout << __FUNCTION__ " result is:" << endl;
	cout << "video device " << videoDeviceNum << endl;
	for (int i = 0; i < videoDeviceNum; i ++)
	{
		wcharToUtf8(videoDeviceName[i].FriendlyName, u);
		cout << i << ": " << u << endl;
	}
	cout << "audio device " << audioDeviceNum << endl;
	for (int i = 0; i < audioDeviceNum; i++)
	{
		wcharToUtf8(audioDeviceName[i].FriendlyName, u);
		cout << i << ": " << u << endl;
	}

	cout << "device name:" << endl;
	cout << desktopName << " " << interCameraName << " " << externCameraName << endl;
	cout << micName << " " << externMicName << endl;
}

bool Media::getName(const char *keyNameInfo, const bool isVideo, char *saveNameBuf)
{
	TDeviceName *names;
	int maxNameNum = 20;
	if (isVideo == true)
	{
		names = videoDeviceName;
	}
	else
	{
		names = audioDeviceName;
	}

	const wchar_t *w = getAimName(keyNameInfo, names, maxNameNum);
	if (!w)
	{
		return false;
	}

	if (isVideo)
	{
		videoWcharToUtf8(w, saveNameBuf);
	}
	else
	{
		audioWcharToUtf8(w, saveNameBuf);
	}
	
	return true;
}

const wchar_t* Media::getAimName(const char *keyNameInfo, const TDeviceName *names, const int maxNameNum) const
{
	char u[128];
	for (int i = 0; i < maxNameNum; i++)
	{
		wcharToUtf8(names[i].FriendlyName, u);
		if (strstr(u, keyNameInfo))
		{
			//cout << "get getAimName success ! " << u << endl;
			return names[i].FriendlyName;
		}
	}

	return NULL;
}

void Media::show_dshow_device()
{
	AVFormatContext *tmpCtx = avformat_alloc_context();
	AVDictionary* options = NULL;
	av_dict_set(&options, "list_devices", "true", 0);
	AVInputFormat *iformat = av_find_input_format("dshow");
	printf("========Device Info=============\n");
	avformat_open_input(&tmpCtx, "video=dummy", iformat, &options);
	printf("================================\n");
	char logBuf[1024];
	av_strerror(0, logBuf, 1024);
	printf("logBuf: (%s)\n", logBuf);
	avformat_close_input(&tmpCtx);
	avformat_free_context(tmpCtx);
}

void Media::show_dshow_device_option()
{
	AVFormatContext *tmpCtx = avformat_alloc_context();
	AVDictionary* options = NULL;
	av_dict_set(&options, "list_options", "true", 0);
	AVInputFormat *iformat = av_find_input_format("dshow");
	printf("========Device Option Info======\n");
	avformat_open_input(&tmpCtx, "video=Integrated Camera", iformat, &options);
	printf("================================\n");
	char logBuf[1024];
	av_strerror(0, logBuf, 1024);
	printf("logBuf: (%s)\n", logBuf);
	avformat_close_input(&tmpCtx);
	avformat_free_context(tmpCtx);
}

void Media::show_vfw_device()
{
	AVFormatContext *tmpCtx = avformat_alloc_context();
	AVInputFormat *iformat = av_find_input_format("vfwcap");
	printf("========VFW Device Info======\n");
	avformat_open_input(&tmpCtx, "list", iformat, NULL);
	printf("=============================\n");
	char logBuf[1024];
	av_strerror(0, logBuf, 1024);
	printf("logBuf: (%s)\n", logBuf);
	avformat_close_input(&tmpCtx);
	avformat_free_context(tmpCtx);
}

int Media::getPlayData(unsigned char *data)
{
	return v.getPlayData(data);
}

int Media::startPushStream(const char *url)
{
	return p.start(url, fr);
}

void Media::stopPushStream()
{
	p.stop();
}

void Media::exit()
{
}

int Media::setSendStreamIndex(const int index)
{
	return v.setSendStreamIndex(index);
}

int Media::getSendStreamIndex() const
{
	return v.getSendStreamIndex();
}

int Media::setLocalStreamIndex(const int index)
{
	return v.setLocalStreamIndex(index);
}

int Media::getLocalStreamIndex() const
{
	return v.getLocalStreamIndex();
}

void Media::setShowSmallStream(const int needShowA, const int needShowB)
{
	v.setShowSmallStream(needShowA, needShowB);
}

void Media::getShowSmallStream(int *needShowA, int *needShowB) const
{
	v.getShowSmallStream(needShowA, needShowB);
}
