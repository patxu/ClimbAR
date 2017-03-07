# climbAR Unity Project

## Overview
- `1_KinectCheck`


## Quickstart
Check out our demo scene, `4_DemoGame`. Let's walk through it.
- `KinectColorManager` and `KinectColorView` are responsible for displaying the color image from the Kinect
- `KinectBodyManager` allows us to track the skeleton, given by the Kinect
- `KinectClassify` takes frames from the Kinect and runs it through our OpenCV classifier. It creates bounding boxes for the hand holds in the scene
  - each scene using the Kinect opens its own Kinect objects
- `KinectLight` illuminates the mesh onto which the color image is projected
- the 'Loader' script is attached as a component to the main camera, which allows us to manage state throughout scenes
