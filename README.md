# ObsAutoRefresh

An annoyance that I've experienced while using OBS is that video capture devices freeze if they get unplugged for a split second, and you have to manually press the "Deactivate" button followed by the "Activate" button to make them work again. This fixes that.

## How it works
Whenever a USB unplugging is detected by Windows, this script will forcefully refresh all desired video capture devices by using the OBS websocket API. In other words, this is only Windows compatible and not very efficent 👍👍

## Prerequisites
- Either OBS version 28 or later, or an older version of OBS with the [websocket plugin](https://github.com/obsproject/obs-websocket/releases).

## How to use
1. Enable the OBS WebSocket server
   
   ![image](https://github.com/user-attachments/assets/1009ac63-49e3-4e58-be2b-c3d745195914)
   
   ![image](https://github.com/user-attachments/assets/263eb5c9-bdef-493d-a94f-f8b66a477c1d)


3. Download the [latest release](https://github.com/turecross321/ObsAutoRefresh/releases) of ObsAutoRefresh
4. Extract `win-x64.zip` and run `ObsAutoRefresh.exe`

   ![image](https://github.com/user-attachments/assets/0db0e1d5-f614-48d5-bb70-497b95919eb9)


   
5. Fill in your credentials

   ![image](https://github.com/user-attachments/assets/89322add-63bb-4cbb-a8a2-5895b3ad27e7)

6. Add the video capture device that you want to be automatically refreshed.

   ![image](https://github.com/user-attachments/assets/b34797f4-3f6d-4b98-9995-d009858079e7)

7. Enjoy 👍👍👍👍👍
