# ClimbAR
Orestis Lykouropoulos, Charley Ren, Pat Xu, David Bain, Jon Kramer

<img src="readme_imgs/videos/hop_demo.gif">

## Table of Contents

- [Overview](https://github.com/patxu/cs98-senior-project#overview)
- [Mockups](https://github.com/patxu/cs98-senior-project#game-mockups)
- [Unity Scene](https://github.com/patxu/cs98-senior-project#unity-scene)
- [Architecture](https://github.com/patxu/cs98-senior-project#architecture)
- [Setup](https://github.com/patxu/cs98-senior-project#setup)
- [Deployment](https://github.com/patxu/cs98-senior-project#deployment)
- [Docs and Poster](https://github.com/patxu/cs98-senior-project#docs)
- [The Team](https://github.com/patxu/cs98-senior-project#the-team)
- [Acknowledgements](https://github.com/patxu/cs98-senior-project#acknowledgements)

## Overview
We are building an augmented reality climbing game platform, with a focus on releasing the platform as a developer-friendly development kit. Components include
- hold recognition via [OpenCV](http://opencv.org/), and
- body (skeleton) detection via [Microsoft Kinect](https://developer.microsoft.com/en-us/windows/kinect)

to allow developers to build 2D, interactive climbing games via Unity (e.g. these are some games that inspired our work [1](https://www.youtube.com/watch?v=W0ErnsIVzkc), [2](https://www.youtube.com/watch?v=kg2uRGf_04g), [3](http://augmentedclimbing.com/games/)).



<img src="readme_imgs/videos/music_game_demo.gif">

*This is an example of what our project can do. David is activating and deactivating holds, each of which have an associated music track.*

<br>
<img src="readme_imgs/videos/demo_vid1.gif">  <img src="readme_imgs/videos/demo_vid2.gif">

*A couple short videos of our Music Game in action at end-of-term demos.*

## Game Mockups
Some basic mockups for how we envision the project. See a simple game UI displayed on the wall. Then, grab a hold to select an option.

<img src="readme_imgs/game_menu.png" width=300x>
<img src="readme_imgs/game_route_generation.png" width=300x>

## The Unity Project
Learn more about our ClimbAR Unity Project [here](/climbARUnity/). This discusses how we incorporate OpenCV and the Kinect into a Unity game!

## Architecture
- OpenCV
  - read about specifics with the OpenCV wrapper [here](./OpenCVUnity/OpenCVUnity)
- Unity – connecting OpenCV and Kinect
  - OpenCV exists in C++ but not in C#, so we build it as a dll
  - a Unity C# script imports the dll so now we can now use OpenCV in Unity! while there is an OpenCV asset on the Unity asset store, but it costs $95 so we opted to write our own wrapper
  - we use the Kinect SDK to access live color images, skeleton and give this data to OpenCV to do classification on each game startup
- OpenCV Hold Recognition Classifier
  - start with ~500 positive & negative images focusing on climbing holds and train an OpenCV custom object detection classifier (Haar classifier) to detect climbing holds
    - we have to define a bounding box for our custom object in each of our images. this took a while until David smartened up and chose to make the bounding box the entire image by making each image a close-up of the climbing hold.
    - we store all of our images on a [Google Drive](https://drive.google.com/drive/folders/0B2FwMr5goxlja1JJWW1TZlhCM2s?usp=sharing)
- Kinect
  - using the official [Microsoft Kinect SDK](https://www.microsoft.com/en-us/download/details.aspx?id=44561) to get live color images (for OpenCV) and body skeleton (e.g. for a game). may also use it for depth/infrared map, etc.

## Setup
- Windows only, currently
- follow the guide above to build OpenCV
  - find our Visual Studio project for the OpenCV wrapper in `OpenCVUnity`
- download [Unity](https://unity3d.com/get-unity/download) and the Kinect SDK
  - find the Unity project in `climbARUnity`

### Hold Recognition Classifier
If you want to actually train your own classifier, here's what we did – YMMV.
- OpenCV Python
  - Install with Homebrew
    - `brew tap homebrew/science`
    - `brew install opencv3`

  - Set system paths
    - `export DYLD_FALLBACK_LIBRARY_PATH=/usr/local/Cellar/opencv3/3.1.0_1/lib:$DYLD_FALLBACK_LIBRARY_PATH`
    - `export PYTHONPATH=/usr/local/Cellar/opencv3/3.1.0_3/lib/python2.7/site-packages:$PYTHONPATH`
      - if this doesn't work then your opencv version might be slightly different. search around for the directory and it should work
    - `export PATH=/usr/local/Cellar/opencv3/3.1.0_3/bin:$PATH`
    - check by opening up `python` on cmd line and then trying to import – `import cv2`

  - Training
    - followed [this tutorial](
  http://docs.opencv.org/trunk/dc/d88/tutorial_traincascade.html)
    - `opencv_createsamples --vec classifier/position_single -info info.dat -bg bg.dat`
      - use the `-img` and `-num` flag to train off one image. this is what we did when only had 7 images and had to generate training images from a single image
    - `opencv_createsamples -vec classifier/position_single -info info.dat -bg bg.dat`
    - `opencv_traincascade -data train_cascade/ -vec classifier/position_single -bg bg.dat -numPos 50 -numNeg 3`

  - Classifying
    - Classification is done in `KinectClassify`. The classifier used can be changed by updating the `classifierPath` in `KinectClassify`. We trained two classifiers, one for the classroom setting and one for the Dartmouth climbing gym, but any cascade classifier can be used if a new classifier is created for a specific environment.

## Deployment
- hook up to a projector and run the Unity project!
- the setup scenes will check if the Kinect is connected and will also run some alignment preprocessing on the Kinect image

## Docs
- [Project Proposal](https://docs.google.com/document/d/1-N9_9W50bxWwFv98lRIs-yA9pZ39pB0hi-4nF0_e69U/edit?usp=sharing)
- [User Personas](https://docs.google.com/document/d/1pRK2dLdDFMOfifJBphd1jmsjdPxHTu6pCOvMk0Kpzp8/edit?usp=sharing)
- [Mockups and Data Model](https://docs.google.com/document/d/1wIeR-_1b2lWhGa01Qd_FU361YMmZCi3dA5iZgJWjieM/edit?usp=sharing)
- [User Testing Plan](https://docs.google.com/document/d/1SZNJkBW5mkPORlVMg3iUN0mf9WrXpGZTmwKvynMM2wU/edit?usp=sharing)
- [Testing Analysis](https://docs.google.com/document/d/1r5oHRH4LkSW818os22fBRWORp74uME6gG6vRarHy7Go/edit?usp=sharing)
- [End of Term Demo Poster](readme_imgs/poster.pdf)
- [End of Term Debrief](https://docs.google.com/document/d/1HiUrD0mVZoncD63i8xACN6EJCa3EvmVWI4mNLXlZb8Y/edit?usp=sharing)

## The Team
Who made this wonderful, open-source piece of art? Look no further.
<img src="readme_imgs/group1.jpg" width=500x>

Left to Right: Orestis, Charley, Pat, David, Jon

## Acknowledgements
  - ### Tim Tregubov
    For his technical insight, mentorship, and humor.

    <img src="readme_imgs/red-panda.jpg" width=300x>

  - ### DALI Lab
    For providing a inspiring and collaborative space (Sudikoff 007), as well as crucial hardware (Macbook Pros, Kinect & projector, Windows PCs).
