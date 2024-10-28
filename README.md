# EyeTrackingwithVideo

---
page_type: sample
name: EyeTrackingwithVideo
description: User will get eye position, gaze direction along with video of the environment in the runtime duration of the application using a HoloLens 2.
languages:
- csharp
products:
- HoloLens 2
---

# EyeTrackingwithVideo 

![License](https://img.shields.io/badge/license-MIT-green.svg)

Supported device  | Supported Unity versions | Built MRTK
:---------------: | :----------------------: | :--------------------------: 
HoloLens 2        | unity 2021.3.6f1         | MRTK3


## Required tools

This repository uses following versions of tools
* HoloLens 2 with OS build minimum 20348.1537
* Unity 2021.3.6f1    
* MRTK3
* Visual Studio 2019 or higher 

## Setup

1. Clone or download this repository and extract all folders.
2. Open 'Unity Hub' and add click on 'Add project from disk' and locate the folder named 'EyeTrackingwithVideo-main' and add the project to your unity Hub.
3. Open the project in Unity editor. This project is made in Unity 2021.3.6f1.
4. Go to Unity editor and click on scenes folder and add 'sample scene' to 'Hierarchy' window.
5. Go to 'Mixed Reality' tab in editor and click on Project --- Apply recommended project settings for HoloLens2.
6. Click on File >>> Build settings >>> swith to UWP if it is not done before. Choose 'ARM 64-bit' architecture, visual studio version - 2019 or higher.
7. Fill the device portal info and click on 'Build'and create a new folder to save the project output ( or you can save in any existing folder). 
8. Go to the folder where output file is saved. Deploy using Microsoft visual studio 2019 or higher.

## Run and Getting Output file from the project
1. Launching this app in HoloLens and granting the gaze, microphone and camera permission in the dialog, the gaze framerate will be set to 90FPS and you will see 3 cubes and 1 cylinder following your gaze direction in 1.5 meter away.
    * The green cube represents your left eye gaze.
    * The red cube represents your right eye gaze.
    * The cyan cube represents your combined eye gaze.
    * The blue cylinder also represents your combined eye gaze but its coordinate is set related to the Unity camera GameObject.
2. Close the application when you are done with data recording.
3. Access the Device Portal of HoloLens2 and go to file explorer 
4. Follow the given below png image to access output files from this project.


![DevicePortal](https://github.com/user-attachments/assets/1814f71c-2629-4814-b4fa-29b98e8b6120)

5. Download gaze_data in csv format and video in mp4 format. Video information : 2272 X 1278 pixels at 30FPS. Downloaded gaze_data will look like the image given below.

![gaze_data_csv](https://github.com/user-attachments/assets/d30de12f-da8a-4b79-8af4-5aa34f03a190)

## References
This project is built by taking reference from Microsoft Open Source Code. 
