# Smart Home Demo
As shown on Microsoft's Low-Code Revolution Show (click image for video):  
[![Low Code Revolution Show](https://img.youtube.com/vi/v0mPmCw5yl0/0.jpg)](https://www.youtube.com/watch?v=v0mPmCw5yl0)

And in longer form (click image for video):  
[![Long Form Tutorial](https://img.youtube.com/vi/BYmdi3mYHhM/0.jpg)](https://www.youtube.com/watch?v=BYmdi3mYHhM)
  
In this repository you will find the following code:
- [The C# code used to deploy the Azure Function (API) to serve for data ingestion and consumption](./src/api/)
- [The Python (MicroPython) code that runs on the Raspberry Pi Pico W and continuously captures and uploads temperature and humidity data](./src/rpi/)

## Canvas App
You can download the Power Apps Canvas app that is demonstrated [here](https://github.com/TimHanewich/smart-home-demo/releases/download/canvas1/Smart.Home.Monitor.msapp).  
![canvas app](https://i.imgur.com/vvNZl6T.png)

## API Endpoints
You can find a description of the API endpoints available in the provided code [here](endpoints.md).