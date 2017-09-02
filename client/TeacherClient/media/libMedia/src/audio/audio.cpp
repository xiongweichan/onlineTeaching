#include "audio/audio.h"

static void* RunThread(void *arg);


Audio::Audio()
{
}

Audio::~Audio()
{
	exit();
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}


int Audio::init(const AudioSendCallBackType f, PushStream *push, const char *micName, const char *externMicName)
{
	threadRun = false;
	if (!f || !push || !micName || !externMicName)
	{
		return -1;
	}

	int ret;
	ret = lm.init(micName);
	if (ret < 0)
	{
		cout << "lm.init fail ! micName = " << micName << ", ret = " << ret << endl;
		return -2;
	}

	//ret = em.init(externMicName);
	//if (ret < 0)
	//{
	//	cout << "em.init fail ! externMicName = " << externMicName << ", ret = " << ret << endl;
	//	//return -3;
	//}

	ret = a.init();
	if (ret < 0)
	{
		cout << "a.init fail ! micName = " << micName << ", ret = " << ret << endl;
		return -4;
	}

	memset(pcmData, 0, sizeof(pcmData));
	memset(aacData, 0, sizeof(aacData));
	pcmSize = a.getPcmSize();
	sendFunc = f;
	p = push;
	threadRun = true;

	ret = pthread_create(&workThreadPid, NULL, RunThread, (void *)this);
	if (ret < 0)
	{
		threadRun = false;
		cout << "pthread_create fail ! ret1 = " << ret << endl;
		return -5;
	}

#ifdef SAVE_AAC_FILE
	fp_aac = fopen("audio.aac", "wb");
#endif
	
	return 0;
}

void Audio::exit()
{
	threadRun = false;
	pthread_join(workThreadPid, NULL);

	sendFunc = NULL;
	p = NULL;

#ifdef SAVE_AAC_FILE
	if (fp_aac)
	{
		fclose(fp_aac);
	}
#endif
}

static void* RunThread(void *arg)
{
	Audio *audio = (Audio *)arg;
	audio->run();

	return NULL;
}

void Audio::run()
{
	while (threadRun)
	{
		runOne();
	}
}

int Audio::runOne()
{
	waitEncodeDataBytes = lm.getFramePCM(pcmData);
	if (waitEncodeDataBytes <= 0)
	{
		return -2;
	}

	waitEncodeData = pcmData;
	while (waitEncodeDataBytes >= pcmSize)
	{
		encodeBytes = a.encode(waitEncodeData, pcmSize, aacData);
		if (encodeBytes > 0)
		{
			if (sendFunc)
			{
				sendFunc(aacData, encodeBytes, p);
#ifdef SAVE_AAC_FILE
				if (fp_aac)
				{
					fwrite(aacData, encodeBytes, 1, fp_aac);
				}
#endif
			}
		}
		waitEncodeDataBytes -= pcmSize;
		waitEncodeData += pcmSize;
	}

	return 0;
}

bool Audio::getThreadRunStatus()
{
	return threadRun;
}



