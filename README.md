**Introduction** 

The project is an API 

- GET `/mission` - get all missions
- POST `/mission` - add new mission
- GET `/countries-by-isolation` - get most isolated country
- POST `/find-closest` - find closest mission

**Steps to run**

1. run the shell file `./run_postgres.sh` to run the PostgreSQL Docker container.
2. run Migration to initial the DB - `dotnet ef database update`.
3. `dotnet build`.
4. `dotnet run`.

**For using GoogleMapsApi**

(https://developers.google.com/maps/documentation/geocoding/overview)

The project contains usage of google geocoding api to get coordinates from address.
Without a valid api key it wil generate random coordiantes instead.

- Add an api key to `appsetting.json` - `"GoogleMapsApiKey": "<YOUR_GOOGLE_MAPS_API_KEY>"`.
