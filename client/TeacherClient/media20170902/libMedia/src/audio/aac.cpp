#include "audio/aac.h"

using namespace std;

AAC::AAC()
{
}

AAC::~AAC()
{
	freeCoder();
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}

int AAC::init()
{
	hEncoder = faacEncOpen(44100, 2, &inputSamples, &maxOutputBytes);
	if (!hEncoder)
	{
		printf("faacEncOpen fail !\n");
		return -1;
	}

	//printf("inputSamples=%u, maxOutputBytes=%u\n", inputSamples, maxOutputBytes);
	faacEncConfigurationPtr myFormat = faacEncGetCurrentConfiguration(hEncoder);
	if (!myFormat)
	{
		printf("faacEncGetCurrentConfiguration fail !\n");
		faacEncClose(hEncoder);
		hEncoder = NULL;
		return -2;
	}

	myFormat->inputFormat = FAAC_INPUT_16BIT;
	myFormat->allowMidside = 1;
	myFormat->aacObjectType = LOW;
	myFormat->mpegVersion = MPEG4;
	myFormat->outputFormat = 1;
	myFormat->useTns = 1;
	myFormat->useLfe = 0;
	myFormat->quantqual = 100;
	myFormat->bitRate = 0;
	myFormat->bandWidth = 0;
	myFormat->shortctl = SHORTCTL_NORMAL;
	faacEncSetConfiguration(hEncoder, myFormat);
	encodeBytesOneTime = (int)inputSamples * 2;
	return 0;
}

int AAC::encode(unsigned char *pcmData, const int pcmSize, unsigned char *aacData)
{
	if (!hEncoder)
	{
		printf("%s: hEncoder has not been init correct !\n", __FUNCTION__);
		return -1;
	}

	if ((unsigned long)pcmSize < inputSamples * 2)
	{
		printf("pcmSize < inputSamples * 2 ! give up the pcm frame !\n");
		return -2;
	}

	int ret = faacEncEncode(hEncoder, (int *)pcmData, inputSamples, aacData, maxOutputBytes);
	if (ret < 0)
	{
		printf("faacEncEncode fail ! ret = %d\n", ret);
		return -3;
	}
	//printf("encode %d bytes pcm data to %d bytes aac data !\n", pcmSize, ret);

	return ret;
}

int AAC::getPcmSize()
{
	return encodeBytesOneTime;
}

void AAC::freeCoder()
{
	if (hEncoder)
	{
		faacEncClose(hEncoder);
	}
}


/*int main()
{
	FILE *fp_pcm = fopen("1.pcm", "rb");
	if (!fp_pcm)
	{
		printf("fopen fail !\n");
		return -1;
	}

	FILE *fp_aac = fopen("1.aac", "wb");
	if (!fp_aac)
	{
		printf("fopen fail ! \n");
		return -2;
	}

	FILE *fp_aacFrameSize = fopen("1.aac.hdr", "wb");
	if (!fp_aacFrameSize)
	{
		printf("fopen fail !\n");
		return -3;
	}

	AAC a;
	unsigned char pcmData[8192], aacData[4096];
	char tmpBuf[64];
	int readBytes, aacBytes, pcmSize = a.getPcmSize(), writeBytes;
	while (1)
	{
		readBytes = fread(pcmData, 1, pcmSize, fp_pcm);
		if (readBytes == pcmSize)
		{
			aacBytes = a.encode(pcmData, pcmSize, aacData);
			if (aacBytes > 0)
			{
				fwrite(aacData, aacBytes, 1, fp_aac);
				writeBytes = snprintf(tmpBuf, 64, "%d\n", aacBytes);
				fwrite(tmpBuf, writeBytes, 1, fp_aacFrameSize);
			}
		}
		else
		{
			printf("readBytes, inputSamples = %d,%d\n", readBytes, pcmSize);
			break;
		}
	}

	fclose(fp_aacFrameSize);
	fclose(fp_pcm);
	fclose(fp_aac);
	system("pause");
	return 0;
}*/
