# Asymmetry
Free, Open Source Asymmetrical VR Repo

![](AsymmetryGif.gif)

### Dependencies ###
Make sure you have the following in your project before importing:
- URP
- XR Plugin Management (Oculus)
- Oculus Utilities
- PUN 2
- Photon Voice 2
- Cinemachine
- Text Mesh Pro
- Probuilder

### __How To Use__
- Create a new Unity Project with the URP Renderer (only tested on 2020.2+)
- Make sure you have all the dependencies above
- Import the package "Asymmetric" from the relases section
- Open the scene "Asymmetry_Online_Demo"
- Setup Input in the Unity Input Manager for "Horizontal_Keyboard", "Vertical_Keyboard", and "Jump_Keyboard"
- Press Play in the editor
- If a VR Headset is connected it will default to VR Controller
- The PC Player can use buttons to add / drop themselves into the scene 
- The PC Player can also disable to VR Player **(Warning This will freeze VR Players view)!**
- If no VR Headset is detected it will automatically use Third Person Control
- Currently Supports Quest / Quest 2, and disables all PC player stuff when android platform is detected

#### Networking
This repo currently contains Photon Multiplayer and Photon Voice. In order to use these services, follow the photon wizard on startup and assign an AppID for both PUN and Voice.
For more information visit https://www.photonengine.com/en-US/Photon

### Things to note
- This repo currently only supports Oculus HMD's
- This repo is not production ready and has not been thoroughly tested
- If your hands are pink, update project materials to URP
