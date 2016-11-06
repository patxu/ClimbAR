#include <vector>

#define DllExport   __declspec(	 dllexport )

extern "C" {
	DllExport int* classifyImage(unsigned char* data, int height, int width);
	DllExport int getNumHolds();
}