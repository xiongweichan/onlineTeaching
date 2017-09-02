#pragma once

#include "video/video.h"
#include "audio/audio.h"

class Media
{
public:
	Media();
	~Media();

	int init(const int frameRate, const int sendBitRate, \
		const int sendWidth, const int sendHeight, const int localWidth, const int localHeight, \
		const int localSmallWidth, const int localSmallHeight);

	int startPushStream(const char *url);
	void stopPushStream();

	int getPlayData(unsigned char *data);

	//0:desktop, 1:interCamera, 2:externCamera 
	int setSendStreamIndex(const int index);	
	int getSendStreamIndex() const;				
	int setLocalStreamIndex(const int index);
	int getLocalStreamIndex() const;		

	void setShowSmallStream(const int needShowA, const int needShowB);
	void getShowSmallStream(int *needShowA, int *needShowB) const;

private:
	void initPara(const int frameRate);

	void show_dshow_device();
	void show_dshow_device_option();
	void show_vfw_device();
	void getMediaDeviceName();
	bool getName(const char *keyNameInfo, const bool isVideo, char *saveNameBuf);
	const wchar_t* getAimName(const char *keyNameInfo, const TDeviceName *names, const int maxNameNum) const;

	void exit();

	Video v;
	Audio a;
	PushStream p;

	int fr;
	TDeviceName videoDeviceName[20], audioDeviceName[20];
	int videoDeviceNum, audioDeviceNum;
	char desktopName[128], interCameraName[128], externCameraName[128], micName[128], externMicName[128];
};

