#include "video/yuvZoom.h"

YuvZoom::YuvZoom()
{

}

YuvZoom::~YuvZoom()
{
	exit();
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}

int YuvZoom::init(const int srcWidth, const int srcHeight, const int dstWidth, const int dstHeight)
{
	swsCtx = sws_getContext(srcWidth, srcHeight, PIX_FMT_YUV420P, dstWidth, dstHeight, PIX_FMT_YUV420P, SWS_BICUBIC, NULL, NULL, NULL);
	if (!swsCtx)
	{
		cout << __FUNCTION__ << ":sws_getContext fail !" << endl;
		return -1;
	}

	srcw = srcWidth;
	srch = srcHeight;
	dstw = dstWidth;
	dsth = dstHeight;

	srcStep[0] = 0;
	srcStep[1] = srcw * srch;
	srcStep[2] = srcw * srch * 5 / 4;
	srcStep[3] = 0;

	srcStride[0] = srcw;
	srcStride[1] = srcw / 2;
	srcStride[2] = srcw / 2;
	srcStride[3] = 0;

	dstStep[0] = 0;
	dstStep[1] = dstw * dsth;
	dstStep[2] = dstw * dsth * 5 / 4;
	dstStep[3] = 0;

	dstStride[0] = dstw;
	dstStride[1] = dstw / 2;
	dstStride[2] = dstw / 2;
	dstStride[3] = 0;

	return 0;
}

void YuvZoom::exit()
{
	sws_freeContext(swsCtx);
	swsCtx = NULL;
}

int YuvZoom::convert(unsigned char *src, unsigned char *dst)
{
	if (!swsCtx)
	{
		return -1;
	}

	for (int i = 0; i < 3; i ++)
	{
		srcData[i] = src + srcStep[i];
		dstData[i] = dst + dstStep[i];
	}

	int ret;
	ret = sws_scale(swsCtx, srcData, srcStride, 0, srch, dstData, dstStride);
	if (ret < 0)
	{
		return -2;
	}

	return 0;
}
