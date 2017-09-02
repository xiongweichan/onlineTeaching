#include "video/yuv2bmp.h"

yuv2bmp::yuv2bmp()
{
}

yuv2bmp::~yuv2bmp()
{
	exit();
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}

void yuv2bmp::initPara(const int orgWidth, const int orgHeight, const int dstWidth, const int dstHeight)
{
	orgw = orgWidth; orgh = orgHeight; dstw = dstWidth; dsth = dstHeight;

	lineBytes = (dstw * 3 + 3) / 4 * 4;
	orgArea = orgw * orgh; dstArea = dstw * dsth;
	srcStride[0] = orgw; srcStride[1] = orgw / 2; srcStride[2] = orgw / 2; srcStride[3] = 0;
	dstStride[0] = lineBytes; dstStride[1] = 0; dstStride[2] = 0; dstStride[3] = 0;
	srcData[3] = NULL;
	srcStep[0] = 0; srcStep[1] = orgArea; srcStep[2] = orgArea * 5 / 4; srcStep[3] = 0;
	dstStep[0] = 0; dstStep[1] = dstArea; dstStep[2] = dstArea * 2; dstStep[3] = dstArea * 3;

	srcStep[0] += srcStride[0] * (dsth - 1);
	srcStep[1] += srcStride[1] * (dsth / 2 - 1);
	srcStep[2] += srcStride[2] * (dsth / 2 - 1);
	srcStride[0] = -srcStride[0]; srcStride[1] = -srcStride[1]; srcStride[2] = -srcStride[2];

	fileHead.bfType = 0x4D42;
	fileHead.bfReserved1 = 0;
	fileHead.bfReserved2 = 0;
	fileHead.bfOffBits = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);
	fileHead.bfSize = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER) + lineBytes * dsth;

	infoHead.biSize = 0x28;
	infoHead.biWidth = dstw;
	infoHead.biHeight = dsth;
	infoHead.biPlanes = 1;
	infoHead.biBitCount = 24;
	//infoHead.biCompression = BI_RGB;
	infoHead.biCompression = 0;
	infoHead.biSizeImage = lineBytes * dsth;
	infoHead.biXPelsPerMeter = 0;
	infoHead.biYPelsPerMeter = 0;
	infoHead.biClrUsed = 0; 
	infoHead.biClrImportant = 0;

	yuvFrameSize = orgArea * 3 / 2;
	rgbFrameSize = dstArea * 3;
	bmpFrameSize = dstArea * 3 + sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);
}

int yuv2bmp::init(const int orgWidth, const int orgHeight, const int dstWidth, const int dstHeight)
{
	initPara(orgWidth, orgHeight, dstWidth, dstHeight);

	av_register_all();
	converContext = sws_getContext(orgw, orgh, PIX_FMT_YUV420P, dstw, dsth, PIX_FMT_RGB24, SWS_BICUBIC, NULL, NULL, NULL);
	return 0;
}

void yuv2bmp::exit()
{
	sws_freeContext(converContext);
}

int yuv2bmp::yuv420TOrgb24(unsigned char *yuv, unsigned char *rgb)
{
	if (!converContext || !yuv || !rgb)
	{
		return -1;
	}

	srcData[0] = yuv + srcStep[0];
	/*srcData[1] = yuv + srcStep[1];
	srcData[2] = yuv + srcStep[2];*/
	srcData[1] = yuv + srcStep[2];
	srcData[2] = yuv + srcStep[1];
	
	dstData[0] = rgb;
	dstData[1] = rgb + dstStep[1];
	dstData[2] = rgb + dstStep[2];
	dstData[3] = rgb + dstStep[3];
	
	int ret = sws_scale(converContext, (const uint8_t * const*)srcData, srcStride, 0, orgh, dstData, dstStride);
	if (ret < 0)
	{
		printf("%s:sws_scale fail ! ret = %d\n", __FUNCTION__, ret);
		return -2;
	}

	return 0;
}

void yuv2bmp::fillBMPHead(unsigned char *bmp)
{
	BITMAPFILEHEADER *f = (BITMAPFILEHEADER *)bmp;
	memcpy(f, &fileHead, sizeof(BITMAPFILEHEADER));

	BITMAPINFOHEADER *i = (BITMAPINFOHEADER *)(bmp + sizeof(BITMAPFILEHEADER));
	memcpy(i, &infoHead, sizeof(BITMAPINFOHEADER));
}

int yuv2bmp::convert(unsigned char *yuv, unsigned char *bmp)
{
	unsigned char *rgb = bmp + sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);
	int ret = yuv420TOrgb24(yuv, rgb);
	if (ret < 0)
	{
		return -1;
	}

	fillBMPHead(bmp);
	return bmpFrameSize;
}

int yuv2bmp::getYUVFrameSize()
{
	return yuvFrameSize;
}

int yuv2bmp::getBMPFrameSize()
{
	return bmpFrameSize;
}

//int main()
//{
//	yuv2bmp c(1920, 1080, 1920, 1080);
//
//	FILE *fp_yuv, *fp_bmp;
//	fp_yuv = fopen("ds.yuv", "rb");
//	fp_bmp = fopen("ds.bmp", "wb");
//	if (!fp_yuv || !fp_bmp)
//	{
//		return -1;
//	}
//
//	int yuvFrameSize = c.getYUVFrameSize();
//	int bmpSize = c.getBMPFrameSize();
//	unsigned char *yuv = new unsigned char[yuvFrameSize + 1];
//	unsigned char *bmp = new unsigned char[bmpSize + 1];
//
//	int readBytes, ret;
//	for (;;)
//	{
//		readBytes = fread(yuv, 1, yuvFrameSize, fp_yuv);
//		if (readBytes == yuvFrameSize)
//		{
//			ret = c.convert(yuv, bmp);
//			if (ret == 0)
//			{
//				fwrite(bmp, bmpSize, 1, fp_bmp);
//			}
//			else
//			{
//				printf("convert fail ! ret = %d\n", ret);
//			}
//			//break;
//		}
//		else
//		{
//			printf("fread fail ! readBytes = %d\n", readBytes);
//			break;
//		}
//	}
//
//	delete[]yuv;
//	delete[]bmp;
//	fclose(fp_yuv);
//	fclose(fp_bmp);
//
//	system("pause");
//	return 0;
//}
