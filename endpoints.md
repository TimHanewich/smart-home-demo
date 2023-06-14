# API Endpoints
Below is a description of each of the API endpoints included in the provided [Azure Function code](./src/api/).

## /odata
This serves as an *odata* provider, but only provides the *insert* (create a record) service. In the below example, the `SingleValueReading` is the name of the table we are creating a record for. The JSON body contains the columns for the new record in this table.

### Example Request
```
POST /odata/SingleValueReading
{
    "Id": "f45db62f-498e-4c07-ac08-1457a98e5ed0",
    "CollectedAtUtc": "2023-02-15 11:45:32",
    "Location": 4,
    "Value": 72.4,
    "ReadingType": 0
}
```

## /snapshot
For any location with data in the last 60 minutes (1 hour), returns the most recent temperature and humidity reading. Temperature in Fahrenheit, humidty in relative humidity.

### Example Request
```
GET /snapshot
```

### Example Response
```
[
    {
        "location": 4,
        "temperature": 72.154236,
        "temperatureCollected": "2023-06-14T13:35:48",
        "humidity": 48.217434,
        "humidityCollected": "2023-06-14T13:35:48"
    },
    {
        "location": 5,
        "temperature": 70.6938,
        "temperatureCollected": "2023-06-14T13:35:49",
        "humidity": 46.970493,
        "humidityCollected": "2023-06-14T13:35:49"
    },
    {
        "location": 7,
        "temperature": 72.674,
        "temperatureCollected": "2023-06-14T13:35:49",
        "humidity": 55.682465,
        "humidityCollected": "2023-06-14T13:35:49"
    }
]
```

## /summary
Provides the average of temperature and humidity readings for the past number of hours (as a "summary").

### Example Request
```
GET summary?hours=12
```
In the above example, the `hours` query parameter determines the number of hours worth of data you would like to get an average for. For example, setting this to 12 would mean you are requesting an average for both temperature and humidity for each location within the last 12 hours. If the `hours` query parameter is *not* provided, it will default to 24 hours.

### Example Response
The response returns a JSON array. Each object within contains the average temperature (in Fahrenheit) and average humidity for the past number of hours that you specified in the request.
```
[
    {
        "location": 4,
        "temperature": 72.154236,
        "humidity": 48.217434
    },
    {
        "location": 5,
        "temperature": 70.6938,
        "humidity": 46.970493
    },
    {
        "location": 7,
        "temperature": 72.674,
        "humidity": 55.682465
    }
]
```

## /sample
Creates sample (fake) data, purely for the purposes of demonstration. This will create both sample temperature values and humidity values for each location you specify you'd like sample data for.

### Example Request
Provide the location numbers which you would like sample data to be made for like so. If your `locations` property is an empty array (`[]`), the default locations of 4 (living room), 5 (master bedroom), and 7 (garage) will be used.
```
POST /sample
{
    "locations": [4, 5, 7]
}
```

### Example Response
A simple `201 Created` response will be returned, without any body. This confirms the sample data has been created.