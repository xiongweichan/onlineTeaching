#include "video/desktop.h"

Desktop::Desktop()
{
	
}

Desktop::~Desktop()
{
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}


void Desktop::setGrabDeviceName()
{
	strcpy(grabDeviceName, "gdigrab");
}