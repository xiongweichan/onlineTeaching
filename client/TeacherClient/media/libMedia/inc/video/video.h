#pragma once

#include "common.h"
#include "getDeviceName.h"
#include "desktop.h"
#include "interCamera.h"
#include "externCamera.h"
#include "encoder.h"
#include "yuvZoom.h"
#include "yuv2bmp.h"
#include "../push/push.h"


typedef void(*VideoSendCallBackType)(unsigned char *data, const int dataSize, PushStream *push);

class Video
{
#define MAX_RESOLUTION	(1920 * 1080)
#define YUV_BUF_SIZE	(MAX_RESOLUTION * 3 / 2)
#define BMP_BUF_SIZE	(MAX_RESOLUTION * 3 + 99)
#define H264_BUF_SIZE	(1024 * 1024)

#define AV_FRAME_CACHE_NUM	(32 * 1024)
#define AV_FRAME_CACHE_SIZE	(32 * 1024 * 1024)

public:
	Video();
	~Video();
	int init(const int frameRate, const int sendBitRate, \
		const int sendWidth, const int sendHeight, const int localWidth, const int localHeight, \
		const int localSmallWidth, const int localSmallHeight, \
		const VideoSendCallBackType sf, PushStream *push, \
		const char *desktopName, const char *interCameraName, const char *externCameraName);
	int setSendStreamIndex(const int index);	//0:desktop, 1:interCamera, 2:externCamera 
	int getSendStreamIndex() const;				//0:desktop, 1:interCamera, 2:externCamera 
	int setLocalStreamIndex(const int index);	//0:desktop, 1:interCamera, 2:externCamera 
	int getLocalStreamIndex() const;			//0:desktop, 1:interCamera, 2:externCamera 
	int getPlayData(unsigned char *data);
	void setShowSmallStream(const int needShowA, const int needShowB);
	void getShowSmallStream(int *needShowA, int *needShowB) const;

	void run();
	//static void* RunThread(void *arg);

private:
	inline bool isIllegal(const int l);
	int initPara(const int frameRate, const int sendBitRate, \
		const int sendWidth, const int sendHeight, const int localWidth, const int localHeight, \
		const int localSmallWidth, const int localSmallHeight, \
		const VideoSendCallBackType sf, PushStream *push, \
		const char *desktopName, const char *interCameraName, const char *externCameraName);
	void exit();
	void send();
	void compose();
	void play();
	int runOne();

	Desktop d;
	InterCamera ic;
	ExternCamera ec;
	Encoder e;
	PushStream *p;
	YuvZoom yDesktop, yInterCamera, yExternCamera, yPush;
	yuv2bmp y2b;
	int fr, sendbr, sendw, sendh, localw, localh;
	VideoSendCallBackType sendFunc;
	pthread_t workThreadid;
	bool threadRun;
	int sendStreamIndex, localStreamIndex;
	int localSw, localSh;
	int localYUVFrameSize;
	unsigned char yuvDesktop[YUV_BUF_SIZE], yuvInterCamera[YUV_BUF_SIZE], yuvExternCamera[YUV_BUF_SIZE];
	unsigned char yuvSmallA[YUV_BUF_SIZE], yuvSmallB[YUV_BUF_SIZE];
	unsigned char yuvLocal[YUV_BUF_SIZE], bmpLocal[BMP_BUF_SIZE];
	unsigned char yuvSend[YUV_BUF_SIZE], h264Send[H264_BUF_SIZE];
	unsigned char *yuvTmpSend, *yuvLocalMain, *yuvLocalA, *yuvLocalB;
	YuvZoom *zoomLocalA, *zoomLocalB;
	int h264Bytes;

	BlockInfo playDataBlock;

	bool showA, showB;
};

