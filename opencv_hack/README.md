# OpenCV Hack-a-thing

## Install with brew
brew tap homebrew/science
brew install opencv3

## Set paths (at least for me)
`export DYLD_FALLBACK_LIBRARY_PATH=/usr/local/Cellar/opencv3/3.1.0_1/lib:$DYLD_FALLBACK_LIBRARY_PATH`

`export PYTHONPATH=/usr/local/Cellar/opencv3/3.1.0_3/lib/python2.7/site-packages:$PYTHONPATH`
- if this doesn't work then your opencv version might be slightly different. search around for the directory and it should work

`export PATH=/usr/local/Cellar/opencv3/3.1.0_3/bin:$PATH`


check by opening up `python` on cmd line and then trying to import – `import cv2`
