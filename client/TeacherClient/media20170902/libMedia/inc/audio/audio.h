#pragma once

#include "common.h"
#include "getDeviceName.h"
#include "mic.h"
#include "aac.h"
#include "push/push.h"

typedef void(*AudioSendCallBackType)(const unsigned char *data, const int dataSize, PushStream *push);

class Audio
{
//#define SAVE_AAC_FILE
public:
	Audio();
	~Audio();
	
	int init(const AudioSendCallBackType f, PushStream *push, const char *micName, const char *externMicName);
	bool getThreadRunStatus();

	void run();

	//static void* RunThread(void *arg);

private:
	void exit();
	int runOne();

	Mic lm, em;
	AAC a;
	PushStream *p;

#ifdef SAVE_AAC_FILE
	FILE *fp_aac;
#endif

	unsigned char pcmData[4096 * 40];
	unsigned char aacData[4096];
	int pcmSize, encodeBytes;
	int waitEncodeDataBytes;
	unsigned char *waitEncodeData;
	AudioSendCallBackType sendFunc;
	pthread_t workThreadPid;
	bool threadRun;
};
