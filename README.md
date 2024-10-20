# ObsAutoRefresh

An annoyance that I've experienced while using OBS is that video capture devices freeze if they get unplugged for a split second, and you have to manually press the "Deactivate" button followed by the "Activate" button to make them work again. This fixes that.

## How it works
Whenever a USB unplugging is detected by Windows, this script will forcefully refresh all desired video capture devices by using the OBS websocket API. In other words, this is only Windows compatible and not very efficent 👍👍

## Example

![obsautorefresh](https://github.com/user-attachments/assets/7c31a6b0-a11e-472c-87b9-3317f7e79496)


## Prerequisites
- Windows
- Either OBS version 28 or later, or an older version of OBS with the [websocket plugin](https://github.com/obsproject/obs-websocket/releases)

## How to use
1. Enable the OBS WebSocket server.
   
   ![image](https://github.com/user-attachments/assets/1009ac63-49e3-4e58-be2b-c3d745195914)
   
   ![image](https://github.com/user-attachments/assets/263eb5c9-bdef-493d-a94f-f8b66a477c1d)


3. Download the [latest release](https://github.com/turecross321/ObsAutoRefresh/releases) of ObsAutoRefresh.
4. Extract `ObsAutoRefresh-Windows-x64.zip` and run `ObsAutoRefresh.exe`.

   ![image](https://github.com/user-attachments/assets/12d0b20e-3299-423a-a8be-788bfe711048)
   
5. Fill in your credentials and connect.

   ![image](https://github.com/user-attachments/assets/61545a16-1dda-46e8-8bd2-a7bcff741312)

   Note:
      - You can enter your IP in the host field like `ws://<ip>`, but `ws://localhost` should already work if OBS and ObsAutoRefresh are running on the same computer.
      - If you do not wish to use a password for the OBS Websocket server, you may leave the password field in ObsAutoRefresh blank.



7. Add the video capture device that you want to be automatically refreshed.

   ![image](https://github.com/user-attachments/assets/1a1d0df9-da6f-4d9f-8f94-3bd0f414a87b)


8. Enjoy 👍👍👍👍👍
