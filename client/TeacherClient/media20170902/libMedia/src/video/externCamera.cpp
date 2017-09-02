#include "video/externCamera.h"

ExternCamera::ExternCamera()
{
	
}

ExternCamera::~ExternCamera()
{
#ifdef PRINT_DCTOR
	cout << "exit " << __FUNCTION__ << endl;
#endif
}
