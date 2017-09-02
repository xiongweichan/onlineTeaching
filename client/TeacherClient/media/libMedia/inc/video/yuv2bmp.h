#pragma once

#include "common.h"

class yuv2bmp
{
public:

#pragma pack(push)
#pragma pack(1)

//14 BYTE
typedef struct {
	unsigned short bfType;
	unsigned int   bfSize;
	unsigned short bfReserved1;
	unsigned short bfReserved2;
	unsigned int   bfOffBits;
}BITMAPFILEHEADER;

//40 BYTE
typedef struct {
	unsigned int   biSize;
	int            biWidth;
	int            biHeight;
	unsigned short biPlanes;
	unsigned short biBitCount;
	unsigned int   biCompression;
	unsigned int   biSizeImage;
	int       	biXPelsPerMeter;
	int       	biYPelsPerMeter;
	unsigned int   biClrUsed;
	unsigned int   biClrImportant;
}BITMAPINFOHEADER;
#pragma pack(pop)

public:
	yuv2bmp();
	~yuv2bmp();

	int init(const int orgWidth, const int orgHeight, const int dstWidth, const int dstHeight);
	int convert(unsigned char *yuv, unsigned char *bmp);

	int getYUVFrameSize();
	int getBMPFrameSize();

private:
	void exit();

	void initPara(const int orgWidth, const int orgHeight, const int dstWidth, const int dstHeight);
	int yuv420TOrgb24(unsigned char *yuv, unsigned char *rgb);
	void fillBMPHead(unsigned char *bmp);

	int orgw, orgh, dstw, dsth, orgArea, dstArea;
	int srcStride[4], dstStride[4];
	unsigned char *srcData[4], *dstData[4];
	unsigned int srcStep[4], dstStep[4];
	struct SwsContext *converContext;

	int lineBytes;
	BITMAPFILEHEADER fileHead;
	BITMAPINFOHEADER infoHead;
	int yuvFrameSize, rgbFrameSize, bmpFrameSize;
	unsigned char *tmpRGB;
};

