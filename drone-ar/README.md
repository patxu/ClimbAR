##AR Drone Game

###Project Description:

We explored the possibility of creating a game that uses Augmented Reality (AR) and a drone with live video transmission. We envisioned something relatively close to Starfox 64 (video sample), or at least a similar viewing angle and gameplay, where AR enemy starships appear against the real-life background of the drone’s camera, shooting the player’s drone as he is trying to take them down and avoid bullets and obstacles. 

For the MVP we would just focus on creating AR obstacles on the live video feed, which would make the game more of an obstacle avoidance/racing game, but from which we could eventually reach the main goal.

###Exploration of Components

####Drone

We need sensor data to track the GPS position, acceleration, and direction of the drone. The sensors necessary to find this information are often already found on drones but we need some developer sdk to access the information. We looked at what drones existed that met this requirement at found the dji sdk. However, the dji drones that support this sdk have a price point ~$3000. This is far beyond the budget for this course and makes use of the dji sdk unfeasible. 
https://developer.dji.com/products/
http://store.dji.com/product/matrice-100?frome=developer

We considered instead creating an embedded system of sensors and attaching them to an existing drone, and acquiring this data manually. However, building out this system would be primarily a hardware sub-project. A perhaps bigger issue with this solution is that drones are usually made to support their weight distribution, and adding hardware to the drone could have unintended consequences on the drone’s flight capabilities. These issues would make this project out of scope for CS98 and a better issue for a ENGS89/90 project to tackle.

####VR Headset

Ideally we would want to use the Oculus Rift or the HTC Vive for a VR headset. However, the hardware requirements of using these headsets makes them impractical for the type of game we want to make. The drone needs to be flown in a very open spacious area, but these headsets require a performant computer that would have a high power requirement. It would difficult to find an environment to meet the needs of both the drone and the headset.  An alternative headset might be Samsung Gear VR, which only requires a Galaxy Smartphone, but then we would have concerns with the processing capabilities of the headset as described below.

####AR object creation

We looked into the possibility of using Unity or Unreal Engine to process the video feed and create the AR objects. Both engines seemed to support our intended goals. However, one worry we had was that the video processing could possibly be slightly slow so that the gameplay would be affected. We could not have a confident estimate of the delay without playing with the graphic engines, but we were hoping that even if the delay was measurable, we could get around it by a combination of lower graphics, and perhaps higher processing power either on extra hardware, or the cloud, if the internet connection is fast.

