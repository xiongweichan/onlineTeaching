#pragma once
#include "mediaInterface.h"




class MediaInterface
{
private:
	typedef Media*(*Media_CreatePtr)();
	typedef void(*Media_DeletePtr)(Media*);
	typedef int(*Media_Init)(Media *m, int frameRate, int sendBitRate, int sendWidth, int sendHeight, int localWidth, int localHeight, int localSmallWidth, int localSmallHeight);
	typedef int(*Media_StartPushStream)(Media *m, const char *url);
	typedef void(*Media_StopPushStream)(Media *m);

	typedef int(*Media_SetSendStreamIndex)(Media *m, int index);
	typedef int(*Media_GetSendStreamIndex)(Media *m);
	typedef int(*Media_SetLocalStreamIndex)(Media *m, int index);
	typedef int(*Media_GetLocalStreamIndex)(Media *m);

	typedef int(*Media_GetPlayData)(Media *m, unsigned char *data);

	typedef void (*Media_SetShowSmallStream)(Media *m, int needShowA, int needShowB);
	typedef void (*Media_GetShowSmallStream)(Media *m, int *needShowA, int *needShowB);

public:
	MediaInterface();
	~MediaInterface();

	int init(int frameRate, int sendBitRate, int sendWidth, int sendHeight, int localWidth, int localHeight, int localSmallWidth, int localSmallHeight);
	int startPushStream(const char *url);
	void stopPushStream();
	int setSendStreamIndex(const int index);	//0:desktop, 1:interCamera, 2:externCamera 
	int getSendStreamIndex();					//0:desktop, 1:interCamera, 2:externCamera 
	int setLocalStreamIndex(const int index);	//0:desktop, 1:interCamera, 2:externCamera 
	int getLocalStreamIndex();					//0:desktop, 1:interCamera, 2:externCamera 
	int getPlayData(unsigned char *data);
	void setShowSmallStream(const int needShowA, const int needShowB);
	void getShowSmallStream(int *needShowA, int *needShowB);

private:
	HINSTANCE hdll;
	Media *m;

	Media_CreatePtr cf;
	Media_DeletePtr df;
	Media_Init iif;
	Media_StartPushStream startf;
	Media_StopPushStream stopf;
	Media_SetSendStreamIndex ssif;
	Media_GetSendStreamIndex gsif;
	Media_SetLocalStreamIndex slif;
	Media_GetLocalStreamIndex glif;
	Media_GetPlayData gpdf;
	Media_SetShowSmallStream ssss;
	Media_GetShowSmallStream gsss;
};


