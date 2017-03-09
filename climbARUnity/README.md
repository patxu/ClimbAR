# climbAR Unity Project

## Overview
Here lies the bulk of our work. We incorporate our OpenCV hand hold detector and Microsoft Kinect into a Unity game. We also make it easily extensible for future work.

This is what our game looks like from the perspective of the Kinect. We take this, run some transformations, and then display it on the actual wall/person via a projector.

<img src="readme_imgs/overlay2.jpg" width=400x>

## Quickstart
- **Start the game** in `ClimbAR_Start`
  - it checks if the Kinect is connected – if the screen remains black then the Kinect is not connected
- Next, **specify the alignment** for the projector
  - if you're displaying your screen via projector, align by dragging each of the corner spheres to the four corners of the screen from the perspective of the Kinect.

    <img src="readme_imgs/alignment_example.png" width=500px>

  - this step is very important as it specifies the graphical transform that is applied to the holds and skeleton to overlay properly on objects in the real world
- **Hold classification** is done automatically, but if you want to pause here you can set the `Auto Classify` boolean to `false` in Unity via the `ClimbAR/FindHolds` scene
- The **Menu Scene** is where you'll probably be starting. Here, you can specify [additional scenes ](https://github.com/patxu/cs98-senior-project/blob/readme/climbARUnity/Assets/ClimbAR/Menu/Menu.cs#L9) and put in your own game!

## Details of A ClimbAR Scene

The **Menu scene** has lots of critical components. Let's take a look:
- `KinectColorManager` and `KinectColorView` are responsible for displaying the color image from the Kinect
- `KinectBodyManager` allows us to track the skeleton, given by the Kinect
- `KinectClassify` takes frames from the Kinect and runs it through our OpenCV classifier. It creates bounding boxes for the hand holds in the scene
  - each scene using the Kinect opens its own Kinect objects
- `KinectLight` illuminates the mesh onto which the color image is projected
- the 'Loader' script is attached as a component to the main camera, which allows us to manage state throughout scenes

  <img src="readme_imgs/menu.png" width=600px>

The MusicGame and RocMan game will be a good starting point for a developer looking to use our platform to create a game of their own.
