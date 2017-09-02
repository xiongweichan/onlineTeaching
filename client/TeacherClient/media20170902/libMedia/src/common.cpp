#include "common.h"

int InitBlockInfoBuffer(BlockInfo *pBlockInfo, int nInfoSize, int nDataSize, unsigned char *pBuf, int nBufCount)
{
	if (sem_init(&(pBlockInfo->m_Semaphore), 0, 0) != 0)
	{
		printf("sem_init failure {%s(%d)}\n", __FILE__, __LINE__);
		return -1;
	}

	if (pthread_mutex_init(&pBlockInfo->m_Lock, NULL) != 0)
	{
		printf("pthread_mutex_init failure {%s(%d)}\n", __FILE__, __LINE__);
		return -2;
	}

	if (pBuf != NULL)
	{
		pBlockInfo->m_pInfo = (BufferInfo*)pBuf;

		if ((int)(nInfoSize * sizeof(BufferInfo) + nDataSize) > nBufCount)
		{
			printf("( nInfoSize*sizeof(BufferInfo)+nDataSize ) > nBufCount {%s(%d)}\n", __FILE__, __LINE__);
			return -3;
		}
	}
	else if ((pBlockInfo->m_pInfo = (BufferInfo*)malloc(nInfoSize * sizeof(BufferInfo) + nDataSize)) == NULL)
	{
		printf("sem_init failure {%s(%d)}\n", __FILE__, __LINE__);
		return -4;
	}
	else
	{
		memset(pBlockInfo->m_pInfo, 0, nInfoSize * sizeof(BufferInfo) + nDataSize);
	}

	pBlockInfo->m_nInfoSize = nInfoSize;
	pBlockInfo->m_nDataSize = nDataSize;
	pBlockInfo->m_pData = (unsigned char *)(pBlockInfo->m_pInfo + pBlockInfo->m_nInfoSize);

	pBlockInfo->m_Push = pBlockInfo->m_Pop = pBlockInfo->m_pInfo;
	pBlockInfo->m_pCurr = pBlockInfo->m_pData;

	BufferInfo *pTempBufInfo = pBlockInfo->m_pInfo;

	for (int i = 0; i < pBlockInfo->m_nInfoSize; i++)
	{
		if (i == (pBlockInfo->m_nInfoSize - 1))
			pTempBufInfo->pNext = pBlockInfo->m_pInfo;
		else
			pTempBufInfo->pNext = pTempBufInfo + 1;

		pTempBufInfo++;
	}

	return 0;
}

static int PushBlockInfoData(BlockInfo *pBlockInfo, const unsigned char *pData, const int iSize, const unsigned char *pFrontData, const int iFrontSize)
{
	int ret = 0;
	BufferInfo *Pop;

	pthread_mutex_lock(&(pBlockInfo->m_Lock));

	if (iSize + iFrontSize > pBlockInfo->m_nDataSize)
	{
		ret = -1;
		pthread_mutex_unlock(&(pBlockInfo->m_Lock));
		return ret;
	}

	if (pBlockInfo->m_Push->nFlag != 0)
	{
		ret = -2;
		pthread_mutex_unlock(&(pBlockInfo->m_Lock));
		return ret;
	}

	if (iSize + iFrontSize > (pBlockInfo->m_pData + pBlockInfo->m_nDataSize - pBlockInfo->m_pCurr))
	{
		pBlockInfo->m_pCurr = pBlockInfo->m_pData;
	}

	Pop = pBlockInfo->m_Pop;

	while (Pop->nFlag)
	{
		if (Pop->pData >= pBlockInfo->m_pCurr)
		{
			if ((Pop->pData - pBlockInfo->m_pCurr) < iSize + iFrontSize)
			{
				ret = -3;
				pthread_mutex_unlock(&(pBlockInfo->m_Lock));
				return ret;
			}

		}
		Pop = Pop->pNext;
	}

	if (pFrontData && iFrontSize > 0)
	{
		memcpy(pBlockInfo->m_pCurr, pFrontData, iFrontSize);
	}

	if (pData && iSize > 0)
	{
		memcpy(pBlockInfo->m_pCurr + iFrontSize, pData, iSize);
	}

	pBlockInfo->m_Push->pData = pBlockInfo->m_pCurr;
	pBlockInfo->m_Push->nSize = iSize + iFrontSize;
	pBlockInfo->m_Push->nFlag = 1;
	pBlockInfo->m_Push = pBlockInfo->m_Push->pNext;

	pBlockInfo->m_pCurr += (iSize + iFrontSize);

	sem_post(&(pBlockInfo->m_Semaphore));

	pthread_mutex_unlock(&(pBlockInfo->m_Lock));

	return ret;
}

int PopBlockInfoData(BlockInfo *pBlockInfo, void(*pFunc)(const unsigned char*, const int, void*), void* arg)
{
	if (pBlockInfo == NULL || pBlockInfo->m_Pop == NULL)
	{
		printf("%s: para error ! pBlockInfo=%p\n", __FUNCTION__, pBlockInfo);
		return -1;
	}

	while (pBlockInfo && pBlockInfo->m_Pop && pBlockInfo->m_Pop->nFlag)
	{
		pFunc(pBlockInfo->m_Pop->pData, pBlockInfo->m_Pop->nSize, arg);
		pthread_mutex_lock(&(pBlockInfo->m_Lock));
		pBlockInfo->m_Pop->nFlag = 0;
		pBlockInfo->m_Pop = pBlockInfo->m_Pop->pNext;
		pthread_mutex_unlock(&(pBlockInfo->m_Lock));
	}

	return 0;
}

int PopBlockInfoDataNotConst(BlockInfo *pBlockInfo, void(*pFunc)(unsigned char*, const int, void*), void* arg)
{
	if (pBlockInfo == NULL || pBlockInfo->m_Pop == NULL)
	{
		printf("%s: para error ! pBlockInfo=%p\n", __FUNCTION__, pBlockInfo);
		return -1;
	}

	while (pBlockInfo && pBlockInfo->m_Pop && pBlockInfo->m_Pop->nFlag)
	{
		pFunc(pBlockInfo->m_Pop->pData, pBlockInfo->m_Pop->nSize, arg);
		pthread_mutex_lock(&(pBlockInfo->m_Lock));
		pBlockInfo->m_Pop->nFlag = 0;
		pBlockInfo->m_Pop = pBlockInfo->m_Pop->pNext;
		pthread_mutex_unlock(&(pBlockInfo->m_Lock));
	}

	return 0;
}

int PopBlockInfoDataEx(BlockInfo *pBlockInfo, void(*pFunc)(const unsigned char*, const int, void*), void* arg, int(*loopConditionFunc)(void), int timeOut)
{
	struct timespec ts;
	struct timeval  tv;
	while (loopConditionFunc())
	{
		gettimeofday(&tv, NULL);
		ts.tv_sec = tv.tv_sec + timeOut;
		ts.tv_nsec = tv.tv_usec * 1000;
		sem_timedwait(&(pBlockInfo->m_Semaphore), &ts);

		PopBlockInfoData(pBlockInfo, pFunc, arg);
	}

	return 0;
}

int PopBlockInfoDataExNotConst(BlockInfo *pBlockInfo, void(*pFunc)(unsigned char*, const int, void*), void* arg, int(*loopConditionFunc)(void *arg), int timeOut)
{
	struct timespec ts;
	struct timeval  tv;
	while (loopConditionFunc(arg))
	{
		gettimeofday(&tv, NULL);
		ts.tv_sec = tv.tv_sec + timeOut;
		ts.tv_nsec = tv.tv_usec * 1000;
		sem_timedwait(&(pBlockInfo->m_Semaphore), &ts);

		PopBlockInfoDataNotConst(pBlockInfo, pFunc, arg);
	}

	return 0;
}

int initVideoAudioBlock(BlockInfo *pBlockInfo)
{
	return InitBlockInfoBuffer(pBlockInfo, tcpVideoAudioBufMaxNumber, tcpVideoAudioBlockSize, NULL, 0);
}
int pushBlock(BlockInfo *pBlockInfo, unsigned char *pData, int iSize)
{
	return PushBlockInfoData(pBlockInfo, pData, iSize, NULL, 0);
}
int pushBlockEx(BlockInfo *pBlockInfo, const unsigned char *pData, const int iSize, const unsigned char type)
{
	return PushBlockInfoData(pBlockInfo, pData, iSize, &type, 1);
}
int pushBlo(BlockInfo *pBlockInfo, const unsigned char *pData, const int iSize, const unsigned char *pf, const int ifSize)
{
	return PushBlockInfoData(pBlockInfo, pData, iSize, pf, ifSize);
}

void releaseBlock(BlockInfo *pBlockInfo)
{
	if (pBlockInfo)
	{
		if (pBlockInfo->m_Semaphore)
		{
			sem_destroy(&(pBlockInfo->m_Semaphore));
		}
		if (pBlockInfo->m_Lock)
		{
			pthread_mutex_destroy(&(pBlockInfo->m_Lock));
		}
	}

	if (pBlockInfo->m_pInfo)
	{
		free(pBlockInfo->m_pInfo);
		pBlockInfo->m_pInfo = NULL;
	}
}

static int create_thread(pthread_t *pid, void*(*thread_callback)(void*), void *arg, const size_t stack_size)
{
	if (!pid || stack_size < 4 * 1024)
	{
		printf("%s: para errror ! %p,%p,%zd\n", __FUNCTION__, pid, thread_callback, stack_size);
		return -1;
	}

	pthread_attr_t *set_attr;
#ifdef __linux__
	size_t stacksize;
	pthread_attr_t attr;
	if (pthread_attr_init(&attr) != 0) udi_erro_log("pthread_attr_init failure !\n");
	if (pthread_attr_getstacksize(&attr, &stacksize) != 0) udi_erro_log("pthread_attr_getstacksize failure !\n");
	if (pthread_attr_setstacksize(&attr, stack_size) != 0) udi_erro_log("pthread_attr_setstacksize failure !\n");
	if (pthread_attr_getstacksize(&attr, &stacksize) != 0) udi_erro_log("pthread_attr_getstacksize failure !\n");
	set_attr = &attr;
#else
	set_attr = NULL;
#endif

	return pthread_create(pid, set_attr, thread_callback, arg);
}


int create_thread_small(pthread_t *pid, void*(*thread_callback)(void*), void *arg)
{
	return create_thread(pid, thread_callback, arg, MIN_THREAD_STACK_SIZE);
}

int create_thread_middle(pthread_t *pid, void*(*thread_callback)(void*), void *arg)
{
	return create_thread(pid, thread_callback, arg, MIDDLE_THREAD_STACK_SIZE);
}

int create_thread_normal(pthread_t *pid, void*(*thread_callback)(void*), void *arg)
{
	return create_thread(pid, thread_callback, arg, NORMAL_THREAD_STACK_SIZE);
}

void gettimeofday(struct timeval *tp, void *tzp)
{
	time_t clock;
	struct tm tm;
	SYSTEMTIME wtm;

	GetLocalTime(&wtm);
	tm.tm_year = wtm.wYear - 1900;
	tm.tm_mon = wtm.wMonth - 1;
	tm.tm_mday = wtm.wDay;
	tm.tm_hour = wtm.wHour;
	tm.tm_min = wtm.wMinute;
	tm.tm_sec = wtm.wSecond;
	tm.tm_isdst = -1;
	clock = mktime(&tm);
	tp->tv_sec = (long)clock;
	tp->tv_usec = wtm.wMilliseconds * 1000;

	return;
}

int GetCode(unsigned char *src, int count, unsigned char **pOut, int *iSize)
{
	unsigned char *pTmp = NULL;
	int  start = 0, i = 0, size;
	*pOut = NULL;
	*iSize = 0;
	for (i = 0; i <= (count - 4); i++)
	{
		if (src[i] == 0 && src[i + 1] == 0 && src[i + 2] == 1)
		{
			if (pTmp)
			{
				size = i - start;
				if (i > 0) if (src[i - 1] == 0)  size--;
				*pOut = pTmp;
				*iSize = size;
				return 1;
			}
			start = i + 3;
			pTmp = src + i + 3;
		}
	}
	size = 0;
	if (pTmp != NULL)
	{
		size = (int)(src + count - pTmp);
	}
	if (size > 0)
	{
		*pOut = pTmp;
		*iSize = size;
		return 0;
	}
	return -1;
}

void utf8ToWchar(const char* u, wchar_t *w)
{
	int nLen = MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, u, -1, NULL, 0);
	if (nLen == 0)
	{
		return;
	}

	MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, u, -1, w, nLen);
}

void wcharToUtf8(const wchar_t *w, char *u)
{
	int l = WideCharToMultiByte(CP_UTF8, 0, w, -1, 0, 0, 0, 0);
	WideCharToMultiByte(CP_UTF8, 0, w, -1, u, l, 0, 0);
}

void videoWcharToUtf8(const wchar_t *w, char *u)
{
	if (!w || !u)
	{
		return;
	}

	char b[256];
	wcharToUtf8(w, b);
	snprintf(u, 256, "video=%s", b);
}

void audioWcharToUtf8(const wchar_t *w, char *u)
{
	if (!w || !u)
	{
		return;
	}

	char b[256];
	wcharToUtf8(w, b);
	snprintf(u, 256, "audio=%s", b);
}

void copyVideo(unsigned char *srcYuv, int srcW, int srcH, int copySrcX, int copySrcY, int copySrcW, int copySrcH, \
	unsigned char *dstYuv, int dstW, int dstH, int copyDstX, int copyDstY)
{
	int i;
	unsigned char *pSrcTmp;
	unsigned char *pDstTmp;

	copySrcX += (copySrcX % 2);
	copySrcY += (copySrcY % 2);
	copyDstX += (copyDstX % 2);
	copyDstY += (copyDstY % 2);

	copySrcW += (copySrcW % 2);
	copySrcH += (copySrcH % 2);

	if (copySrcX >= srcW) return;
	if (copySrcY >= srcH) return;
	if (copyDstX >= dstW) return;
	if (copyDstY >= dstH) return;

	if ((copySrcX + copySrcW) > srcW)	copySrcW = srcW - copySrcX;
	if ((copySrcY + copySrcH) > srcH)	copySrcH = srcH - copySrcY;

	if ((copyDstX + copySrcW) > dstW) copySrcW = dstW - copyDstX;
	if ((copyDstY + copySrcH) > dstH) copySrcH = dstH - copyDstY;

	//Y
	pSrcTmp = srcYuv + copySrcX + copySrcY * srcW;
	pDstTmp = dstYuv + copyDstX + copyDstY * dstW;
	for (i = 0; i < copySrcH; i++)
	{
		memcpy(pDstTmp, pSrcTmp, copySrcW);
		pSrcTmp += srcW;
		pDstTmp += dstW;
	}

	//U
	pSrcTmp = srcYuv + (copySrcX / 2) + (copySrcY / 2) * (srcW / 2) + srcW * srcH;
	pDstTmp = dstYuv + (copyDstX / 2) + (copyDstY / 2) * (dstW / 2) + dstW * dstH;
	for (i = 0; i < copySrcH / 2; i++)
	{
		memcpy(pDstTmp, pSrcTmp, copySrcW / 2);
		pSrcTmp += srcW / 2;
		pDstTmp += dstW / 2;
	}

	//V
	pSrcTmp = srcYuv + (copySrcX / 2) + (copySrcY / 2) * (srcW / 2) + srcW * srcH + srcW * srcH / 4;
	pDstTmp = dstYuv + (copyDstX / 2) + (copyDstY / 2) * (dstW / 2) + dstW * dstH + dstW * dstH / 4;
	for (i = 0; i < copySrcH / 2; i++)
	{
		memcpy(pDstTmp, pSrcTmp, copySrcW / 2);
		pSrcTmp += srcW / 2;
		pDstTmp += dstW / 2;
	}
}
