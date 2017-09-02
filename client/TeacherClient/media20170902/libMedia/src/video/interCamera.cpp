#include "video/interCamera.h"

InterCamera::InterCamera()
{
	
}

InterCamera::~InterCamera()
{
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}
