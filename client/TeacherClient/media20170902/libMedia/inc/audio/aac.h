#pragma once

#include "common.h"

class AAC
{
public:
	AAC();
	~AAC();

	int init();
	int encode(unsigned char *pcmData, const int pcmSize, unsigned char *aacData);
	int getPcmSize();

private:
	void freeCoder();

	faacEncHandle hEncoder;
	unsigned long inputSamples, maxOutputBytes;
	int encodeBytesOneTime;
};

