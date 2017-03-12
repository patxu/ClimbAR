# Climbing Hold Recognition Training
Our implementation of an object recognition classifier for climbing holds.

## Overview

- `negatives/`
- `positives/`
- `test/`

Where `negatives` is a folder that contains background images, `positives` is a folder containing images of the object (cropped so the image is mainly the object), and `test` contains testing images. There are about 2000 images in total, which are stored on the training machine (Dartmouth's CS cluster).

## Run

##### To begin, run `./photo_create_pos positives/`

This will resize every image in positives to 3% its original size. It will then flip, blur, lighten, darken, and add and remove contrast to generate more images. This decreases overfitting and makes our classifier invariant to common functions applied to real-worled images.

It will then create a description file named `positive.txt`. Using this, it then runs `opencv_createsamples` which creates a file `training.vec` in the `positives` directory.

##### Then run `./photo_create_neg negatives/`

This will resize every image in negatives to 25% and apply all modifications described above to create more data. It will then create a `negatives.txt` file.

NOTE: Some values in both scripts are hard coded for the images I am using. 3% of image size my iPhone takes produces an output image of about the right size (it should be no larger than 100x100). If images of lower res are used, this value might need to be tweaked. Similarly, the positive script assumes there are 959 total images after generating all images. If this is not the case, that value needs to be changed.
