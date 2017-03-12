# OpenCV Wrapper for Unity

We use [OpenCV](https://github.com/opencv/opencv) to perform climbing hold classification on a wall. If you want to learn more about how we train our model, you can go [here](https://github.com/patxu/cs98-senior-project/tree/master/checkpoints/opencv_hack#opencv-hack-a-thing). This was the basic process we used. We expanded upon this throughout the two terms during which ClimbAR was developed. We took many more images (~2000 in total). The results of our classifier can be seen in the several images in our main README.

We also need to use OpenCV's [contributor modules](https://github.com/opencv/opencv_contrib), since it allows us to use the handy [charuco markers](https://github.com/opencv/opencv_contrib/tree/master/modules/aruco).

<img src="https://raw.githubusercontent.com/opencv/opencv_contrib/master/modules/aruco/tutorials/charuco_detection/images/board.jpg" height=300px>

We haven't yet implemented the automatic projector alignment actually using these markers but in the future the [alignment](https://github.com/patxu/cs98-senior-project/tree/master/climbARUnity#quickstart) step would be automatic.

## Setup
  - following [this guide](http://dogfeatherdesign.com/opencv-3-0-microsoft-visual-studio-2015-cmake-and-c/), we build OpenCV via CMake as a dll (Dynamically-Linked Library) for Windows
  - , in addition to the above guide, follow this [official one](https://github.com/opencv/opencv_contrib#how-to-build-opencv-with-extra-modules) from OpenCV that builds the base code and contributor modules. you'll just need to add a small flag to the CMake build process
