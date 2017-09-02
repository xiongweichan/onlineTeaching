#include "interface.h"

MediaInterface::MediaInterface()
{
	hdll = LoadLibrary(L"libMedia.dll");
	if (!hdll)
	{
		cout << "LoadLibrary fail !" << endl;
		return;
	}

	cf = (Media_CreatePtr)GetProcAddress(hdll, "Media_CreatePtr");
	if (!cf)
	{
		cout << "can not find Media_CreatePtr from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	df = (Media_DeletePtr)GetProcAddress(hdll, "Media_DeletePtr");
	if (!df)
	{
		cout << "can not find Media_DeletePtr from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	iif = (Media_Init)GetProcAddress(hdll, "Media_Init");
	if (!df)
	{
		cout << "can not find Media_Init from libMedia.dll" << endl;
		FreeLibrary(hdll);
		return;
	}

	startf = (Media_StartPushStream)GetProcAddress(hdll, "Media_StartPushStream");
	if (!startf)
	{
		cout << "can not find Media_StartPushStream from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	stopf = (Media_StopPushStream)GetProcAddress(hdll, "Media_StopPushStream");
	if (!stopf)
	{
		cout << "can not find Media_StopPushStream from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	ssif = (Media_SetSendStreamIndex)GetProcAddress(hdll, "Media_SetSendStreamIndex");
	if (!ssif)
	{
		cout << "can not find Media_SetSendStreamIndex from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	gsif = (Media_GetSendStreamIndex)GetProcAddress(hdll, "Media_GetSendStreamIndex");
	if (!gsif)
	{
		cout << "can not find Media_GetSendStreamIndex from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	slif = (Media_SetLocalStreamIndex)GetProcAddress(hdll, "Media_SetLocalStreamIndex");
	if (!slif)
	{
		cout << "can not find Media_SetLocalStreamIndex from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	glif = (Media_GetLocalStreamIndex)GetProcAddress(hdll, "Media_GetLocalStreamIndex");
	if (!glif)
	{
		cout << "can not find Media_GetLocalStreamIndex from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	gpdf = (Media_GetPlayData)GetProcAddress(hdll, "Media_GetPlayData");
	if (!gpdf)
	{
		cout << "can not find Media_GetPlayData from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	ssss = (Media_SetShowSmallStream)GetProcAddress(hdll, "Media_SetShowSmallStream");
	if (!ssss)
	{
		cout << "can not find Media_SetShowSmallStream from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	gsss = (Media_GetShowSmallStream)GetProcAddress(hdll, "Media_GetShowSmallStream");
	if (!gsss)
	{
		cout << "can not find Media_GetShowSmallStream from libMedia.dll" << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}

	m = cf();
	if (!m)
	{
		cout << "new Media fail ! " << endl;
		FreeLibrary(hdll);
		hdll = 0;
		return;
	}
}

MediaInterface::~MediaInterface()
{
	df(m);
	m = NULL;
	FreeLibrary(hdll);
	hdll = 0;
}

int MediaInterface::init(int frameRate, int sendBitRate, int sendWidth, int sendHeight, int localWidth, int localHeight, int localSmallWidth, int localSmallHeight)
{
	return iif(m, frameRate, sendBitRate, sendWidth, sendHeight, localWidth, localHeight, localSmallWidth, localSmallHeight);
}

int MediaInterface::startPushStream(const char *url)
{
	return startf(m, url);
}

void MediaInterface::stopPushStream()
{
	stopf(m);
}

int MediaInterface::setSendStreamIndex(const int index)
{
	return ssif(m, index);
}

int MediaInterface::getSendStreamIndex()
{
	return gsif(m);
}

int MediaInterface::setLocalStreamIndex(const int index)
{
	return slif(m, index);
}

int MediaInterface::getLocalStreamIndex()
{
	return glif(m);
}

int MediaInterface::getPlayData(unsigned char *data)
{
	return gpdf(m, data);
}

void MediaInterface::setShowSmallStream(const int needShowA, const int needShowB)
{
	return ssss(m, needShowA, needShowB);
}

void MediaInterface::getShowSmallStream(int *needShowA, int *needShowB)
{
	return gsss(m, needShowA, needShowB);
}
