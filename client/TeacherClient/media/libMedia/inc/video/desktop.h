#pragma once

#include "videoDevice.h"

class Desktop: public VideoDevice
{
public:
	Desktop();
	~Desktop();

	void setGrabDeviceName();

private:

};
