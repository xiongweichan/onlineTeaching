#pragma once

#include "common.h"

class YuvZoom
{
public:
	YuvZoom();
	~YuvZoom();

	int init(const int srcWidth, const int srcHeight, const int dstWidth, const int dstHeight);
	int convert(unsigned char *src, unsigned char *dst);

private:
	void exit();

	SwsContext *swsCtx;
	int srcw, srch, dstw, dsth;
	int srcStep[4], srcStride[4], dstStep[4], dstStride[4];
	unsigned char *srcData[4], *dstData[4];
};
