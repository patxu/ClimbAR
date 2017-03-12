# Automatic Projector Alignment

Automatically detect the projector bounds based on an image taken by the Kinect

## Overview
Use OpenCV and [aruco markers](https://www.uco.es/investiga/grupos/ava/node/26) to robustly detect a skewed plane. Specifically, a chessboard + aruco marker combination, known as [charuco](https://github.com/opencv/opencv_contrib/blob/246ea8f3bdf174a2aad6216c2601e3a93bf75c29/modules/aruco/tutorials/charuco_detection/charuco_detection.markdown) lets us precisely detect the edges of a plane.

We haven't yet implemented the automatic projector alignment actually using these markers but in the future the [alignment](https://github.com/patxu/cs98-senior-project/tree/master/climbARUnity#quickstart) step would be automatic.
