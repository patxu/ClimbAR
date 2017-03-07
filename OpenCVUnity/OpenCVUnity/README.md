# OpenCV Wrapper for Unity

## Setup
  - following [this guide](http://dogfeatherdesign.com/opencv-3-0-microsoft-visual-studio-2015-cmake-and-c/), we build OpenCV via CMake as a dll (Dynamically-Linked Library) for Windows
  - we also need to build [OpenCV](https://github.com/opencv/opencv) with its [contributor modules](https://github.com/opencv/opencv_contrib), since it allows us to use the handy [charuco markers](https://github.com/opencv/opencv_contrib/tree/master/modules/aruco)
  - so, in addition to the above guide, follow this [official one](https://github.com/opencv/opencv_contrib#how-to-build-opencv-with-extra-modules) from OpenCV that builds the base code and contributor modules. you'll just need to add a small flag to the cmake step
