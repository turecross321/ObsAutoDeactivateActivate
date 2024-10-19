# ObsAutoRefresh

An annoyance that I've experienced while using OBS is that video capture devices freeze if they get unplugged for a split second, and you have to manually press the "Deactivate" button followed by the "Activate" button to make them work again. This fixes that.

## How it works
Whenever a USB unplugging is detected by Windows, this script will forcefully refresh all desired video capture devices by using the OBS websocket API. In other words, this is only Windows compatible and not very efficent 👍👍
