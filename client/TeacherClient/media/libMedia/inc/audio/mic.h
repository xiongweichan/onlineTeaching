#pragma once

#include "common.h"

class Mic
{
public:
	Mic();
	~Mic();

	int init(const char *micName);
	int getFramePCM(unsigned char *pcm);

private:
	void exit();

	char devName[128];
	AVFormatContext *grabCtx;
	AVPacket packet;
};

