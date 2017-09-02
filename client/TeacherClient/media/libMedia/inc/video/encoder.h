#pragma once

#include "common.h"

class Encoder
{
public:
	typedef struct _coder_decoder_handle
	{
		AVCodec    *codec;
		AVCodecContext *c;
		AVFrame  *picture;
		AVPacket    avpkt;
		long        start;
		int			size;
	}CoderDecoderHandle;

public:
	Encoder();
	~Encoder();

	int init(const int w, const int h, const int framerate, const int bit_rate);
	int frameCode(unsigned char *yuv, unsigned char *h264);
	int frameCode(unsigned char *yuv, unsigned char *buf, const int off, int *key);
	int fflushCoder(unsigned char *buf, const int off, int *key);

private:
	void exit();
	int frameCode(unsigned char *buf, const int off, int *key);
	AVFrame* alloc_picture(const enum PixelFormat pix_fmt, const int width, const int height);
	int _frameCode(unsigned char *buf, const int off, int *key, const int isFlush);
	int setPicture(unsigned char *yuv);

	CoderDecoderHandle handle;
	int k;
};

