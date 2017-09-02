#include "mediaInterface.h"

Media* Media_CreatePtr()
{
	//cout << "into " << __FUNCTION__ << endl;
	return new Media();
}

void Media_DeletePtr(Media *m)
{
	//cout << "into " << __FUNCTION__ << endl;
	if (m)
	{
		delete m;
	}
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}

int Media_Init(Media *m, int frameRate, int sendBitRate, int sendWidth, int sendHeight, int localWidth, int localHeight, int localSmallWidth, int localSmallHeight)
{
	//cout << "into " << __FUNCTION__ << endl;
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return -1;
	}

	return m->init(frameRate, sendBitRate, sendWidth, sendHeight, localWidth, localHeight, localSmallWidth, localSmallHeight);
}

int Media_StartPushStream(Media *m, const char *url)
{
	//cout << "into " << __FUNCTION__ << endl;
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return -1;
	}

	return m->startPushStream(url);
}

void Media_StopPushStream(Media *m)
{
	//cout << "into " << __FUNCTION__ << endl;
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return;
	}

	m->stopPushStream();
}

int Media_SetSendStreamIndex(Media *m, int index)
{
	//cout << "into " << __FUNCTION__ << endl;
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return -1;
	}

	return m->setSendStreamIndex(index);
}

int Media_GetSendStreamIndex(Media *m)
{
	//cout << "into " << __FUNCTION__ << endl;
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return -1;
	}

	return m->getSendStreamIndex();
}

int Media_SetLocalStreamIndex(Media *m, int index)
{
	//cout << "into " << __FUNCTION__ << endl;
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return -1;
	}

	return m->setLocalStreamIndex(index);
}

int Media_GetLocalStreamIndex(Media *m)
{
	//cout << "into " << __FUNCTION__ << endl;
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return -1;
	}

	return m->getLocalStreamIndex();
}

int Media_GetPlayData(Media *m, unsigned char *data)
{
	//cout << "into " << __FUNCTION__ << endl;
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return -1;
	}

	return m->getPlayData(data);
}

void Media_SetShowSmallStream(Media *m, int needShowA, int needShowB)
{
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return;
	}

	m->setShowSmallStream(needShowA, needShowB);
}

void Media_GetShowSmallStream(Media *m, int *needShowA, int *needShowB)
{
	if (!m)
	{
		cout << "m is NULL !" << endl;
		return;
	}

	m->getShowSmallStream(needShowA, needShowB);
}
