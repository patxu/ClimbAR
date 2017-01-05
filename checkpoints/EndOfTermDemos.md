# climb.AR || End of Term User Testing and Demos

## Our Work
These are links to various directories within our repo.
- the [OpenCV Unity wrapper](https://github.com/patxu/cs98-senior-project/tree/master/OpenCVUnity/OpenCVUnity)
- [climbing hold detection classifier](https://github.com/patxu/cs98-senior-project/tree/master/object_rec)
- the [Unity demo](https://github.com/patxu/cs98-senior-project/tree/master/climbARUnity) of our classifier and Kinect interaction


## Debrief
**So far** we've been able to write an OpenCV C++ wrapper for use in Unity, perform climbing hold recognition with an live image from the Kinect sensor, and finally pull everything together in a Unity application where we overlay a live image from the Kinect, the skeleton of the climber, and recognized holds.

**What worked well:** training an accurate classifier via OpenCV, using the Kinect sensor (color image, skeleton), using Unity for detect hold grabbing (i.e. collision detection)

**What was trickier:** overlaying the image, skeleton, and holds AND showing this on a projector that overlays everything ON the holds. several transformations of the space are needed here but I'm confident we can solve this

**What didn't happen at all:** a actual game pulling all this together into something *fun*

## User Testing Overview
We were able to test our system during the last couple weeks of the term. Users looked at our computer screen showing the Unity app running, which had a live image with all the relevant features overlaid. They saw the Kinect skeleton moving with their body and when they grasped the hold, they saw the color of the hold change.

**Awesome:** people were really excited to see this augmented reality scene actually happening, even if it was only on a screen. they were also really impressed with the accuracy of our classifier (basically no false positives)

**Needs work:** the Kinect skeleton detection isn't great and often doesn't respond well to users turning their bodies away from the camera

## Plan of Attack
Moving forward, we want to
- incorporate the projector so that this is truly augmented reality
- clean up the code base and repo
- create *at least* one game, ideally two, showcasing some for the use cases here
