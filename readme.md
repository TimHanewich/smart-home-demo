# Smart Home Demo
As shown on Microsoft's Low-Code Revolution Show (click image for video):  
[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/v0mPmCw5yl0/0.jpg)](https://www.youtube.com/watch?v=v0mPmCw5yl0)

And in longer form (click image for video):  
[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/BYmdi3mYHhM/0.jpg)](https://www.youtube.com/watch?v=BYmdi3mYHhM)
  
In this repository you will find:
- [The C# code used to deploy the Azure Function (API) to serve for data ingestion and consumption](./src/api/)
- [The Python (MicroPython) code that runs on the Raspberry Pi Pico W and continuously captures and uploads temperature and humidity data](./src/rpi/)

## API Endpoints
You can find a description of the API endpoints available in the provided code [here](endpoints.md).