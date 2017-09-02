#include "video/video.h"

static void* RunThread(void *arg);

Video::Video()
{
}

Video::~Video()
{
	exit();
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}

inline bool Video::isIllegal(const int l)
{
	if (l <= 0 || l > 2048)
	{
		return true;
	}

	return false;
}

int Video::initPara(const int frameRate, const int sendBitRate, \
	const int sendWidth, const int sendHeight, const int localWidth, const int localHeight, \
	const int localSmallWidth, const int localSmallHeight, \
	const VideoSendCallBackType sf, PushStream *push, \
	const char *desktopName, const char *interCameraName, const char *externCameraName)
{
	if (frameRate <= 0 || frameRate > 300)
	{
		return -1;
	}

	if (isIllegal(sendWidth) || isIllegal(sendHeight) || isIllegal(localWidth) || isIllegal(localHeight) || isIllegal(localSmallWidth) || isIllegal(localSmallHeight) || \
		!sf || !push)
	{
		return -2;
	}

	if (localSmallWidth > localWidth || (localSmallHeight * 2 > localHeight))
	{
		return -3;
	}

	if (!desktopName || !interCameraName || !externCameraName)
	{
		return -4;
	}

	showA = false;
	showB = true;

	fr = frameRate;
	sendbr = sendBitRate;
	localw = localWidth;
	localh = localHeight;
	sendw = sendWidth;
	sendh = sendHeight;
	localSw = localSmallWidth;
	localSh = localSmallHeight;

	sendStreamIndex = 0;
	localStreamIndex = 0;

	sendFunc = sf;
	p = push;

	memset(yuvDesktop, 0, sizeof(yuvDesktop));
	memset(yuvInterCamera, 0, sizeof(yuvInterCamera));
	memset(yuvExternCamera, 0, sizeof(yuvExternCamera));
	memset(yuvSmallA, 0, sizeof(yuvSmallA));
	memset(yuvSmallB, 0, sizeof(yuvSmallB));
	memset(yuvLocal, 0, sizeof(yuvLocal));
	memset(bmpLocal, 0, sizeof(bmpLocal));
	memset(yuvSend, 0, sizeof(yuvSend));
	memset(h264Send, 0, sizeof(h264Send));

	int ret;
	ret = InitBlockInfoBuffer(&playDataBlock, AV_FRAME_CACHE_NUM, AV_FRAME_CACHE_SIZE, NULL, 0);
	if (ret < 0)
	{
		//cout << "InitBlockInfoBuffer fail !" << endl;
		return -5;
	}

	return 0;
}

int Video::init(const int frameRate, const int sendBitRate, \
	const int sendWidth, const int sendHeight, const int localWidth, const int localHeight, \
	const int localSmallWidth, const int localSmallHeight, \
	const VideoSendCallBackType sf, PushStream *push, \
	const char *desktopName, const char *interCameraName, const char *externCameraName)
{
	threadRun = false;

	int ret;
	ret = initPara(frameRate,sendBitRate,sendWidth,sendHeight,localWidth,localHeight,localSmallWidth,localSmallHeight,sf,push,desktopName,interCameraName,externCameraName);
	if (ret < 0)
	{
		return -1;
	}

	ret = d.init(fr, localw, localh, desktopName);
	if (ret < 0)
	{
		cout << "d.init fail ! ret = " << ret << endl;
		return -2;
	}

	ret = ic.init(fr, localw, localh, interCameraName);
	if (ret < 0)
	{
		cout << "ic.init fail ! ret = " << ret << endl;
		//return -3;
	}

	ret = ec.init(fr, localw, localh, externCameraName);
	if (ret < 0)
	{
		cout << "ec.init fail ! ret = " << ret << endl;
		//return -3;
	}

	ret = e.init(sendw, sendHeight, fr, sendbr);
	if (ret < 0)
	{
		cout << "e.init fail ! ret = " << ret << endl;
		return -4;
	}

	ret = yDesktop.init(localw, localh, localSw, localSh);
	if (ret < 0)
	{
		cout << "yDesktop.init fail ! ret = " << ret << endl;
		return -5;
	}

	ret = yInterCamera.init(localw, localh, localSw, localSh);
	if (ret < 0)
	{
		cout << "yInterCamera.init fail ! ret = " << ret << endl;
		return -6;
	}

	ret = yExternCamera.init(localw, localh, localSw, localSh);
	if (ret < 0)
	{
		cout << "yExternCamera.init fail ! ret = " << ret << endl;
		return -7;
	}

	ret = yPush.init(localw, localh, sendw, sendh);
	if (ret < 0)
	{
		cout << "yPush.init fail ! ret = " << ret << endl;
		return -8;
	}

	ret = y2b.init(localw, localh, localw, localh);
	if (ret < 0)
	{
		cout << "y2b.init fail ! ret = " << ret << endl;
		return -9;
	}

	localYUVFrameSize = y2b.getYUVFrameSize();

	threadRun = true;
	ret = pthread_create(&workThreadid, NULL, RunThread, (void *)this);
	if (ret < 0)
	{
		threadRun = false;
		cout << "pthread_create fail ! ret = " << ret << endl;
		return -10;
	}

	return 0;
}

void Video::exit()
{
	threadRun = false;
	pthread_join(workThreadid, NULL);

	sendFunc = NULL;
	releaseBlock(&playDataBlock);
}

static void* RunThread(void *arg)
{
	Video *v = (Video *)arg;
	v->run();

	return NULL;
}

void Video::run()
{
	while (threadRun)
	{
		runOne();
	}
}

int Video::runOne()
{
	//cout << "into " << __FUNCTION__ << endl;

	d.getFrame(yuvDesktop);
	ic.getFrame(yuvInterCamera);
	ec.getFrame(yuvExternCamera);

	send();
	play();

	//usleep(20 * 1000);

	//cout << "exit " << __FUNCTION__ << endl;
	return 0;
}

void Video::send()
{
	//cout << "into " << __FUNCTION__ << endl;
	int ret; 

	switch (sendStreamIndex)
	{
	case 0:
		yuvTmpSend = yuvDesktop;
		break;
	case 1:
		yuvTmpSend = yuvInterCamera;
		break;
	case 2:
		yuvTmpSend = yuvExternCamera;
		break;
	default:
		cout << "sendStreamIndex error ! sendStreamIndex = " << sendStreamIndex << ", fix it to 0 !" << endl;
		sendStreamIndex = 0;
		yuvTmpSend = yuvDesktop;
		break;
	}

	ret = yPush.convert(yuvTmpSend, yuvSend);
	if (ret < 0)
	{
		cout << "yPush.convert fail ! ret = " << ret << endl;
	}
	else
	{
		h264Bytes = e.frameCode(yuvSend, h264Send);
		if (h264Bytes > 0)
		{
			sendFunc(h264Send, h264Bytes, p);
		}
	}
}

void Video::play()
{
	//cout << "into " << __FUNCTION__ << endl;
	switch (localStreamIndex)
	{
	case 0:
		yuvLocalMain = yuvDesktop;
		yuvLocalA = yuvInterCamera;
		yuvLocalB = yuvExternCamera;
		zoomLocalA = &yInterCamera;
		zoomLocalB = &yExternCamera;
		break;
	case 1:
		yuvLocalMain = yuvInterCamera;
		yuvLocalA = yuvDesktop;
		yuvLocalB = yuvExternCamera;
		zoomLocalA = &yDesktop;
		zoomLocalB = &yExternCamera;
		break;
	case 2:
		yuvLocalMain = yuvExternCamera;
		yuvLocalA = yuvDesktop;
		yuvLocalB = yuvInterCamera;
		zoomLocalA = &yDesktop;
		zoomLocalB = &yInterCamera;
		break;
	default:
		cout << "localStreamIndex error ! localStreamIndex = " << localStreamIndex << ", fix it to 0 !" << endl;
		localStreamIndex = 0;
		yuvLocalMain = yuvDesktop;
		yuvLocalA = yuvInterCamera;
		yuvLocalB = yuvExternCamera;
		zoomLocalA = &yInterCamera;
		zoomLocalB = &yExternCamera;
		break;
	}

	compose();

	int ret;
	ret = pushBlock(&playDataBlock, yuvLocal, localYUVFrameSize);
	if (ret < 0)
	{
		//cout << __FUNCTION__ ":pushBlock fail ! ret = " << ret << endl;
	}
}

void Video::compose()
{
	copyVideo(yuvLocalMain, localw, localh, 0, 0, localw - localSw, localSh * 2, yuvLocal, localw, localh, 0, 0);
	copyVideo(yuvLocalMain, localw, localh, 0, localSh * 2, localw, localh - localSh * 2, yuvLocal, localw, localh, 0, localSh * 2);

	int ret;
	if (showA)
	{
		ret = zoomLocalA->convert(yuvLocalA, yuvSmallA);
		if (ret < 0)
		{
			cout << "zoomLocalA->convert fail ! ret = " << ret << endl;
		}
		copyVideo(yuvSmallA, localSw, localSh, 0, 0, localSw, localSh, yuvLocal, localw, localh, localw - localSw, 0);
	}
	else
	{
		copyVideo(yuvLocalMain, localw, localh, localw - localSw, 0, localSw, localSh, yuvLocal, localw, localh, localw - localSw, 0);
	}
	
	if (showB)
	{
		ret = zoomLocalB->convert(yuvLocalB, yuvSmallB);
		if (ret < 0)
		{
			cout << "zoomLocalB->convert fail ! ret = " << ret << endl;
		}
		copyVideo(yuvSmallB, localSw, localSh, 0, 0, localSw, localSh, yuvLocal, localw, localh, localw - localSw, localSh);
	}
	else
	{
		copyVideo(yuvLocalMain, localw, localh, localw - localSw, localSh, localSw, localSh, yuvLocal, localw, localh, localw - localSw, localSh);
	}
}

int Video::setSendStreamIndex(const int index)
{
	if (index < 0 || index > 2)
	{
		return -1;
	}

	sendStreamIndex = index;
	return 0;
}

int Video::getSendStreamIndex() const
{
	return sendStreamIndex;
}

int Video::setLocalStreamIndex(const int index)
{
	if (index < 0 || index > 2)
	{
		return -1;
	}

	localStreamIndex = index;
	return 0;
}

int Video::getLocalStreamIndex() const
{
	return localStreamIndex;
}

int Video::getPlayData(unsigned char *data)
{
	if (!data)
	{
		return -1;

	}

	if (threadRun == false)
	{
		return -2;
	}

	struct timespec ts;
	struct timeval  tv;
	gettimeofday(&tv, NULL);
	ts.tv_sec = tv.tv_sec + 1;
	ts.tv_nsec = tv.tv_usec * 1000;
	sem_timedwait(&playDataBlock.m_Semaphore, &ts);

	BufferInfo *b = playDataBlock.m_Pop;
	if (b == NULL || b->nFlag == NULL)
	{
		//cout << __FUNCTION__ << ":playDataBlock status error ! playDataBlock.m_Pop = " << b << endl;
		return -2;
	}

	int dataSize = y2b.convert(b->pData, data);
	if (dataSize < 0)
	{
		cout << "y2b.convert fail ! dataSize = " << dataSize << endl;
	}
	
	pthread_mutex_lock(&(playDataBlock.m_Lock));
	b->nFlag = 0;
	playDataBlock.m_Pop = b->pNext;
	pthread_mutex_unlock(&(playDataBlock.m_Lock));

	return dataSize;
}

void Video::setShowSmallStream(const int needShowA, const int needShowB)
{
	showA = needShowA ? true : false;
	showB = needShowB ? true : false;
}

void Video::getShowSmallStream(int *needShowA, int *needShowB) const
{
	if (!needShowA || !needShowB)
	{
		cout << __FUNCTION__ << ":para error ! needShowA = " << needShowA << ", needShowB = " << needShowB << endl;
		return;
	}

	*needShowA = showA ? 1 : 0;
	*needShowB = showB ? 1 : 0;
}