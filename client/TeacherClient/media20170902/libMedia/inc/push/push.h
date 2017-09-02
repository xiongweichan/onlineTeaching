#pragma once

#include "pushConstPara.h"

class PushStream: public PushConstPara
{
public:
	PushStream();
	~PushStream();

	int init();
	int start(const char *url, const unsigned int frameRate);
	void stop();

	int video(unsigned char *data, int size);
	int audio(const unsigned char *data, const int size);

	void sendPackage();
	void reconnect();
	void pushData(const unsigned char *pData, const int dataSize);
	//static void dealBlockData(const unsigned char *pData, const int dataSize, void *arg);
	//static void* sendPackageThread(void *arg);
	//static void* reconnectThread(void *arg);

private:
	void exit();

	void restart();
	void createAudioSpecificConfig(unsigned char *pAudioSpecificConfig, unsigned int *pAudioSpecificConfigSize);
	unsigned char getSamplingFrequencyIndex(const unsigned int sampleRate);
	int push_rtmp_send_h264_head();
	int push_rtmp_send_video(const unsigned char *data, const unsigned int size);
	int push_rtmp_send_aac_head();
	int push_rtmp_send_audio(const unsigned char *data, const unsigned int size);
	void pushDataToBlock(PackageType type, const unsigned char *pData, const int dataSize, BlockInfo *pBlock);

	pthread_rwlock_t sg_lock;
	char sg_url[1024];
	int sg_frameRate;
	pthread_t sg_reconnectPid;
	PUSH_RTMP_INFO push_rtmp_info;
	bool isPushing;
	bool finishInit;
};


void pushVideo(unsigned char *data, const int size, PushStream *p);
void pushAudio(const unsigned char *data, const int size, PushStream *p);


