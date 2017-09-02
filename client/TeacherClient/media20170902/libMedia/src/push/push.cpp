#include "push/push.h"

static void dealBlockData(const unsigned char *pData, const int dataSize, void *arg);
static void* sendPackageThread(void *arg);
static void* reconnectThread(void *arg);

void pushVideo(unsigned char *data, const int size, PushStream *p)
{
	p->video(data, size);
}

void pushAudio(const unsigned char *data, const int size, PushStream *p)
{
	p->audio(data, size);
}


PushStream::PushStream()
{

}

PushStream::~PushStream()
{
	exit();
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
	
}


int PushStream::init()
{
	//printf("+%s\n", __FUNCTION__);
	finishInit = false;
	isPushing = false;
	int ret = pthread_rwlock_init(&sg_lock, NULL);
	if (ret < 0)
	{
		printf("%s:pthread_rwlock_init fail !\n", __FUNCTION__);
		return -1;
	}

	memset(&push_rtmp_info, 0, sizeof(PUSH_RTMP_INFO));

	finishInit = true;
	ret = create_thread_normal(&sg_reconnectPid, reconnectThread, (void *)this);
	if (ret < 0)
	{
		printf("%s:create_thread_small fail !\n", __FUNCTION__);
		finishInit = false;
		return -2;
	}

	//printf("-%s\n", __FUNCTION__);
	return 0;
}

void PushStream::exit()
{
	//printf("+%s\n", __FUNCTION__);

	stop();
	finishInit = false;
	pthread_join(sg_reconnectPid, NULL);
	memset(&sg_reconnectPid, 0, sizeof(sg_reconnectPid));
	pthread_rwlock_destroy(&sg_lock);
	//printf("-%s\n", __FUNCTION__);
	return;
}

int PushStream::start(const char *url, const unsigned int frameRate)
{
	//printf("!!!!!!+%s!!!!!!!!!!!!\n", __FUNCTION__);
	if (!finishInit)
	{
		return -1;
	}
	
	pthread_rwlock_wrlock(&sg_lock);
	//printf("%s: get sg_lock success !\n", __FUNCTION__);

	if (NULL == url)
	{
		printf("invalid param url ! url == NULL\n");
		pthread_rwlock_unlock(&sg_lock);
		return -2;
	}
	if (frameRate < g_minVideoFrameRate || frameRate > g_maxVideoFrameRate)
	{
		printf("invalid param frameRate ! frameRate = %u\n", frameRate);
		pthread_rwlock_unlock(&sg_lock);
		return -3;
	}

	printf("push_rtmp_init ! url=%s, frameRate=%u\n", url, frameRate);
	RTMP *rtmp_obj = NULL;
	rtmp_obj = RTMP_Alloc();
	if (NULL == rtmp_obj)
	{
		printf("RTMP_Alloc failed!\n");
		pthread_rwlock_unlock(&sg_lock);
		return -4;
	}
	//printf("RTMP_Alloc success !\n");

	RTMP_Init(rtmp_obj);
	//printf("RTMP_Init success !\n");
	rtmp_obj->Link.timeout = g_rtmpLinkTimeOut;
	RTMP_SetupURL(rtmp_obj, (char*)url);
	RTMP_EnableWrite(rtmp_obj);
	//printf("prepare to RTMP_Connect !\n");
	int ret = RTMP_Connect(rtmp_obj, NULL);
	if (ret == 0)
	{
		printf("RTMP_Connect fail ! ret = %d\n", ret);
		RTMP_Close(rtmp_obj);
		RTMP_Free(rtmp_obj);
		pthread_rwlock_unlock(&sg_lock);
		printf("RTMP_Connect failed!\n");
		return -5;
	}
	//printf("RTMP_Connect success !\n");
	if (!RTMP_ConnectStream(rtmp_obj, 0))
	{
		RTMP_Close(rtmp_obj);
		RTMP_Free(rtmp_obj);
		pthread_rwlock_unlock(&sg_lock);
		printf("RTMP_ConnectStream fail !\n");
		return -6;
	}
	//printf("RTMP_ConnectStream success !\n");
	RTMPPacket *video_packet_obj = (RTMPPacket*)malloc(sizeof(RTMPPacket));
	if (NULL == video_packet_obj)
	{
		RTMP_Close(rtmp_obj);
		RTMP_Free(rtmp_obj);
		pthread_rwlock_unlock(&sg_lock);
		printf("malloc video RTMPPacket failed!\n");
		return -7;
	}
	memset(video_packet_obj, 0, sizeof(RTMPPacket));
	RTMPPacket_Alloc(video_packet_obj, g_rtmp_video_package_max_size);
	RTMPPacket_Reset(video_packet_obj);
	//printf("RTMPPacket_Alloc success !\n");
	RTMPPacket *audio_packet_obj = (RTMPPacket*)malloc(sizeof(RTMPPacket));
	if (NULL == audio_packet_obj)
	{
		RTMP_Close(rtmp_obj);
		RTMP_Free(rtmp_obj);
		RTMPPacket_Free(video_packet_obj);
		free(video_packet_obj);
		pthread_rwlock_unlock(&sg_lock);
		printf("malloc audio RTMPPacket failed!\n");
		return -8;
	}
	memset(audio_packet_obj, 0, sizeof(RTMPPacket));
	RTMPPacket_Alloc(audio_packet_obj, g_rtmp_audio_package_max_size);
	RTMPPacket_Reset(audio_packet_obj);
	//printf("RTMPPacket_Alloc success !\n");

	memset(&push_rtmp_info, 0, sizeof(PUSH_RTMP_INFO));
	strcpy(push_rtmp_info.rtmp_url, url);
	push_rtmp_info.rtmp_obj = rtmp_obj;

	push_rtmp_info.video_packet_obj = video_packet_obj;
	push_rtmp_info.videoFrameIndex = 1;
	push_rtmp_info.hasSendVideoHead = 0;
	push_rtmp_info.videoFrameTick = g_clockTicks / frameRate;
	push_rtmp_info.video_channel_number = g_video_channel_number;
	push_rtmp_info.lastVideoTimeStamp = 0;
	push_rtmp_info.makeUpVideoTimeStamp = g_clockTicks % frameRate;
	if (push_rtmp_info.makeUpVideoTimeStamp == 0)
	{
		push_rtmp_info.makeUpVideoInternal = 0;
	}
	else
	{
		push_rtmp_info.makeUpVideoInternal = frameRate / push_rtmp_info.makeUpVideoTimeStamp;
	}
	push_rtmp_info.nowMakeUpVideoTimeStamp = 0;
	push_rtmp_info.videoFrameRate = frameRate;

	push_rtmp_info.audio_packet_obj = audio_packet_obj;
	push_rtmp_info.audioFrameIndex = 1;
	push_rtmp_info.hasSendAudioHead = 0;
	push_rtmp_info.audioFrameTick = g_clockTicks * g_samplePerSecond / g_sampleRate;
	createAudioSpecificConfig(push_rtmp_info.audioSpecificConfig, &(push_rtmp_info.audioSpecificConfigSize));
	push_rtmp_info.audio_channel_number = g_audio_channel_number;
	push_rtmp_info.lastAudioTimeStamp = 0;
	push_rtmp_info.makeUpAudioTimeStamp = (g_clockTicks * g_samplePerSecond) % g_sampleRate;
	push_rtmp_info.makeUpAudioInternal = g_sampleRate / push_rtmp_info.makeUpAudioTimeStamp;
	push_rtmp_info.nowMakeUpAudioTimeStamp = 0;
	push_rtmp_info.audioFrameRate = g_sampleRate;

	//printf("orgv:makeUpVideoTimeStamp,makeUpVideoInternal,videoFrameRate=%u,%u,%u\n", \
	//	push_rtmp_info.makeUpVideoTimeStamp, push_rtmp_info.makeUpVideoInternal, push_rtmp_info.videoFrameRate);
	//printf("orga:makeUpAudioTimeStamp,makeUpAudioInternal,audioFrameRate=%u,%u,%u\n", \
	//	push_rtmp_info.makeUpAudioTimeStamp, push_rtmp_info.makeUpAudioInternal, push_rtmp_info.audioFrameRate);
	//printf("push_rtmp_init success !\n");

	if (initVideoAudioBlock(&(push_rtmp_info.packageBlock)))
	{
		printf("initVideoAudioBlock fail !\n");
		RTMP_Close(rtmp_obj);
		RTMP_Free(rtmp_obj);
		RTMPPacket_Free(video_packet_obj);
		free(video_packet_obj);
		RTMPPacket_Free(audio_packet_obj);
		free(audio_packet_obj);
		pthread_rwlock_unlock(&sg_lock);
		return -9;
	}

	if (create_thread_middle(&(push_rtmp_info.thread_id), sendPackageThread, (void *)this))
	{
		printf("create thread failure {%s(%d)}\n", __FILE__, __LINE__);
		RTMP_Close(rtmp_obj);
		RTMP_Free(rtmp_obj);
		RTMPPacket_Free(video_packet_obj);
		free(video_packet_obj);
		RTMPPacket_Free(audio_packet_obj);
		free(audio_packet_obj);
		releaseBlock(&(push_rtmp_info.packageBlock));
		pthread_rwlock_unlock(&sg_lock);
		return -10;
	}

	strcpy(sg_url, url);
	sg_frameRate = frameRate;

	push_rtmp_info.threadIsRunning = 1;
	push_rtmp_info.disConnectErrTimes = 0;
	push_rtmp_info.start_time = GetTickCount();
	isPushing = true;

	pthread_rwlock_unlock(&sg_lock);
	return 0;
}

void PushStream::stop()
{
	//printf("!!!!!!+%s!!!!!!!!!!!!\n", __FUNCTION__);
	if (!finishInit)
	{
		return;
	}

	pthread_rwlock_wrlock(&sg_lock);
	//printf("%s: get sg_lock success !\n", __FUNCTION__);

	if (isPushing == false)
	{
		pthread_rwlock_unlock(&sg_lock);
		return;
	}

	isPushing = false;
	RTMP_Close(push_rtmp_info.rtmp_obj);
	RTMP_Free(push_rtmp_info.rtmp_obj);
	RTMPPacket_Free(push_rtmp_info.video_packet_obj);
	free(push_rtmp_info.video_packet_obj);
	RTMPPacket_Free(push_rtmp_info.audio_packet_obj);
	free(push_rtmp_info.audio_packet_obj);

	push_rtmp_info.threadIsRunning = 0;
	pthread_join(push_rtmp_info.thread_id, NULL);
	memset(&(push_rtmp_info.thread_id), 0, sizeof(push_rtmp_info.thread_id));
	//printf("send rtmp package thread is over !\n");

	usleep(1000 * 1000);
	releaseBlock(&(push_rtmp_info.packageBlock));

	pthread_rwlock_unlock(&sg_lock);
	return;
}

int PushStream::video(unsigned char *data, int size)
{
	if (!finishInit)
	{
		return -1;
	}

	if (NULL == data || size <= 0)
	{
		printf("data invalid data, please check!\n");
		return -2;
	}

	int ret = pthread_rwlock_tryrdlock(&sg_lock);
	if (ret != 0)
	{
		//printf("%s:pthread_rwlock_tryrdlock fail !\n", __FUNCTION__);
		return -3;
	}

	//udi_info_log("%s get rd lock !\n", __FUNCTION__);
	if (push_rtmp_info.threadIsRunning == 0)
	{
		pthread_rwlock_unlock(&sg_lock);
		//printf("%s release rd lock for push_obj->threadIsRunning !\n", __FUNCTION__);
		return -4;
	}

	unsigned char *pTmp = data;
	unsigned char *pBuf;
	int  ret1, size1;
	unsigned char frameKeyValue;
	for (;;)
	{
		ret1 = GetCode(pTmp, (int)(data + size - pTmp), &pBuf, &size1);
		if ((ret1 == 0) || (ret1 == 1))
		{
			if ((pBuf[0] & 0x80) == 0) //forbid bit is 0, the frame is ok to be used
			{
				frameKeyValue = pBuf[0] & 0x1F;
				switch (frameKeyValue)
				{
				case NAL_SPS:
					if (push_rtmp_info.hasSendVideoHead == 0)
					{
						memcpy(push_rtmp_info.sps, pBuf, size1);
						push_rtmp_info.sps_size = size1;
						//printf("nspsSize = %d\n", push_obj->sps_size);
					}
					break;
				case NAL_PPS:
					if (push_rtmp_info.hasSendVideoHead == 0)
					{
						memcpy(push_rtmp_info.pps, pBuf, size1);
						push_rtmp_info.pps_size = size1;
						//printf("nppsSize = %d\n", push_obj->pps_size);
					}
					break;
				case NAL_SEI:
					break;
				default:
					if (push_rtmp_info.hasSendVideoHead != 0)
					{
						//push_rtmp_send_video(push_obj, pBuf, size1);
						pushDataToBlock(eVideoData, pBuf, size1, &(push_rtmp_info.packageBlock));
					}
					else if (push_rtmp_info.sps_size > 0 && push_rtmp_info.pps_size > 0)
					{
						//unsigned int iii;
						//printf("sps_size=%u, sps= ", push_rtmp_info.sps_size);
						//for (iii = 0; iii < push_rtmp_info.sps_size; iii++) printf("%2x ", push_rtmp_info.sps[iii]);
						//printf("\n");
						//printf("pps_size=%u, pps= ", push_rtmp_info.pps_size);
						//for (iii = 0; iii < push_rtmp_info.pps_size; iii++) printf("%2x ", push_rtmp_info.pps[iii]);
						//printf("\n");
						//push_rtmp_send_h264_head(push_obj);
						//push_rtmp_send_video(push_obj, pBuf, size1);
						pushDataToBlock(eVideoHead, NULL, 0, &(push_rtmp_info.packageBlock));
						pushDataToBlock(eVideoData, pBuf, size1, &(push_rtmp_info.packageBlock));
						push_rtmp_info.hasSendVideoHead = 1;
					}
					break;
				}
			}
		}
		pTmp = pBuf + size1;
		if ((ret1 == 0) || (ret1 == -1))
		{
			break;
		}
	}

	pthread_rwlock_unlock(&sg_lock);
	//printf("%s release rd lock !\n", __FUNCTION__);

	return 0;
}

int PushStream::audio(const unsigned char *data, const int size)
{
	if (!finishInit)
	{
		return -1;
	}

	if (NULL == data || size <= 0)
	{
		printf("data invalid data, please check!\n");
		return -2;
	}

	int ret = pthread_rwlock_tryrdlock(&sg_lock);
	if (ret != 0)
	{
		//printf("%s:pthread_rwlock_tryrdlock fail !\n", __FUNCTION__);
		return -3;
	}

	if (push_rtmp_info.threadIsRunning == 0)
	{
		pthread_rwlock_unlock(&sg_lock);
		return -4;
	}

	//udi_info_log("%s get rd lock !\n", __FUNCTION__);
	//printf("audio before dataSize = %d\n", size);
	if (push_rtmp_info.hasSendAudioHead == 0)
	{
		push_rtmp_info.hasSendAudioHead = 1;
		pushDataToBlock(eAudioHead, NULL, 0, &(push_rtmp_info.packageBlock));
	}
	pushDataToBlock(eAudioData, data, size, &(push_rtmp_info.packageBlock));

	pthread_rwlock_unlock(&sg_lock);
	//udi_info_log("%s release rd lock !\n", __FUNCTION__);

	return 0;
}

void PushStream::createAudioSpecificConfig(unsigned char *pAudioSpecificConfig, unsigned int *pAudioSpecificConfigSize)
{
	if (!pAudioSpecificConfig || !pAudioSpecificConfigSize)
	{
		printf("input param error !\n");
		return;
	}

	unsigned char samplingFrequencyIndex = getSamplingFrequencyIndex(g_sampleRate);

	pAudioSpecificConfig[0] = ((g_audioObjectType << 3) & 0xF8) | ((samplingFrequencyIndex >> 1) & 0x07);
	pAudioSpecificConfig[1] = ((samplingFrequencyIndex << 7) & 0x80) | ((g_channelConfiguration << 3) & 0x78) | \
		((g_frameLengthFlag << 2) & 0x04) | ((g_dependsOnCoreCoder << 1) & 0x02) | (g_extensionFlag & 0x01);
	*pAudioSpecificConfigSize = g_audioSpecificConfigSize;

	//printf("pAudioSpecificConfig:%2x,%2x,size=%u,sfIndex=%u\n", pAudioSpecificConfig[0], pAudioSpecificConfig[1], *pAudioSpecificConfigSize, samplingFrequencyIndex);
}

unsigned char PushStream::getSamplingFrequencyIndex(const unsigned int sampleRate)
{
	unsigned char i;
	unsigned char arrayLen = sizeof(g_aacSampleRateFrequency) / sizeof(unsigned int);
	for (i = 0; i < arrayLen; i++)
	{
		if (sampleRate == g_aacSampleRateFrequency[i])
		{
			return i;
		}
	}
	//printf("are you kidding me ? input sampleRate not found ! sampleRate = %u\n", sampleRate);
	return 4; //default sampleRate = 44100
}

void PushStream::pushDataToBlock(PackageType type, const unsigned char *pData, const int dataSize, BlockInfo *pBlock)
{
	if (type >= eMaxPackageType || !pBlock)
	{
		printf("param error ! type=%u,pBlock=%p\n", (unsigned int)type, pBlock);
		return;
	}

	if (type != eVideoHead && type != eAudioHead)
	{
		if (!pData || dataSize <= 0)
		{
			printf("param2 error ! type=%u, pData=%p, dataSize=%d\n", (unsigned char)type, pData, dataSize);
			return;
		}
	}

	//udi_info_log("before pushBlockEx: %p, %d, %u\n", pData, dataSize, (unsigned char)type);
	int ret = pushBlockEx(pBlock, pData, dataSize, (unsigned char)type);
	if (ret)
	{
		printf("block is full ! ret = %d, type=%u, dataSize=%d\n", ret, (unsigned char)type, dataSize);
		return;
	}
}

void PushStream::sendPackage()
{
	BlockInfo *pBlock = &(push_rtmp_info.packageBlock);
	struct timespec ts;
	struct timeval  tv;

	while (push_rtmp_info.threadIsRunning)
	{
		gettimeofday(&tv, NULL);
		ts.tv_sec = tv.tv_sec + g_sendThreadWaitTime;
		ts.tv_nsec = tv.tv_usec * 1000;
		sem_timedwait(&(pBlock->m_Semaphore), &ts);

		if (!pBlock)
		{
			printf("sendPackageThread exit due to param is NULL ! push_rtmp_info=%p, pBlock=%p\n", &push_rtmp_info, pBlock);
			return;
		}

		PopBlockInfoData(pBlock, dealBlockData, this);
	}
}

void PushStream::pushData(const unsigned char *pData, const int dataSize)
{
	if (!pData || dataSize <= 0)
	{
		//printf("%s:param error ! pData=%u, dataSize=%d\n", __FUNCTION__, (unsigned int)pData, dataSize);
		return;
	}
	PackageType type = (PackageType)pData[0];
	if (type >= eMaxPackageType)
	{
		printf("package type error ! type = %u\n", (unsigned int)type);
		return;
	}

	const unsigned char *pRealData = &pData[1];
	const int realDataSize = dataSize - 1;
	switch (type)
	{
	case eVideoHead:
		push_rtmp_send_h264_head();
		break;
	case eAudioHead:
		push_rtmp_send_aac_head();
		break;
	case eVideoData:
		push_rtmp_send_video(pRealData, realDataSize);
		break;
	case eAudioData:
		//printf("audio after dataSize = %d\n", realDataSize);
		push_rtmp_send_audio(pRealData, realDataSize);
		break;
	default:
		printf("unknow type %u\n", (unsigned int)type);
		break;
	}
}

int PushStream::push_rtmp_send_h264_head()
{
	//printf("+push_rtmp_send_h264_head\n");
	if (push_rtmp_info.sps_size == 0 || push_rtmp_info.pps_size == 0)
	{
		printf("push_rtmp_info.sps_size=%p,push_rtmp_info.pps_size=%p\n", push_rtmp_info.sps, push_rtmp_info.pps);
		return -1;
	}

	RTMPPacket*packet = NULL;
	RTMP *rtmp = NULL;
	packet = push_rtmp_info.video_packet_obj;
	rtmp = push_rtmp_info.rtmp_obj;
	if (NULL == packet || NULL == rtmp)
	{
		printf("packet_obj or rtmp_obj invalid, please check!\n");
		return -2;
	}
	RTMPPacket_Reset(packet);
	unsigned char *body = (unsigned char *)packet->m_body;

	if (NULL == body)
	{
		printf("packet m_body invalid\n");
		return -3;
	}
	int i = 0;
	body[i++] = 0x17;
	body[i++] = 0x00;
	body[i++] = 0x00;
	body[i++] = 0x00;
	body[i++] = 0x00;
	/*AVCDecoderConfigurationRecord*/
	body[i++] = 0x01;
	body[i++] = push_rtmp_info.sps[1];
	body[i++] = push_rtmp_info.sps[2];
	body[i++] = push_rtmp_info.sps[3];
	body[i++] = 0xff;
	/*sps*/
	body[i++] = 0xe1;
	body[i++] = (push_rtmp_info.sps_size >> 8) & 0xff;
	body[i++] = push_rtmp_info.sps_size & 0xff;
	memcpy(&body[i], push_rtmp_info.sps, push_rtmp_info.sps_size);
	i += push_rtmp_info.sps_size;
	/*pps*/
	body[i++] = 0x01;
	body[i++] = (push_rtmp_info.pps_size >> 8) & 0xff;
	body[i++] = (push_rtmp_info.pps_size) & 0xff;
	memcpy(&body[i], push_rtmp_info.pps, push_rtmp_info.pps_size);
	i += push_rtmp_info.pps_size;
	//packet info
	packet->m_packetType = RTMP_PACKET_TYPE_VIDEO;
	packet->m_nBodySize = i;
	packet->m_nChannel = push_rtmp_info.video_channel_number;
	packet->m_nTimeStamp = 0;
	packet->m_hasAbsTimestamp = 0;
	packet->m_headerType = RTMP_PACKET_SIZE_MEDIUM;
	//packet->m_nInfoField2 = 0;//rtmp->m_stream_id;
	packet->m_nInfoField2 = rtmp->m_stream_id;
	if (!RTMP_IsConnected(rtmp))
	{
		printf("rtmp disconnected\n");
		//push_obj->disConnectErrTimes++;
		return -4;
	}
	if (!RTMP_SendPacket(rtmp, packet, 0))
	{
		printf("RTMP_SendPacket failed\n");
		return -5;
	}
	//push_obj->start_time = GetTickCount();
	return 0;
}

int PushStream::push_rtmp_send_video(const unsigned char *data, const unsigned int size)
{
	RTMPPacket*packet = NULL;
	RTMP *rtmp = NULL;
	packet = push_rtmp_info.video_packet_obj;
	rtmp = push_rtmp_info.rtmp_obj;
	if (NULL == packet || NULL == rtmp)
	{
		printf("packet_obj or rtmp_obj invalid, please check!\n");
		return -1;
	}
	RTMPPacket_Reset(packet);
	char *body = packet->m_body;

	if (NULL == body)
	{
		printf("packet m_body invalid\n");
		return -2;
	}

	int type = 0;
	const unsigned char *buf = data;
	int len = size;
	type = buf[0] & 0x1f;
	packet->m_nBodySize = len + 9;
	/*send video packet*/
	memset(body, 0, len + 9);
	/*key frame*/
	if (type == NAL_IDR_SLICE)
	{
		body[0] = 0x17;
	}
	else
	{
		body[0] = 0x27;
	}
	body[1] = 0x01;  /*nal unit*/
	memset(&body[2], 0, 3);
	body[5] = (len >> 24) & 0xff;
	body[6] = (len >> 16) & 0xff;
	body[7] = (len >> 8) & 0xff;
	body[8] = (len) & 0xff;
	/*copy data*/
	memcpy(&body[9], buf, len);
	packet->m_hasAbsTimestamp = 0;
	packet->m_packetType = RTMP_PACKET_TYPE_VIDEO;
	//packet->m_nInfoField2 = 0;//winsys->rtmp->m_stream_id;
	packet->m_nInfoField2 = rtmp->m_stream_id;
	packet->m_nChannel = push_rtmp_info.video_channel_number;
	packet->m_headerType = RTMP_PACKET_SIZE_MEDIUM;//RTMP_PACKET_SIZE_LARGE;
#ifdef USE_ABS_TIMESTAMP
	packet->m_nTimeStamp = GetTickCount() - push_obj->start_time;
	//udi_info_log("packet->m_nTimeStamp = %u\n", packet->m_nTimeStamp);
#else
	if (push_rtmp_info.makeUpVideoInternal > 0 && (push_rtmp_info.videoFrameIndex % push_rtmp_info.makeUpVideoInternal == 0))
	{
		if (push_rtmp_info.nowMakeUpVideoTimeStamp > push_rtmp_info.makeUpVideoTimeStamp)
		{
			printf("something error ! now=%u, max=%u\n", push_rtmp_info.nowMakeUpVideoTimeStamp, push_rtmp_info.makeUpVideoTimeStamp);
			push_rtmp_info.nowMakeUpVideoTimeStamp = push_rtmp_info.makeUpVideoTimeStamp;
		}
		if (push_rtmp_info.nowMakeUpVideoTimeStamp == push_rtmp_info.makeUpVideoTimeStamp)
		{
			packet->m_nTimeStamp = push_rtmp_info.lastVideoTimeStamp + push_rtmp_info.videoFrameTick;
		}
		else
		{
			push_rtmp_info.nowMakeUpVideoTimeStamp++;
			packet->m_nTimeStamp = push_rtmp_info.lastVideoTimeStamp + push_rtmp_info.videoFrameTick + 1;
		}
	}
	else
	{
		packet->m_nTimeStamp = push_rtmp_info.lastVideoTimeStamp + push_rtmp_info.videoFrameTick;
	}
	if (push_rtmp_info.videoFrameIndex % push_rtmp_info.videoFrameRate == 0)
	{
		push_rtmp_info.nowMakeUpVideoTimeStamp = 0;
	}
#endif
	push_rtmp_info.videoFrameIndex++;
	push_rtmp_info.lastVideoTimeStamp = packet->m_nTimeStamp;
	if (!RTMP_IsConnected(rtmp))
	{
		printf("rtmp disconnected\n");
		push_rtmp_info.disConnectErrTimes++;
		return -3;
	}
	if (!RTMP_SendPacket(rtmp, packet, 0))
	{
		printf("RTMP_SendPacket failed\n");
		return -4;
	}
	return 0;
}

int PushStream::push_rtmp_send_aac_head()
{
	//printf("+push_rtmp_send_aac_head\n");
	RTMPPacket*packet = NULL;
	RTMP *rtmp = NULL;
	packet = push_rtmp_info.audio_packet_obj;
	rtmp = push_rtmp_info.rtmp_obj;
	if (NULL == packet || NULL == rtmp)
	{
		printf("packet_obj or rtmp_obj invalid, please check!\n");
		return -1;
	}
	RTMPPacket_Reset(packet);
	unsigned char *body = (unsigned char *)packet->m_body;
	if (NULL == body)
	{
		printf("packet m_body invalid\n");
		return -2;
	}
	int len = push_rtmp_info.audioSpecificConfigSize;  /*spec data长度,一般是2*/
												  /*AF 00 + AAC RAW data*/
	body[0] = 0xAF;
	body[1] = 0x00;
	memcpy(&body[2], push_rtmp_info.audioSpecificConfig, len); /*spec_buf是AAC sequence header数据*/
	packet->m_packetType = RTMP_PACKET_TYPE_AUDIO;
	packet->m_nBodySize = len + 2;
	packet->m_nChannel = push_rtmp_info.audio_channel_number;
	packet->m_nTimeStamp = 0;
	packet->m_hasAbsTimestamp = 0;
	packet->m_headerType = RTMP_PACKET_SIZE_LARGE;
	//packet->m_nInfoField2 = 1;//rtmp->m_stream_id;
	packet->m_nInfoField2 = rtmp->m_stream_id;
	if (!RTMP_IsConnected(rtmp))
	{
		printf("rtmp disconnected\n");
		//push_obj->disConnectErrTimes++;
		return -3;
	}
	if (!RTMP_SendPacket(rtmp, packet, 0))
	{
		printf("RTMP_SendPacket failed\n");
		return -4;
	}
	return 0;
}

int PushStream::push_rtmp_send_audio(const unsigned char *data, const unsigned int size)
{
	if (NULL == data || size == 0)
	{
		printf("data invalid data, please check!\n");
		return -1;
	}
	RTMPPacket*packet = NULL;
	RTMP *rtmp = NULL;
	packet = push_rtmp_info.audio_packet_obj;
	rtmp = push_rtmp_info.rtmp_obj;
	if (NULL == packet || NULL == rtmp)
	{
		printf("packet_obj or rtmp_obj invalid, please check!\n");
		return -2;
	}
	RTMPPacket_Reset(packet);
	unsigned char *body = (unsigned char *)packet->m_body;
	if (NULL == body)
	{
		printf("packet m_body invalid\n");
		return -3;
	}
	const unsigned char *buf = data + 7;
	int len = size - 7;
	if (len > 0)
	{
		/*AF 01 + AAC RAW data*/
		body[0] = 0xAF;
		body[1] = 0x01;
		memcpy(&body[2], buf, len);
		packet->m_packetType = RTMP_PACKET_TYPE_AUDIO;
		packet->m_nBodySize = len + 2;
		packet->m_nChannel = push_rtmp_info.audio_channel_number;

#ifdef USE_ABS_TIMESTAMP
		packet->m_nTimeStamp = GetTickCount() - push_obj->start_time;
#else
		if (push_rtmp_info.makeUpAudioInternal > 0 && (push_rtmp_info.audioFrameIndex % push_rtmp_info.makeUpAudioInternal == 0))
		{
			if (push_rtmp_info.nowMakeUpAudioTimeStamp > push_rtmp_info.makeUpAudioTimeStamp)
			{
				printf("something error ! now=%u, max=%u\n", push_rtmp_info.nowMakeUpAudioTimeStamp, push_rtmp_info.makeUpAudioTimeStamp);
				push_rtmp_info.nowMakeUpAudioTimeStamp = push_rtmp_info.makeUpAudioTimeStamp;
			}
			if (push_rtmp_info.nowMakeUpAudioTimeStamp == push_rtmp_info.makeUpAudioTimeStamp)
			{
				packet->m_nTimeStamp = push_rtmp_info.lastAudioTimeStamp + push_rtmp_info.audioFrameTick;
			}
			else
			{
				push_rtmp_info.nowMakeUpAudioTimeStamp++;
				packet->m_nTimeStamp = push_rtmp_info.lastAudioTimeStamp + push_rtmp_info.audioFrameTick + 1;
			}
		}
		else
		{
			packet->m_nTimeStamp = push_rtmp_info.lastAudioTimeStamp + push_rtmp_info.audioFrameTick;
		}
		if (push_rtmp_info.audioFrameIndex % push_rtmp_info.audioFrameRate == 0)
		{
			push_rtmp_info.nowMakeUpAudioTimeStamp = 0;
		}
#endif
		push_rtmp_info.audioFrameIndex++;
		push_rtmp_info.lastAudioTimeStamp = packet->m_nTimeStamp;
		packet->m_hasAbsTimestamp = 0;
		packet->m_headerType = RTMP_PACKET_SIZE_MEDIUM;
		//packet->m_nInfoField2 = 1;//rtmp->m_stream_id;
		packet->m_nInfoField2 = rtmp->m_stream_id;
		if (!RTMP_IsConnected(rtmp))
		{
			printf("rtmp disconnected\n");
			push_rtmp_info.disConnectErrTimes++;
			return -4;
		}
		if (!RTMP_SendPacket(rtmp, packet, 0))
		{
			printf("RTMP_SendPacket failed\n");
			return -5;
		}
	}
	else
	{
		printf("no data to send\n");
	}
	return 0;
}

void PushStream::reconnect()
{
	while (finishInit)
	{
		sleep(g_reconnectCheckTimeInterval);

		pthread_rwlock_rdlock(&sg_lock);
		if (push_rtmp_info.disConnectErrTimes >= g_maxDisConnectTimes)
		{
			push_rtmp_info.disConnectErrTimes = 0;

			pthread_rwlock_unlock(&sg_lock);
			restart();
			continue;
		}

		pthread_rwlock_unlock(&sg_lock);
	}
}

void PushStream::restart()
{
	printf("!!!!!!!!!!! +%s !!!!!!!!!!!!!!!!\n", __FUNCTION__);

	stop();
	printf("finish stop !\n");

	start(sg_url, sg_frameRate);
	printf("!!!!!!!!!!! -%s !!!!!!!!!!!!!!!!\n", __FUNCTION__);
}

static void dealBlockData(const unsigned char *pData, const int dataSize, void *arg)
{
	PushStream *p = (PushStream *)arg;
	p->pushData(pData, dataSize);
}

static void* sendPackageThread(void *arg)
{
	PushStream *p = (PushStream *)arg;
	p->sendPackage();
	return NULL;
}

static void* reconnectThread(void *arg)
{
	PushStream *p = (PushStream *)arg;
	p->reconnect();
	return NULL;
}
