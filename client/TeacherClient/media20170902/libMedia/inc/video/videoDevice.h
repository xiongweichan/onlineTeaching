#pragma once

#include "common.h"


class VideoDevice
{
public:
	VideoDevice();
	~VideoDevice();

	int init(const int frameRate, const int width, const int height, const char *cameraDeviceName);
	int getYUVFrameSize();
	int getFrame(unsigned char *yuv);

	virtual void setGrabDeviceName();
	char grabDeviceName[32];
	char cameraName[64];

private:
	int initPara(const int frameRate, const int width, const int height);
	void exit();

	int openInputStream(const char *cameraDeviceName);

	AVFormatContext *grabCtx;
	AVCodecContext *codecCtx;
	SwsContext *bmp2yuvCtx;
	int videoIndex;

	AVPacket packet;
	AVFrame	*bmpFrame, *yuvFrame;
	int bmpFrameSize, yuvFrameSize;

	int yuvFrameRate;
	int yuvWidth, yuvHeight, yuvArea;
	int yuvStep[4];
	unsigned char **outCache;

	bool finishInit;
};

