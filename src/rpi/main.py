# connect to wifi
import network
import time
for x in range(0, 8):

    # connect
    wlan = network.WLAN(network.STA_IF)
    wlan.active(True)
    wlan.connect("(SSID here)", "(password here)")

    # wait a moment
    time.sleep(1.5)

    if wlan.isconnected():
        print("Connected!")
        break
    else:
        print("Wifi connection was not succesful on attempt # " + str(x+1))

# create DHT22 sensor
import dht
import machine
sensor = dht.DHT22(machine.Pin(28, machine.Pin.IN))

# continuous loop
import urequests
import toolkit
while True:

    #read
    print("Reading...")
    humidity = None
    temperature = None
    while humidity == None and temperature == None:
        sensor.measure()
        temperature = sensor.temperature()
        humidity = sensor.humidity()
    
    # convert temperature from celsius to fahrenheit
    temperature = (temperature * (9/5)) + 32

    print("Data captured! Temperature (F): " + str(temperature) + ", humidity: " + str(humidity))

    # collect other information for upload
    utc = toolkit.get_datetime_utc()
    url = "(URL endpoint of your Azure Function/API to post the data to)"

    # upload temperature
    print("Uploading temperature...")
    body = {"Id": str(toolkit.uuid4()), "CollectedAtUtc": utc, "Location": 99, "ReadingType": 0, "Value": temperature}
    tpr = urequests.post(url, json=body)
    tpr.close()

    # upload humidity
    print("Uploading humidity...")
    body = {"Id": str(toolkit.uuid4()), "CollectedAtUtc": utc, "Location": 99, "ReadingType": 1, "Value": humidity}
    hpr = urequests.post(url, json=body)
    hpr.close()

    # wait before proceeding with another read and upload
    print("Waiting...")
    time.sleep(60)