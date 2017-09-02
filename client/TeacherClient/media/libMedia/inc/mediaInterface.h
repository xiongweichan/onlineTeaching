#pragma once

#include "media.h"

#ifdef LIBMEDIA_EXPORTS
#define DLL_API __declspec(dllexport)
#else
#define DLL_API __declspec(dllimport)
#endif

extern "C"
{
	DLL_API Media* Media_CreatePtr();
	DLL_API void Media_DeletePtr(Media *m);

	DLL_API int Media_Init(Media *m, int frameRate, int sendBitRate, int sendWidth, int sendHeight, int localWidth, int localHeight, int localSmallWidth, int localSmallHeight);
	DLL_API int Media_StartPushStream(Media *m, const char *url);
	DLL_API void Media_StopPushStream(Media *m);

	DLL_API int Media_SetSendStreamIndex(Media *m, int index);
	DLL_API int Media_GetSendStreamIndex(Media *m);
	DLL_API int Media_SetLocalStreamIndex(Media *m, int index);
	DLL_API int Media_GetLocalStreamIndex(Media *m);

	DLL_API int Media_GetPlayData(Media *m, unsigned char *data);

	DLL_API void Media_SetShowSmallStream(Media *m, int needShowA, int needShowB);
	DLL_API void Media_GetShowSmallStream(Media *m, int *needShowA, int *needShowB);
}
