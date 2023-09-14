# Development of an advanced tool for analyzing and visualizing stabilometry results

A repository containing code developed as part of the Bachelor thesis at the University of Ljubljana, Faculty of Computer and Information Science by [Aljaž Podgornik](https://github.com/whiteRocket/).

## Abstract
As part of this thesis, we implemented an open-source program to analyze and visualize stabilometry results. The program is written in Unity and is currently available on the Windows operating system. The program aims to offer as many valuable parameters as possible, has as many visualization options as possible, and is as easy to use as possible. We also evaluated the program. The evaluation of the program took place in such a way that the expert in the field of stabilometry fulfilled the set tasks that covered the entire use of this program. During this, we observed and wrote down the expert’s comments and our observations regarding the use of the program. The results showed that the program successfully analyzed and visualized the results of stabilometry. The program would benefit from having the option to export data in PDF format, which is beyond the scope.

The thesis is accessible at:
[https://repozitorij.uni-lj.si/Dokument.php?id=173650]

Bibtex:
```
@thesis{Podgornik2023,
    title = {Development of an advanced tool for analyzing and visualizing stabilometry results},
    author = {Podgornik, Aljaž},
    year = {2023},
    school = {University of Ljubljana, Faculty of Computer and Information Science},
    type = {Bachelor thesis},
    address = {Ljubljana, Slovenia},
    note = {Mentor: Ciril Bohak, Co-mentor: Urška Puh, Language: Slovenian, Slovenian title: Razvoj naprednega orodja za analizo in vizualizacijo rezultatov stabilometrije},
    url = {https://repozitorij.uni-lj.si/Dokument.php?id=173650}
}
```

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
