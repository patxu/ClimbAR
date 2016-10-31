#include "opencvtest2.h"
#include <algorithm>
#include <opencv2/opencv.hpp>
#include <opencv\highgui.h>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <opencv2/objdetect/objdetect.hpp>
#include <iostream>

using namespace cv;
using namespace std;

extern "C" {
	int *bb_array;
	int num_holds;
	

	void TestSort(int a[], int length) {
		std::sort(a, a + length);
	}

	int NumHolds() {
		return num_holds;
	}

	int* OpenCVFunc(unsigned char* data, int height, int width) {
		CascadeClassifier classifier;
		String classifierName = "C:\\cascade.xml";
		String img = "C:\\img.jpg";

		if (!classifier.load(classifierName)) {
			cout << "not working";
			/*return nullptr;*/
		}
		Mat image = imread(img, CV_LOAD_IMAGE_GRAYSCALE);
			//Mat(height, width, CV_8UC3, data);
		
		std::vector<Rect> holds;
		classifier.detectMultiScale(image, holds, 1.2, 30, 0, Size(30, 30), Size(600, 600));
		num_holds = holds.size();

		// TODO another func that will clear memory
		bb_array = new int[holds.size() * 4]; // top left x, top left y, width, height )
		int array_index=0;
		for (int i = 0; i < holds.size(); i++) {
			bb_array[array_index++] = holds[i].x;
			bb_array[array_index++] = holds[i].y;
			bb_array[array_index++] = holds[i].width;
			bb_array[array_index++] = holds[i].height;
		}
		return &bb_array[0];
	}
}
