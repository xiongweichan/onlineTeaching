#include "video/encoder.h"

Encoder::Encoder()
{
}

Encoder::~Encoder()
{
	exit();
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}

int Encoder::init(const int w, const int h, const int framerate, const int bit_rate)
{
	avcodec_register_all();
	int gop_size = 30;
	memset(&handle, 0, sizeof(CoderDecoderHandle));

	handle.start = -1;

	if (!(handle.codec = avcodec_find_encoder(CODEC_ID_H264)))
	{
		printf("can not find encoder !\n");
		return -1;
	}

	if ((handle.c = avcodec_alloc_context3(handle.codec)) == NULL)
	{
		printf("alloc context fail !\n");
		return -2;
	}

	AVCodecContext *c = handle.c;
	c->flags |= CODEC_FLAG_QSCALE;
	c->rc_min_rate = bit_rate;
	c->rc_max_rate = bit_rate;
	c->bit_rate = bit_rate;
	c->rc_buffer_size = bit_rate;
	c->rc_initial_buffer_occupancy = c->rc_buffer_size * 3 / 4;
	c->rc_buffer_aggressivity = (float)1.0;
	c->rc_initial_cplx = 0.5;
	c->qcompress = (float)0.6;
	c->qmin = 10;
	c->qmax = 30;
	c->max_qdiff = 4;

	c->width = w;
	c->height = h;
	c->time_base.den = framerate;
	c->time_base.num = 1;
	c->gop_size = gop_size;
	c->pix_fmt = PIX_FMT_YUV420P;
	c->max_b_frames = 0;
	av_opt_set(c->priv_data, "preset", "veryfast", 0);
	//av_opt_set(pHandle->c->priv_data, "preset", "slow", 0);

	if (avcodec_open2(handle.c, handle.codec, NULL) < 0)
	{
		av_free(c);
		handle.c = NULL;
		printf("open coder fail !\n");
		return -3;
	}

	if ((handle.picture = alloc_picture(c->pix_fmt, c->width, c->height)) == NULL)
	{
		avcodec_close(c);
		av_free(c);
		handle.c = NULL;
		printf("alloc_picture fail !\n");
		return -4;
	}

	handle.picture->pts = 0;
	handle.size = 1024 * 1024;
	return 0;
}

AVFrame* Encoder::alloc_picture(const enum PixelFormat pix_fmt, const int width, const int height)
{
	AVFrame *picture;
	uint8_t *picture_buf;
	int size;

	picture = av_frame_alloc();
	if (!picture)
		return NULL;
	size = avpicture_get_size(pix_fmt, width, height);
	picture_buf = (uint8_t *)av_malloc(size);
	if (!picture_buf) {
		av_free(picture);
		return NULL;
	}
	avpicture_fill((AVPicture *)picture, picture_buf,
		pix_fmt, width, height);
	return picture;
}

int Encoder::setPicture(unsigned char *yuv)
{
	int i, w, h;
	AVFrame *picture = handle.picture;

	w = (handle.c)->width;
	h = (handle.c)->height;

	unsigned char *pY = yuv;
	unsigned char *pU = pY + w*h;
	unsigned char *pV = pU + w*h / 4;

	for (i = 0; i < h; i++)
	{
		memcpy(picture->data[0] + i*picture->linesize[0], pY + i*w, w);

		if (!(i % 2))
		{
			memcpy(picture->data[1] + (i / 2)*picture->linesize[1], pU + (i / 2)*(w / 2), w / 2);
			memcpy(picture->data[2] + (i / 2)*picture->linesize[2], pV + (i / 2)*(w / 2), w / 2);
		}
	}

	picture->pts++;

	return 0;
}

int Encoder::frameCode(unsigned char *yuv, unsigned char *h264)
{
	return frameCode(yuv, h264, 0, &k);
}

int Encoder::frameCode(unsigned char *buf, const int off, int *key)
{
	return _frameCode(buf, off, key, 0);
}

int Encoder::frameCode(unsigned char *yuv, unsigned char *buf, const int off, int *key)
{
	setPicture(yuv);
	return frameCode(buf, off, key);
}

int Encoder::fflushCoder(unsigned char *buf, const int off, int *key)
{
	return _frameCode(buf, off, key, 1);
}

int Encoder::_frameCode(unsigned char *buf, const int off, int *key, const int isFlush)
{
	int got = 0;
	av_init_packet(&(handle.avpkt));

	handle.avpkt.size = handle.size;
	handle.avpkt.data = (uint8_t *)buf + off;
	AVFrame *avP = isFlush ? (AVFrame *)NULL : handle.picture;

	if (0 != avcodec_encode_video2(handle.c, &handle.avpkt, avP, &got))
	{
		return -1;
	}

	if (got)
	{
		*key = handle.c->coded_frame->key_frame;
		return handle.avpkt.size;
	}

	return -2;
}

void Encoder::exit()
{
	if (handle.c)
	{
		avcodec_close(handle.c);
		av_free(handle.c);
		handle.c = NULL;
	}

	if (handle.picture)
	{
		av_free(handle.picture->data[0]);
		av_free(handle.picture);
		handle.picture = NULL;
	}
}


/*int main()
{
	printf("+%s\n", __FUNCTION__);
	const char filename_in[] = "ds_480x272.yuv";
	const char filename_out[] = "ds.h264";
	const int in_w = 480, in_h = 272, frameRate = 25, bitRate = 400000;
	int frameSize = in_w * in_h * 3 / 2;
	FILE *fp_in, *fp_out;
	int ret;
	Encoder *e = new Encoder();
	if (!e)
	{
	printf("new Encoder fail !\n");
	return -1;
	}

	ret = e->init(in_w, in_h, frameRate, bitRate);
	if (ret < 0)
	{
	delete e;
	return -2;
	}

	fp_in = fopen(filename_in, "rb");
	if (!fp_in) {
	printf("Could not open %s\n", filename_in);
	delete e;
	system("pause");
	return -3;
	}

	fp_out = fopen(filename_out, "wb");
	if (!fp_out) {
	printf("Could not open %s\n", filename_out);
	fclose(fp_in);
	delete e;
	system("pause");
	return -4;
	}

	unsigned char *yuv = new unsigned char[1920 * 1080 * 3 / 2];
	if (!yuv)
	{
	printf("new yuv fail \n");
	fclose(fp_in);
	fclose(fp_out);
	delete e;
	system("pause");
	return -5;
	}

	int isKeyFrame = 0;
	unsigned char *h264 = new unsigned char[1024 * 1024];
	if (!h264)
	{
	printf("malloc h264 buf fail !\n");
	delete[] yuv;
	fclose(fp_in);
	fclose(fp_out);
	delete e;
	system("pause");
	return -6;
	}

	for (;;)
	{
	ret = fread(yuv, 1, frameSize, fp_in);
	if (ret < frameSize)
	{
	printf("ret < frameSize ! finish ! ret = %d\n", ret);
	break;
	}

	ret = e->frameCode(yuv, h264, 0, &isKeyFrame);
	if (ret < 0)
	{
	//printf("frameCode fail ! ret = %d\n", ret);
	}
	else
	{
	fwrite(h264, ret, 1, fp_out);
	//printf("encode %d bytes !\n", ret);
	}
	}

	do
	{
	ret = e->fflushCoder(h264, 0, &isKeyFrame);
	if (ret > 0)
	{
	fwrite(h264, ret, 1, fp_out);
	}
	//printf("fflush byte = %d\n", ret);
	} while (ret > 0);

	delete[] yuv;
	delete[] h264;
	fclose(fp_in);
	fclose(fp_out);
	delete e;

	system("pause");
	return 0;
}*/
