#include "video/videoDevice.h"

VideoDevice::VideoDevice()
{
	
}

VideoDevice::~VideoDevice()
{
	//cout << "into " << __FUNCTION__ << endl;
	exit();
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}


int VideoDevice::initPara(const int frameRate, const int width, const int height)
{
	if (frameRate <= 0 || frameRate > 300)
	{
		printf("%s: frameRate error ! frameRate = %d\n", __FUNCTION__, frameRate);
		return -1;
	}

	yuvFrameRate = frameRate;
	yuvWidth = width;
	yuvHeight = height;
	yuvArea = yuvWidth * yuvHeight;

	grabCtx = NULL;
	codecCtx = NULL;
	bmp2yuvCtx = NULL;
	bmpFrame = NULL;
	yuvFrame = NULL;

	return 0;
}

int VideoDevice::init(const int frameRate, const int width, const int height, const char *cameraDeviceName)
{
	finishInit = false;
	setGrabDeviceName();
	strcpy(cameraName, cameraDeviceName);

	int ret;
	ret = initPara(frameRate, width, height);
	if (ret < 0)
	{
		return -20;
	}

	grabCtx = avformat_alloc_context();
	if (!grabCtx)
	{
		printf("avformat_alloc_context grabCtx fail !\n");
		return -1;
	}

	ret = openInputStream(cameraDeviceName);
	if (ret < 0)
	{
		printf("openInputStream fail ! ret = %d\n", ret);
		return -2;
	}

	ret = avformat_find_stream_info(grabCtx, NULL);
	if (ret < 0)
	{
		printf("avformat_find_stream_info fail ! ret = %d\n", ret);
		return -3;
	}

	videoIndex = -1;
	for (int i = 0; i < (int)grabCtx->nb_streams; i ++)
	{
		if (grabCtx->streams[i]->codec->codec_type == AVMEDIA_TYPE_VIDEO)
		{
			videoIndex = i;
			break;
		}
	}
	if (videoIndex < 0)
	{
		printf("couldn't find a video stream.\n");
		return -4;
	}

	codecCtx = grabCtx->streams[videoIndex]->codec;
	AVCodec *codec = avcodec_find_decoder(codecCtx->codec_id);
	if (!codec)
	{
		printf("can not find codec ! codec_id = %u\n", codecCtx->codec_id);
		return -5;
	}

	//printf("videoIndex = %d, codecCtx->codec_id = %u\n", videoIndex, codecCtx->codec_id);

	ret = avcodec_open2(codecCtx, codec, NULL);
	if (ret < 0)
	{
		printf("avcodec_open2 fail ! ret = %d\n", ret);
		return -6;
	}

	bmp2yuvCtx = sws_getContext(codecCtx->width, codecCtx->height, codecCtx->pix_fmt, yuvWidth, yuvHeight, PIX_FMT_YUV420P, SWS_BICUBIC, NULL, NULL, NULL);
	if (!bmp2yuvCtx)
	{
		printf("sws_getContext fail !\n");
		return -7;
	}

	bmpFrame = av_frame_alloc();
	if (!bmpFrame)
	{
		printf("avcodec_alloc_frame fail ! bmpFrame == NULL !\n");
		return -8;
	}

	yuvFrame = av_frame_alloc();
	if (!yuvFrame)
	{
		printf("avcodec_alloc_frame fail ! yuvFrame == NULL !\n");
		return -9;
	}

	//printf("codecCtx->pix_fmt = %u\n", codecCtx->pix_fmt);

	bmpFrameSize = avpicture_get_size(codecCtx->pix_fmt, codecCtx->width, codecCtx->height);
	yuvFrameSize = avpicture_get_size(PIX_FMT_YUV420P, yuvWidth, yuvHeight);

	int *s = yuvFrame->linesize;
	s[0] = yuvWidth; s[1] = yuvWidth / 2; s[2] = yuvWidth / 2; s[3] = 0;
	yuvStep[0] = 0; yuvStep[1] = yuvArea; yuvStep[2] = yuvArea * 5 / 4; yuvStep[3] = 0;
	outCache = yuvFrame->data;
	outCache[3] = NULL;

	av_init_packet(&packet);

	finishInit = true;
	return 0;
}

void VideoDevice::exit()
{
	finishInit = false;

	if (bmpFrame)
	{
		av_frame_free(&bmpFrame);
		bmpFrame = NULL;
	}

	if (yuvFrame)
	{
		av_frame_free(&yuvFrame);
		yuvFrame = NULL;
	}

	if (bmp2yuvCtx)
	{
		sws_freeContext(bmp2yuvCtx);
		bmp2yuvCtx = NULL;
	}

	if (codecCtx)
	{
		avcodec_close(codecCtx);
		av_free(codecCtx);
		codecCtx = NULL;
	}

	if (grabCtx)
	{
		//avformat_close_input(&grabCtx);
		//avformat_free_context(grabCtx);

		grabCtx = NULL;
	}
}

int VideoDevice::getFrame(unsigned char *yuv)
{
	if (!yuv)
	{
		return -1;
	}

	if (!finishInit)
	{
		return -2;
	}

	int ret;
	packet.data = NULL;
	packet.size = 0;
	ret = av_read_frame(grabCtx, &packet);
	if (ret < 0)
	//if (ret < 0 || packet.buf == NULL || packet.data == NULL || packet.size <= 0)
	{
		//printf("av_read_frame fail ! ret = %d\n", ret);
		return -3;
	}

	int gotPicture = 0;
	if (packet.stream_index != 0)
	{
		printf("packet.stream_index error ! packet.stream_index = %d\n", packet.stream_index);
		return -4;
	}

	ret = avcodec_decode_video2(codecCtx, bmpFrame, &gotPicture, &packet);
	if (ret < 0)
	{
		printf("codec fail ! ret = %d\n", ret);
		return -5;
	}

	if (!gotPicture)
	{
		return -6;
	}

	outCache[0] = yuv;
	outCache[1] = yuv + yuvStep[1];
	outCache[2] = yuv + yuvStep[2];

	ret = sws_scale(bmp2yuvCtx, bmpFrame->data, bmpFrame->linesize, 0, codecCtx->height, yuvFrame->data, yuvFrame->linesize);
	if (ret < 0)
	{
		printf("sws_scale fail ! ret = %d\n", ret);
		return -6;
	}

	av_free_packet(&packet);
	return yuvFrameSize;
}

int VideoDevice::getYUVFrameSize()
{
	return yuvFrameSize;
}

int VideoDevice::openInputStream(const char *cameraDeviceName)
{
	if (cameraDeviceName[0] == '\0')
	{
		cout << "cameraDeviceName[0] is 0 !" << endl;
		return -1;
	}

	char fr[16];
	snprintf(fr, 16, "%d", yuvFrameRate);
	AVDictionary* options = NULL;
	av_dict_set(&options, "framerate", fr, 0);

	AVInputFormat *ifmt = av_find_input_format(grabDeviceName);

	int ret;
	ret = avformat_open_input(&grabCtx, cameraDeviceName, ifmt, &options);
	if (ret != 0)
	{
		/*char logBuf[1024];
		memset(logBuf, 0, sizeof(logBuf));
		av_strerror(ret, logBuf, 1024);
		cout << __FUNCTION__ << " avformat_open_input fail:" << ret << " cameraDeviceName = " << cameraDeviceName << " " << logBuf << endl;*/
		return -2;
	}

	return 0;
}

void VideoDevice::setGrabDeviceName()
{
	strcpy(grabDeviceName, "dshow");
}
