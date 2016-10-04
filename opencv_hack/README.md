# OpenCV Hack-a-thing

## Setup
- ### Install with brew
  - `brew tap homebrew/science`
  - `brew install opencv3`

- ### Set paths (at least for me)
  - `export DYLD_FALLBACK_LIBRARY_PATH=/usr/local/Cellar/opencv3/3.1.0_1/lib:$DYLD_FALLBACK_LIBRARY_PATH`
  - `export PYTHONPATH=/usr/local/Cellar/opencv3/3.1.0_3/lib/python2.7/site-packages:$PYTHONPATH`
    - if this doesn't work then your opencv version might be slightly different. search around for the directory and it should work
  - `export PATH=/usr/local/Cellar/opencv3/3.1.0_3/bin:$PATH`
  - check by opening up `python` on cmd line and then trying to import – `import cv2`

## Training

- ### Gathering Data
We took 64 training images, and then resized it using this ugly bash script:

```
counter=0; name="out.jpg"; for i in $( ls ); do convert -resize 25% $i "../single_hold_training_resize/$counter$name"; let counter+=1; done;
```

- ### Image Annotation
Used [this](http://nicodjimenez.github.io/boxLabel/annotate.html) tool to help get the boudning box (pixels, top left to bottom right) of the image



- ### Training
  - `opencv_createsamples -img img/hold_view1.JPG -num 50 -vec position_single -info info.dat -bg bg.dat`
  - `opencv_traincascade -data train_cascade/ -vec position_single -bg bg.dat -numPos 50 -numNeg 3`

## Results
Initial training with 7 images produced poor results. We even tested it on one of our training images 😕.

![detected_7](readme_imgs/detected.jpg)
