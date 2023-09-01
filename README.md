# Stabilometry-Analysis-Software

Open source program used for analysing and visualisation of Stabilometry data

Supported Poses:
- Both Legs Joined Parallel
- Both Legs 30° Angle
- Both Legs Parallel Apart
- Tandem Left Front
- Tandem Right Front
- Left Leg
- Right Leg

Supported Tasks:
- Solid Surface & Eyes Open
- Solid Surface & Eyes Closed
- Soft Surface & Eyes Open
- Soft Surface & Eyes Closed

Calculated parameters:
- Sway Path Total
- Sway Path AP
- Sway Path ML
- Mean Sway Velocity Total
- Mean Sway Velocity AP
- Mean Sway Velocity ML
- Sway Average Amplitude AP
- Sway Average Amplitude ML
- Sway Maximum Amplitude AP
- Sway Maximum Amplitude ML
- Mean Distance
- 95% Elipse Area

Supported visualisation methods:
- Compare Stabilometry Charts
- Compare Stabilograms
- Compare Different Poses

Software imports CSV files with the following columns:
f_time, copX, copY

Used version Unity 2021.3.23f1
Used Unity extensions
- Runtime File Browser https://assetstore.unity.com/packages/tools/gui/runtime-file-browser-113006
- DOTween https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676
- com.unit.uiextensions https://github.com/Unity-UI-Extensions/com.unity.uiextensions
Used database SQLite

Database Remover is a simple C# script that makes sure all data is deleted.

Data of the program is stored in C:\Users\[USER-NAME]\AppData\LocalLow\GreyRocketIndustries
