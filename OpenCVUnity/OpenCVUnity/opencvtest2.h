#include <vector>

#define DllExport   __declspec(	 dllexport )

extern "C" {
	DllExport int* classifyImage(unsigned char* data, int height, int width);
	DllExport int* findProjectorBox(unsigned char* redData, unsigned char* greenData, unsigned char* blueData, int imageWidth, int imageHeight);
	DllExport int getNumHolds();
}