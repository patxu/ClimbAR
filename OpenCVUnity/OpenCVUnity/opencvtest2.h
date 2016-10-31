#include <vector>

#define DllExport   __declspec(	 dllexport )

extern "C" {
	DllExport void TestSort(int a[], int length);
	DllExport int* OpenCVFunc(unsigned char* data, int height, int width);
	DllExport int NumHolds();
}