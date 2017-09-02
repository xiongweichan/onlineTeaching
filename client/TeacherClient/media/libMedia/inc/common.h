#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include <corecrt_io.h>
#include <winsock2.h>
#include <windows.h>

#ifdef __cplusplus
extern "C" {
#endif
#include "pthread.h"
#include "semaphore.h"
#pragma comment(lib, "pthreadVC2.lib")
#pragma comment(lib, "WS2_32.Lib")

#include "libavcodec/avcodec.h"
#include "libavformat/avformat.h"
#include "libswscale/swscale.h"
#include "libavdevice/avdevice.h"
#pragma comment(lib, "avcodec.lib")
#pragma comment(lib, "avformat.lib")
#pragma comment(lib, "avutil.lib")
#pragma comment(lib, "avdevice.lib")
#pragma comment(lib, "avfilter.lib")
#pragma comment(lib, "postproc.lib")
#pragma comment(lib, "swresample.lib")
#pragma comment(lib, "swscale.lib")

#include "faac.h"
#pragma comment(lib, "libfaac.lib")
#ifdef __cplusplus
}
#endif

#include <iostream>
using namespace std;

#define PRINT_DCTOR

//#ifndef sleep
//#define sleep(s) Sleep(s * 1000)
//#endif
#define usleep(us) Sleep(us / 1000)
#define close(fd) closesocket(fd)

#define MIN_THREAD_STACK_SIZE		(16 * 1024)
#define MIDDLE_THREAD_STACK_SIZE	(1024 * 1024)
#define NORMAL_THREAD_STACK_SIZE	(8 * 1024 * 1024)


typedef struct _BufferInfo
{
	unsigned char *pData;
	int nSize;
	int nFlag;
	struct _BufferInfo *pNext;
}BufferInfo, *lpBufferInfo;

typedef struct _BlockInfo
{
	sem_t m_Semaphore;
	pthread_mutex_t m_Lock;
	BufferInfo *m_Push;
	BufferInfo *m_Pop;
	BufferInfo *m_pInfo;
	unsigned char *m_pData;
	unsigned char *m_pCurr;
	int m_nInfoSize;
	int m_nDataSize;
}BlockInfo, *lpBlockInfo;


#define MAIN_VIDEO_STREAM_BIT_RATE_M	2
static const unsigned int udpAudioBlockSize = 8 * 1024 * 1024;
static const unsigned int tcpVideoAudioBufMaxNumber = 32 * 1024;
static const unsigned int tcpVideoAudioBlockSize = MAIN_VIDEO_STREAM_BIT_RATE_M * 1024 * 1024 * 32;
//static const unsigned int tcpVideoAudioBlockSize = 1024;

int InitBlockInfoBuffer(BlockInfo *pBlockInfo, int nInfoSize, int nDataSize, unsigned char *pBuf, int nBufCount);
int pushBlock(BlockInfo *pBlockInfo, unsigned char *pData, int iSize);
int pushBlockEx(BlockInfo *pBlockInfo, const unsigned char *pData, const int iSize, const unsigned char type);
int pushBlo(BlockInfo *pBlockInfo, const unsigned char *pData, const int iSize, const unsigned char *pf, const int ifSize);
int PopBlockInfoData(BlockInfo *pBlockInfo, void(*pFunc)(const unsigned char*, const int, void*), void* arg);
int PopBlockInfoDataNotConst(BlockInfo *pBlockInfo, void(*pFunc)(unsigned char*, const int, void*), void* arg);
int PopBlockInfoDataEx(BlockInfo *pBlockInfo, void(*pFunc)(const unsigned char*, const int, void*), void* arg, int(*loopConditionFunc)(void), int timeOut);
int PopBlockInfoDataExNotConst(BlockInfo *pBlockInfo, void(*pFunc)(unsigned char*, const int, void*), void* arg, int(*loopConditionFunc)(void *arg), int timeOut);
void releaseBlock(BlockInfo *pBlockInfo);

int create_thread_small(pthread_t *pid, void*(*thread_callback)(void*), void *arg);
int create_thread_middle(pthread_t *pid, void*(*thread_callback)(void*), void *arg);
int create_thread_normal(pthread_t *pid, void*(*thread_callback)(void*), void *arg);

int initVideoAudioBlock(BlockInfo *pBlockInfo);

void gettimeofday(struct timeval *tp, void *tzp);

int GetCode(unsigned char *src, int count, unsigned char **pOut, int *iSize);

void utf8ToWchar(const char* u, wchar_t *w);
void wcharToUtf8(const wchar_t *w, char *u);
void videoWcharToUtf8(const wchar_t *w, char *u);
void audioWcharToUtf8(const wchar_t *w, char *u);

void copyVideo(unsigned char *srcYuv, int srcW, int srcH, int copySrcX, int copySrcY, int copySrcW, int copySrcH, \
	unsigned char *dstYuv, int dstW, int dstH, int copyDstX, int copyDstY);

