#include "getDeviceName.h"  

static HRESULT getDeviveNames(TDeviceName *deviceNames, int &deviceNum, REFGUID guidValue);


HRESULT getVideoDeviceNames(TDeviceName *deviceNames, int &deviceNum)
{
	return getDeviveNames(deviceNames, deviceNum, CLSID_VideoInputDeviceCategory);
}

HRESULT getAudioDeviceNames(TDeviceName *deviceNames, int &deviceNum)
{
	return getDeviveNames(deviceNames, deviceNum, CLSID_AudioInputDeviceCategory);
}


HRESULT getDeviveNames(TDeviceName *deviceNames, int &deviceNum, REFGUID guidValue)
{
	deviceNum = 0;

	TDeviceName *name;
	HRESULT hr;

	// 初始化COM  
	hr = CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
	if (FAILED(hr))
	{
		cout << "CoInitializeEx fail !" << endl;
		CoUninitialize();
		hr = CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
		if (FAILED(hr))
		{
			cout << "CoInitializeEx fail !" << endl;
			return hr;
		}
	}

	// 创建系统设备枚举器实例  
	ICreateDevEnum *pSysDevEnum = NULL;
	hr = CoCreateInstance(CLSID_SystemDeviceEnum, NULL, CLSCTX_INPROC_SERVER, IID_ICreateDevEnum, (void **)&pSysDevEnum);
	if (FAILED(hr))
	{
		cout << "CoCreateInstance fail !" << endl;
		CoUninitialize();
		return hr;
	}

	// 获取设备类枚举器  
	IEnumMoniker *pEnumCat = NULL;
	hr = pSysDevEnum->CreateClassEnumerator(guidValue, &pEnumCat, 0);
	if (hr == S_OK)
	{
		// 枚举设备名称  
		IMoniker *pMoniker = NULL;
		ULONG cFetched;
		while (pEnumCat->Next(1, &pMoniker, &cFetched) == S_OK)
		{
			name = &deviceNames[deviceNum];
			IPropertyBag *pPropBag;
			hr = pMoniker->BindToStorage(NULL, NULL, IID_IPropertyBag, (void **)&pPropBag);
			if (SUCCEEDED(hr))
			{
				// 获取设备友好名  
				VARIANT varName;
				VariantInit(&varName);

				hr = pPropBag->Read(L"FriendlyName", &varName, NULL);
				if (SUCCEEDED(hr))
				{
					StringCchCopy(name->FriendlyName, MAX_FRIENDLY_NAME_LENGTH, varName.bstrVal);
					//cout << "name->FriendlyName = " << name->FriendlyName << endl;

					// 获取设备Moniker名  
					LPOLESTR pOleDisplayName = reinterpret_cast<LPOLESTR>(CoTaskMemAlloc(MAX_MONIKER_NAME_LENGTH * 2));
					if (pOleDisplayName != NULL)
					{
						hr = pMoniker->GetDisplayName(NULL, NULL, &pOleDisplayName);
						if (SUCCEEDED(hr))
						{
							StringCchCopy(name->MonikerName, MAX_MONIKER_NAME_LENGTH, pOleDisplayName);
							//cout << "name->MonikerName = " << name->MonikerName << endl;
							
						}

						CoTaskMemFree(pOleDisplayName);
					}
				}

				deviceNum++;

				VariantClear(&varName);
				pPropBag->Release();
			}

			pMoniker->Release();
		} // End for While  

		pEnumCat->Release();
	}
	else
	{
		cout << "pSysDevEnum->CreateClassEnumerator fail !" << endl;
	}

	pSysDevEnum->Release();
	CoUninitialize();

	return hr;

}

