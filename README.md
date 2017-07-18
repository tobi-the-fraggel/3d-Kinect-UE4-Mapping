# Control-Kinect-Center
This is school project solely made to explore some features of Microsoft Kinect V2

### Description
This program is designed to inherit basic mouse functionality in order to control a predefined set of programs. Currently control of Microsoft PowerPoint and Windows Media Player is featured. After compiling you get a simple straight forward GUI control center.

### Requirements
 - 64bit Windows OS
 - .NET >4.5.x
 - USB 3.0
 - Microsoft Kinect V2
 - Microsoft Visual Studio (Version 2016 recommended)
 - Power Point and Windows Media Player (obviously...)

### Usage
As this repo contains uncompiled C#-Code you need to compile it via VS at first.
Then simply open the program.
**WARNING:** The Graphic UI is in **not** in English. We are a German team of students and therefore it is written in our uber language^^

The running control center presents a green button. If you click on it "Kinect Control" is activated and the button color turns red to indicate that. Now you can use your right arm to move the mouse across the screen. To deactivate just click the button which just turned red. During "Kinect Control" a mouse click can be executed by simply presenting your left hand to the camera opened up and then closing it (making a fist). The process of your open left hand switching to the closed position is what the program translates as 'doClick'.

##### Other possible gestures or postures are
  - **Double Lasso** = closes the whole program
  - **Media Player**
    - **Right Hand Open** = Play/Pause music
    - **Left Hand Open** = Mute Sound
    - **Right Hand Lasso** = Next Song
    - **Left Hand Lasso** = Previous Song
  - **Power Point**
    - **Right Hand Open** = Arrow Right (usually *next slide*)
    - **Left Hand Open** = Arrow Left (usually *previous slide*)

### Development?
Do you want to contribute? Great!
Just fork the repo and create something of your own is my recommendation however and this directly leads me to a little issue. It is covered in the following paragraph.

### License
I wish I could offer this code completely open source **but** I can't as we have used (and included) proprietary libraries. So whatever you want to do with this code, you need to at least follow the copyright notes of [Microsofts Kinect SDK](https://www.microsoft.com/en-us/download/details.aspx?id=44561) and [Lightbuzz](https://github.com/LightBuzz/Kinect-Drawing) "Kinect-Drawing" application. The latter is published under the [MIT-License](https://github.com/LightBuzz/Kinect-Drawing/blob/master/LICENSE).

### Final Notes
Big shoutout goes to [Jingzhou Chen](github.com/TangoChen/KinectV2MouseControl). His repository gave us a first glimpse of Kinect and we used it as an entry point for our own development.
