# Steps to Demonstrate
1. Make sample data using the `/sample` endpoint, described in the [endpoints description](./endpoints.md). This step is not necessary if you have Raspberry Pi IoT devices deployed that are collecting and uploading data. **Do this again immediately before demonstrating to be sure recent data is available**.
2. Open (install if needed) Postman. Have the following two examples open. Be prepared to also make a custom connector for both of these endpoints. You can find sample requests and responses in the [endpoints description](./endpoints.md).
    1. `/snapshot` endpoint
    2. `/summary` endpoint
3. Download necessary images that will be used in the canvas app. Be mindful of what name you save the images as on your PC; when you upload them to Power Apps, you will have to use this exact name to reference them, so don't use spaces or any weird characters.
    1. [garage](https://i.imgur.com/KOU0Gp3.png) for garage
    2. [bed](https://i.imgur.com/NjnYvFA.png) for master bedroom
    3. [couch](https://i.imgur.com/n1W7j62.png) for living room
    4. [beaker](https://i.imgur.com/3gyg6L7.png) for test location (99)
4. Optionally, build the assets in a separate environment for you to follow along to as you demo:
    1. Deploy Azure Function code (if needed)
    2. Build custom connector to the `snapshot` and `summary` endpoints.
    3. Build canvas app to consume custom connector. You can find a link to download the latest version of the canvas app in the [main page](readme.md). When importing, you may have trouble because the custom connector that *it* was pointing to does not exist in your environment. Deny the import from connecting to the connector. Delete the placeholder connector it is pointing to and reconnect to your own (the one you build above in step 2).

## Screenshots: Power Apps Canvas App
![img](https://i.imgur.com/aNUkcK1.png)
![img](https://i.imgur.com/oBsSGzP.png)
![img](https://i.imgur.com/T2wjSZv.png)

## Screenshots: Power Automate Flow
![img](https://i.imgur.com/kjkA0wz.png)
![img](https://i.imgur.com/vzZbCK4.png)
![img](https://i.imgur.com/KMrc9q0.png)
