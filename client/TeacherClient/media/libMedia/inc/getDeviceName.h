#pragma once

#pragma comment(lib, "strmiids.lib")  

#include <iostream>
using namespace std;

//#include <windows.h>  
#include <dshow.h>  

#define _WIN32_DCOM

#ifndef MACRO_GROUP_DEVICENAME  
#define MACRO_GROUP_DEVICENAME  

#define MAX_FRIENDLY_NAME_LENGTH    128  
#define MAX_MONIKER_NAME_LENGTH     256  

typedef struct
{
	WCHAR FriendlyName[MAX_FRIENDLY_NAME_LENGTH];   // �豸�Ѻ���  
	WCHAR MonikerName[MAX_MONIKER_NAME_LENGTH];     // �豸Moniker��  
} TDeviceName;
#endif  

#ifdef __cplusplus  
extern "C"
{
#endif  

	/*
	���ܣ���ȡ��Ƶ��Ƶ�����豸�б�
	����˵����
	vectorDevices�����ڴ洢���ص��豸�Ѻ�����Moniker��
	guidValue��
	CLSID_AudioInputDeviceCategory����ȡ��Ƶ�����豸�б�
	CLSID_VideoInputDeviceCategory����ȡ��Ƶ�����豸�б�
	����ֵ��
	�������
	˵����
	����DirectShow
	�б��еĵ�һ���豸Ϊϵͳȱʡ�豸
	capGetDriverDescriptionֻ�ܻ���豸������
	*/


	HRESULT getVideoDeviceNames(TDeviceName *deviceNames, int &deviceNum);
	HRESULT getAudioDeviceNames(TDeviceName *deviceNames, int &deviceNum);

#ifdef __cplusplus  
}

#endif  