# OpenCV Hack-a-thing

# Install with brew
brew tap homebrew/science
brew install opencv3

# Set paths (at least for me)
export DYLD_FALLBACK_LIBRARY_PATH=/usr/local/Cellar/opencv3/3.1.0_1/lib:$DYLD_FALLBACK_LIBRARY_PATH 
export PYTHONPATH=/usr/local/Cellar/opencv3/3.1.0_1/lib/python2.7/site-packages:$PYTHONPATH 