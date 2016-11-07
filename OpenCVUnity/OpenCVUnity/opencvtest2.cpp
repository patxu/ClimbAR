#include "opencvtest2.h"
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
	String img = "C:\\cs98-senior-project\\OpenCV_files\\img.jpg";


	int getNumHolds() {
		return num_holds;
	}

	int* classifyImage(unsigned char* data, int width, int height) {
		CascadeClassifier classifier;
		String classifierName = "C:\\cs98-senior-project\\OpenCV_files\\cascade.xml";
		//this should be a condition that checks whether data is passed in
		Mat image;
		if (false)
		{
			image = imread(img, CV_LOAD_IMAGE_GRAYSCALE);
		}
		else
		{
			Mat temp = Mat(height, width, CV_8UC4, data); // might be CV_8UC3?
			cv::cvtColor(temp, image, CV_BGR2GRAY);
		}

		if (!classifier.load(classifierName)) {
			cout << "not working";
			/*return nullptr;*/
		}

		std::vector<Rect> holds;
		classifier.detectMultiScale(image, holds, 1.2, 30, 0, Size(30, 30), Size(600, 600));
		num_holds = holds.size();

		// TODO another func that will clear memory
		bb_array = new int[holds.size() * 4]; // top left x, top left y, width, height )
		int array_index = 0;
		for (int i = 0; i < holds.size(); i++) {
			bb_array[array_index++] = holds[i].x;
			bb_array[array_index++] = holds[i].y;
			bb_array[array_index++] = holds[i].width;
			bb_array[array_index++] = holds[i].height;
		}
		return &bb_array[0];
	}

	int* findProjectorBox(unsigned char* redData, unsigned char* greenData, unsigned char* blueData, int imageWidth, int imageHeight) {
		
		// Load three colors as Mat
		Mat redImage, blueImage, greenImage;
		redImage = Mat(imageHeight, imageWidth, CV_8UC4, redData);
		blueImage = Mat(imageHeight, imageWidth, CV_8UC4, blueData);
		greenImage = Mat(imageHeight, imageWidth, CV_8UC4, greenData);

		// Write out for debugging
		imwrite("C:\\Users\\f000z5z\\redOrig.jpg", redImage);
		imwrite("C:\\Users\\f000z5z\\blueOrig.jpg", blueImage);
		imwrite("C:\\Users\\f000z5z\\greenOrig.jpg", greenImage);

		// Convert to HSV color space
		Mat hsvRed, hsvBlue, hsvGreen;
		cv::cvtColor(redImage, hsvRed, COLOR_BGR2HSV);
		cv::cvtColor(greenImage, hsvGreen, COLOR_BGR2HSV);
		cv::cvtColor(blueImage, hsvBlue, COLOR_BGR2HSV);


		// Get Masks
		Mat lowerRed, upperRed, green, blue;
		inRange(hsvRed, Scalar(0, 100, 200), Scalar(10, 255, 255), lowerRed);
		inRange(hsvRed, Scalar(160, 100, 200), Scalar(179, 255, 255), upperRed);
		inRange(hsvGreen, Scalar(55, 100, 200), Scalar(70, 255, 255), green);
		inRange(hsvBlue, Scalar(110, 100, 200), Scalar(130, 255, 255), blue);

		// Combine
		Mat redCombined, redGreenCombined, combined;
		addWeighted(lowerRed, 1.0, upperRed, 1.0, 0.0, redCombined);
		addWeighted(redCombined, 1.0, green, 1.0, 0.0, redGreenCombined);
		addWeighted(redGreenCombined, 1.0, blue, 1.0, 0.0, combined);

		// Write out for debugging 
		imwrite("C:\\Users\\f000z5z\\combined.jpg", combined);
		imwrite("C:\\Users\\f000z5z\\redInRange.jpg", redCombined);
		imwrite("C:\\Users\\f000z5z\\greenInRange.jpg", green);
		imwrite("C:\\Users\\f000z5z\\blueInRange.jpg", blue);

		// Apply hugh transform to combined
		Mat dst, cdst;
		Canny(combined, dst, 50, 200, 3);
		cvtColor(dst, cdst, COLOR_GRAY2BGR);
		vector<Vec4i> lines;
		HoughLinesP(dst, lines, 1, CV_PI / 180, 50, 50, 10);
		for (size_t i = 0; i < lines.size(); i++) {
			Vec4i l = lines[i];
			line(cdst, Point(l[0], l[1]), Point(l[2], l[3]), Scalar(0, 0, 255), 3, CV_AA);
		}

		imwrite("C:\\Users\\f000z5z\\hough.jpg", cdst);


		int *projectorCoord = new int[4];
		projectorCoord[0] = 26;
		return &projectorCoord[0];
	}
}
