# climbAR Unity Project

## Overview
Here lies the bulk of our work. We incorporate our OpenCV hand hold detector and Microsoft Kinect into a Unity game. We also make it easily extensible for future work.

This is what our game looks like from the perspective of the Kinect. We take this, run some transformations, and then display it on the actual wall/person via a projector.

<img src="readme_imgs/overlay2.jpg" width=400x>



- `1_KinectCheck`

## Quickstart
- **Start the game** in `ClimbAR_Start`
  - it checks if the Kinect is connected – if the screen remains black then the Kinect is not connected
- Next, **specify the alignment** for the projector
  - if you're displaying your screen via projector, align by dragging each of the corner spheres to the four corners of the screen from the perspective of the Kinect.

  - [ Picture Here ]
- Automatic **hold detection**
  - next we automatically detect holds and then send you to the menu scene

Check out our demo scene, `4_DemoGame`. Let's walk through it.
- `KinectColorManager` and `KinectColorView` are responsible for displaying the color image from the Kinect
- `KinectBodyManager` allows us to track the skeleton, given by the Kinect
- `KinectClassify` takes frames from the Kinect and runs it through our OpenCV classifier. It creates bounding boxes for the hand holds in the scene
  - each scene using the Kinect opens its own Kinect objects
- `KinectLight` illuminates the mesh onto which the color image is projected
- the 'Loader' script is attached as a component to the main camera, which allows us to manage state throughout scenes
