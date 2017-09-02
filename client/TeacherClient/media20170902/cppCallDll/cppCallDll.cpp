#include "interface.h"

static FILE *fp;
int main()
{
	unsigned char *data = new unsigned char[1920 * 1080 * 4 + 99];
	if (!data)
	{
		cout << "new fail !" << endl;
		return -11;
	}
	int dataSize;

	fp = fopen("play.bmp", "wb");
	if (!fp)
	{
		delete data;
		system("pause");
		return -1;
	}

	MediaInterface *mi = new MediaInterface();
	if (!mi)
	{
		delete data;
		fclose(fp);
		system("pause");
		return -2;
	}

	int ret;
	ret = mi->init(25, 1024 * 1024, 800, 600, 1920, 1080, 352, 288);
	if (ret < 0)
	{
		cout << "mi->init fail ! ret = " << ret << endl;
		delete data;
		delete mi;
		fclose(fp);
		system("pause");
		return -3;
	}

	ret = mi->startPushStream("rtmp://192.168.100.49/live/w");
	if (ret < 0)
	{
		cout << "mi->startPushStream fail ! ret = " << ret << endl;
		delete data;
		delete mi;
		fclose(fp);
		system("pause");
		return -4;
	}

	cout << "cur send stream index is " << mi->getSendStreamIndex() << endl;
	cout << "cur play stream index is " << mi->getLocalStreamIndex() << endl;
	mi->setShowSmallStream(1, 0);
	int a, b;
	mi->getShowSmallStream(&a, &b);
	cout << "setShowSmallStream = " << a << " " << b << endl;

	bool hasWrite = false;
	static unsigned int frameNum = 0;
	while (true)
	{
		dataSize = mi->getPlayData(data);
		if (hasWrite == false && dataSize > 0)
		{
			hasWrite = true;
			fwrite(data, dataSize, 1, fp);
			fclose(fp);
		}

		/*frameNum++;
		if (frameNum> 250)
		{
			break;
		}*/
		usleep(10 * 1000);
	}

	mi->stopPushStream();
	delete mi;

	if (fp)
	{
		fclose(fp);
	}
	system("pause");
	return 0;
}
