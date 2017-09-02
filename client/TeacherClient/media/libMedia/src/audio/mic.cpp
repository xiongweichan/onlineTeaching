#include "audio/mic.h"

Mic::Mic()
{
}

Mic::~Mic()
{
	exit();
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}


int Mic::init(const char *micName)
{
	int ret;
	grabCtx = avformat_alloc_context();
	if (!grabCtx)
	{
		printf("avformat_alloc_context fail !\n");
		return -1;
	}

	AVInputFormat *ifmt = av_find_input_format("dshow");
	ret = avformat_open_input(&grabCtx, micName, ifmt, NULL);
	if (ret < 0)
	{
		printf("avformat_open_input fail ! ret = %d\n", ret);
		return -2;
	}

	ret = avformat_find_stream_info(grabCtx, NULL);
	if (ret < 0)
	{
		printf("avformat_find_stream_info fail ! ret = %d\n", ret);
		return -3;
	}

	av_init_packet(&packet);

	return 0;
}

void Mic::exit()
{
	if (grabCtx)
	{
		//avformat_close_input(&grabCtx);
		//avformat_free_context(grabCtx);
		grabCtx = NULL;
	}
}

int Mic::getFramePCM(unsigned char *pcm)
{
	packet.data = NULL;
	packet.size = 0;
	if (av_read_frame(grabCtx, &packet) < 0)
	{
		return -1;
	}

	memcpy(pcm, packet.data, packet.size);
	return packet.size;
}
