# climb.AR
Orestis Lykouropoulos, Charley Ren, Pat Xu, David Bain, Jon Kramer

## Overview
An augmented reality climbing game library, with a focus on releasing the platform as a developer-friendly development kit. Components include
- wall, hold, and climber body recognition,
- route generation, and
- hold grabbing detection

to allow developers to build 2D, interactive climbing games via Unity (e.g. [1](https://www.youtube.com/watch?v=W0ErnsIVzkc), [2](https://www.youtube.com/watch?v=kg2uRGf_04g), [3](http://augmentedclimbing.com/games/)).


- ### Game Mockup

<img src="readme_imgs/game_menu.png" width=400x>
<img src="readme_imgs/game_route_generation.png" width=400x>

## Architecture
- Hold Recognition
  - OpenCV (copy from hack-a-thing?)
  - Image Pre-Processing in Java
    This is currently an exploration of image pre-processing and will later be incorporated in a single hold recognition component with OpenCV, and will also likely be translated to C#.
    The java program reads an image, finds the most common color, which is part of the background in our climbing gym images. Then for every pixel, if it is close to the most common color (using a color distance function), the color is replaced with white, which isolates the holds. Currently we have no mechanism of removing the pieces tape that is on most climbing walls, so besides the holds it also isolates the tape, but we will work on removing that.
- Unity
- Kinect

## Setup
- Hold Recognition
  - OpenCV (copy from hack-a-thing?)
  - Image Pre-Processing in Java: Simple Java class - run from terminal or IDE

## Deployment
- Hold Recognition
  - OpenCV (copy from hack-a-thing?)
  - Image Pre-Processing in Java: not yet integrated in Hold Detection module.

## Docs
- [Project Proposal](https://docs.google.com/document/d/1-N9_9W50bxWwFv98lRIs-yA9pZ39pB0hi-4nF0_e69U/edit?usp=sharing)
- [User Personas](https://docs.google.com/document/d/1pRK2dLdDFMOfifJBphd1jmsjdPxHTu6pCOvMk0Kpzp8/edit?usp=sharing)
- [Mockups and Data Model](https://docs.google.com/document/d/1wIeR-_1b2lWhGa01Qd_FU361YMmZCi3dA5iZgJWjieM/edit?usp=sharing)

## The Team
<img src="readme_imgs/group1.jpg" width=500x>

Left to Right: Orestis, Charley, Pat, David, Jon

## Acknowledgements
  - ### Tim Tregubov
    For his technical insight, mentorship, and teaching us to love (red pandas).

    <img src="readme_imgs/red-panda.jpg" width=300x>

  - ### DALI Lab
    For providing a inspiring and collaborative space (Sudikoff 007), as well as crucial hardware (Macbook Pros, Kinect, PCs).
