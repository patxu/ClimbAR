#!/bin/bash
# The name of the job, can be anything, simply used when displaying the list of running jobs
#$ -N climbing_wall_traing_cascade
# Combining output/error messages into one file
#$ -j y
# Set memory request:
#$ -l vf=32G
# Set walltime request:
#$ -l h_rt=48:00:00
# One needs to tell the queue system to use the current directory as the working directory
# Or else the script may fail as it will execute in your top level home directory /home/username
#$ -cwd
# then you tell it retain all environment variables (as the default is to scrub your environment)
#$ -V
# Now comes the command to be executed
opencv_traincascade -data data -vec positives/training.vec -bg negatives.txt -numPos 950 -numNeg 434 -numStages 10 -nsplits 2 -w 73 -h 98 -featureType LBP
exit 0
